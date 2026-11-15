using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoleBasedEmployeeApp.Models;

namespace RoleBasedEmployeeApp.Controllers
{
    [Authorize]
    public abstract class PermissionControllerBase : Controller
    {
        private readonly MenuDataAccessLayer menuDal = new();

        // each controller says which Menus.Url it represents
        protected abstract string MenuUrl { get; }

        protected Permission GetPermission()
        {
            var roleIdValue = User.FindFirst("RoleId")?.Value;

            if (!int.TryParse(roleIdValue, out int roleId))
                return new Permission();

            return menuDal.GetPermission(roleId, MenuUrl);
        }

        protected IActionResult Denied() => RedirectToAction("AccessDenied", "Account");
    }
}