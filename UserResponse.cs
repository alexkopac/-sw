namespace Auth.DTO
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
    }
}
