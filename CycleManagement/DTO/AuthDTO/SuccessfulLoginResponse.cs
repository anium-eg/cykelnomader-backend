using CycleManagement.Models;

namespace CycleManagement.DTO.AuthDTO
{
    public class SuccessfulLoginResponse
    {
        public string Token { get; set; }
        public Employee Employee { get; set; }
    }
}
