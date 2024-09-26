namespace Vueapi.Dtos.AssignmentDTO
{
    public class AssignmentUpdateDto
    {
     //   public Guid AssignmentId { get; set; }
        public DateTime AllotmentDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public Guid DeviceId { get; set; }
        public Guid EmployeeId { get; set; }
    }
}
