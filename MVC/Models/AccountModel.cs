using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace MVC.Models
{
    // [Index(nameof(AccountName), IsUnique = true)]
    public class AccountModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public String AccountName { get; set; }
        [Required]
        public String Password { get; set; }
    }
}