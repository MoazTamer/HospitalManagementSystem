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
    public class MedicalRecordsController : BaseApiController
    {
        private readonly IMedicalRecordService _medicalRecordService;

        public MedicalRecordsController(IMedicalRecordService medicalRecordService)
        {
            _medicalRecordService = medicalRecordService;
        }

        /// <summary>
        /// Get all medical records (Admin, Doctor only)
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _medicalRecordService.GetAllAsync();
            return HandleResponse(response);
        }

        /// <summary>
        /// Get medical record by ID
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _medicalRecordService.GetByIdAsync(id);
            return HandleResponse(response);
        }

        /// <summary>
        /// Get medical records with pagination
        /// </summary>
        [HttpGet("paged")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchTerm = null)
        {
            var response = await _medicalRecordService.GetPagedAsync(pageNumber, pageSize, searchTerm);
            return HandleResponse(response);
        }

        /// <summary>
        /// Get medical records by patient
        /// </summary>
        [HttpGet("patient/{patientId}")]
        [Authorize(Roles = "Admin,Doctor,Patient")]
        public async Task<IActionResult> GetByPatient(int patientId)
        {
            var response = await _medicalRecordService.GetByPatientAsync(patientId);
            return HandleResponse(response);
        }

        /// <summary>
        /// Get medical records by doctor
        /// </summary>
        [HttpGet("doctor/{doctorId}")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> GetByDoctor(int doctorId)
        {
            var response = await _medicalRecordService.GetByDoctorAsync(doctorId);
            return HandleResponse(response);
        }

        /// <summary>
        /// Create a new medical record (Doctor only)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> Create([FromBody] CreateMedicalRecordDto dto)
        {
            var response = await _medicalRecordService.CreateAsync(dto);
            return HandleResponse(response);
        }

        /// <summary>
        /// Update a medical record (Doctor only)
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateMedicalRecordDto dto)
        {
            if (id != dto.Id)
                return BadRequest("ID mismatch");

            var response = await _medicalRecordService.UpdateAsync(dto);
            return HandleResponse(response);
        }

        /// <summary>
        /// Delete a medical record (Admin only)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _medicalRecordService.DeleteAsync(id);
            return HandleResponse(response);
        }
    }
}
