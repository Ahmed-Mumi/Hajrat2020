using System.ComponentModel.DataAnnotations;

namespace Hajrat2020.Models
{
    public class TypeOfHelp
    {
        public byte Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}