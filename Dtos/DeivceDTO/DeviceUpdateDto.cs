namespace Vueapi.Dtos.DeivceDTO
{
    public class DeviceUpdateDto
    {
      //  public Guid DeviceId { get; set; }
        public string MobileModel { get; set; }
        public string? Company { get; set; }
        public string SerialNumber { get; set; }
        public string OpratingSystem { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int? WarrantyMonths { get; set; }


       // public Guid? AssignedTo { get; set; }
    }
}
