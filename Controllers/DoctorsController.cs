using DoctorAppointmentSystemAPI.Data;
using DoctorAppointmentSystemAPI.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoctorAppointmentSystemAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class DoctorsController : ControllerBase
    {
        private readonly DoctorService _doctorService;

        public DoctorsController(DoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet("{id}/slots")]
        public async Task<IActionResult> GetAvailableSlots(int id, DateTime date, int duration)
        {
            try
            {
                var availableSlots = await _doctorService.GetAvailableSlots(id, date, duration);
                return Ok(availableSlots);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

        }

        [HttpGet("{id}/schedule")]
        public async Task<IActionResult> GetDoctorSchedule(int id, DateTime date)
        {
            try
            {
                var schedule = await _doctorService.GetDoctorSchedule(id, date);
                return Ok(schedule);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        
    }
}





       
        
           
        

        