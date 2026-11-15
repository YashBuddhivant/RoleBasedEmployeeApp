using Microsoft.AspNetCore.Mvc;

namespace RoleBasedEmployeeApp.ViewComponents
{
    public class SidebarViewComponent : ViewComponent
    {
        private readonly MenuDataAccessLayer dal = new();

        public IViewComponentResult Invoke()
        {
            var roleIdValue = UserClaimsPrincipal.FindFirst("RoleId")?.Value;

            if (!int.TryParse(roleIdValue, out int roleId))
                return Content("");

            var menus = dal.GetMenusByRole(roleId);
            return View(menus);
        }
    }
}