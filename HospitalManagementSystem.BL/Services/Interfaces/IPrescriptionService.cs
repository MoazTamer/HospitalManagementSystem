using HospitalManagementSystem.BL.Common;
using HospitalManagementSystem.BL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.BL.Services.Interfaces
{
    public interface IPrescriptionService
    {
        Task<ApiResponse<PrescriptionDto>> GetByIdAsync(int id);
        Task<ApiResponse<List<PrescriptionDto>>> GetByMedicalRecordAsync(int medicalRecordId);
        Task<ApiResponse<PrescriptionDto>> CreateAsync(CreatePrescriptionDto dto);
        Task<ApiResponse<PrescriptionDto>> UpdateAsync(UpdatePrescriptionDto dto);
        Task<ApiResponse<bool>> DeleteAsync(int id);
    }
}
