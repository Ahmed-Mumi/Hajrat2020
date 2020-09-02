using System.ComponentModel.DataAnnotations;

namespace Hajrat2020.Models
{
    public class Currency
    {
        public byte Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}