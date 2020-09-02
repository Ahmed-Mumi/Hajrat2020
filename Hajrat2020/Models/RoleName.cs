namespace Hajrat2020.Models
{
    public class RoleName
    {
        public const string SuperAdmin = "Superadmin";
        public const string Admin = "Admin";
        public const string User = "User";
        public const string SuperAdminOrAdmin = SuperAdmin + "," + Admin;
    }
}