namespace Vueapi.Dtos.AssignmentDTO
{
    public class AssignmentsByDeviceDto
    {
       
       

        public Guid AssignmentId { get; set; }
        public DateTime AllotmentDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public string EmpName { get; set; }

        public Guid EmployeeId { get; set; }

        public bool AssignmentIsActive { get; set; }

    }
}
