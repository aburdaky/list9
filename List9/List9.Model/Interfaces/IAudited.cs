using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Interfaces
{
    public interface IAudited
    {
        DateTime CreationDate { get; set; }
        string CreatedBy { get; set; }
        DateTime? LastEditDate { get; set; }
        string LastEditedBy { get; set; }
    }
}
