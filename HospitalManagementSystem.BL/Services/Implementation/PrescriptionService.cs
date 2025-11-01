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
    public class PrescriptionService : IPrescriptionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PrescriptionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<PrescriptionDto>> GetByIdAsync(int id)
        {
            try
            {
                var prescription = await _unitOfWork.Prescriptions.GetByIdAsync(id);
                if (prescription == null)
                    return ApiResponse<PrescriptionDto>.FailureResponse($"Prescription with ID {id} not found");

                var dto = _mapper.Map<PrescriptionDto>(prescription);
                return ApiResponse<PrescriptionDto>.SuccessResponse(dto);
            }
            catch (Exception ex)
            {
                return ApiResponse<PrescriptionDto>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<PrescriptionDto>>> GetByMedicalRecordAsync(int medicalRecordId)
        {
            try
            {
                var prescriptions = await _unitOfWork.Prescriptions.GetQueryable()
                    .Where(p => p.MedicalRecordId == medicalRecordId)
                    .ToListAsync();

                var dtos = _mapper.Map<List<PrescriptionDto>>(prescriptions);
                return ApiResponse<List<PrescriptionDto>>.SuccessResponse(dtos);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<PrescriptionDto>>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<PrescriptionDto>> CreateAsync(CreatePrescriptionDto dto)
        {
            try
            {
                // Validate medical record exists
                var medicalRecord = await _unitOfWork.MedicalRecords.GetByIdAsync(dto.MedicalRecordId);
                if (medicalRecord == null)
                    return ApiResponse<PrescriptionDto>.FailureResponse("Medical record not found");

                var prescription = _mapper.Map<Prescription>(dto);
                await _unitOfWork.Prescriptions.AddAsync(prescription);
                await _unitOfWork.CompleteAsync();

                var resultDto = _mapper.Map<PrescriptionDto>(prescription);
                return ApiResponse<PrescriptionDto>.SuccessResponse(resultDto, "Prescription created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<PrescriptionDto>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<PrescriptionDto>> UpdateAsync(UpdatePrescriptionDto dto)
        {
            try
            {
                var prescription = await _unitOfWork.Prescriptions.GetByIdAsync(dto.Id);
                if (prescription == null)
                    return ApiResponse<PrescriptionDto>.FailureResponse($"Prescription with ID {dto.Id} not found");

                _mapper.Map(dto, prescription);
                _unitOfWork.Prescriptions.Update(prescription);
                await _unitOfWork.CompleteAsync();

                var resultDto = _mapper.Map<PrescriptionDto>(prescription);
                return ApiResponse<PrescriptionDto>.SuccessResponse(resultDto, "Prescription updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<PrescriptionDto>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            try
            {
                var prescription = await _unitOfWork.Prescriptions.GetByIdAsync(id);
                if (prescription == null)
                    return ApiResponse<bool>.FailureResponse($"Prescription with ID {id} not found");

                _unitOfWork.Prescriptions.Delete(prescription);
                await _unitOfWork.CompleteAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Prescription deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.FailureResponse($"Error: {ex.Message}");
            }
        }
    }
}
