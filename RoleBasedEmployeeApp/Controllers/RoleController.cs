using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RoleBasedEmployeeApp.Models;

namespace RoleBasedEmployeeApp.Controllers
{
    public class RoleController : PermissionControllerBase
    {
        private readonly AdminDataAccessLayer dal = new();

        protected override string MenuUrl => "/Role";

        // GET needs CanView
        [HttpGet]
        public IActionResult Index(int? roleId)
        {
            var perm = GetPermission();
            if (!perm.CanView) return Denied();

            var roles = dal.GetRoles();
            int selected = roleId ?? roles.First().RoleId;

            ViewBag.Permission = perm;
            ViewBag.Roles = new SelectList(roles, "RoleId", "RoleName", selected);

            return View(new PermissionMatrixViewModel
            {
                RoleId = selected,
                Rows = dal.GetPermissionMatrix(selected)
            });
        }

        // saving needs CanEdit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(PermissionMatrixViewModel model)
        {
            if (!GetPermission().CanEdit) return Denied();

            foreach (var row in model.Rows)
                dal.SavePermission(model.RoleId, row);

            TempData["Msg"] = "Permissions saved.";
            return RedirectToAction(nameof(Index), new { roleId = model.RoleId });
        }
    }
}