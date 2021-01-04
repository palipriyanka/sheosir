using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Management.Smo;

namespace NewCore3xMVCAuth.Areas.Identity.Pages.Account
{
    public class SetUserRoleModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public SetUserRoleModel(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void OnGet()
        {
            GetUsers();
            GetRoles();
        }

        private void GetUsers()
        {
            var users = _userManager.Users.ToList();
            ViewData["Users"] = users;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var roleId = Request.Form["SelectedRole"].ToString();
            var user = Request.Form["SelectedUser"].ToString();


            var thisUser = await _userManager.FindByIdAsync(user);

            await _userManager.AddToRoleAsync(thisUser, roleId);

            GetRoles();
            GetUsers();

            ViewData["Success"] = "Done";
            return Page();
        }

        private void GetRoles()
        {
            var roles = _roleManager.Roles.ToList();
            ViewData["Roles"] = roles;
        }

    }
}
