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
    public class MedicalRecordService : IMedicalRecordService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MedicalRecordService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<MedicalRecordDto>> GetByIdAsync(int id)
        {
            try
            {
                var record = await _unitOfWork.MedicalRecords.GetQueryable()
                    .Include(mr => mr.Patient)
                    .Include(mr => mr.Doctor)
                    .Include(mr => mr.Prescriptions)
                    .FirstOrDefaultAsync(mr => mr.Id == id);

                if (record == null)
                    return ApiResponse<MedicalRecordDto>.FailureResponse($"Medical record with ID {id} not found");

                var dto = _mapper.Map<MedicalRecordDto>(record);
                return ApiResponse<MedicalRecordDto>.SuccessResponse(dto);
            }
            catch (Exception ex)
            {
                return ApiResponse<MedicalRecordDto>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<MedicalRecordDto>>> GetAllAsync()
        {
            try
            {
                var records = await _unitOfWork.MedicalRecords.GetQueryable()
                    .Include(mr => mr.Patient)
                    .Include(mr => mr.Doctor)
                    .Include(mr => mr.Prescriptions)
                    .ToListAsync();

                var dtos = _mapper.Map<List<MedicalRecordDto>>(records);
                return ApiResponse<List<MedicalRecordDto>>.SuccessResponse(dtos);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<MedicalRecordDto>>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<PagedResponse<MedicalRecordDto>>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm = null)
        {
            try
            {
                var query = _unitOfWork.MedicalRecords.GetQueryable()
                    .Include(mr => mr.Patient)
                    .Include(mr => mr.Doctor)
                    .Include(mr => mr.Prescriptions);

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    query = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<MedicalRecord, ICollection<Prescription>>)query.Where(mr => mr.Diagnosis.Contains(searchTerm) ||
                                            (mr.Symptoms != null && mr.Symptoms.Contains(searchTerm)) ||
                                            mr.Patient.FirstName.Contains(searchTerm) ||
                                            mr.Patient.LastName.Contains(searchTerm));
                }

                var totalCount = await query.CountAsync();
                var items = await query
                    .OrderByDescending(mr => mr.VisitDate)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var dtos = _mapper.Map<List<MedicalRecordDto>>(items);
                var pagedResponse = new PagedResponse<MedicalRecordDto>
                {
                    Items = dtos,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };

                return ApiResponse<PagedResponse<MedicalRecordDto>>.SuccessResponse(pagedResponse);
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<MedicalRecordDto>>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<MedicalRecordDto>> CreateAsync(CreateMedicalRecordDto dto)
        {
            try
            {
                // Validate patient exists
                var patient = await _unitOfWork.Patients.GetByIdAsync(dto.PatientId);
                if (patient == null)
                    return ApiResponse<MedicalRecordDto>.FailureResponse("Patient not found");

                // Validate doctor exists
                var doctor = await _unitOfWork.Doctors.GetByIdAsync(dto.DoctorId);
                if (doctor == null)
                    return ApiResponse<MedicalRecordDto>.FailureResponse("Doctor not found");

                var record = _mapper.Map<MedicalRecord>(dto);
                await _unitOfWork.MedicalRecords.AddAsync(record);
                await _unitOfWork.CompleteAsync();

                // Reload with related data
                record = await _unitOfWork.MedicalRecords.GetQueryable()
                    .Include(mr => mr.Patient)
                    .Include(mr => mr.Doctor)
                    .Include(mr => mr.Prescriptions)
                    .FirstOrDefaultAsync(mr => mr.Id == record.Id);

                var resultDto = _mapper.Map<MedicalRecordDto>(record);
                return ApiResponse<MedicalRecordDto>.SuccessResponse(resultDto, "Medical record created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<MedicalRecordDto>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<MedicalRecordDto>> UpdateAsync(UpdateMedicalRecordDto dto)
        {
            try
            {
                var record = await _unitOfWork.MedicalRecords.GetByIdAsync(dto.Id);
                if (record == null)
                    return ApiResponse<MedicalRecordDto>.FailureResponse($"Medical record with ID {dto.Id} not found");

                _mapper.Map(dto, record);
                _unitOfWork.MedicalRecords.Update(record);
                await _unitOfWork.CompleteAsync();

                // Reload with related data
                record = await _unitOfWork.MedicalRecords.GetQueryable()
                    .Include(mr => mr.Patient)
                    .Include(mr => mr.Doctor)
                    .Include(mr => mr.Prescriptions)
                    .FirstOrDefaultAsync(mr => mr.Id == record.Id);

                var resultDto = _mapper.Map<MedicalRecordDto>(record);
                return ApiResponse<MedicalRecordDto>.SuccessResponse(resultDto, "Medical record updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<MedicalRecordDto>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            try
            {
                var record = await _unitOfWork.MedicalRecords.GetByIdAsync(id);
                if (record == null)
                    return ApiResponse<bool>.FailureResponse($"Medical record with ID {id} not found");

                _unitOfWork.MedicalRecords.Delete(record);
                await _unitOfWork.CompleteAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Medical record deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<MedicalRecordDto>>> GetByPatientAsync(int patientId)
        {
            try
            {
                var records = await _unitOfWork.MedicalRecords.GetQueryable()
                    .Include(mr => mr.Patient)
                    .Include(mr => mr.Doctor)
                    .Include(mr => mr.Prescriptions)
                    .Where(mr => mr.PatientId == patientId)
                    .OrderByDescending(mr => mr.VisitDate)
                    .ToListAsync();

                var dtos = _mapper.Map<List<MedicalRecordDto>>(records);
                return ApiResponse<List<MedicalRecordDto>>.SuccessResponse(dtos);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<MedicalRecordDto>>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<MedicalRecordDto>>> GetByDoctorAsync(int doctorId)
        {
            try
            {
                var records = await _unitOfWork.MedicalRecords.GetQueryable()
                    .Include(mr => mr.Patient)
                    .Include(mr => mr.Doctor)
                    .Include(mr => mr.Prescriptions)
                    .Where(mr => mr.DoctorId == doctorId)
                    .OrderByDescending(mr => mr.VisitDate)
                    .ToListAsync();

                var dtos = _mapper.Map<List<MedicalRecordDto>>(records);
                return ApiResponse<List<MedicalRecordDto>>.SuccessResponse(dtos);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<MedicalRecordDto>>.FailureResponse($"Error: {ex.Message}");
            }
        }
    }
}
