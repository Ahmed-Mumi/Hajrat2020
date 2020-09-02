using Hajrat2020.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Hajrat2020.ViewModel
{
    public class DonationViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Datum donacije:")]
        public DateTime DateOfDonation { get; set; } = DateTime.Now;

        [Display(Name = "Datum zadnjeg update-a:")]
        public DateTime DateOfLastUpdate { get; set; } = DateTime.Now;

        [Display(Name = "Iznos novca:")]
        [RegularExpression("(^[+]?([0-9]+(?:[\\.][0-9]*)?|\\.[0-9]+)$|null)", ErrorMessage = "Iznos mora biti pozitivan broj, koristiti tačku za decimalne!")]
        public decimal? AmountOfMoney { get; set; }

        [Display(Name = "Donator:")]
        [StringLength(100, ErrorMessage = "Ime ne može biti duže od 100 karaktera!")]
        public string Donator { get; set; }

        [Display(Name = "Napomena:")]
        public string Note { get; set; }

        [Display(Name = "Adresa:")]
        public string Address { get; set; }

        [Display(Name = "Tip pomoći:")]
        [Required(ErrorMessage = "Odaberite tip pomoći!")]
        public byte TypeOfHelpId { get; set; }

        public byte CurrencyId { get; set; }
        public string CurrencyName { get; set; }

        [Display(Name = "Tip pomoći:")]
        public string TypeOfHelpName { get; set; }

        [Required(ErrorMessage = "Ovo polje je obavezno")]
        public string ApplicationUserId { get; set; }

        [Display(Name = "Admin zadužen:")]
        public string UserName { get; set; }

        [Display(Name = "Donacija za:")]
        [Required(ErrorMessage = "Odaberite porodicu!")]
        public int FamilyInNeedId { get; set; }

        [Display(Name = "Donacija za:")]
        public string FamilyInNeedFullname { get; set; }

        [Display(Name = "Slika:")]
        public string Image { get; set; }

        public string CityName { get; set; }

        public HttpPostedFileBase ImageUpload { get; set; }

        public IEnumerable<TypeOfHelp> TypesOfHelp { get; set; }
        public IEnumerable<FamilyInNeed> FamiliesInNeed { get; set; }
        public IEnumerable<Currency> Currencies { get; set; }

        public IPagedList<DonationViewModel> Donations { get; set; }

        public string LoggedInUser { get; set; }
        public bool IsAdmin { get; set; }

        public int? SearchTypeId { get; set; }
        public int? SearchFamilyId { get; set; }

        public DonationViewModel()
        {
            Image = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) +
                    (MagicNumbers.DefaultImagePath);
        }
    }


}