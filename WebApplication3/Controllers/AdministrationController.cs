using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.Models;
using WebApplication3.ViewModels;

namespace WebApplication3.Controllers
{
   // [Authorize(Roles = "admin, Onbestaand")]
    
    // [Authorize(Roles = "admin")]
    // [Authorize(Roles = "Onbestaand")]
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

        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id = {id} cannot be found";
                return View("../Home/NotFound");
            }
            else
            {
                var result = _userManager.DeleteAsync(user);

                if (result.Result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }

                foreach (var error in result.Result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View("ListUsers");
            }
        }


        [HttpPost]
        [Authorize(Policy = "DeleteRolePolicy" )]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _rolemanager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id = {id} cannot be found";
                return View("../Home/NotFound");
            }
            else
            {
                try
                {
                    var result = _rolemanager.DeleteAsync(role);

                    if (result.Result.Succeeded)
                    {
                        return RedirectToAction("ListUsers");
                    }

                    foreach (var error in result.Result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View("ListRoles");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    ViewBag.ErrorMessage = "test";
                    return View("Error");
                }
            }
        }


        // GET
        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = _userManager.Users;
            return View(users);
        }

        // GET
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // GET
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        // GET
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
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
                    return RedirectToAction("ListRoles", "Administration");
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
        public async Task<IActionResult> EditRole(string id)
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

            foreach (var user in await _userManager.GetUsersInRoleAsync(role.Name))
            {
                model.Users.Add(user.UserName);
            }

            return View(model);
        }


        // Post
        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
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

        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;

            var role = await _rolemanager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be not found";
                // navigate to not found
                return View("../Home/NotFound");
            }

            var model = new List<UserRoleViewModel>();

            foreach (var user in _userManager.Users.ToList())
            {
                var userRoleViewModel = new UserRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }

                model.Add(userRoleViewModel);
            }

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            var role = await _rolemanager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be not found";
                // navigate to not found
                return View("../Home/NotFound");
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(model[i].UserId);

                IdentityResult result = null;

                if (model[i].IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await _userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                    {
                        continue;
                    }

                    else
                    {
                        return RedirectToAction("EditRole", new {Id = roleId});
                    }
                }
            }

            return RedirectToAction("EditRole", new {Id = roleId});
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id = {id} cannot be found";
                return View("../Home/NotFound");
            }

            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);

            var model = new EditUserViewModel
            {
                City = user.City,
                Username = user.UserName,
                Email = user.Email,
                Id = user.Id,
                Claims = userClaims.Select(c => c.Value).ToList(),
                Roles = userRoles.ToList()

            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id = {model.Id} cannot be found";
                return View("../Home/NotFound");
            }
            else
            {
                user.Email = model.Email;
                user.UserName = model.Username;
                user.City = model.City;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
        }



        [HttpGet]
        public async Task<IActionResult> ManageUserRoles(string userId)
        {
            ViewBag.userId = userId;

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }

            var model = new List<UserRolesViewModel>();

            foreach (var role in _rolemanager.Roles.ToList())
            {
                var userRolesViewModel = new UserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.IsSelected = true;
                }
                else
                {
                    userRolesViewModel.IsSelected = false;
                }
                model.Add(userRolesViewModel);
            }
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> ManageUserRoles(List<UserRolesViewModel> model, string userId)
        {

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }

            result = await _userManager.AddToRolesAsync(user,
                model.Where(x => x.IsSelected).Select( y => y.RoleName));
            
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }

            return RedirectToAction("EditUser", new {Id = userId});
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserClaims(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"user with Id = {userId} not found";
                return View("NotFound");
            }

            var existingUserClaims = await _userManager.GetClaimsAsync(user);

            var model = new UserClaimsViewModel
            {
                UserId = userId
            };

            foreach (Claim claim in ClaimsStore.AllClaims)
            {
                UserClaim userClaim = new UserClaim()
                {
                    ClaimType = claim.Type
                };

                if (existingUserClaims.Any(c => c.Type == claim.Type))
                {
                    userClaim.IsSelected = true;
                }
                
                model.Claims.Add(userClaim);
            }

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> ManageUserClaims(UserClaimsViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"user with Id = {model.UserId} not found";
                return View("NotFound");
            }

            var claims = await _userManager.GetClaimsAsync(user);
            var result = await _userManager.RemoveClaimsAsync(user, claims);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing claims");
                return View(model);
            }

            result = await _userManager.AddClaimsAsync(user,
                model.Claims.Where(c => c.IsSelected).Select(c => new Claim(c.ClaimType, c.ClaimType)));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected claims to user");
                return View(model);
            }

            return RedirectToAction("EditUser", new {Id = model.UserId});
        }
    }
}