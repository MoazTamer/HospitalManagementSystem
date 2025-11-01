using HospitalManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.DAL.Configurations
{
    public class BillingConfiguration : IEntityTypeConfiguration<Billing>
    {
        public void Configure(EntityTypeBuilder<Billing> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.BillingDate)
                .IsRequired();

            builder.Property(b => b.TotalAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(b => b.PaidAmount)
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0);

            builder.Property(b => b.OutstandingAmount)
                .HasColumnType("decimal(18,2)");

            builder.Property(b => b.PaymentStatus)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Pending");

            builder.Property(b => b.PaymentMethod)
                .HasMaxLength(50);

            builder.Property(b => b.Description)
                .HasMaxLength(500);

            builder.Property(b => b.Notes)
                .HasMaxLength(1000);

            // Indexes
            builder.HasIndex(b => new { b.PatientId, b.BillingDate });
            builder.HasIndex(b => b.PaymentStatus);

            // Relationship configured in Patient configuration
        }
    }
}
