using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement1.ViewModels
{
    public class RegistrationViewModel
    {
        [Required]
        public string UserName{ get; set; }
        [Required]
        [EmailAddress]
        [Remote(action:"IsEmailInUse",controller:"Account")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Remote(action: "IsUserNameInUse", controller: "Account")]
        public string Password{ get; set; }
        [DataType(DataType.Password)]
        [Display(Name ="Confirm password")]
        [Compare("Password",ErrorMessage ="password and confirm password must match")]
        public string ConfirmPassword { get; set; }
    }
}
