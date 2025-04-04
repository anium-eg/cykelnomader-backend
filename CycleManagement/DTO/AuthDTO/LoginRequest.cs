namespace CycleManagement.DTO.AuthDTO
{
    public class LoginRequest
    {
        public string LoginId { get; set; }
        public string PlainTextPassword { get; set; }
        public string RoleClaim { get; set; }
    }
}
