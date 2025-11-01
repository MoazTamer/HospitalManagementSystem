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
    public class AppointmentService : IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AppointmentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<AppointmentDto>> GetByIdAsync(int id)
        {
            try
            {
                var appointment = await _unitOfWork.Appointments.GetQueryable()
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .ThenInclude(d => d.Department)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (appointment == null)
                    return ApiResponse<AppointmentDto>.FailureResponse($"Appointment with ID {id} not found");

                var dto = _mapper.Map<AppointmentDto>(appointment);
                return ApiResponse<AppointmentDto>.SuccessResponse(dto);
            }
            catch (Exception ex)
            {
                return ApiResponse<AppointmentDto>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<AppointmentDto>>> GetAllAsync()
        {
            try
            {
                var appointments = await _unitOfWork.Appointments.GetQueryable()
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .ToListAsync();

                var dtos = _mapper.Map<List<AppointmentDto>>(appointments);
                return ApiResponse<List<AppointmentDto>>.SuccessResponse(dtos);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<AppointmentDto>>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<PagedResponse<AppointmentDto>>> GetPagedAsync(int pageNumber, int pageSize, string? status = null)
        {
            try
            {
                var query = _unitOfWork.Appointments.GetQueryable()
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor);

                if (!string.IsNullOrWhiteSpace(status))
                {
                    query = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Appointment, Doctor>)query.Where(a => a.Status == status);
                }

                var totalCount = await query.CountAsync();
                var items = await query
                    .OrderByDescending(a => a.AppointmentDate)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var dtos = _mapper.Map<List<AppointmentDto>>(items);
                var pagedResponse = new PagedResponse<AppointmentDto>
                {
                    Items = dtos,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };

                return ApiResponse<PagedResponse<AppointmentDto>>.SuccessResponse(pagedResponse);
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<AppointmentDto>>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<AppointmentDto>> CreateAsync(CreateAppointmentDto dto)
        {
            try
            {
                // Validate patient exists
                var patient = await _unitOfWork.Patients.GetByIdAsync(dto.PatientId);
                if (patient == null)
                    return ApiResponse<AppointmentDto>.FailureResponse("Patient not found");

                // Validate doctor exists
                var doctor = await _unitOfWork.Doctors.GetByIdAsync(dto.DoctorId);
                if (doctor == null)
                    return ApiResponse<AppointmentDto>.FailureResponse("Doctor not found");

                // Check for conflicting appointments
                var hasConflict = await _unitOfWork.Appointments.GetQueryable()
                    .AnyAsync(a => a.DoctorId == dto.DoctorId &&
                                  a.AppointmentDate == dto.AppointmentDate &&
                                  a.Status != "Cancelled");

                if (hasConflict)
                    return ApiResponse<AppointmentDto>.FailureResponse("Doctor already has an appointment at this time");

                var appointment = _mapper.Map<Appointment>(dto);
                await _unitOfWork.Appointments.AddAsync(appointment);
                await _unitOfWork.CompleteAsync();

                // Reload with related data
                appointment = await _unitOfWork.Appointments.GetQueryable()
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .FirstOrDefaultAsync(a => a.Id == appointment.Id);

                var resultDto = _mapper.Map<AppointmentDto>(appointment);
                return ApiResponse<AppointmentDto>.SuccessResponse(resultDto, "Appointment created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<AppointmentDto>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<AppointmentDto>> UpdateAsync(UpdateAppointmentDto dto)
        {
            try
            {
                var appointment = await _unitOfWork.Appointments.GetByIdAsync(dto.Id);
                if (appointment == null)
                    return ApiResponse<AppointmentDto>.FailureResponse($"Appointment with ID {dto.Id} not found");

                _mapper.Map(dto, appointment);
                _unitOfWork.Appointments.Update(appointment);
                await _unitOfWork.CompleteAsync();

                // Reload with related data
                appointment = await _unitOfWork.Appointments.GetQueryable()
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .FirstOrDefaultAsync(a => a.Id == appointment.Id);

                var resultDto = _mapper.Map<AppointmentDto>(appointment);
                return ApiResponse<AppointmentDto>.SuccessResponse(resultDto, "Appointment updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<AppointmentDto>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            try
            {
                var appointment = await _unitOfWork.Appointments.GetByIdAsync(id);
                if (appointment == null)
                    return ApiResponse<bool>.FailureResponse($"Appointment with ID {id} not found");

                _unitOfWork.Appointments.Delete(appointment);
                await _unitOfWork.CompleteAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Appointment deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> CancelAppointmentAsync(int id)
        {
            try
            {
                var appointment = await _unitOfWork.Appointments.GetByIdAsync(id);
                if (appointment == null)
                    return ApiResponse<bool>.FailureResponse($"Appointment with ID {id} not found");

                appointment.Status = "Cancelled";
                appointment.ModifiedDate = DateTime.UtcNow;

                _unitOfWork.Appointments.Update(appointment);
                await _unitOfWork.CompleteAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Appointment cancelled successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<AppointmentDto>>> GetByPatientAsync(int patientId)
        {
            try
            {
                var appointments = await _unitOfWork.Appointments.GetQueryable()
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .Where(a => a.PatientId == patientId)
                    .OrderByDescending(a => a.AppointmentDate)
                    .ToListAsync();

                var dtos = _mapper.Map<List<AppointmentDto>>(appointments);
                return ApiResponse<List<AppointmentDto>>.SuccessResponse(dtos);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<AppointmentDto>>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<AppointmentDto>>> GetByDoctorAsync(int doctorId)
        {
            try
            {
                var appointments = await _unitOfWork.Appointments.GetQueryable()
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .Where(a => a.DoctorId == doctorId)
                    .OrderByDescending(a => a.AppointmentDate)
                    .ToListAsync();

                var dtos = _mapper.Map<List<AppointmentDto>>(appointments);
                return ApiResponse<List<AppointmentDto>>.SuccessResponse(dtos);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<AppointmentDto>>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<AppointmentDto>>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var appointments = await _unitOfWork.Appointments.GetQueryable()
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .Where(a => a.AppointmentDate >= startDate && a.AppointmentDate <= endDate)
                    .OrderBy(a => a.AppointmentDate)
                    .ToListAsync();

                var dtos = _mapper.Map<List<AppointmentDto>>(appointments);
                return ApiResponse<List<AppointmentDto>>.SuccessResponse(dtos);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<AppointmentDto>>.FailureResponse($"Error: {ex.Message}");
            }
        }
    }
}
