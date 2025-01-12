﻿using System.ComponentModel.DataAnnotations;

namespace SchoolManagement1.ViewModels
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            Roles= new List<string>();
        }
        public string Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public List<string> Roles { get; set; }
    }
}
