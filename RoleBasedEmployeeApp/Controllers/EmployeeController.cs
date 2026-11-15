using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoleBasedEmployeeApp.Models;

namespace RoleBasedEmployeeApp.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly EmployeeDataAccessLayer dal = new();
        private readonly MenuDataAccessLayer menuDal = new();
        private const string MenuUrl = "/Employee";

        // Reads the logged-in user's role from the cookie, then asks the database
        private Permission GetPermission()
        {
            var roleIdValue = User.FindFirst("RoleId")?.Value;

            if (!int.TryParse(roleIdValue, out int roleId))
                return new Permission();

            return menuDal.GetPermission(roleId, MenuUrl);
        }

        private IActionResult Denied() => RedirectToAction("AccessDenied", "Account");

        // NEW: fills the department dropdown used by the Create/Edit form
        private void LoadDepartments() => ViewBag.Departments = dal.GetDepartments();

        public IActionResult Index()
        {
            var perm = GetPermission();
            if (!perm.CanView) return Denied();

            ViewBag.Permission = perm;
            return View(dal.GetAllEmployees());
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!GetPermission().CanAdd) return Denied();

            LoadDepartments();                      // NEW
            return View(new Employee());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employee emp)
        {
            if (!GetPermission().CanAdd) return Denied();

            if (!ModelState.IsValid)
            {
                LoadDepartments();                  // NEW
                return View(emp);
            }

            dal.AddEmployee(emp);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (!GetPermission().CanEdit) return Denied();

            var emp = dal.GetEmployeeById(id);
            if (emp == null) return NotFound();

            LoadDepartments();                      // NEW
            return View(emp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Employee emp)
        {
            if (!GetPermission().CanEdit) return Denied();

            if (!ModelState.IsValid)
            {
                LoadDepartments();                  // NEW
                return View(emp);
            }

            dal.UpdateEmployee(emp);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            if (!GetPermission().CanDelete) return Denied();

            dal.DeleteEmployee(id);
            return RedirectToAction(nameof(Index));
        }
    }
}