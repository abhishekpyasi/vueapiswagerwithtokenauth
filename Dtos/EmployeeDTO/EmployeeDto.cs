namespace Vueapi.Dtos.EmployeeDTO
{
    public class EmployeeDto
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
    }
}
