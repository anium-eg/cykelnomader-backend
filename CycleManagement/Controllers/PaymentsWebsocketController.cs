using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;
using CycleManagement.Data;
using CycleManagement.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.ObjectPool;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace CycleManagement.Controllers
{
    enum PaymentStatus
    {
        NotFound,
        Cancelled,
        Success,
        Error,
        Timeout
    }

    [Route("ws/payments")]
    [ApiController]
    public class PaymentsWebsocketController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public PaymentsWebsocketController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("paymentListener/{paymentId}")]
        public async Task<IActionResult> GetPaymentStatus(Guid paymentId)
        {
            if (!HttpContext.WebSockets.IsWebSocketRequest)
            {
                return BadRequest("Only Websocket requests allowed");
            }


            var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();

            CancellationTokenSource timeoutToken = new CancellationTokenSource(TimeSpan.FromMinutes(2));
            CancellationTokenSource linkedCancellationToken = CancellationTokenSource.CreateLinkedTokenSource(timeoutToken.Token, HttpContext.RequestAborted);

            PaymentStatus dbPollStatus;

            try
            {
                dbPollStatus = await PollDatabase(paymentId, linkedCancellationToken);
            }
            catch (OperationCanceledException)
            {
                dbPollStatus = PaymentStatus.Timeout;
            }


            await webSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, dbPollStatus.ToString(), CancellationToken.None);

            return new EmptyResult();

        }

        async Task<PaymentStatus> PollDatabase(Guid paymentId, CancellationTokenSource cancellationToken)
        {
            Payment? paymentEntity;
            
            paymentEntity = await _context.Payments.AsNoTracking().FirstOrDefaultAsync(payment => payment.Id == paymentId);

            if (paymentEntity == null)
                return PaymentStatus.NotFound;

            while (!cancellationToken.IsCancellationRequested)
            {
                paymentEntity = await _context.Payments.AsNoTracking().FirstOrDefaultAsync(payment => payment.Id == paymentId);

                if (paymentEntity != null && paymentEntity.Status == "Cancelled")
                {
                    return PaymentStatus.Cancelled;
                }

                else if (paymentEntity.Status == "Completed")
                { 
                    return PaymentStatus.Success;
                }

                await Task.Delay(2000, cancellationToken.Token);
            }

            return PaymentStatus.Error;
        }
    }
}
