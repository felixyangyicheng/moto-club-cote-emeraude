namespace Capybara.Models.Authentications
{
    public class LoginModel
    {
        [Required]

        [Display(Name = "Email")]
        public string Email { get; set; } = "";
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";
    }
}
