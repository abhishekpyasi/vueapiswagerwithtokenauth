using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vueapi.Model
{
    public class Device
    {       
        public Guid DeviceId { get; set; }       
        public string MobileModel { get; set; }     
        public string? Company { get; set; }        
        public string SerialNumber { get; set; }        
        public string OpratingSystem { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int? WarrantyMonths { get; set; }        
        public Guid? AssignedTo { get; set; }

        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual Employee employee { get; set; }
     
        public virtual ICollection<Assignment> Assignments { get; set; }



    }
}
