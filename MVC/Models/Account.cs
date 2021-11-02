using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    // [Index(nameof(AccountName), IsUnique = true)]
    public class Account
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public String AccountName { get; set; }
        [Required]
        public String Password { get; set; }
        public ICollection<sso_account_binding> bindings { get; set; } = new List<sso_account_binding>();
    }
}