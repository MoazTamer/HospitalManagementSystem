using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.BL.DTOs
{
    public class BillingDto
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public DateTime BillingDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal OutstandingAmount { get; set; }
        public string PaymentStatus { get; set; } = string.Empty;
        public string? PaymentMethod { get; set; }
        public string? Description { get; set; }
        public string? Notes { get; set; }
    }

    public class CreateBillingDto
    {
        public int PatientId { get; set; }
        public DateTime BillingDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; } = 0;
        public string? PaymentMethod { get; set; }
        public string? Description { get; set; }
        public string? Notes { get; set; }
    }

    public class UpdateBillingDto
    {
        public int Id { get; set; }
        public decimal PaidAmount { get; set; }
        public string PaymentStatus { get; set; } = string.Empty;
        public string? PaymentMethod { get; set; }
        public string? Notes { get; set; }
    }

    public class PaymentDto
    {
        public int BillingId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
    }
}
