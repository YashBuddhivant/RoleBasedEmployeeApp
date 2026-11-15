using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using RoleBasedEmployeeApp.Models;

namespace RoleBasedEmployeeApp.Controllers
{
    public class UserController : PermissionControllerBase
    {
        private readonly AdminDataAccessLayer dal = new();

        protected override string MenuUrl => "/User";

        private void LoadRoles() =>
            ViewBag.Roles = new SelectList(dal.GetRoles(), "RoleId", "RoleName");

        public IActionResult Index()
        {
            var perm = GetPermission();
            if (!perm.CanView) return Denied();

            ViewBag.Permission = perm;
            return View(dal.GetUsers());
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!GetPermission().CanAdd) return Denied();

            LoadRoles();
            return View(new CreateUserViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateUserViewModel model)
        {
            if (!GetPermission().CanAdd) return Denied();

            if (ModelState.IsValid)
            {
                try
                {
                    string hash = BCrypt.Net.BCrypt.HashPassword(model.Password);
                    dal.AddUser(model.Username, model.FullName, hash, model.RoleId);
                    return RedirectToAction(nameof(Index));
                }
                catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
                {
                    // UNIQUE constraint on Users.Username
                    ModelState.AddModelError(nameof(model.Username), "That username is already taken.");
                }
            }

            LoadRoles();
            return View(model);
        }
    }
}