namespace TestTask.Requests
{
    public class UpdateUserRequest
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
    }
}
