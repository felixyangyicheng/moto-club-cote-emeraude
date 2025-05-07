namespace Capybara.Models.Authentications
{
    public class ResetPasswordModel : LoginModel
    {

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = "";
        [Required]
        [DataType(DataType.Password)]
        public string NewPasswordConfirm { get; set; } = "";
    }
}
