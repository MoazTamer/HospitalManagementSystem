using HospitalManagementSystem.BL.Common;
using HospitalManagementSystem.BL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.BL.Services.Interfaces
{
    public interface IBillingService
    {
        Task<ApiResponse<BillingDto>> GetByIdAsync(int id);
        Task<ApiResponse<List<BillingDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<BillingDto>>> GetPagedAsync(int pageNumber, int pageSize, string? paymentStatus = null);
        Task<ApiResponse<BillingDto>> CreateAsync(CreateBillingDto dto);
        Task<ApiResponse<BillingDto>> UpdateAsync(UpdateBillingDto dto);
        Task<ApiResponse<bool>> DeleteAsync(int id);
        Task<ApiResponse<List<BillingDto>>> GetByPatientAsync(int patientId);
        Task<ApiResponse<BillingDto>> ProcessPaymentAsync(PaymentDto dto);
    }
}
