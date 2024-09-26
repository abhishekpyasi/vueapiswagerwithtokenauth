using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vueapi;
using Vueapi.Dtos.AssignmentDTO;
using Vueapi.Dtos.DeivceDTO;
using Vueapi.Helper;
using Vueapi.Model;

namespace Vueapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class DevicesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DevicesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Devices
        [HttpGet]
        //[Authorize]
        public async Task<ActionResult<IEnumerable<DeviceDto>>> GetDevices()
        {
            if (_context.Devices == null)
            {
                return NotFound();
            }
            var devices = await _context.Devices.Where(a => a.IsActive == true).Select(s => new DeviceDto()
            {
                DeviceId = s.DeviceId,
                SerialNumber = s.SerialNumber,
                Company = s.Company,
                MobileModel = s.MobileModel,
                OpratingSystem = s.OpratingSystem,
                PurchaseDate = s.PurchaseDate,
                WarrantyMonths = s.WarrantyMonths,
                AssignedTo = s.AssignedTo,
                IsDeleted = s.IsDeleted,
                IsActive = s.IsActive
            }).Where(x => x.IsActive == true || x.IsDeleted == false).ToListAsync();
            //return await _context.Devices.ToListAsync();
            return devices;
        }
       
        [HttpGet]
        [Route("/devices/active")]

        public async Task<ActionResult<IEnumerable<DeviceDto>>> GetActiveDevices()
        {
            if (_context.Devices == null)
            {
                return NotFound();
            }
            var devices = await _context.Devices.Where(a => a.IsActive == true && a.AssignedTo == null).Select(s => new DeviceDto()
            {
                DeviceId = s.DeviceId,
                SerialNumber = s.SerialNumber,
                Company = s.Company,
                MobileModel = s.MobileModel,
                OpratingSystem = s.OpratingSystem,
                PurchaseDate = s.PurchaseDate,
                WarrantyMonths = s.WarrantyMonths,
                AssignedTo = s.AssignedTo,
                IsDeleted = s.IsDeleted,
                IsActive = s.IsActive
            }).Where(x => x.IsActive == true || x.IsDeleted == false).ToListAsync();
            //return await _context.Devices.ToListAsync();
            return devices;
        }

        // GET: api/Devices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DeviceDto>> GetDevice(Guid id)
        {
            if (_context.Devices == null)
            {
                return NotFound();
            }
            var device = await _context.Devices.FindAsync(id);
            var devices = new DeviceDto
            {
                DeviceId = device.DeviceId,
                SerialNumber = device.SerialNumber,
                Company = device.Company,
                MobileModel = device.MobileModel,
                OpratingSystem = device.OpratingSystem,
                PurchaseDate = device.PurchaseDate,
                WarrantyMonths = device.WarrantyMonths,
                AssignedTo = device.AssignedTo,
                IsActive = device.IsActive,
                IsDeleted = device.IsDeleted
            


    };

            if (devices == null)
            {
                return NotFound();
            }

            return devices;
        }

        // PUT: api/Devices/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDevice(Guid id, DeviceUpdateDto deviceUpdateDto)
        {
            /*  if (id != deviceUpdateDto.DeviceId)
              {
                  return BadRequest();
              } */

            var device = _context.Devices.Find(id);

            if (device == null)
            {

                return BadRequest();


            }
            device.SerialNumber = deviceUpdateDto.SerialNumber;
            device.Company = deviceUpdateDto.Company;
            device.MobileModel = deviceUpdateDto.MobileModel;

            device.OpratingSystem = deviceUpdateDto.OpratingSystem;
            device.WarrantyMonths = deviceUpdateDto?.WarrantyMonths;
            device.SerialNumber = deviceUpdateDto?.SerialNumber;



            _context.Entry(device).State = EntityState.Modified;


            
           
                await _context.SaveChangesAsync();
           

            return NoContent();
        }

        // POST: api/Devices
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DeviceCreateDto>> PostDevice(DeviceCreateDto devPostDto)
        {
            if (_context.Devices == null)
            {
                return Problem("Entity set 'AppDbContext.Devices'  is null.");
            }

            if (devPostDto.AssignedTo == Guid.Empty)
            {
                devPostDto.AssignedTo = null;
                Console.WriteLine("inside if block");
            }
            var devices = new Device
            {

                AssignedTo = devPostDto.AssignedTo,
                Company = devPostDto.Company,
                MobileModel = devPostDto.MobileModel,
                OpratingSystem = devPostDto.OpratingSystem,
                PurchaseDate = devPostDto.PurchaseDate,
                SerialNumber = devPostDto.SerialNumber,
                WarrantyMonths = devPostDto.WarrantyMonths,
                IsActive = true,
                IsDeleted = false

            };
            _context.Devices.Add(devices);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDevice", new { id = devices.DeviceId }, devices.AsAsDeviceGetDto());
        }

        // DELETE: api/Devices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDevice(Guid id)
        {
            if (_context.Devices == null)
            {
                return NotFound();
            }
            var device = await _context.Devices.FindAsync(id);
            if (device == null)
            {
                return NotFound();
            }
            device.IsDeleted = true;
            device.IsActive = false;

            _context.Entry(device).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Assignments by Device ID
        [HttpGet]
        [Route("{id}/assignments/")]
        public async Task<ActionResult<IEnumerable<AssignmentsByDeviceDto>>> GetAllAssignmentsByDeviceID(Guid id)
        {
            var isDeviceExists = await _context.Devices.FindAsync(id);

            if (isDeviceExists == null)
            {

                return BadRequest("Device doesnot exist ");
            }
            var assignments = _context.Assignments.Where(a => a.DeviceId == id);

            var AssignmentsWithEmp = await (from assignmnt in assignments
                                      join emp in _context.Employees
                                         on assignmnt.EmployeeId equals emp.EmployeeId
                                      select new AssignmentsByDeviceDto
                                      {

                                          AssignmentId = assignmnt.AssignmentId,
                                          AllotmentDate = assignmnt.AllotmentDate,
                                          ReturnDate = assignmnt.ReturnDate,
                                          AssignmentIsActive = assignmnt.IsActive,
                                          EmpName = emp.Name,
                                          EmployeeId = assignmnt.EmployeeId


                                      }).ToListAsync();


                                 

            if (AssignmentsWithEmp == null)

            {

                return NotFound("No assignment found for device");
            }



            return AssignmentsWithEmp;
        }
        private bool DeviceExists(Guid id)
        {
            return (_context.Devices?.Any(e => e.DeviceId == id)).GetValueOrDefault();
        }
    }
}
