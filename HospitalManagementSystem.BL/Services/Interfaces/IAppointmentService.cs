using HospitalManagementSystem.BL.Common;
using HospitalManagementSystem.BL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.BL.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<ApiResponse<AppointmentDto>> GetByIdAsync(int id);
        Task<ApiResponse<List<AppointmentDto>>> GetAllAsync();
        Task<ApiResponse<PagedResponse<AppointmentDto>>> GetPagedAsync(int pageNumber, int pageSize, string? status = null);
        Task<ApiResponse<AppointmentDto>> CreateAsync(CreateAppointmentDto dto);
        Task<ApiResponse<AppointmentDto>> UpdateAsync(UpdateAppointmentDto dto);
        Task<ApiResponse<bool>> DeleteAsync(int id);
        Task<ApiResponse<bool>> CancelAppointmentAsync(int id);
        Task<ApiResponse<List<AppointmentDto>>> GetByPatientAsync(int patientId);
        Task<ApiResponse<List<AppointmentDto>>> GetByDoctorAsync(int doctorId);
        Task<ApiResponse<List<AppointmentDto>>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
