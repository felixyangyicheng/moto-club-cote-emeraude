namespace Capybara.Models.Authentications
{
    public class BaseUserModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = "";
        [Required]
        public string UserName { get; set; } = "";
    }
}
