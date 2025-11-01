using HospitalManagementSystem.BL.Common;
using HospitalManagementSystem.BL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.BL.Services.Interfaces
{
    public interface IPatientService
    {
        Task<ApiResponse<PatientDto>> GetByIdAsync(int id);
        Task<ApiResponse<List<PatientDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<PatientDto>>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm = null);
        Task<ApiResponse<PatientDto>> CreateAsync(CreatePatientDto dto);
        Task<ApiResponse<PatientDto>> UpdateAsync(UpdatePatientDto dto);
        Task<ApiResponse<bool>> DeleteAsync(int id);
        Task<ApiResponse<bool>> SoftDeleteAsync(int id);
        Task<ApiResponse<List<PatientDto>>> SearchByNameAsync(string name);
    }
}
