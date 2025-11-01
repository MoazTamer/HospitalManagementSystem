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
    public class PatientService : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PatientService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<PatientDto>> GetByIdAsync(int id)
        {
            try
            {
                var patient = await _unitOfWork.Patients.GetByIdAsync(id);
                if (patient == null)
                    return ApiResponse<PatientDto>.FailureResponse($"Patient with ID {id} not found");

                var dto = _mapper.Map<PatientDto>(patient);
                return ApiResponse<PatientDto>.SuccessResponse(dto);
            }
            catch (Exception ex)
            {
                return ApiResponse<PatientDto>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<PatientDto>>> GetAllAsync()
        {
            try
            {
                var patients = await _unitOfWork.Patients.GetAllAsync();
                var dtos = _mapper.Map<List<PatientDto>>(patients);
                return ApiResponse<List<PatientDto>>.SuccessResponse(dtos);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<PatientDto>>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<PagedResponse<PatientDto>>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm = null)
        {
            try
            {
                var query = _unitOfWork.Patients.GetQueryable();

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    query = query.Where(p => p.FirstName.Contains(searchTerm) ||
                                           p.LastName.Contains(searchTerm) ||
                                           (p.Email != null && p.Email.Contains(searchTerm)) ||
                                           (p.Phone != null && p.Phone.Contains(searchTerm)));
                }

                var totalCount = await query.CountAsync();
                var items = await query
                    .OrderBy(p => p.LastName)
                    .ThenBy(p => p.FirstName)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var dtos = _mapper.Map<List<PatientDto>>(items);
                var pagedResponse = new PagedResponse<PatientDto>
                {
                    Items = dtos,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };

                return ApiResponse<PagedResponse<PatientDto>>.SuccessResponse(pagedResponse);
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<PatientDto>>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<PatientDto>> CreateAsync(CreatePatientDto dto)
        {
            try
            {
                var patient = _mapper.Map<Patient>(dto);
                await _unitOfWork.Patients.AddAsync(patient);
                await _unitOfWork.CompleteAsync();

                var resultDto = _mapper.Map<PatientDto>(patient);
                return ApiResponse<PatientDto>.SuccessResponse(resultDto, "Patient created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<PatientDto>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<PatientDto>> UpdateAsync(UpdatePatientDto dto)
        {
            try
            {
                var patient = await _unitOfWork.Patients.GetByIdAsync(dto.Id);
                if (patient == null)
                    return ApiResponse<PatientDto>.FailureResponse($"Patient with ID {dto.Id} not found");

                _mapper.Map(dto, patient);
                _unitOfWork.Patients.Update(patient);
                await _unitOfWork.CompleteAsync();

                var resultDto = _mapper.Map<PatientDto>(patient);
                return ApiResponse<PatientDto>.SuccessResponse(resultDto, "Patient updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<PatientDto>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            try
            {
                var patient = await _unitOfWork.Patients.GetByIdAsync(id);
                if (patient == null)
                    return ApiResponse<bool>.FailureResponse($"Patient with ID {id} not found");

                _unitOfWork.Patients.Delete(patient);
                await _unitOfWork.CompleteAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Patient deleted successfully");
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
                await _unitOfWork.Patients.SoftDeleteAsync(id);
                await _unitOfWork.CompleteAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Patient soft deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<PatientDto>>> SearchByNameAsync(string name)
        {
            try
            {
                var patients = await _unitOfWork.Patients
                    .FindAsync(p => p.FirstName.Contains(name) || p.LastName.Contains(name));

                var dtos = _mapper.Map<List<PatientDto>>(patients);
                return ApiResponse<List<PatientDto>>.SuccessResponse(dtos);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<PatientDto>>.FailureResponse($"Error: {ex.Message}");
            }
        }
    }
}
