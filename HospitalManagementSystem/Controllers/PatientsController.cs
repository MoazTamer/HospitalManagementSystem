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
    public class PatientsController : BaseApiController
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        /// <summary>
        /// Get all patients
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _patientService.GetAllAsync();
            return HandleResponse(response);
        }

        /// <summary>
        /// Get patient by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _patientService.GetByIdAsync(id);
            return HandleResponse(response);
        }

        /// <summary>
        /// Get patients with pagination
        /// </summary>
        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchTerm = null)
        {
            var response = await _patientService.GetPagedAsync(pageNumber, pageSize, searchTerm);
            return HandleResponse(response);
        }

        /// <summary>
        /// Search patients by name
        /// </summary>
        [HttpGet("search")]
        public async Task<IActionResult> SearchByName([FromQuery] string name)
        {
            var response = await _patientService.SearchByNameAsync(name);
            return HandleResponse(response);
        }

        /// <summary>
        /// Create a new patient
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePatientDto dto)
        {
            var response = await _patientService.CreateAsync(dto);
            return HandleResponse(response);
        }

        /// <summary>
        /// Update an existing patient
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePatientDto dto)
        {
            if (id != dto.Id)
                return BadRequest("ID mismatch");

            var response = await _patientService.UpdateAsync(dto);
            return HandleResponse(response);
        }

        /// <summary>
        /// Delete a patient (hard delete)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _patientService.DeleteAsync(id);
            return HandleResponse(response);
        }

        /// <summary>
        /// Soft delete a patient
        /// </summary>
        [HttpDelete("{id}/soft")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var response = await _patientService.SoftDeleteAsync(id);
            return HandleResponse(response);
        }
    }
}
