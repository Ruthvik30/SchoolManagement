using SchoolManagement1.Data.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement1.ViewModels
{
    public class RegisterStudent
    {
        [Required]
        [Display(Name ="First name")]
        public string FirstName{ get; set; }
        [Required]
        [Display(Name = "Last name")]
        public string LastName{ get; set; }
        [Required]
        [Display(Name = "Father")]
        public string FatherName{ get; set; }
        [Required]
        public  string Phone{ get; set; }
        [Required]
        public  string Address{ get; set; }
        public  Gender Gender { get; set; }
        public Standard Standard { get; set; }
        public DateTime DateOfBirth{ get; set; }
    }
}
