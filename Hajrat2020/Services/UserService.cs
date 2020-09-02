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
    public class UserService : IUserService
    {
        ApplicationDbContext ctx = new ApplicationDbContext();
        private readonly System.Security.Principal.IPrincipal _loggedInUser = HttpContext.Current.User;
        private readonly string _loggedInUserId = HttpContext.Current.User.Identity.GetUserId();

        public UserViewModel GetUsers(int? page)
        {
            IEnumerable<ApplicationUser> users = new List<ApplicationUser>();
            users = PopulateUserList(users);
            var userViewModel = AutoMapper.Mapper.Map<IEnumerable<UserViewModel>>(users.OrderBy(x => x.FirstName));
            var pageSize = 10;
            var usersViewModel = new UserViewModel
            {
                Users = userViewModel.ToPagedList(page ?? 1, pageSize),
                IsRoleAdmin = !_loggedInUser.IsInRole(RoleName.User),
                LoggedInUser = _loggedInUserId
            };

            return usersViewModel;
        }

        public IEnumerable<ApplicationUser> PopulateUserList(IEnumerable<ApplicationUser> users)
        {
            if (_loggedInUser.IsInRole(RoleName.SuperAdmin))
            {
                users = ctx.Users;
            }
            else
            {
                var superAdminRoleId = ctx.Roles.Where(x => x.Name == RoleName.SuperAdmin).FirstOrDefault();
                if (_loggedInUser.IsInRole(RoleName.Admin))
                {
                    users = ctx.Users.Where(x => x.Roles
                        .Any(n => n.RoleId != superAdminRoleId.Id));
                }
                else if (_loggedInUser.IsInRole(RoleName.User))
                {

                    users = ctx.Users.Where(x => x.Roles
                        .Any(n => n.RoleId != superAdminRoleId.Id) && x.Active);
                }
            }
            return users;
        }

        public UserViewModel GetUser(string id)
        {
            if (!String.IsNullOrWhiteSpace(id))
            {
                var userViewModel = AutoMapper.Mapper.Map<UserViewModel>(ctx.Users
                                            .Include(x => x.City)
                                            .Include(x => x.Gender)
                                            .Where(x => x.Id == id).FirstOrDefault());
                return userViewModel;
            }
            return null;
        }

        public void DeActivateUser(string id)
        {
            if (!String.IsNullOrWhiteSpace(id))
            {
                var user = ctx.Users.Where(x => x.Id == id).FirstOrDefault();
                if (user.Active)
                {
                    user.Active = false;
                }
                else
                {
                    user.Active = true;
                }
                ctx.SaveChanges();
            }
        }

        public UserViewModel AddUser()
        {
            var userViewModel = new UserViewModel()
            {
                Genders = ctx.Genders.OrderBy(x => x.Name),
                Cities = ctx.Cities.OrderBy(x => x.Name),
                PasswordHash = GenerateRandomPassword(),
                IsRoleAdmin = !_loggedInUser.IsInRole(RoleName.User)
            };
            return userViewModel;
        }

        public UserViewModel EditUser(string id)
        {
            if (!String.IsNullOrWhiteSpace(id))
            {
                var user = ctx.Users.Where(x => x.Id == id).FirstOrDefault();
                var userViewModel = AutoMapper.Mapper.Map<UserViewModel>(user);
                userViewModel.Cities = ctx.Cities.OrderBy(x => x.Name);
                userViewModel.Genders = ctx.Genders.OrderBy(x => x.Name);
                userViewModel.IsRoleSuperAdmin = _loggedInUser.IsInRole(RoleName.SuperAdmin);
                userViewModel.IsRoleAdmin = !_loggedInUser.IsInRole(RoleName.User);
                userViewModel.IsChosenRoleAdmin = userViewModel.RoleName != RoleName.User;
                userViewModel.LoggedInUser = _loggedInUserId;
                userViewModel.FamilyUsers = ctx.FamilyUsers.Include(x => x.FamilyInNeed)
                    .Where(x => x.UserId == _loggedInUserId);
                userViewModel = ReceivedMoney(userViewModel);
                return userViewModel;
            }
            return null;
        }

        public UserViewModel ReceivedMoney(UserViewModel model)
        {
            var receivedSum = ctx.Donations.Where(x => x.FamilyInNeed.IsHajrat);
            model.ReceivedMoneySumKM = receivedSum.Where(x => x.CurrencyId == MagicNumbers.BAM).Select(x => (decimal)x.AmountOfMoney).DefaultIfEmpty().Sum();
            model.ReceivedMoneySumEuro = receivedSum.Where(x => x.CurrencyId == MagicNumbers.Euro).Select(x => (decimal)x.AmountOfMoney).DefaultIfEmpty().Sum();
            model.ReceivedMoneySumDolar = receivedSum.Where(x => x.CurrencyId == MagicNumbers.Dolar).Select(x => (decimal)x.AmountOfMoney).DefaultIfEmpty().Sum();
            return model;
        }

        public static string GenerateRandomPassword()
        {
            var requiredLength = 12;
            var requiredUniqueChars = 4;
            var requireDigit = true;
            var requireLowercase = true;
            var requireNonAlphanumeric = true;
            var requireUppercase = true;
            string[] randomChars = new[] {
            "ABCDEFGHJKLMNOPQRSTUVWXYZ",    // uppercase 
            "abcdefghijkmnopqrstuvwxyz",    // lowercase
            "0123456789",                   // digits
            "#%@$?_-"                        // non-alphanumeric
        };

            Random rand = new Random(Environment.TickCount);
            List<char> chars = new List<char>();

            if (requireUppercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[0][rand.Next(0, randomChars[0].Length)]);

            if (requireLowercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[1][rand.Next(0, randomChars[1].Length)]);

            if (requireDigit)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[2][rand.Next(0, randomChars[2].Length)]);

            if (requireNonAlphanumeric)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[3][rand.Next(0, randomChars[3].Length)]);

            for (int i = chars.Count; i < requiredLength
                || chars.Distinct().Count() < requiredUniqueChars; i++)
            {
                string rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count),
                    rcs[rand.Next(0, rcs.Length)]);
            }

            return new string(chars.ToArray());
        }
    }
}