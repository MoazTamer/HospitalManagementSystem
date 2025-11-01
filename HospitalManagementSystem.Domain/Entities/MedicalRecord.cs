using HospitalManagementSystem.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Domain.Entities
{
    public class MedicalRecord : BaseEntity
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }

        [Required]
        public DateTime VisitDate { get; set; }

        [Required]
        [MaxLength(200)]
        public string Diagnosis { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string? Symptoms { get; set; }

        [MaxLength(2000)]
        public string? Treatment { get; set; }

        [MaxLength(1000)]
        public string? Notes { get; set; }

        // Navigation Properties
        public virtual Patient Patient { get; set; } = null!;
        public virtual Doctor Doctor { get; set; } = null!;
        public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
    }
}
