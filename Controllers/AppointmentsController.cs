using DoctorAppointmentSystemAPI.Data;
using DoctorAppointmentSystemAPI.DTOs;
using DoctorAppointmentSystemAPI.Service;
using Microsoft.AspNetCore.Mvc;

namespace DoctorAppointmentSystemAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly AppointmentService _appointmentService;

        public AppointmentsController(AppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost]
        public async Task<IActionResult> BookAppointment( AppointmentRequest request)
        {
            try
            {
                var appointment = await _appointmentService.BookAppointment(request);
                return Ok(appointment);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelAppointment(int id)
        {
            try
            {
                await _appointmentService.CancelAppointment(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}

        
          