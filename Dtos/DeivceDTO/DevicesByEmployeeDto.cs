namespace Vueapi.Dtos.DeivceDTO
{
    public class DevicesByEmployeeDto
    {

        public Guid EmployeeId { get; set; }
        public string Name { get; set; }

        public Guid DeviceId { get; set; }

        public string MobileModel { get; set; }
        public string? Company { get; set; }
        public DateTime AllotmentDate { get; set; }
        public int? WarrantyMonths { get; set; }

        public bool? DeviceIsActive { get; set; }
       
        public Guid AssignmentId { get; set; }
    }
}
