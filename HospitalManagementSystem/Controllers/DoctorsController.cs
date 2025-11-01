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
    public class DoctorsController : BaseApiController
    {
        private readonly IDoctorService _doctorService;

        public DoctorsController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        /// <summary>
        /// Get all doctors
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _doctorService.GetAllAsync();
            return HandleResponse(response);
        }

        /// <summary>
        /// Get doctor by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _doctorService.GetByIdAsync(id);
            return HandleResponse(response);
        }

        /// <summary>
        /// Get doctors with pagination
        /// </summary>
        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchTerm = null)
        {
            var response = await _doctorService.GetPagedAsync(pageNumber, pageSize, searchTerm);
            return HandleResponse(response);
        }

        /// <summary>
        /// Get doctors by department
        /// </summary>
        [HttpGet("department/{departmentId}")]
        public async Task<IActionResult> GetByDepartment(int departmentId)
        {
            var response = await _doctorService.GetByDepartmentAsync(departmentId);
            return HandleResponse(response);
        }

        /// <summary>
        /// Create a new doctor (Admin only)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateDoctorDto dto)
        {
            var response = await _doctorService.CreateAsync(dto);
            return HandleResponse(response);
        }

        /// <summary>
        /// Update an existing doctor (Admin only)
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDoctorDto dto)
        {
            if (id != dto.Id)
                return BadRequest("ID mismatch");

            var response = await _doctorService.UpdateAsync(dto);
            return HandleResponse(response);
        }

        /// <summary>
        /// Delete a doctor (Admin only)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _doctorService.DeleteAsync(id);
            return HandleResponse(response);
        }

        /// <summary>
        /// Soft delete a doctor (Admin only)
        /// </summary>
        [HttpDelete("{id}/soft")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var response = await _doctorService.SoftDeleteAsync(id);
            return HandleResponse(response);
        }
    }
}
