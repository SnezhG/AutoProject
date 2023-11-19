namespace UserService.Services
{
    public class UserManagerResponce
    {
        public string Message { get; set; }
        
        public string Role { get; set; }
        public string Token { get; set; }

        public bool IsSuccess { get; set; }

        public IEnumerable<string> Errors { get; set; }

        public DateTime? ExpireDate { get; set; }
    }
}
