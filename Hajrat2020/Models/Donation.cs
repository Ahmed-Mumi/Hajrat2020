using System;
using System.ComponentModel.DataAnnotations;

namespace Hajrat2020.Models
{
    public class Donation
    {
        public int Id { get; set; }
        public DateTime DateOfDonation { get; set; } = DateTime.Now;
        public DateTime DateOfLastUpdate { get; set; } = DateTime.Now;
        public decimal? AmountOfMoney { get; set; } = 0;
        [StringLength(100)]
        public string Donator { get; set; }
        public string Note { get; set; }

        public TypeOfHelp TypeOfHelp { get; set; }
        public byte TypeOfHelpId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        [Required]
        [StringLength(100)]
        public string ApplicationUserId { get; set; }

        public FamilyInNeed FamilyInNeed { get; set; }
        public int FamilyInNeedId { get; set; }

        public Currency Currency { get; set; }
        public byte CurrencyId { get; set; }

        public string Image { get; set; }
    }
}