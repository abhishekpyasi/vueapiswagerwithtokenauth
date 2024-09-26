using Vueapi.Migrations;

namespace Vueapi.Dtos.DeivceDTO
{
    public class DeviceCreateDto
    {
        public string MobileModel { get; set; }
        public string? Company { get; set; }
        public string SerialNumber { get; set; }
        public string OpratingSystem { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int? WarrantyMonths { get; set; }
        public Nullable<Guid> AssignedTo { get; set; }


    }
}
