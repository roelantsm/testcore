using System.ComponentModel.DataAnnotations;

namespace WebApplication3.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required]
        public string RoleName { get; set; }  
    }
}