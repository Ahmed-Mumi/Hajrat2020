using Hajrat2020.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hajrat2020.ViewModel
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "Email je obavezan")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Ime je obavezno")]
        [Display(Name = "Ime:")]
        [StringLength(50, ErrorMessage = "Ime ne može biti duže od 50 karaktera!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Prezime je obavezno")]
        [Display(Name = "Prezime:")]
        [StringLength(50, ErrorMessage = "Prezime ne može biti duže od 50 karaktera!")]
        public string LastName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Šifra {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Šifra:")]
        public string PasswordHash { get; set; }

        [StringLength(100, ErrorMessage = "Šifra mora imati minimalno {2} karaktera!", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nova šifra:")]
        public string NewPassword { get; set; }

        [Display(Name = "Grad:")]
        [Required(ErrorMessage = "Izaberite grad!")]
        public int CityId { get; set; }

        [Display(Name = "Grad:")]
        public string CityName { get; set; }


        [Display(Name = "Spol:")]
        public string GenderName { get; set; }

        [Display(Name = "Spol:")]
        [Required(ErrorMessage = "Izaberite spol!")]
        public byte GenderId { get; set; }

        [Display(Name = "Datum unosa admina:")]
        public DateTime DateOfAddingAdmin { get; set; }

        [Display(Name = "Telefon:")]
        [Required(ErrorMessage = "Unesite telefon!")]
        [StringLength(50, ErrorMessage = "Telefon ne može biti duži od 50 karaktera!")]
        public string Phone { get; set; }

        public string Id { get; set; }
        public bool Active { get; set; }

        [Display(Name = "Ime i prezime:")]
        public string FullName { get; set; }

        public IPagedList<UserViewModel> Users { get; set; }

        public IEnumerable<FamilyUser> FamilyUsers { get; set; }
        public IEnumerable<Gender> Genders { get; set; }
        public IEnumerable<City> Cities { get; set; }

        public bool IsRoleAdmin { get; set; }
        public bool IsRoleSuperAdmin { get; set; }

        [Display(Name = "Admin:")]
        public bool IsChosenRoleAdmin { get; set; } = false;

        [Display(Name = "Uloga:")]
        public string RoleName { get; set; }

        public string LoggedInUser { get; set; }

        [Display(Name = "Novčano stanje udruženja:")]
        public decimal ReceivedMoneySumKM { get; set; } = 0;
        public decimal ReceivedMoneySumDolar { get; set; } = 0;
        public decimal ReceivedMoneySumEuro { get; set; } = 0;
    }
}