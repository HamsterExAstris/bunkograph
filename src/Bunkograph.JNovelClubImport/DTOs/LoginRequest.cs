namespace Bunkograph.JNovelClubImport.DTOs
{
    internal class LoginRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }

        /// <summary>
        /// Suppress HTTP cookies from being included in the response.
        /// </summary>
        public bool Slim => true;

        public LoginRequest(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }
}
