using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.ViewModels
{
    public class EditUserViewModel
    {

        public EditUserViewModel()
        {
            Claims = new List<string>();
            Roles = new List<string>();
        }
        public string Id { get; set; }

        [Required]
        [EmailAddress]
        public String Email { get; set; }
        
        [Required]
        public String Username { get; set; }

        public String City { get; set; }

        public List<string> Claims { get; set; }
        
        public List<string> Roles { get; set; }
    }
}