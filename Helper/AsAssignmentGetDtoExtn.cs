using Vueapi.Dtos.AssignmentDTO;
using Vueapi.Model;

namespace Vueapi.Helper
{
    public static class AsAssignmentGetDtoExtn

    {
        public static AssignmentDto AsAssignmentsDto(this Assignment assignment)

        {
            return new AssignmentDto()
            {
                AssignmentId = assignment.AssignmentId,
                AllotmentDate = assignment.AllotmentDate,
                DeviceId = assignment.DeviceId,
                EmployeeId = assignment.EmployeeId,
                ReturnDate = assignment.ReturnDate,
                IsActive=assignment.IsActive,
                


            };
    }
    }
}
