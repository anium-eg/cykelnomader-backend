using CycleManagement.Data;
using CycleManagement.Models;

namespace CycleManagement.Controllers
{
    public class PaymentsController : BaseController<Payment>
    {
        public PaymentsController(ApplicationDbContext context) : base(context)
        {
        }
    }
}
