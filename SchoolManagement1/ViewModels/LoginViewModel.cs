using System.ComponentModel.DataAnnotations;

namespace SchoolManagement1.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName{ get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
