namespace Capybara.Models.Authentications
{
    public class RegisterModel:BaseUserModel
    {
        public string? Avatar { get; set; } = "";
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";
        [Required]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; } = "";
    }
}
