using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    /// <summary>
    /// This Model generates a table for tasks
    /// </summary>
    public class Task:IHasId,IAudited
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name Required", AllowEmptyStrings = false)]
        public string Name { get; set; }
        public string Notes { get; set; }

        /// <summary>
        /// Foregin Key to find the User assigned to a task
        /// </summary>
        public int? UserId { get; set; }
        public virtual User Users { get; set; }

        /// <summary>
        /// Foregin Key to find the Project assigned to a task
        /// </summary>
        [Required(ErrorMessage = "Assign a Project")]
        public int ProjectId { get; set; }
        public virtual Project Projects { get; set; }

        /// <summary>
        /// Foregin Key to find the Category assigned to a task
        /// </summary>
        [Required(ErrorMessage="Assign a Category")]
        public int TaskCategoryId { get; set; }
        public virtual TaskCategory TaskCategories { get; set; }

        /// <summary>
        /// Datetime requires format dd-MM-yyyyT00:00:00Z
        /// </summary>
        public DateTime? DateDue { get; set; }
        [DefaultValue(false), Required(AllowEmptyStrings= false)]
        public Boolean Done {get;set;}

        public DateTime CreationDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastEditDate { get; set; }
        public string LastEditedBy { get; set; }
        

    }
}
