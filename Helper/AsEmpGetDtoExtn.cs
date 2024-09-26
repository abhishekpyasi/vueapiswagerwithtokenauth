using Vueapi.Dtos.EmployeeDTO;
using Vueapi.Model;

namespace Vueapi.Helper
{
    public static class AsEmpGetDtoExtn
    {

        public static EmployeeDto AsEmpGetDto(this Employee employee)

        {

            return new EmployeeDto()
            {
                EmployeeId = employee.EmployeeId,

                Department = employee.Department,
                Designation = employee.Designation,
                Doj = employee.Doj,
                Email = employee.Email,
                EmpCode = employee.EmpCode,
                Name = employee.Name,
                IsActive= employee.IsActive,
                IsDeleted= employee.IsDeleted,



            };
        }
    }
}
