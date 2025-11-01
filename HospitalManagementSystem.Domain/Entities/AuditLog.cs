using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Domain.Entities
{
    public class AuditLog
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string EntityName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Action { get; set; } = string.Empty; // Create, Update, Delete

        public int EntityId { get; set; }

        public string? OldValues { get; set; } // JSON

        public string? NewValues { get; set; } // JSON

        [Required]
        [MaxLength(100)]
        public string ChangedBy { get; set; } = string.Empty;

        [Required]
        public DateTime ChangedDate { get; set; } = DateTime.UtcNow;

        [MaxLength(500)]
        public string? Changes { get; set; }
    }
}
