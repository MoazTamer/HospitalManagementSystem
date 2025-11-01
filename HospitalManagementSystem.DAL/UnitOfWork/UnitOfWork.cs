using HospitalManagementSystem.DAL.Data;
using HospitalManagementSystem.DAL.Repositories;
using HospitalManagementSystem.DAL.Repositories.Implementation;
using HospitalManagementSystem.DAL.Repositories.Interfaces;
using HospitalManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            Departments = new GenericRepository<Department>(_context);
            Doctors = new GenericRepository<Doctor>(_context);
            Patients = new GenericRepository<Patient>(_context);
            Appointments = new GenericRepository<Appointment>(_context);
            MedicalRecords = new GenericRepository<MedicalRecord>(_context);
            Prescriptions = new GenericRepository<Prescription>(_context);
            Billings = new GenericRepository<Billing>(_context);
            AuditLogs = new GenericRepository<AuditLog>(_context);
            Users = new GenericRepository<User>(_context);
        }

        public IGenericRepository<Department> Departments { get; }
        public IGenericRepository<Doctor> Doctors { get; }
        public IGenericRepository<Patient> Patients { get; }
        public IGenericRepository<Appointment> Appointments { get; }
        public IGenericRepository<MedicalRecord> MedicalRecords { get; }
        public IGenericRepository<Prescription> Prescriptions { get; }
        public IGenericRepository<Billing> Billings { get; }
        public IGenericRepository<AuditLog> AuditLogs { get; }
        public IGenericRepository<User> Users { get; }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                }
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}
