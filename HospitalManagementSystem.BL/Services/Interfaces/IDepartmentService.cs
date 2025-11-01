using HospitalManagementSystem.BL.Common;
using HospitalManagementSystem.BL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.BL.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<ApiResponse<DepartmentDto>> GetByIdAsync(int id);
        Task<ApiResponse<List<DepartmentDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<DepartmentDto>>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm = null);
        Task<ApiResponse<DepartmentDto>> CreateAsync(CreateDepartmentDto dto);
        Task<ApiResponse<DepartmentDto>> UpdateAsync(UpdateDepartmentDto dto);
        Task<ApiResponse<bool>> DeleteAsync(int id);
        Task<ApiResponse<bool>> SoftDeleteAsync(int id);
    }
}
