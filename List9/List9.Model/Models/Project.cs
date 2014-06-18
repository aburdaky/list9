using Model.Interfaces;
using Model.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    /// <summary>
    /// This Model generates a table for Projects
    /// </summary>
    public class Project : IHasId, IAudited
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage="Name Required"  ,AllowEmptyStrings = false)]
        public string Name { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// Datetime requires format dd-MM-yyyyT00:00:00Z
        /// </summary>
        public DateTime? DateDue { get; set; }

        public DateTime CreationDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastEditDate { get; set; }
        public string LastEditedBy { get; set; }

        public ICollection<List9User> List9Users { get; set; }
        public ICollection<Task> Tasks { get; set; }
       
        public Project() {
            
            Tasks = new List<Task>();
        }
    }
}
