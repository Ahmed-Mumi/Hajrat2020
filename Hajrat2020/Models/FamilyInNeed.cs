using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hajrat2020.Models
{
    public class FamilyInNeed
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(100)]
        public string Address { get; set; }

        [Required]
        [StringLength(50)]
        public string Phone { get; set; }

        public DateTime DateOfInsert { get; set; } = DateTime.Now;

        [Required]
        [StringLength(100)]
        public string ContactPersonName { get; set; }

        [Required]
        [StringLength(50)]
        public string ContactPersonPhone { get; set; }

        public int NumberOfFamilyMembers { get; set; } = 1;
        public DateTime? DateOfLastHelp { get; set; }
        public int NumberOfHelpsSoFar { get; set; } = 0;
        public string Note { get; set; }
        public string FamilyMembers { get; set; }
        public DateTime? DateOfLastUpdate { get; set; } = DateTime.Now;
        public bool IsUrgent { get; set; } = false;
        public bool IsActive { get; set; } = true;

        public City City { get; set; }
        public int CityId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        [StringLength(100)]
        public string ApplicationUserId { get; set; }

        public bool IsHajrat { get; set; } = false;

        [NotMapped]
        private string _fullName;
        public string FullName
        {
            get { return _fullName; }
            set { _fullName = $"{FirstName} {LastName}"; }
        }
    }
}