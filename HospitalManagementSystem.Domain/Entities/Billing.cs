using HospitalManagementSystem.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Domain.Entities
{
    public class Billing : BaseEntity
    {
        public int PatientId { get; set; }

        [Required]
        public DateTime BillingDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PaidAmount { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal OutstandingAmount { get; set; }

        [Required]
        [MaxLength(50)]
        public string PaymentStatus { get; set; } = "Pending"; // Pending, Paid, PartiallyPaid, Cancelled

        [MaxLength(50)]
        public string? PaymentMethod { get; set; } // Cash, Card, Insurance, Online

        [MaxLength(500)]
        public string? Description { get; set; }

        [MaxLength(1000)]
        public string? Notes { get; set; }

        // Navigation Properties
        public virtual Patient Patient { get; set; } = null!;
    }
}
