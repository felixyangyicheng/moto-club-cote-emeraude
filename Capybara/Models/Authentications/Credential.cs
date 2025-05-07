namespace Capybara.Models.Authentications
{
    public class Credential
    {
        public String UserId { get; set; } = "";
        public String Email { get; set; } = "";
        public String Name { get; set; } = "";
        public String Avatar { get; set; } = "";
        public bool IsLogged { get; set; } = false;
    }
}
