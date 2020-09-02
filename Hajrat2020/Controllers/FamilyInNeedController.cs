using Hajrat2020.Interfaces;
using Hajrat2020.ViewModel;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;

namespace Hajrat2020.Controllers
{
    public class FamilyInNeedController : Controller
    {
        private readonly IFamilyInNeedService _familyInNeedService;
        public FamilyInNeedController(IFamilyInNeedService familyInNeedService)
        {
            _familyInNeedService = familyInNeedService;
        }
        public ActionResult GetFamilies(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.StartRowIndex = ((pageNumber - 1) * pageSize) + 1;
            var familiesViewModel = _familyInNeedService.GetFamilies(page);
            return View(familiesViewModel);
        }

        public ActionResult AddFamily()
        {
            var family = _familyInNeedService.AddFamily();
            return View("AddEditFamily", family);
        }

        public ActionResult DeActivateFamily(int id)
        {
            _familyInNeedService.DeActivateFamily(id);
            return RedirectToAction("GetFamilies");
        }

        public ActionResult EditFamily(int id)
        {
            var familyViewModel = _familyInNeedService.EditFamily(id);
            return View("AddEditFamily", familyViewModel);
        }

        [ValidateAntiForgeryToken]
        public ActionResult Save(FamilyViewModel familyViewModel)
        {
            if (!ModelState.IsValid)
            {
                var familyVM = _familyInNeedService.AddFamily();
                return View("AddEditFamily", familyVM);
            }
            _familyInNeedService.SaveFamily(familyViewModel);
            return RedirectToAction("GetFamilies");
        }

        public ActionResult GetFamily(int id)
        {
            var familyViewModel = _familyInNeedService.GetFamily(id);
            return PartialView("_GetFamily", familyViewModel);
        }


        public ActionResult AddFamilyToPrint(int id)
        {
            _familyInNeedService.AddFamilyToPrint(id);
            return RedirectToAction("GetFamilies");
        }

        public ActionResult RemoveAllFamilyToPrint()
        {
            _familyInNeedService.RemoveAllFamilyToPrint();
            return RedirectToAction("EditUser", "User", new { id = User.Identity.GetUserId() });
        }

        public ActionResult RemoveFamilyToPrint(int id)
        {
            _familyInNeedService.RemoveFamilyToPrint(id);
            return RedirectToAction("EditUser", "User", new { id = User.Identity.GetUserId() });
        }
    }
}