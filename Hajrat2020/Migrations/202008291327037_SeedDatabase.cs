namespace Hajrat2020.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class SeedDatabase : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO TypeOfHelps (Id, Name) VALUES (1,'Novčana pomoć')");
            Sql("INSERT INTO TypeOfHelps (Id, Name) VALUES (2,'Paket namirnica')");
            Sql("INSERT INTO TypeOfHelps (Id, Name) VALUES (3,'Odjeća')");

            Sql("INSERT INTO Genders (Id, Name) VALUES (1,'Muško')");
            Sql("INSERT INTO Genders (Id, Name) VALUES (2,'Žensko')");

            Sql("INSERT INTO Currencies (Id, Name) VALUES (1,'$')");
            Sql("INSERT INTO Currencies (Id, Name) VALUES (2,'KM')");
            Sql("INSERT INTO Currencies (Id, Name) VALUES (3,'€')");


            Sql("SET IDENTITY_INSERT Cities ON");
            Sql("INSERT INTO Cities (Id,Name) VALUES (1,'Zenica')");
            Sql("INSERT INTO Cities (Id,Name) VALUES (2,'Banja Luka')");
            Sql("INSERT INTO Cities (Id,Name) VALUES (3,'Tuzla')");
            Sql("INSERT INTO Cities (Id,Name) VALUES (4,'Sarajevo')");
            Sql("INSERT INTO Cities (Id,Name) VALUES (5,'Mostar')");
            Sql("INSERT INTO Cities (Id,Name) VALUES (6,'Prijedor')");
            Sql("INSERT INTO Cities (Id,Name) VALUES (7,'Cazin')");
            Sql("INSERT INTO Cities (Id,Name) VALUES (8,'Gradačac')");
            Sql("INSERT INTO Cities (Id,Name) VALUES (9,'Visoko')");
            Sql("SET IDENTITY_INSERT Cities OFF");



            Sql(@"
            INSERT[dbo].[AspNetUsers]([Id], [FirstName], [LastName], [Phone], [DateOfAddingAdmin], [Active], [CityId], [GenderId], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [RoleName]) VALUES(N'14648dca-d731-407f-b79d-79e071413736', N'user', N'user', N'062222222', CAST(N'2020-06-23 21:27:46.457' AS DateTime), 1, 1, 1, N'user@hajrat.com', 0, N'AMUvbwk+NAQYLFOban3plfC+i1t3gNPbUThbbCIj43xflddqHPbxL7jrCVx6tkwf2g==', N'f1acb5b4-f9c6-48af-9e29-2d46d109d124', NULL, 0, 0, NULL, 1, 0, N'user@hajrat.com','User')
            INSERT[dbo].[AspNetUsers] ([Id], [FirstName], [LastName], [Phone], [DateOfAddingAdmin], [Active], [CityId], [GenderId], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [RoleName]) VALUES(N'47660ac1-52e7-4454-9834-2fbf51f0d6dc', N'superadmin', N'superadmin', N'063333333', CAST(N'2020-06-23 21:29:26.850' AS DateTime), 1, 1, 1, N'superadmin@hajrat.com', 0, N'AFnbWpYo2cnAWO2sfGDjgdQHpJD1WpMRWl1fX33TImzaVf/YbfKSvy1LEk3Azzzm0w==', N'6aa61c9a-d89e-4c04-9205-6b8defb2200d', NULL, 0, 0, NULL, 1, 0, N'superadmin@hajrat.com','Superadmin')
            INSERT[dbo].[AspNetUsers] ([Id], [FirstName], [LastName], [Phone], [DateOfAddingAdmin], [Active], [CityId], [GenderId], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [RoleName]) VALUES(N'8531758a-62d0-4a73-bfc1-fb072e626e63', N'admin', N'admin', N'061111111', CAST(N'2020-06-23 21:25:51.070' AS DateTime), 1, 1, 1, N'admin@hajrat.com', 0, N'AIcB3OXhkDEsW5GyLebRGDEtzk8L0NshLD8/7/PQ2vGgxX1OdovjE0jykzzM9QOxWg==', N'15ec8608-0b10-4e51-bf5b-aa5d99e38fc7', NULL, 0, 0, NULL, 1, 0, N'admin@hajrat.com','Admin')
            INSERT[dbo].[AspNetRoles]
                    ([Id], [Name]) VALUES(N'34b1244b-2000-4190-80d1-a0ac386ac6d8', N'Admin')
            INSERT[dbo].[AspNetRoles]
                    ([Id], [Name]) VALUES(N'32343dc3-eae6-4105-8b7c-bc765428f5ce', N'Superadmin')
            INSERT[dbo].[AspNetRoles]
                    ([Id], [Name]) VALUES(N'84b5dfff-e5e9-48fa-9be8-525cee826adb', N'User')
            INSERT[dbo].[AspNetUserRoles]
                    ([UserId], [RoleId]) VALUES(N'47660ac1-52e7-4454-9834-2fbf51f0d6dc', N'32343dc3-eae6-4105-8b7c-bc765428f5ce')
            INSERT[dbo].[AspNetUserRoles]
                    ([UserId], [RoleId]) VALUES(N'8531758a-62d0-4a73-bfc1-fb072e626e63', N'34b1244b-2000-4190-80d1-a0ac386ac6d8')
            INSERT[dbo].[AspNetUserRoles]
                    ([UserId], [RoleId]) VALUES(N'14648dca-d731-407f-b79d-79e071413736', N'84b5dfff-e5e9-48fa-9be8-525cee826adb')
            ");

            Sql(@"INSERT INTO FamilyInNeeds (FirstName,LastName,Address,Phone,DateOfInsert,ContactPersonName,ContactPersonPhone,NumberOfFamilyMembers,
                    NumberOfHelpsSoFar, IsUrgent, CityId, ApplicationUserId, IsActive,IsHajrat)
                VALUES('Udruženje', 'Hajrat', 'Srpska mahala', '060 32 99 699', 08-16-2020, 'Albin', '061000000', 1, 0, 0, 1, '8531758a-62d0-4a73-bfc1-fb072e626e63', 1,1)");

        }

        public override void Down()
        {
        }
    }
}