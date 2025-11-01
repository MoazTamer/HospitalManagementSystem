using AutoMapper;
using HospitalManagementSystem.BL.DTOs;
using HospitalManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.BL.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Department mappings
            CreateMap<Department, DepartmentDto>();
            CreateMap<CreateDepartmentDto, Department>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => "System"))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<UpdateDepartmentDto, Department>()
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => "System"))
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Doctor mappings
            CreateMap<Doctor, DoctorDto>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name));
            CreateMap<CreateDoctorDto, Doctor>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => "System"))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<UpdateDoctorDto, Doctor>()
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => "System"))
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Patient mappings
            CreateMap<Patient, PatientDto>();
            CreateMap<CreatePatientDto, Patient>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => "System"))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<UpdatePatientDto, Patient>()
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => "System"))
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Appointment mappings
            CreateMap<Appointment, AppointmentDto>()
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => $"{src.Patient.FirstName} {src.Patient.LastName}"))
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => $"{src.Doctor.FirstName} {src.Doctor.LastName}"));
            CreateMap<CreateAppointmentDto, Appointment>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Scheduled"))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => "System"))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<UpdateAppointmentDto, Appointment>()
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => "System"))
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

            // MedicalRecord mappings
            CreateMap<MedicalRecord, MedicalRecordDto>()
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => $"{src.Patient.FirstName} {src.Patient.LastName}"))
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => $"{src.Doctor.FirstName} {src.Doctor.LastName}"));
            CreateMap<CreateMedicalRecordDto, MedicalRecord>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => "System"))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<UpdateMedicalRecordDto, MedicalRecord>()
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => "System"))
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Prescription mappings
            CreateMap<Prescription, PrescriptionDto>();
            CreateMap<CreatePrescriptionDto, Prescription>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => "System"))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<UpdatePrescriptionDto, Prescription>()
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => "System"))
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Billing mappings
            CreateMap<Billing, BillingDto>()
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => $"{src.Patient.FirstName} {src.Patient.LastName}"));
            CreateMap<CreateBillingDto, Billing>()
                .ForMember(dest => dest.OutstandingAmount, opt => opt.MapFrom(src => src.TotalAmount - src.PaidAmount))
                .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.PaidAmount >= src.TotalAmount ? "Paid" : src.PaidAmount > 0 ? "PartiallyPaid" : "Pending"))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => "System"))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<UpdateBillingDto, Billing>()
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => "System"))
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

            // User mappings
            CreateMap<User, UserDto>();
        }
    }
}
