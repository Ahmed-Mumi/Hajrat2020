using System.ComponentModel.DataAnnotations;

namespace Hajrat2020.Models
{
    public class City
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}