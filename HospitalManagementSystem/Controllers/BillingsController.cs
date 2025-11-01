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
    public class BillingsController : BaseApiController
    {
        private readonly IBillingService _billingService;

        public BillingsController(IBillingService billingService)
        {
            _billingService = billingService;
        }

        /// <summary>
        /// Get all billings (Admin, Receptionist only)
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin,Receptionist")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _billingService.GetAllAsync();
            return HandleResponse(response);
        }

        /// <summary>
        /// Get billing by ID
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Receptionist,Patient")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _billingService.GetByIdAsync(id);
            return HandleResponse(response);
        }

        /// <summary>
        /// Get billings with pagination
        /// </summary>
        [HttpGet("paged")]
        [Authorize(Roles = "Admin,Receptionist")]
        public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? paymentStatus = null)
        {
            var response = await _billingService.GetPagedAsync(pageNumber, pageSize, paymentStatus);
            return HandleResponse(response);
        }

        /// <summary>
        /// Get billings by patient
        /// </summary>
        [HttpGet("patient/{patientId}")]
        [Authorize(Roles = "Admin,Receptionist,Patient")]
        public async Task<IActionResult> GetByPatient(int patientId)
        {
            var response = await _billingService.GetByPatientAsync(patientId);
            return HandleResponse(response);
        }

        /// <summary>
        /// Create a new billing (Admin, Receptionist only)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,Receptionist")]
        public async Task<IActionResult> Create([FromBody] CreateBillingDto dto)
        {
            var response = await _billingService.CreateAsync(dto);
            return HandleResponse(response);
        }

        /// <summary>
        /// Update a billing (Admin, Receptionist only)
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Receptionist")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBillingDto dto)
        {
            if (id != dto.Id)
                return BadRequest("ID mismatch");

            var response = await _billingService.UpdateAsync(dto);
            return HandleResponse(response);
        }

        /// <summary>
        /// Process payment for a billing
        /// </summary>
        [HttpPost("process-payment")]
        [Authorize(Roles = "Admin,Receptionist")]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentDto dto)
        {
            var response = await _billingService.ProcessPaymentAsync(dto);
            return HandleResponse(response);
        }

        /// <summary>
        /// Delete a billing (Admin only)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _billingService.DeleteAsync(id);
            return HandleResponse(response);
        }
    }
}
