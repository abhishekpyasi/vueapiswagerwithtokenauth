using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vueapi.Model
{
    public class Assignment
    {
        public Guid AssignmentId { get; set; }
        public DateTime AllotmentDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public Guid DeviceId { get; set; }
        public Guid EmployeeId { get; set; }
        public bool IsActive { get; set; }
        public virtual Employee employee { get; set; }

        public virtual Device device { get; set; }
    }
}
