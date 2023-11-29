using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement1.ViewModels;

namespace SchoolManagement1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager,
                                        UserManager<IdentityUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = userManager.Users.ToList();
            return View(users);
        }
        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id {id} not found!";
                return View("NotFound");
            }
            var userRoles = await userManager.GetRolesAsync(user);
            var model = new EditUserViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Roles = userRoles.ToList(),
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id {model.Id} not found!";
                return View("NotFound");
            }
            else
            {
                user.Email = model.Email;
                user.UserName = model.UserName;
                var result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user= await userManager.FindByIdAsync(id);
            if(user == null)
            {
                ViewBag.ErrorMessage = $"User with id {id} not found!";
                return View("NotFound");
            }
            else
            {
                var result=await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View("LsitUsers");
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role= await roleManager.FindByIdAsync(id);
            if(role == null)
            {
                ViewBag.ErrorMessage = $"Role with id {id} not found!";
                return View("NotFound");
            }
            else
            {
                var result=await roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View("LsitRoles");
            }
        }
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model, IdentityRole role)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };
                IdentityResult result = await roleManager.CreateAsync(identityRole);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles.ToList();
            return View(roles);
        }
        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id {id} not found!";
                return View("NotFound");
            }
            var model = new EditRoleViewModel
            {
                Id = id,
                RoleName = role.Name
            };
            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                    model.Users.Add(user.UserName);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id {model.Id} not found!";
                return View("NotFound");
            }
            else
            {
                role.Name = model.RoleName;
                var result = await roleManager.UpdateAsync(role);
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
        [HttpGet]
        public async Task<IActionResult> EditUserInRole(string roleId)
        {
            ViewBag.RoleId = roleId;
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id {roleId} not found!";
                return View("NotFound");
            }
            else
            {
                var model = new List<RoleUserViewModel>();
                foreach (var user in userManager.Users)
                {
                    var roleUserViewModel = new RoleUserViewModel()
                    {
                        UserId = user.Id,
                        UserName = user.UserName
                    };
                    if (await userManager.IsInRoleAsync(user, role.Name))
                    {
                        roleUserViewModel.IsSelected = true;
                    }
                    else
                    {
                        roleUserViewModel.IsSelected = false;
                    }
                    model.Add(roleUserViewModel);
                }
                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditUserInRole(List<RoleUserViewModel> models, string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id {roleId} not found!";
                return View("NotFound");
            }
            else
            {
                foreach (var model in models)
                {
                    var user = await userManager.FindByIdAsync(model.UserId);
                    IdentityResult? result = null;
                    if (model.IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                    {
                        result = await userManager.AddToRoleAsync(user, role.Name);
                    }
                    else if (!model.IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                    {
                        result = await userManager.RemoveFromRoleAsync(user, role.Name);
                    }
                    else
                    {
                        continue;
                    }
                    if (result.Succeeded)
                    {
                        continue;
                    }
                }
                return RedirectToAction("EditRole", new { Id = roleId });
            }
        }
        [HttpGet]
        public async Task<IActionResult> ManageUserRoles(string userId)
        {
            ViewBag.UserId = userId;
            var user= await userManager.FindByIdAsync(userId);
            if(user == null)
            {
                ViewBag.ErrorMessage = $"User with id {userId} not found!";
                return View("NotFound");
            }
            var model = new List<UserRoleViewModel>();
            foreach(var role in roleManager.Roles)
            {
                var userRoleViewModel = new UserRoleViewModel()
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                if(await userManager.IsInRoleAsync(user,role.Name))
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
        public async Task<IActionResult> ManageUserRoles(List<UserRoleViewModel> models, string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id {userId} not found!";
                return View("NotFound");
            }
            var roles = await userManager.GetRolesAsync(user);
            var result = await userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Cannot remove user existing roles");
                return View(models);
            }
            var roleIdsToAdd = models.Where(role => role.IsSelected).Select(role => role.RoleName).ToArray();
            result = await userManager.AddToRolesAsync(user, roleIdsToAdd);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Cannot add selected role to user!");
                return View(models);
            }
            return RedirectToAction("EditUser",new {Id=userId});
        }
    }
}
