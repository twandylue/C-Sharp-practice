using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class Platform
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nmae { get; set; }

        public string LicenseKey { get; set; }

        public ICollection<Command> Commands { get; set; } = new List<Command>();
    }
}