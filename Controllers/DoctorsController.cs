using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoctorAppointmentSystemAPI.Data;
using DoctorAppointmentSystemAPI.Service;

namespace DoctorAppointmentSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly DoctorAppointmentService _service;

        public DoctorsController(DoctorAppointmentService service)
        {
            _service = service;
        }

        [HttpGet("{id}/slots")]
        public async Task<IActionResult> GetAvailableTimeSlots(int id, [FromQuery] DateTime date, [FromQuery] int duration)
        {
            date = date.ToUniversalTime();

            try
            {
                var result = await _service.GetAvailableTimeSlotsForDoctorAsync(id, date, duration);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
            }
        }
    

        
        [HttpGet("{id}/schedule")]
        public async Task<IActionResult> GetDoctorSchedule(int id, [FromQuery] DateTime? date = null)
        {
            var scheduleDate = (date ?? DateTime.Today).ToUniversalTime();
            var appointments = await _service.GetDoctorScheduleAsync(scheduleDate);
            if (!appointments.Any())
                return NotFound("No appointments found for the specified date.");

            return Ok(new
            {
                Date = scheduleDate.ToString("yyyy-MM-dd"),
                Appointments = appointments
            });
        }
    }
}
