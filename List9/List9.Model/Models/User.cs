using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Model.Interfaces;

namespace Model.Models
{
    /// <summary>
    /// This Model generates a table for Users
    /// </summary>
    public class User : IHasId, IAudited
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name Required", AllowEmptyStrings = false)]
        public string Name { get; set; }
        public string Mobile {get;set;}
        public string Email {get;set;}

        public DateTime CreationDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastEditDate { get; set; }
        public string LastEditedBy { get; set; }

        public ICollection<Task> Tasks { get; set; }

        public User() {

            Tasks = new List<Task>();
        }

    }
}
