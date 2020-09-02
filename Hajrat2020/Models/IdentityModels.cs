using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Hajrat2020.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        [Required]
        [StringLength(50)]
        public string Phone { get; set; }
        public DateTime DateOfAddingAdmin { get; set; } = DateTime.Now;
        public bool Active { get; set; } = true;

        public City City { get; set; }
        public int CityId { get; set; }

        public Gender Gender { get; set; }
        public byte GenderId { get; set; }

        public string RoleName { get; set; }

        [NotMapped]
        private string _fullName;
        public string FullName
        {
            get { return _fullName; }
            set { _fullName = $"{FirstName} {LastName}"; }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
        : base("DefaultConnectionStr", throwIfV1Schema: false)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<City> Cities { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<FamilyInNeed> FamilyInNeeds { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<TypeOfHelp> TypeOfHelps { get; set; }
        public DbSet<FamilyUser> FamilyUsers { get; set; }
        public DbSet<Currency> Currencies { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Donation>().HasRequired(c => c.ApplicationUser).WithMany()
                .HasForeignKey(c => c.ApplicationUserId);
            modelBuilder.Entity<FamilyInNeed>().HasRequired(c => c.ApplicationUser).WithMany()
                .HasForeignKey(c => c.ApplicationUserId);
            modelBuilder.Entity<FamilyUser>().HasKey(table => new { table.FamilyInNeedId, table.UserId });
            modelBuilder.Entity<FamilyUser>().HasRequired(c => c.User).WithMany()
                .HasForeignKey(c => c.UserId);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}