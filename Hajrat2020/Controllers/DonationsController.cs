using Hajrat2020.Interfaces;
using Hajrat2020.ViewModel;
using System.Web.Mvc;

namespace Hajrat2020.Controllers
{
    public class DonationsController : Controller
    {
        private readonly IDonationService _donationService;
        public DonationsController(IDonationService donationService)
        {
            _donationService = donationService;
        }

        public ActionResult GetDonations(int? TypeOfHelpId, int? FamilyInNeedId, int? page, bool all = false)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.StartRowIndex = ((pageNumber - 1) * pageSize) + 1;
            var donationsViewModel = _donationService.GetDonations(TypeOfHelpId, FamilyInNeedId, page, all);
            return View(donationsViewModel);
        }

        public ActionResult AddDonation()
        {
            var donation = _donationService.AddDonation();
            return View("AddEditDonation", donation);
        }

        public ActionResult EditDonation(int id)
        {
            var donationViewModel = _donationService.EditDonation(id);
            return View("AddEditDonation", donationViewModel);
        }

        public ActionResult DeleteDonation(int id)
        {
            _donationService.DeleteDonation(id);
            return RedirectToAction("GetDonations");
        }

        [ValidateAntiForgeryToken]
        public ActionResult Save(DonationViewModel donationViewModel)
        {
            if (!ModelState.IsValid)
            {
                var donationVM = _donationService.AddDonation();
                return View("AddEditDonation", donationViewModel);
            }
            _donationService.SaveDonation(donationViewModel);
            return RedirectToAction("GetDonations");
        }

        public ActionResult GetDonation(int id)
        {
            var donationViewModel = _donationService.GetDonation(id);
            return PartialView("_GetDonation", donationViewModel);
        }
    }
}