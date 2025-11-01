using HospitalManagementSystem.BL.Common;
using HospitalManagementSystem.BL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.BL.Services.Interfaces
{
    public interface IMedicalRecordService
    {
        Task<ApiResponse<MedicalRecordDto>> GetByIdAsync(int id);
        Task<ApiResponse<List<MedicalRecordDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<MedicalRecordDto>>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm = null);
        Task<ApiResponse<MedicalRecordDto>> CreateAsync(CreateMedicalRecordDto dto);
        Task<ApiResponse<MedicalRecordDto>> UpdateAsync(UpdateMedicalRecordDto dto);
        Task<ApiResponse<bool>> DeleteAsync(int id);
        Task<ApiResponse<List<MedicalRecordDto>>> GetByPatientAsync(int patientId);
        Task<ApiResponse<List<MedicalRecordDto>>> GetByDoctorAsync(int doctorId);
    }
}
