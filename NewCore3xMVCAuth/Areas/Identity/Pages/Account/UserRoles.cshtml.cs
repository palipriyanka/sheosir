using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace NewCore3xMVCAuth.Areas.Identity.Pages.Account
{
    public class UserRolesModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserRolesModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGet(string id)
        {
            var thisUser = await _userManager.FindByIdAsync(id);
            ViewData["ThisUser"] = thisUser;
            var roles = await _userManager.GetRolesAsync(thisUser);
            ViewData["Roles"] = roles;
            return Page();
        }
    }
}
