using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVC.GraphQL.Data
{
    public class Speaker
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        [StringLength(4000)]
        public string Bio { get; set; }
        
        [StringLength(1000)]
        public virtual string Website { get; set; }

        public ICollection<SessionSpeaker> SessionSpeakers {get; set;} = new List<SessionSpeaker>();
    }
}