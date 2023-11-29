using System.ComponentModel.DataAnnotations;

namespace SchoolManagement1.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required]
        public string RoleName{ get; set; }
    }
}
