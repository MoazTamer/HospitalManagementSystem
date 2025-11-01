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
    public class PrescriptionsController : BaseApiController
    {
        private readonly IPrescriptionService _prescriptionService;

        public PrescriptionsController(IPrescriptionService prescriptionService)
        {
            _prescriptionService = prescriptionService;
        }

        /// <summary>
        /// Get prescription by ID
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Doctor,Patient")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _prescriptionService.GetByIdAsync(id);
            return HandleResponse(response);
        }

        /// <summary>
        /// Get prescriptions by medical record
        /// </summary>
        [HttpGet("medical-record/{medicalRecordId}")]
        [Authorize(Roles = "Admin,Doctor,Patient")]
        public async Task<IActionResult> GetByMedicalRecord(int medicalRecordId)
        {
            var response = await _prescriptionService.GetByMedicalRecordAsync(medicalRecordId);
            return HandleResponse(response);
        }

        /// <summary>
        /// Create a new prescription (Doctor only)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> Create([FromBody] CreatePrescriptionDto dto)
        {
            var response = await _prescriptionService.CreateAsync(dto);
            return HandleResponse(response);
        }

        /// <summary>
        /// Update a prescription (Doctor only)
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePrescriptionDto dto)
        {
            if (id != dto.Id)
                return BadRequest("ID mismatch");

            var response = await _prescriptionService.UpdateAsync(dto);
            return HandleResponse(response);
        }

        /// <summary>
        /// Delete a prescription (Admin, Doctor only)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _prescriptionService.DeleteAsync(id);
            return HandleResponse(response);
        }
    }
}
