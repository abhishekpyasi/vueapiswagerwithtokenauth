using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vueapi;
using Vueapi.Dtos.AssignmentDTO;
using Vueapi.Dtos.DeivceDTO;
using Vueapi.Dtos.EmployeeDTO;
using Vueapi.Dtos.EmployeetDTO;
using Vueapi.Helper;
using Vueapi.Model;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Vueapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class EmployeesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmployeesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees()
        {
            if (_context.Employees == null)
            {
                return NotFound();
            }

            var employees = await _context.Employees.Where(a => a.IsActive == true).Select(s => new EmployeeDto()
            {
                EmployeeId = s.EmployeeId,
                Name = s.Name,
                Department = s.Department,
                Designation = s.Designation,
                Doj = s.Doj,
                Email = s.Email,
                EmpCode = s.EmpCode,
                IsActive = s.IsActive,
                IsDeleted = s.IsDeleted,

            }).Where(x => x.IsActive == true || x.IsDeleted == false).ToListAsync();

            //return await _context.Employees.ToListAsync();
            return Ok(employees);
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDto>> GetEmployee(Guid id)
        {
            if (_context.Employees == null)
            {
                return NotFound();
            }
            var employee = await _context.Employees.FindAsync(id);
            var employees = new EmployeeDto
            {
                Department = employee.Department,
                EmpCode = employee.EmpCode,
                Designation = employee.Designation,
                Doj = employee.Doj,
                Email = employee.Email,
                EmployeeId = employee.EmployeeId,
                Name = employee.Name,
                IsActive = employee.IsActive,
                IsDeleted = employee.IsDeleted
            };

            if (employee == null)
            {
                return NotFound();
            }

            return employees;
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]

        public async Task<IActionResult> PutEmployee(Guid id, EmployeeUpdateDto employeeDto)
        {

            if (employeeDto == null)
            {

                return BadRequest("employee not provided in request");
            }

            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {

                return BadRequest("employee not found");


            }

            employee.Name = employeeDto.Name;
            employee.EmpCode = employeeDto.EmpCode;
            employee.Department = employeeDto.Department;
            employee.Doj = employeeDto.Doj;
            employee.Email = employeeDto.Email;
            employee.Designation = employeeDto.Designation;
            employee.IsActive = true;
            employee.IsDeleted = false;

            _context.Employees.Update(employee);


            //id

            //fetch from db

            //property compare

            //


            //_context.Entry(employee).State = EntityState.Modified;

            _context.SaveChanges();
            return NoContent();
        }

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EmployeeCreateDto>> PostEmployee(EmployeeCreateDto employeeDto)
        {
            /*  if (_context.Employees == null)
              {
                  return Problem("Entity set 'AppDbContext.Employees'  is null.");
              } */
            var employees = new Employee
            {
                Name = employeeDto.Name,
                Email = employeeDto.Email,
                Department = employeeDto.Department,
                Doj = employeeDto.Doj,
                EmpCode = employeeDto.EmpCode,
                Designation = employeeDto.Designation,
                IsActive = true,
                IsDeleted = false

            };

            _context.Employees.Add(employees);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployee", new { id = employees.EmployeeId }, employees.AsEmpGetDto());
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            if (_context.Employees == null)
            {
                return NotFound();
            }
            var existingEmployee = await _context.Employees.FindAsync(id);

            if (existingEmployee == null)
            {
                return NotFound();
            }
            existingEmployee.IsDeleted = true;
            existingEmployee.IsActive = false;
            _context.Entry(existingEmployee).State = EntityState.Modified;
            var assignment = await _context.Assignments.Where(x => x.EmployeeId == id && x.IsActive == true).ToListAsync();
            assignment.ForEach(x =>
            {


                x.IsActive = false;
            }

            );
            // _context.Entry(assignment).State = EntityState.Modified;

            var device = await _context.Devices.Where(x => x.AssignedTo == id && x.IsActive == true).ToListAsync();
            device.ForEach(x =>
            {
                x.AssignedTo = null;
                

            });


            // _context.Entry(device).State = EntityState.Modified;



            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }



            return NoContent();
        }

        // Get Devices by Emp ID
        [HttpGet]
        [Route("{id}/devices/active")]
        public async Task<ActionResult<IEnumerable<DevicesByEmployeeDto>>> GetDevicesByEmployeeID(Guid id)
        {
             var employee = await _context.Employees.FindAsync(id);
            
            if(employee == null)
            {

                return BadRequest("Employee doesnot exist ");
            }


            var devices = await (from assignmnt in _context.Assignments
                                 join device in _context.Devices on assignmnt.DeviceId equals device.DeviceId
                                 join emp in _context.Employees on device.AssignedTo equals emp.EmployeeId
                                 where assignmnt.IsActive == true
                                 && emp.EmployeeId == id
                                 select new DevicesByEmployeeDto
                                 {
                                     EmployeeId = emp.EmployeeId,
                                     Name = emp.Name,
                                     DeviceId = device.DeviceId,
                                     MobileModel = device.MobileModel,
                                     Company = device.Company,
                                     AllotmentDate = assignmnt.AllotmentDate,
                                     WarrantyMonths = device.WarrantyMonths,
                                     DeviceIsActive = device.IsActive,
                                     AssignmentId = assignmnt.AssignmentId

                                 }).ToListAsync();

            if( devices == null)

            {

                return NotFound("No device found for employee");
            }



            return devices;
        }

        [HttpGet]
        [Route("{id}/devices/")]
        public async Task<ActionResult<IEnumerable<DevicesByEmployeeDto>>> GetAllDevicesByEmployeeID(Guid id)
        {
             var employee = await _context.Employees.FindAsync(id);
            
            if(employee == null)
            {

                return BadRequest("Employee doesnot exist ");
            }
            

            var devices = await (from assignmnt in _context.Assignments
                                 join device in _context.Devices on assignmnt.DeviceId equals device.DeviceId
                                 join emp in _context.Employees on device.AssignedTo equals emp.EmployeeId
                                 where emp.EmployeeId == id
                                 select new DevicesByEmployeeDto
                                 {
                                     EmployeeId = emp.EmployeeId,
                                     Name = emp.Name,
                                     DeviceId = device.DeviceId,
                                     MobileModel = device.MobileModel,
                                     Company = device.Company,
                                     AllotmentDate = assignmnt.AllotmentDate,
                                     WarrantyMonths = device.WarrantyMonths,
                                     DeviceIsActive = device.IsActive,
                                     AssignmentId = assignmnt.AssignmentId

                                 }).ToListAsync();

            if (devices == null)

            {

                return NotFound("No device found for employee");
            }



            return devices;
        }

        private bool EmployeeExists(Guid id)
        {
            return (_context.Employees?.Any(e => e.EmployeeId == id)).GetValueOrDefault();
        }
    }
}
