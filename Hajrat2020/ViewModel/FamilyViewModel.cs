using Hajrat2020.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hajrat2020.ViewModel
{
    public class FamilyViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ime je obavezno!")]
        [Display(Name = "Ime:")]
        [StringLength(50, ErrorMessage = "Ime ne može biti duže od 50 karaktera!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Prezime je obavezno!")]
        [Display(Name = "Prezime:")]
        [StringLength(50, ErrorMessage = "Prezime ne može biti duže od 50 karaktera!")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Adresa je obavezna!")]
        [Display(Name = "Adresa:")]
        [StringLength(50, ErrorMessage = "Adresa ne može biti duža od 100 karaktera!")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Telefon je obavezan!")]
        [Display(Name = "Telefon:")]
        [StringLength(50, ErrorMessage = "Telefon ne može biti duži od 50 karaktera!")]
        public string Phone { get; set; }

        [Display(Name = "Datum unosa:")]
        public DateTime DateOfInsert { get; set; }

        [Display(Name = "Kontakt osoba:")]
        [StringLength(50, ErrorMessage = "Ime ne može biti duže od 50 karaktera!")]
        public string ContactPersonName { get; set; }

        [Display(Name = "Telefon kontakt osobe:")]
        [StringLength(50, ErrorMessage = "Telefon ne može biti duži od 50 karaktera!")]
        public string ContactPersonPhone { get; set; }

        [Display(Name = "Broj članova porodice:")]
        [RegularExpression("(^[1-9][0-9]*$)", ErrorMessage = "Broj mora biti pozitivan!")]
        public int NumberOfFamilyMembers { get; set; } = 1;

        [Display(Name = "Datum zadnje pomoći:")]
        public DateTime? DateOfLastHelp { get; set; }

        [Display(Name = "Broj pomoći do sada:")]
        public int NumberOfHelpsSoFar { get; set; } = 0;

        [Display(Name = "Napomena:")]
        public string Note { get; set; }

        [Display(Name = "Datum zadnje izmjene:")]
        public DateTime? DateOfLastUpdate { get; set; }

        [Display(Name = "Da li je hitno:")]
        public bool IsUrgent { get; set; } = false;

        public bool IsActive { get; set; } = true;

        public bool IsHajrat { get; set; } = false;

        [Display(Name = "Grad:")]
        [Required(ErrorMessage = "Izaberite grad!")]
        public int CityId { get; set; }

        public IEnumerable<City> Cities { get; set; }

        [Display(Name = "Članovi:")]
        public string FamilyMembers { get; set; }

        public IPagedList<FamilyViewModel> FamiliesInNeed { get; set; }

        [Display(Name = "Grad:")]
        public string CityName { get; set; }

        [Display(Name = "Admin zadužen:")]
        public string AdminName { get; set; }

        [Display(Name = "Ime i prezime:")]
        public string FullName { get; set; }

        public bool IsAdmin { get; set; }
        public string LoggedInUser { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }
    }
}