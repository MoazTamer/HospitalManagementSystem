using HospitalManagementSystem.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Domain.Entities
{
    public class Prescription : BaseEntity
    {
        public int MedicalRecordId { get; set; }

        [Required]
        [MaxLength(200)]
        public string MedicationName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Dosage { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Frequency { get; set; } = string.Empty;

        public int DurationDays { get; set; }

        [MaxLength(500)]
        public string? Instructions { get; set; }

        // Navigation Properties
        public virtual MedicalRecord MedicalRecord { get; set; } = null!;
    }
}
