using AutoMapper;
using HospitalManagementSystem.BL.Common;
using HospitalManagementSystem.BL.DTOs;
using HospitalManagementSystem.BL.Services.Interfaces;
using HospitalManagementSystem.DAL.UnitOfWork;
using HospitalManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.BL.Services.Implementation
{
    public class DoctorService : IDoctorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DoctorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<DoctorDto>> GetByIdAsync(int id)
        {
            try
            {
                var doctor = await _unitOfWork.Doctors.GetQueryable()
                    .Include(d => d.Department)
                    .FirstOrDefaultAsync(d => d.Id == id);

                if (doctor == null)
                    return ApiResponse<DoctorDto>.FailureResponse($"Doctor with ID {id} not found");

                var dto = _mapper.Map<DoctorDto>(doctor);
                return ApiResponse<DoctorDto>.SuccessResponse(dto);
            }
            catch (Exception ex)
            {
                return ApiResponse<DoctorDto>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<DoctorDto>>> GetAllAsync()
        {
            try
            {
                var doctors = await _unitOfWork.Doctors.GetQueryable()
                    .Include(d => d.Department)
                    .ToListAsync();

                var dtos = _mapper.Map<List<DoctorDto>>(doctors);
                return ApiResponse<List<DoctorDto>>.SuccessResponse(dtos);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<DoctorDto>>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<PagedResponse<DoctorDto>>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm = null)
        {
            try
            {
                var query = _unitOfWork.Doctors.GetQueryable().Include(d => d.Department);

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    query = query.Include(d => d.Department);
                    //query = query.Where(d => d.FirstName.Contains(searchTerm) ||
                    //                       d.LastName.Contains(searchTerm) ||
                    //                       d.Specialization.Contains(searchTerm) ||
                    //                       (d.Email != null && d.Email.Contains(searchTerm)));
                }

                var totalCount = await query.CountAsync();
                var items = await query
                    .OrderBy(d => d.LastName)
                    .ThenBy(d => d.FirstName)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var dtos = _mapper.Map<List<DoctorDto>>(items);
                var pagedResponse = new PagedResponse<DoctorDto>
                {
                    Items = dtos,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };

                return ApiResponse<PagedResponse<DoctorDto>>.SuccessResponse(pagedResponse);
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<DoctorDto>>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<DoctorDto>> CreateAsync(CreateDoctorDto dto)
        {
            try
            {
                // Check if license number exists
                var exists = await _unitOfWork.Doctors.AnyAsync(d => d.LicenseNumber == dto.LicenseNumber);
                if (exists)
                    return ApiResponse<DoctorDto>.FailureResponse("License number already exists");

                // Check if department exists
                var department = await _unitOfWork.Departments.GetByIdAsync(dto.DepartmentId);
                if (department == null)
                    return ApiResponse<DoctorDto>.FailureResponse("Department not found");

                var doctor = _mapper.Map<Doctor>(dto);
                await _unitOfWork.Doctors.AddAsync(doctor);
                await _unitOfWork.CompleteAsync();

                // Reload with department
                doctor = await _unitOfWork.Doctors.GetQueryable()
                    .Include(d => d.Department)
                    .FirstOrDefaultAsync(d => d.Id == doctor.Id);

                var resultDto = _mapper.Map<DoctorDto>(doctor);
                return ApiResponse<DoctorDto>.SuccessResponse(resultDto, "Doctor created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<DoctorDto>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<DoctorDto>> UpdateAsync(UpdateDoctorDto dto)
        {
            try
            {
                var doctor = await _unitOfWork.Doctors.GetByIdAsync(dto.Id);
                if (doctor == null)
                    return ApiResponse<DoctorDto>.FailureResponse($"Doctor with ID {dto.Id} not found");

                // Check if department exists
                var department = await _unitOfWork.Departments.GetByIdAsync(dto.DepartmentId);
                if (department == null)
                    return ApiResponse<DoctorDto>.FailureResponse("Department not found");

                _mapper.Map(dto, doctor);
                _unitOfWork.Doctors.Update(doctor);
                await _unitOfWork.CompleteAsync();

                // Reload with department
                doctor = await _unitOfWork.Doctors.GetQueryable()
                    .Include(d => d.Department)
                    .FirstOrDefaultAsync(d => d.Id == doctor.Id);

                var resultDto = _mapper.Map<DoctorDto>(doctor);
                return ApiResponse<DoctorDto>.SuccessResponse(resultDto, "Doctor updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<DoctorDto>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            try
            {
                var doctor = await _unitOfWork.Doctors.GetByIdAsync(id);
                if (doctor == null)
                    return ApiResponse<bool>.FailureResponse($"Doctor with ID {id} not found");

                _unitOfWork.Doctors.Delete(doctor);
                await _unitOfWork.CompleteAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Doctor deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(int id)
        {
            try
            {
                await _unitOfWork.Doctors.SoftDeleteAsync(id);
                await _unitOfWork.CompleteAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Doctor soft deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<DoctorDto>>> GetByDepartmentAsync(int departmentId)
        {
            try
            {
                var doctors = await _unitOfWork.Doctors.GetQueryable()
                    .Include(d => d.Department)
                    .Where(d => d.DepartmentId == departmentId)
                    .ToListAsync();

                var dtos = _mapper.Map<List<DoctorDto>>(doctors);
                return ApiResponse<List<DoctorDto>>.SuccessResponse(dtos);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<DoctorDto>>.FailureResponse($"Error: {ex.Message}");
            }
        }
    }
}
