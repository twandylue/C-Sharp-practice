using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class sso_account_binding
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int idp { get; set; }
        [Required]
        public string sourceId { get; set; }
        [Required]
        public int accountId { get; set; }
        public Account account {get; set;}
    }
}