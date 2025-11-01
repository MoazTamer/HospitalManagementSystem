using AutoMapper;
using HospitalManagementSystem.BL.Common;
using HospitalManagementSystem.BL.DTOs;
using HospitalManagementSystem.BL.Services.Interfaces;
using HospitalManagementSystem.DAL.UnitOfWork;
using HospitalManagementSystem.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.BL.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly JwtSettings _jwtSettings;

        public AuthService(IUnitOfWork unitOfWork, IMapper mapper, IOptions<JwtSettings> jwtSettings)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginDto dto)
        {
            try
            {
                var user = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
                if (user == null)
                    return ApiResponse<AuthResponseDto>.FailureResponse("Invalid username or password");

                if (!user.IsActive)
                    return ApiResponse<AuthResponseDto>.FailureResponse("User account is inactive");

                if (!PasswordHelper.VerifyPassword(dto.Password, user.PasswordHash))
                    return ApiResponse<AuthResponseDto>.FailureResponse("Invalid username or password");

                // Update last login date
                user.LastLoginDate = DateTime.UtcNow;
                _unitOfWork.Users.Update(user);
                await _unitOfWork.CompleteAsync();

                var token = GenerateJwtToken(user);
                var response = new AuthResponseDto
                {
                    Token = token,
                    Username = user.Username,
                    Email = user.Email,
                    Role = user.Role,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes)
                };

                return ApiResponse<AuthResponseDto>.SuccessResponse(response, "Login successful");
            }
            catch (Exception ex)
            {
                return ApiResponse<AuthResponseDto>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterDto dto)
        {
            try
            {
                // Check if username exists
                var usernameExists = await _unitOfWork.Users.AnyAsync(u => u.Username == dto.Username);
                if (usernameExists)
                    return ApiResponse<AuthResponseDto>.FailureResponse("Username already exists");

                // Check if email exists
                var emailExists = await _unitOfWork.Users.AnyAsync(u => u.Email == dto.Email);
                if (emailExists)
                    return ApiResponse<AuthResponseDto>.FailureResponse("Email already exists");

                var user = new User
                {
                    Username = dto.Username,
                    Email = dto.Email,
                    PasswordHash = HashPassword(dto.Password),
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Role = dto.Role,
                    IsActive = true,
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow
                };

                await _unitOfWork.Users.AddAsync(user);
                await _unitOfWork.CompleteAsync();

                var token = GenerateJwtToken(user);
                var response = new AuthResponseDto
                {
                    Token = token,
                    Username = user.Username,
                    Email = user.Email,
                    Role = user.Role,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes)
                };

                return ApiResponse<AuthResponseDto>.SuccessResponse(response, "Registration successful");
            }
            catch (Exception ex)
            {
                return ApiResponse<AuthResponseDto>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<UserDto>> GetCurrentUserAsync(string username)
        {
            try
            {
                var user = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.Username == username);
                if (user == null)
                    return ApiResponse<UserDto>.FailureResponse("User not found");

                var userDto = _mapper.Map<UserDto>(user);
                return ApiResponse<UserDto>.SuccessResponse(userDto);
            }
            catch (Exception ex)
            {
                return ApiResponse<UserDto>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> ChangePasswordAsync(string username, string oldPassword, string newPassword)
        {
            try
            {
                var user = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.Username == username);
                if (user == null)
                    return ApiResponse<bool>.FailureResponse("User not found");

                if (!VerifyPassword(oldPassword, user.PasswordHash))
                    return ApiResponse<bool>.FailureResponse("Old password is incorrect");

                user.PasswordHash = HashPassword(newPassword);
                user.ModifiedBy = username;
                user.ModifiedDate = DateTime.UtcNow;

                _unitOfWork.Users.Update(user);
                await _unitOfWork.CompleteAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Password changed successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.FailureResponse($"Error: {ex.Message}");
            }
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }
    }
}
