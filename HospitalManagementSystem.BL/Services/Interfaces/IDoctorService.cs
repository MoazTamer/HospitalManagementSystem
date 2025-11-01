using HospitalManagementSystem.BL.Common;
using HospitalManagementSystem.BL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.BL.Services.Interfaces
{
    public interface IDoctorService
    {
        Task<ApiResponse<DoctorDto>> GetByIdAsync(int id);
        Task<ApiResponse<List<DoctorDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<DoctorDto>>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm = null);
        Task<ApiResponse<DoctorDto>> CreateAsync(CreateDoctorDto dto);
        Task<ApiResponse<DoctorDto>> UpdateAsync(UpdateDoctorDto dto);
        Task<ApiResponse<bool>> DeleteAsync(int id);
        Task<ApiResponse<bool>> SoftDeleteAsync(int id);
        Task<ApiResponse<List<DoctorDto>>> GetByDepartmentAsync(int departmentId);
    }
}
