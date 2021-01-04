using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace NewCore3xMVCAuth.Areas.Identity.Pages.Account
{
    public class AddRoleModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public AddRoleModel(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public void OnGet()
        {
            GetRoles();
        }

        private void GetRoles()
        {
            var roles = _roleManager.Roles.ToList();
            ViewData["Roles"] = roles;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var roleName = Request.Form["RoleName"].ToString();

            bool x = await _roleManager.RoleExistsAsync(roleName);
            if (!x)
            {
                // first we create Admin rool    
                var role = new IdentityRole();
                role.Name = roleName;
                await _roleManager.CreateAsync(role);
            }

            GetRoles();

            return Page();
        }
    }
}
