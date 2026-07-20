namespace ChatApp.Domain.Entities
{
    // This service stores the logged-in user's state across different pages
    public class UserState
    {
        public string Username { get; set; } = string.Empty;
        public bool IsLoggedIn => !string.IsNullOrEmpty(Username);

        public void LogIn(string username)
        {
            Username = username;
        }

        public void LogOut()
        {
            Username = string.Empty;
        }
    }
}