using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.BL.DTOs
{
    public class MedicalRecordDto
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public int DoctorId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public DateTime VisitDate { get; set; }
        public string Diagnosis { get; set; } = string.Empty;
        public string? Symptoms { get; set; }
        public string? Treatment { get; set; }
        public string? Notes { get; set; }
        public List<PrescriptionDto> Prescriptions { get; set; } = new List<PrescriptionDto>();
    }

    public class CreateMedicalRecordDto
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime VisitDate { get; set; }
        public string Diagnosis { get; set; } = string.Empty;
        public string? Symptoms { get; set; }
        public string? Treatment { get; set; }
        public string? Notes { get; set; }
    }

    public class UpdateMedicalRecordDto
    {
        public int Id { get; set; }
        public string Diagnosis { get; set; } = string.Empty;
        public string? Symptoms { get; set; }
        public string? Treatment { get; set; }
        public string? Notes { get; set; }
    }

    public class PrescriptionDto
    {
        public int Id { get; set; }
        public int MedicalRecordId { get; set; }
        public string MedicationName { get; set; } = string.Empty;
        public string Dosage { get; set; } = string.Empty;
        public string Frequency { get; set; } = string.Empty;
        public int DurationDays { get; set; }
        public string? Instructions { get; set; }
    }

    public class CreatePrescriptionDto
    {
        public int MedicalRecordId { get; set; }
        public string MedicationName { get; set; } = string.Empty;
        public string Dosage { get; set; } = string.Empty;
        public string Frequency { get; set; } = string.Empty;
        public int DurationDays { get; set; }
        public string? Instructions { get; set; }
    }

    public class UpdatePrescriptionDto
    {
        public int Id { get; set; }
        public string MedicationName { get; set; } = string.Empty;
        public string Dosage { get; set; } = string.Empty;
        public string Frequency { get; set; } = string.Empty;
        public int DurationDays { get; set; }
        public string? Instructions { get; set; }
    }
}
