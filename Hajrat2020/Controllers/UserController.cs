using Hajrat2020.Interfaces;
using Hajrat2020.Models;
using System.Web.Mvc;

namespace Hajrat2020.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public ActionResult GetUsers(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.StartRowIndex = ((pageNumber - 1) * pageSize) + 1;
            var usersViewModel = _userService.GetUsers(page);
            return View(usersViewModel);
        }

        [Authorize(Roles = RoleName.SuperAdminOrAdmin)]
        public ActionResult DeActivateUser(string id)
        {
            _userService.DeActivateUser(id);
            return RedirectToAction("GetUsers");
        }

        [Authorize(Roles = RoleName.SuperAdminOrAdmin)]
        public ActionResult AddUser()
        {
            var userViewModel = _userService.AddUser();
            return View("AddEditUser", userViewModel);
        }

        public ActionResult EditUser(string id)
        {
            var userViewModel = _userService.EditUser(id);
            return View("AddEditUser", userViewModel);
        }

        public ActionResult GetUser(string id)
        {
            var userViewModel = _userService.GetUser(id);
            return PartialView("_GetUser", userViewModel);
        }

    }
}