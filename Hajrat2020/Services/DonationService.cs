using Hajrat2020.Interfaces;
using Hajrat2020.Models;
using Hajrat2020.ViewModel;
using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;

namespace Hajrat2020.Services
{
    public class DonationService : IDonationService
    {
        public ApplicationDbContext ctx = new ApplicationDbContext();
        private readonly System.Security.Principal.IPrincipal _loggedInUser = HttpContext.Current.User;
        private readonly string _loggedInUserId = HttpContext.Current.User.Identity.GetUserId();
        public DonationViewModel GetDonations(int? TypeOfHelpId, int? FamilyInNeedId, int? page, bool all)
        {
            var donations = ctx.Donations
               .Include(t => t.TypeOfHelp)
               .Include(u => u.ApplicationUser)
               .Include(f => f.FamilyInNeed)
               .OrderByDescending(x => x.DateOfDonation);
            var donationViewModel = AutoMapper.Mapper.Map<IEnumerable<DonationViewModel>>(donations);
            var pagedDonations = donationViewModel
                .Where(x => (TypeOfHelpId != null ? x.TypeOfHelpId == TypeOfHelpId : true)
                        && (FamilyInNeedId != null ? x.FamilyInNeedId == FamilyInNeedId : true));
            var pageSize = all == false ? 10 : donationViewModel.Count();
            var donationsViewModel = new DonationViewModel
            {
                Donations = all == false ? pagedDonations.ToPagedList(page ?? 1, pageSize)
                    : pagedDonations.ToPagedList(1, pageSize),
                LoggedInUser = _loggedInUserId,
                IsAdmin = !_loggedInUser.IsInRole(RoleName.User),
                TypesOfHelp = ctx.TypeOfHelps.OrderBy(x => x.Name),
                FamiliesInNeed = ctx.FamilyInNeeds.OrderBy(x => x.FullName),
                SearchTypeId = TypeOfHelpId != null ? TypeOfHelpId : null,
                SearchFamilyId = FamilyInNeedId != null ? FamilyInNeedId : null
            };
            return donationsViewModel;
        }

        public DonationViewModel GetDonation(int id)
        {
            if (id > 0)
            {
                var donationViewModel = AutoMapper.Mapper.Map<DonationViewModel>(ctx.Donations.Where(x => x.Id == id)
                             .Include(t => t.TypeOfHelp)
                             .Include(u => u.ApplicationUser)
                             .Include(f => f.FamilyInNeed)
                             .Include(c => c.FamilyInNeed.City)
                             .Include(cu => cu.Currency)
                             .FirstOrDefault());
                return donationViewModel;
            }
            return null;
        }

        public DonationViewModel AddDonation()
        {
            var donation = new DonationViewModel()
            {
                ApplicationUserId = _loggedInUserId,
                TypesOfHelp = ctx.TypeOfHelps.OrderBy(x => x.Name),
                FamiliesInNeed = ctx.FamilyInNeeds.OrderBy(x => x.FirstName).Where(x => x.IsActive),
                Currencies = ctx.Currencies
            };
            return donation;
        }

        public DonationViewModel EditDonation(int id)
        {
            if (id > 0)
            {
                var donation = ctx.Donations.Where(x => x.Id == id).FirstOrDefault();
                if (donation != null)
                {
                    var donationViewModel = AutoMapper.Mapper.Map<DonationViewModel>(donation);
                    donationViewModel.TypesOfHelp = ctx.TypeOfHelps.OrderBy(x => x.Name);
                    donationViewModel.FamiliesInNeed = ctx.FamilyInNeeds.OrderBy(x => x.FirstName).Where(x => x.IsActive);
                    donationViewModel.DateOfLastUpdate = DateTime.Now;
                    donationViewModel.Currencies = ctx.Currencies;
                    return donationViewModel;
                }
            }
            return null;
        }

        public void DeleteDonation(int id)
        {
            if (id > 0)
            {
                var donation = ctx.Donations.Where(x => x.Id == id).FirstOrDefault();
                var family = ctx.FamilyInNeeds.Where(x => x.Id == donation.FamilyInNeedId).FirstOrDefault();
                if (family != null && family.NumberOfHelpsSoFar > 0)
                {
                    family.NumberOfHelpsSoFar--;
                }
                ctx.Donations.Remove(donation);
                ctx.SaveChanges();
            }
        }

        public void SaveDonation(DonationViewModel donationViewModel)
        {
            if (donationViewModel.ImageUpload != null)
            {
                donationViewModel = AddImage(donationViewModel);
            }
            var donation = new Donation();
            if (donationViewModel.Id > 0)
            {
                donation = ctx.Donations.Include(x=>x.FamilyInNeed)
                    .Where(x => x.Id == donationViewModel.Id).FirstOrDefault();
                ChangeNumberOfHelps(donation, donationViewModel);
            }
            else
            {
                var family = ctx.FamilyInNeeds.Where(x => x.Id == donationViewModel.FamilyInNeedId).FirstOrDefault();
                family.NumberOfHelpsSoFar++;
                family.DateOfLastHelp = DateTime.Now;
                ctx.Donations.Add(donation);
            }
            AutoMapper.Mapper.Map(donationViewModel, donation);
            ctx.SaveChanges();
        }

        public void ChangeNumberOfHelps(Donation donation, DonationViewModel donationViewModel)
        {
            if (donation.FamilyInNeedId != donationViewModel.FamilyInNeedId)
            {
                if (donation.FamilyInNeed.NumberOfHelpsSoFar > 0)
                {
                    var familyToDecreaseHelps = ctx.FamilyInNeeds.Where(x => x.Id == donation.FamilyInNeedId).FirstOrDefault();
                    familyToDecreaseHelps.NumberOfHelpsSoFar--;
                }
                var familyToIncreaseHelps = ctx.FamilyInNeeds.Where(x => x.Id == donationViewModel.FamilyInNeedId).FirstOrDefault();
                familyToIncreaseHelps.NumberOfHelpsSoFar++;
            }
        }

        public DonationViewModel AddImage(DonationViewModel donationViewModel)
        {
            string fileName = Path.GetFileNameWithoutExtension(donationViewModel.ImageUpload.FileName);
            string extenstion = Path.GetExtension(donationViewModel.ImageUpload.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extenstion;
            donationViewModel.Image = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) +
             (MagicNumbers.ImagePath + fileName);
            donationViewModel.ImageUpload.SaveAs(Path.Combine(HttpContext.Current.
                Server.MapPath("~" + MagicNumbers.ImagePath), fileName));
            return donationViewModel;
        }
    }
}