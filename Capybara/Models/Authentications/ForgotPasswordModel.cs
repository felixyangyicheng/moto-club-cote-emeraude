
namespace Capybara.Models.Authentications
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Adresse Email")]
        public string Email { get; set; } = "";
    }
}
