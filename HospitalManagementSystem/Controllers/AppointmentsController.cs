using HospitalManagementSystem.BL.DTOs;
using HospitalManagementSystem.BL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppointmentsController : BaseApiController
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentsController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        /// <summary>
        /// Get all appointments
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _appointmentService.GetAllAsync();
            return HandleResponse(response);
        }

        /// <summary>
        /// Get appointment by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _appointmentService.GetByIdAsync(id);
            return HandleResponse(response);
        }

        /// <summary>
        /// Get appointments with pagination
        /// </summary>
        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? status = null)
        {
            var response = await _appointmentService.GetPagedAsync(pageNumber, pageSize, status);
            return HandleResponse(response);
        }

        /// <summary>
        /// Get appointments by patient
        /// </summary>
        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetByPatient(int patientId)
        {
            var response = await _appointmentService.GetByPatientAsync(patientId);
            return HandleResponse(response);
        }

        /// <summary>
        /// Get appointments by doctor
        /// </summary>
        [HttpGet("doctor/{doctorId}")]
        public async Task<IActionResult> GetByDoctor(int doctorId)
        {
            var response = await _appointmentService.GetByDoctorAsync(doctorId);
            return HandleResponse(response);
        }

        /// <summary>
        /// Get appointments by date range
        /// </summary>
        [HttpGet("date-range")]
        public async Task<IActionResult> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var response = await _appointmentService.GetByDateRangeAsync(startDate, endDate);
            return HandleResponse(response);
        }

        /// <summary>
        /// Create a new appointment
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,Receptionist,Doctor")]
        public async Task<IActionResult> Create([FromBody] CreateAppointmentDto dto)
        {
            var response = await _appointmentService.CreateAsync(dto);
            return HandleResponse(response);
        }

        /// <summary>
        /// Update an existing appointment
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Receptionist,Doctor")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAppointmentDto dto)
        {
            if (id != dto.Id)
                return BadRequest("ID mismatch");

            var response = await _appointmentService.UpdateAsync(dto);
            return HandleResponse(response);
        }

        /// <summary>
        /// Cancel an appointment
        /// </summary>
        [HttpPatch("{id}/cancel")]
        [Authorize(Roles = "Admin,Receptionist,Doctor,Patient")]
        public async Task<IActionResult> Cancel(int id)
        {
            var response = await _appointmentService.CancelAppointmentAsync(id);
            return HandleResponse(response);
        }

        /// <summary>
        /// Delete an appointment
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _appointmentService.DeleteAsync(id);
            return HandleResponse(response);
        }
    }
}
