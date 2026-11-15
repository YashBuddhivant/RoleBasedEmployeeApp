using Microsoft.AspNetCore.Mvc;
using RoleBasedEmployeeApp.Models;

namespace RoleBasedEmployeeApp.Controllers
{
    public class SettingController : PermissionControllerBase
    {
        private readonly AdminDataAccessLayer dal = new();

        protected override string MenuUrl => "/Setting";

        [HttpGet]
        public IActionResult Index()
        {
            var perm = GetPermission();
            if (!perm.CanView) return Denied();

            ViewBag.Permission = perm;
            return View(dal.GetSettings());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(List<SettingItem> settings)
        {
            if (!GetPermission().CanEdit) return Denied();

            foreach (var s in settings)
                dal.UpdateSetting(s.SettingKey, s.SettingValue ?? "");

            TempData["Msg"] = "Settings saved.";
            return RedirectToAction(nameof(Index));
        }
    }
}