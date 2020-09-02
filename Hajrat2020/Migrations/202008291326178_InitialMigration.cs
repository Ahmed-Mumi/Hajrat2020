namespace Hajrat2020.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Currencies",
                c => new
                    {
                        Id = c.Byte(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Donations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateOfDonation = c.DateTime(nullable: false),
                        DateOfLastUpdate = c.DateTime(nullable: false),
                        AmountOfMoney = c.Decimal(precision: 18, scale: 2),
                        Donator = c.String(maxLength: 100),
                        Note = c.String(),
                        TypeOfHelpId = c.Byte(nullable: false),
                        ApplicationUserId = c.String(nullable: false, maxLength: 128),
                        FamilyInNeedId = c.Int(nullable: false),
                        CurrencyId = c.Byte(nullable: false),
                        Image = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Currencies", t => t.CurrencyId)
                .ForeignKey("dbo.FamilyInNeeds", t => t.FamilyInNeedId)
                .ForeignKey("dbo.TypeOfHelps", t => t.TypeOfHelpId)
                .Index(t => t.TypeOfHelpId)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.FamilyInNeedId)
                .Index(t => t.CurrencyId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        Phone = c.String(nullable: false, maxLength: 50),
                        DateOfAddingAdmin = c.DateTime(nullable: false),
                        Active = c.Boolean(nullable: false),
                        CityId = c.Int(nullable: false),
                        GenderId = c.Byte(nullable: false),
                        RoleName = c.String(),
                        FullName = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId)
                .ForeignKey("dbo.Genders", t => t.GenderId)
                .Index(t => t.CityId)
                .Index(t => t.GenderId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Genders",
                c => new
                    {
                        Id = c.Byte(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.FamilyInNeeds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        Address = c.String(nullable: false, maxLength: 100),
                        Phone = c.String(nullable: false, maxLength: 50),
                        DateOfInsert = c.DateTime(nullable: false),
                        ContactPersonName = c.String(nullable: false, maxLength: 100),
                        ContactPersonPhone = c.String(nullable: false, maxLength: 50),
                        NumberOfFamilyMembers = c.Int(nullable: false),
                        DateOfLastHelp = c.DateTime(),
                        NumberOfHelpsSoFar = c.Int(nullable: false),
                        Note = c.String(),
                        FamilyMembers = c.String(),
                        DateOfLastUpdate = c.DateTime(),
                        IsUrgent = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CityId = c.Int(nullable: false),
                        ApplicationUserId = c.String(nullable: false, maxLength: 128),
                        IsHajrat = c.Boolean(nullable: false),
                        FullName = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Cities", t => t.CityId)
                .Index(t => t.CityId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.TypeOfHelps",
                c => new
                    {
                        Id = c.Byte(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FamilyUsers",
                c => new
                    {
                        FamilyInNeedId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.FamilyInNeedId, t.UserId })
                .ForeignKey("dbo.FamilyInNeeds", t => t.FamilyInNeedId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.FamilyInNeedId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.FamilyUsers", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.FamilyUsers", "FamilyInNeedId", "dbo.FamilyInNeeds");
            DropForeignKey("dbo.Donations", "TypeOfHelpId", "dbo.TypeOfHelps");
            DropForeignKey("dbo.Donations", "FamilyInNeedId", "dbo.FamilyInNeeds");
            DropForeignKey("dbo.FamilyInNeeds", "CityId", "dbo.Cities");
            DropForeignKey("dbo.FamilyInNeeds", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Donations", "CurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.Donations", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "GenderId", "dbo.Genders");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "CityId", "dbo.Cities");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.FamilyUsers", new[] { "UserId" });
            DropIndex("dbo.FamilyUsers", new[] { "FamilyInNeedId" });
            DropIndex("dbo.FamilyInNeeds", new[] { "ApplicationUserId" });
            DropIndex("dbo.FamilyInNeeds", new[] { "CityId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "GenderId" });
            DropIndex("dbo.AspNetUsers", new[] { "CityId" });
            DropIndex("dbo.Donations", new[] { "CurrencyId" });
            DropIndex("dbo.Donations", new[] { "FamilyInNeedId" });
            DropIndex("dbo.Donations", new[] { "ApplicationUserId" });
            DropIndex("dbo.Donations", new[] { "TypeOfHelpId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.FamilyUsers");
            DropTable("dbo.TypeOfHelps");
            DropTable("dbo.FamilyInNeeds");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.Genders");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Donations");
            DropTable("dbo.Currencies");
            DropTable("dbo.Cities");
        }
    }
}
