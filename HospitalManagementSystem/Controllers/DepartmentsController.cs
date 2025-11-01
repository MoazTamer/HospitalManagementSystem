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
    public class DepartmentsController : BaseApiController
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentsController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        /// <summary>
        /// Get all departments
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _departmentService.GetAllAsync();
            return HandleResponse(response);
        }

        /// <summary>
        /// Get department by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _departmentService.GetByIdAsync(id);
            return HandleResponse(response);
        }

        /// <summary>
        /// Get departments with pagination
        /// </summary>
        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchTerm = null)
        {
            var response = await _departmentService.GetPagedAsync(pageNumber, pageSize, searchTerm);
            return HandleResponse(response);
        }

        /// <summary>
        /// Create a new department
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDepartmentDto dto)
        {
            var response = await _departmentService.CreateAsync(dto);
            return HandleResponse(response);
        }

        /// <summary>
        /// Update an existing department
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDepartmentDto dto)
        {
            if (id != dto.Id)
                return BadRequest("ID mismatch");

            var response = await _departmentService.UpdateAsync(dto);
            return HandleResponse(response);
        }

        /// <summary>
        /// Delete a department (hard delete)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _departmentService.DeleteAsync(id);
            return HandleResponse(response);
        }

        /// <summary>
        /// Soft delete a department
        /// </summary>
        [HttpDelete("{id}/soft")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var response = await _departmentService.SoftDeleteAsync(id);
            return HandleResponse(response);
        }
    }
}
