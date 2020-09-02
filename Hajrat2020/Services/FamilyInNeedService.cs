using Hajrat2020.Interfaces;
using Hajrat2020.Models;
using Hajrat2020.ViewModel;
using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Hajrat2020.Services
{
    public class FamilyInNeedService : IFamilyInNeedService
    {
        ApplicationDbContext ctx = new ApplicationDbContext();
        private readonly System.Security.Principal.IPrincipal _loggedInUser = HttpContext.Current.User;
        private readonly string _loggedInUserId = HttpContext.Current.User.Identity.GetUserId();

        public FamilyViewModel GetFamilies(int? page)
        {
            var isAdmin = !_loggedInUser.IsInRole(RoleName.User);
            var families = ctx.FamilyInNeeds.Include(c => c.City)
                                .Include(u => u.ApplicationUser)
                                .Where(x => ((x.ApplicationUserId == _loggedInUserId || isAdmin) ? true : x.IsActive)
                                && !x.IsHajrat)
                                .OrderByDescending(x => x.IsUrgent)
                                .ToList();
            var donations = ctx.Donations.OrderByDescending(x => x.DateOfDonation);
            foreach (var item in families)
            {
                var donation = donations.Where(x => x.FamilyInNeedId == item.Id).FirstOrDefault() ?? null;
                item.DateOfLastHelp = donation == null ? (DateTime?)null : donation.DateOfDonation;
            }
            var familyViewModel = AutoMapper.Mapper.Map<IEnumerable<FamilyViewModel>>(families
                                                                    .OrderBy(x => x.DateOfLastHelp));
            var pageSize = 10;
            var familiesViewModel = new FamilyViewModel
            {
                FamiliesInNeed = familyViewModel.ToPagedList(page ?? 1, pageSize),
                LoggedInUser = _loggedInUserId,
                IsAdmin = isAdmin,
            };
            return familiesViewModel;
        }

        public FamilyViewModel GetFamily(int id)
        {
            if (id > 0)
            {
                var familyViewModel = AutoMapper.Mapper.Map<FamilyViewModel>(
                                                                    ctx.FamilyInNeeds
                                                                    .Include(c => c.City)
                                                                    .Include(u => u.ApplicationUser)
                                                                    .Where(x => x.Id == id).FirstOrDefault());
                var donation = ctx.Donations.Where(x => x.FamilyInNeedId == familyViewModel.Id)
                    .OrderByDescending(x => x.DateOfDonation).FirstOrDefault() ?? null;
                familyViewModel.DateOfLastHelp = donation == null ? (DateTime?)null : donation.DateOfDonation;


                return familyViewModel;
            }
            return null;

        }

        public FamilyViewModel AddFamily()
        {
            var family = new FamilyViewModel()
            {
                ApplicationUserId = _loggedInUserId,
                Cities = ctx.Cities.OrderBy(x => x.Name).ToList(),
            };
            return family;
        }

        public void DeActivateFamily(int id)
        {
            if (id > 0)
            {
                var family = ctx.FamilyInNeeds.Where(x => x.Id == id).FirstOrDefault();
                if (family.IsActive)
                {
                    family.IsActive = false;
                }
                else
                {
                    family.IsActive = true;
                }
                ctx.SaveChanges();
            }
        }

        public FamilyViewModel EditFamily(int id)
        {
            if (id > 0)
            {
                var family = ctx.FamilyInNeeds.Where(x => x.Id == id).FirstOrDefault();
                var familyViewModel = AutoMapper.Mapper.Map<FamilyViewModel>(family);
                familyViewModel.Cities = ctx.Cities.OrderBy(x => x.Name).ToList();
                familyViewModel.DateOfLastUpdate = DateTime.Now;
                return familyViewModel;
            }
            return null;
        }

        public void SaveFamily(FamilyViewModel familyViewModel)
        {
            if (String.IsNullOrWhiteSpace(familyViewModel.ContactPersonName) ||
                String.IsNullOrWhiteSpace(familyViewModel.ContactPersonPhone))
            {
                var user = ctx.Users.Where(x => x.Id == familyViewModel.ApplicationUserId).FirstOrDefault();
                familyViewModel.ContactPersonPhone = user.Phone;
                familyViewModel.ContactPersonName = user.FullName;
            }
            var family = new FamilyInNeed();
            if (familyViewModel.Id > 0)
            {
                family = ctx.FamilyInNeeds.Where(x => x.Id == familyViewModel.Id).FirstOrDefault();
            }
            else
            {
                ctx.FamilyInNeeds.Add(family);
            }
            AutoMapper.Mapper.Map(familyViewModel, family);
            ctx.SaveChanges();
        }

        public void AddFamilyToPrint(int id)
        {
            if (id > 0)
            {
                var familyUser = ctx.FamilyUsers.Where(x => x.FamilyInNeedId == id && x.UserId == _loggedInUserId).FirstOrDefault();
                if (familyUser != null)
                    return;
                var family = new FamilyUser()
                {
                    UserId = _loggedInUserId,
                    FamilyInNeedId = id
                };
                ctx.FamilyUsers.Add(family);
                ctx.SaveChanges();
            }
        }

        public void RemoveAllFamilyToPrint()
        {
            ctx.FamilyUsers.RemoveRange(ctx.FamilyUsers.Where(x => x.UserId == _loggedInUserId).ToList());
            ctx.SaveChanges();
        }

        public void RemoveFamilyToPrint(int id)
        {
            if (id > 0)
            {
                ctx.FamilyUsers.Remove(ctx.FamilyUsers
                    .Where(x => x.FamilyInNeedId == id && x.UserId == _loggedInUserId).FirstOrDefault());
                ctx.SaveChanges();
            }
        }
    }
}