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
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<DepartmentDto>> GetByIdAsync(int id)
        {
            try
            {
                var department = await _unitOfWork.Departments.GetByIdAsync(id);
                if (department == null)
                    return ApiResponse<DepartmentDto>.FailureResponse($"Department with ID {id} not found");

                var dto = _mapper.Map<DepartmentDto>(department);
                return ApiResponse<DepartmentDto>.SuccessResponse(dto);
            }
            catch (Exception ex)
            {
                return ApiResponse<DepartmentDto>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<DepartmentDto>>> GetAllAsync()
        {
            try
            {
                var departments = await _unitOfWork.Departments.GetAllAsync();
                var dtos = _mapper.Map<List<DepartmentDto>>(departments);
                return ApiResponse<List<DepartmentDto>>.SuccessResponse(dtos);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<DepartmentDto>>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<PagedResponse<DepartmentDto>>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm = null)
        {
            try
            {
                var query = _unitOfWork.Departments.GetQueryable();

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    //query = query.Where(d => d.Name.Contains(searchTerm) ||
                    //(d.Description != null && d.Description.Contains(searchTerm)));
                    query = query.Where(d =>
                    (!string.IsNullOrEmpty(d.Name) && d.Name.Contains(searchTerm)) ||
                    (!string.IsNullOrEmpty(d.Description) && d.Description.Contains(searchTerm)));

                }

                var totalCount = await query.CountAsync();
                var items = await query
                    .OrderBy(d => d.Name)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var dtos = _mapper.Map<List<DepartmentDto>>(items);
                var pagedResponse = new PagedResponse<DepartmentDto>
                {
                    Items = dtos,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };

                return ApiResponse<PagedResponse<DepartmentDto>>.SuccessResponse(pagedResponse);
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<DepartmentDto>>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<DepartmentDto>> CreateAsync(CreateDepartmentDto dto)
        {
            try
            {
                // Check if department name already exists
                //var exists = await _unitOfWork.Departments.AnyAsync(d => d.Name == dto.Name);
                var exists = await _unitOfWork.Departments
                    .AnyAsync(d => d.Name != null && d.Name.ToLower() == dto.Name.ToLower());

                if (exists)
                    return ApiResponse<DepartmentDto>.FailureResponse("Department with this name already exists");

                var department = _mapper.Map<Department>(dto);
                await _unitOfWork.Departments.AddAsync(department);
                await _unitOfWork.CompleteAsync();

                var resultDto = _mapper.Map<DepartmentDto>(department);
                return ApiResponse<DepartmentDto>.SuccessResponse(resultDto, "Department created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<DepartmentDto>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<DepartmentDto>> UpdateAsync(UpdateDepartmentDto dto)
        {
            try
            {
                var department = await _unitOfWork.Departments.GetByIdAsync(dto.Id);
                if (department == null)
                    return ApiResponse<DepartmentDto>.FailureResponse($"Department with ID {dto.Id} not found");

                // Check if new name conflicts with existing department
                var exists = await _unitOfWork.Departments.AnyAsync(d => d.Name == dto.Name && d.Id != dto.Id);
                if (exists)
                    return ApiResponse<DepartmentDto>.FailureResponse("Department with this name already exists");

                _mapper.Map(dto, department);
                _unitOfWork.Departments.Update(department);
                await _unitOfWork.CompleteAsync();

                var resultDto = _mapper.Map<DepartmentDto>(department);
                return ApiResponse<DepartmentDto>.SuccessResponse(resultDto, "Department updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<DepartmentDto>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            try
            {
                var department = await _unitOfWork.Departments.GetByIdAsync(id);
                if (department == null)
                    return ApiResponse<bool>.FailureResponse($"Department with ID {id} not found");

                _unitOfWork.Departments.Delete(department);
                await _unitOfWork.CompleteAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Department deleted successfully");
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
                await _unitOfWork.Departments.SoftDeleteAsync(id);
                await _unitOfWork.CompleteAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Department soft deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.FailureResponse($"Error: {ex.Message}");
            }
        }
    }
}
