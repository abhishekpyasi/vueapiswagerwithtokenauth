using Vueapi.Dtos.AssignmentDTO;
using Vueapi.Dtos.DeivceDTO;
using Vueapi.Model;

namespace Vueapi.Helper
{
    public static class AsDeviceGetDtoExtn
    {

        public static DeviceDto AsAsDeviceGetDto(this Device device)

        {
            return new DeviceDto()
            {

                DeviceId = device.DeviceId,
                AssignedTo = device.AssignedTo,
                Company = device.Company,
                MobileModel = device.MobileModel,
                OpratingSystem = device.OpratingSystem,
                PurchaseDate = device.PurchaseDate,
                SerialNumber = device.SerialNumber,
                WarrantyMonths = device.WarrantyMonths,
                IsActive = device.IsActive,
                IsDeleted = device.IsDeleted




            };
        }
    }
}
