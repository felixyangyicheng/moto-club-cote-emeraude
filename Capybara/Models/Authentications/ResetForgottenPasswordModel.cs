namespace Capybara.Models.Authentications
{
    public class ResetForgottenPasswordModel : ForgotPasswordModel
    {
        [Required]
        public string Token { get; set; } = "";
        [Required]
        public string Password { get; set; } = "";
        [Required]
        public string PasswordConfirm { get; set; } = "";
    }
}
