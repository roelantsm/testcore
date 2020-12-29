using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.Models;
using WebApplication3.ViewModels;

namespace WebApplication3.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> _rolemanager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdministrationController(RoleManager<IdentityRole> rolemanager,
                UserManager<ApplicationUser> userManager)
        {
            _rolemanager = rolemanager;
            _userManager = userManager;
        }
        
        // GET
         [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        
        // GET
        [HttpPost]
        public async Task <IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole()
                {
                    Name = model.RoleName
                };
                
                IdentityResult result = await _rolemanager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("index", "home");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            
            return View(model);
        }
        
        
        // GET
        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = _rolemanager.Roles;
            return View(roles);
        }
        
        // GET
        [HttpGet]
        public async Task<IActionResult>  EditRole(string id)
        {
           var role = await _rolemanager.FindByIdAsync(id);

           if (role == null)
           {
               ViewBag.ErrorMessage = $"Role with Id = {id} cannot be not found";
               // navigate to not found
               return View("../Home/NotFound");
           }

           var model = new EditRoleViewModel
           {
               Id = role.Id,
               RoleName = role.Name,

           };

           // foreach (var user  in  _userManager.Users.ToList())
           // {
           //     if (await _userManager.IsInRoleAsync(user, role.Name))
           //     {
           //         model.Users.Add(user.UserName);
           //     }
           // }
           
           foreach(var user in await _userManager.GetUsersInRoleAsync(role.Name))
           {
               model.Users.Add(user.UserName);
           }

           return View(model);
        }

        
        // Post
        [HttpPost]
        public async Task<IActionResult>  EditRole(EditRoleViewModel model)
        {
            var role = await _rolemanager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {model.Id} cannot be not found";
                // navigate to not found
                return View("../Home/NotFound");
            }
            else
            {
                role.Name = model.RoleName;
                var result = await _rolemanager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                
                return View(model);
                
            }
        }
        
    }
}