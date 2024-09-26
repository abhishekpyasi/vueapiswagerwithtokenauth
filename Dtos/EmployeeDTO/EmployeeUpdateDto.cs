namespace Vueapi.Dtos.EmployeeDTO
{
    public class EmployeeUpdateDto
    {

        // public Guid EmployeeId;
        public string? Name { get; set; }
        public string? Email { get; set; }
        public DateTime? Doj { get; set; }
        public string? Department { get; set; }
        public string? Designation { get; set; }
        public string? EmpCode { get; set; }
    }
}
