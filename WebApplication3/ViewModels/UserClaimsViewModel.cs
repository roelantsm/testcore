using System.Collections.Generic;
using WebApplication3.Models;

namespace WebApplication3.ViewModels
{
    public class UserClaimsViewModel
    {
        public string UserId { get; set; }
        public List<UserClaim> Claims { get; set; }
        
        
        public UserClaimsViewModel()
        {
            Claims = new List<UserClaim>();
        }
    }
}