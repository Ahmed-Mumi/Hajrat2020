using System.ComponentModel.DataAnnotations;

namespace Hajrat2020.Models
{
    public class Gender
    {
        public byte Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}