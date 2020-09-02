namespace Hajrat2020.Models
{
    public static class MagicNumbers
    {
        public static int MoneyTypeDonations = 1;
        public static int Dolar = 1;
        public static int BAM = 2;
        public static int Euro = 3;
        public static string ImagePath { get; set; } = "/Files/Images/";
        public static string Default { get; set; } = "default.jpg";
        public static string DefaultImagePath { get; set; } = ImagePath + Default;
    }
}