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
using Vueapi.Helper;
using Vueapi.Model;

namespace Vueapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class AssignmentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AssignmentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Assignments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssignmentDto>>> GetAssignments()
        {
            if (_context.Assignments == null)
            {
                return NotFound();
            }
            //var a = await _context.Assignments.ToListAsync();
            //var assignment = new List<GetDto>();
            //foreach (var item in a)
            //{
            //    GetDto obj = new GetDto()
            //    {
            //        AssignmentId = item.AssignmentId,
            //        AllotmentDate = item.AllotmentDate,
            //        IsActive = item.IsActive,
            //        ReturnDate = item.ReturnDate,
            //    };
            //    assignment.Add(obj);
            //}
            var assignments = await _context.Assignments.Where(a => a.IsActive ==true).Select(s => new AssignmentDto()
            {
                AssignmentId = s.AssignmentId,
                AllotmentDate = s.AllotmentDate,
                EmployeeId = s.EmployeeId,
                DeviceId = s.DeviceId,
                ReturnDate = s.ReturnDate,
                IsActive = s.IsActive,


            }).ToListAsync();

            //return await _context.Assignments.ToListAsync();
            return Ok(assignments);
            //return Ok(a);
        }

        // GET: api/Assignments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AssignmentDto>> GetAssignment(Guid id)
        {
            if (_context.Assignments == null)
            {
                return NotFound();
            }
            var assignment = await _context.Assignments.FindAsync(id);

            if (assignment.IsActive == false)
            {

                return NotFound("Assignment is not found");
            }
            var assignments = new AssignmentDto
            {
                AssignmentId = assignment.AssignmentId,
                AllotmentDate = assignment.AllotmentDate,
                EmployeeId = assignment.EmployeeId,
                DeviceId = assignment.DeviceId,
                ReturnDate = assignment.ReturnDate,
                IsActive = assignment.IsActive,


            };
            if (assignments == null)
            {
                return NotFound();
            }

            return assignments;
        }

        // PUT: api/Assignments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAssignment(Guid id, AssignmentUpdateDto assignmentDto)
        {
            /*if (id != assignmentDto.AssignmentId)
            {
                return BadRequest();
            } */
            
          

            if (assignmentDto == null)
            {

                return BadRequest("employee not provided in request");
            }

            var assignment = await _context.Assignments.FindAsync(id);

            if (assignment == null)
            {

                return BadRequest("employee not found");


            }

            assignment.AllotmentDate = assignmentDto.AllotmentDate;

            assignment.ReturnDate = assignmentDto.ReturnDate;
            assignment.DeviceId = assignmentDto.DeviceId;

            assignment.EmployeeId = assignmentDto.EmployeeId;
            assignment.IsActive = true;
            _context.Entry(assignment).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return NoContent();


        }

        // POST: api/Assignments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AssignmentCreateDto>> PostAssignment(AssignmentCreateDto assignmentDto)
        {

            var deviceId = assignmentDto.DeviceId;

            var isActiveDeviceExists = _context.Assignments.Where(a => a.IsActive == true).Select(a => a.DeviceId).Any(b => b == deviceId);

            if (isActiveDeviceExists)
            {

                return BadRequest("Device already assigned to another employee");
            }


            var device = _context.Devices.Where(d => deviceId == d.DeviceId).FirstOrDefault();
            if (device.AssignedTo is null && device.IsActive == true)
            {
                device.AssignedTo = assignmentDto.EmployeeId;

                _context.Entry(device).State = EntityState.Modified;

            }
            else
            {

                return BadRequest("Device is already assigned to Employee");
            }

            var assignments = new Assignment
            {
                AllotmentDate = DateTime.UtcNow,
                DeviceId = assignmentDto.DeviceId,
                EmployeeId = assignmentDto.EmployeeId,
                ReturnDate = DateTime.UtcNow,
                IsActive = true
            };



            if (_context.Assignments == null)
            {
                return Problem("Entity set 'AppDbContext.Assignments'  is null.");
            }
            _context.Assignments.Add(assignments);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAssignment", new { id = assignments.AssignmentId }, assignments.AsAssignmentsDto());
        }

        // DELETE: api/Assignments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAssignment(Guid id)
        {
            if (_context.Assignments == null)
            {
                return NotFound();
            }
            var assignment = await _context.Assignments.FindAsync(id);
            if (assignment == null)
            {
                return NotFound();
            }


            assignment.IsActive = false;
            assignment.ReturnDate = DateTime.UtcNow;
   ;

            var device = _context.Devices.Where(d => d.DeviceId == assignment.DeviceId).FirstOrDefault();

            device.AssignedTo = null;
            

            _context.Devices.Attach(device).Property(x => x.AssignedTo).IsModified = true;

    




            _context.Entry(assignment).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

      

        private bool AssignmentExists(Guid id)
        {
            return (_context.Assignments?.Any(e => e.AssignmentId == id)).GetValueOrDefault();
        }
    }
}
