using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vueapi.Model
{
    public class Employee
    {
        public Guid EmployeeId { get; set; }
        public string Name { get; set; }
        public string? Email { get; set; }
        public DateTime? Doj { get; set; }
        public string? Department { get; set; }
        public string Designation { get; set; }
        public string? EmpCode { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDeleted { get; set; }

        public  ICollection<Device>? Devices { get; set; }
   
        public  ICollection<Assignment>? Assignments { get; set; }

    }
}
