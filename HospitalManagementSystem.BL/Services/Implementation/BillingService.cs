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
    public class BillingService : IBillingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BillingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<BillingDto>> GetByIdAsync(int id)
        {
            try
            {
                var billing = await _unitOfWork.Billings.GetQueryable()
                    .Include(b => b.Patient)
                    .FirstOrDefaultAsync(b => b.Id == id);

                if (billing == null)
                    return ApiResponse<BillingDto>.FailureResponse($"Billing with ID {id} not found");

                var dto = _mapper.Map<BillingDto>(billing);
                return ApiResponse<BillingDto>.SuccessResponse(dto);
            }
            catch (Exception ex)
            {
                return ApiResponse<BillingDto>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<BillingDto>>> GetAllAsync()
        {
            try
            {
                var billings = await _unitOfWork.Billings.GetQueryable()
                    .Include(b => b.Patient)
                    .ToListAsync();

                var dtos = _mapper.Map<List<BillingDto>>(billings);
                return ApiResponse<List<BillingDto>>.SuccessResponse(dtos);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<BillingDto>>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<PagedResponse<BillingDto>>> GetPagedAsync(int pageNumber, int pageSize, string? paymentStatus = null)
        {
            try
            {
                var query = _unitOfWork.Billings.GetQueryable().Include(b => b.Patient);

                if (!string.IsNullOrWhiteSpace(paymentStatus))
                {
                    query = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Billing, Patient>)query.Where(b => b.PaymentStatus == paymentStatus);
                }

                var totalCount = await query.CountAsync();
                var items = await query
                    .OrderByDescending(b => b.BillingDate)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var dtos = _mapper.Map<List<BillingDto>>(items);
                var pagedResponse = new PagedResponse<BillingDto>
                {
                    Items = dtos,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };

                return ApiResponse<PagedResponse<BillingDto>>.SuccessResponse(pagedResponse);
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<BillingDto>>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<BillingDto>> CreateAsync(CreateBillingDto dto)
        {
            try
            {
                // Validate patient exists
                var patient = await _unitOfWork.Patients.GetByIdAsync(dto.PatientId);
                if (patient == null)
                    return ApiResponse<BillingDto>.FailureResponse("Patient not found");

                var billing = _mapper.Map<Billing>(dto);
                await _unitOfWork.Billings.AddAsync(billing);
                await _unitOfWork.CompleteAsync();

                // Reload with related data
                billing = await _unitOfWork.Billings.GetQueryable()
                    .Include(b => b.Patient)
                    .FirstOrDefaultAsync(b => b.Id == billing.Id);

                var resultDto = _mapper.Map<BillingDto>(billing);
                return ApiResponse<BillingDto>.SuccessResponse(resultDto, "Billing created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<BillingDto>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<BillingDto>> UpdateAsync(UpdateBillingDto dto)
        {
            try
            {
                var billing = await _unitOfWork.Billings.GetByIdAsync(dto.Id);
                if (billing == null)
                    return ApiResponse<BillingDto>.FailureResponse($"Billing with ID {dto.Id} not found");

                _mapper.Map(dto, billing);
                billing.OutstandingAmount = billing.TotalAmount - billing.PaidAmount;

                _unitOfWork.Billings.Update(billing);
                await _unitOfWork.CompleteAsync();

                // Reload with related data
                billing = await _unitOfWork.Billings.GetQueryable()
                    .Include(b => b.Patient)
                    .FirstOrDefaultAsync(b => b.Id == billing.Id);

                var resultDto = _mapper.Map<BillingDto>(billing);
                return ApiResponse<BillingDto>.SuccessResponse(resultDto, "Billing updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<BillingDto>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            try
            {
                var billing = await _unitOfWork.Billings.GetByIdAsync(id);
                if (billing == null)
                    return ApiResponse<bool>.FailureResponse($"Billing with ID {id} not found");

                _unitOfWork.Billings.Delete(billing);
                await _unitOfWork.CompleteAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Billing deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<BillingDto>>> GetByPatientAsync(int patientId)
        {
            try
            {
                var billings = await _unitOfWork.Billings.GetQueryable()
                    .Include(b => b.Patient)
                    .Where(b => b.PatientId == patientId)
                    .OrderByDescending(b => b.BillingDate)
                    .ToListAsync();

                var dtos = _mapper.Map<List<BillingDto>>(billings);
                return ApiResponse<List<BillingDto>>.SuccessResponse(dtos);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<BillingDto>>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<BillingDto>> ProcessPaymentAsync(PaymentDto dto)
        {
            try
            {
                var billing = await _unitOfWork.Billings.GetQueryable()
                    .Include(b => b.Patient)
                    .FirstOrDefaultAsync(b => b.Id == dto.BillingId);

                if (billing == null)
                    return ApiResponse<BillingDto>.FailureResponse($"Billing with ID {dto.BillingId} not found");

                billing.PaidAmount += dto.Amount;
                billing.OutstandingAmount = billing.TotalAmount - billing.PaidAmount;
                billing.PaymentMethod = dto.PaymentMethod;
                billing.ModifiedDate = DateTime.UtcNow;

                if (billing.PaidAmount >= billing.TotalAmount)
                {
                    billing.PaymentStatus = "Paid";
                    billing.OutstandingAmount = 0;
                }
                else if (billing.PaidAmount > 0)
                {
                    billing.PaymentStatus = "PartiallyPaid";
                }

                _unitOfWork.Billings.Update(billing);
                await _unitOfWork.CompleteAsync();

                var resultDto = _mapper.Map<BillingDto>(billing);
                return ApiResponse<BillingDto>.SuccessResponse(resultDto, "Payment processed successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<BillingDto>.FailureResponse($"Error: {ex.Message}");
            }
        }
    }
}
