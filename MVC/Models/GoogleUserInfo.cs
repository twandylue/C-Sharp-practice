using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class GoogleUserInfo
    {
        public string id { get; set; }
        public string email { get; set; }
        public bool verified_email { get; set; }
        public string name { get; set; }
        public string given_name { get; set; }
        public string family_name { get; set; }
        public string link { get; set; }
        public string picture { get; set; }
        public string locale { get; set; }
    }
}