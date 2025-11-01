using HospitalManagementSystem.BL.Common;
using HospitalManagementSystem.BL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.BL.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginDto dto);
        Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterDto dto);
        Task<ApiResponse<UserDto>> GetCurrentUserAsync(string username);
        Task<ApiResponse<bool>> ChangePasswordAsync(string username, string oldPassword, string newPassword);
    }
}
