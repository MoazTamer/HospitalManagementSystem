🏥 Hospital Management System API
<div align="center">
Show Image
Show Image
Show Image
Show Image
Show Image
A comprehensive Hospital Management System built with ASP.NET Core Web API
Features • Architecture • Installation • Documentation • API Reference
</div>

📋 Overview
A full-featured Hospital Management System API that handles all aspects of hospital operations including patient management, appointments, medical records, prescriptions, and billing. Built with enterprise-level architecture and best practices.
✨ Features
🔐 Authentication & Authorization

JWT Token-based authentication
BCrypt password hashing
Role-based access control (Admin, Doctor, Receptionist, Patient)
Secure API endpoints

🏥 Core Functionality

Department Management - Organize hospital departments
Doctor Management - Manage doctor profiles and assignments
Patient Management - Complete patient records system
Appointment Scheduling - Book and manage appointments
Medical Records - Comprehensive medical history tracking
Prescriptions - Digital prescription management
Billing & Payments - Invoice generation and payment processing

🎯 Technical Features

✅ 61 RESTful API Endpoints
✅ Clean Architecture (4 layers)
✅ Repository & Unit of Work Pattern
✅ Automatic Audit Logging
✅ Soft Delete Implementation
✅ Pagination & Advanced Search
✅ Global Error Handling
✅ Structured Logging (Serilog)
✅ API Documentation (Swagger/OpenAPI)
✅ Input Validation (FluentValidation)
✅ DTO Mapping (AutoMapper)


🏗️ Architecture
┌─────────────────────────────────────────┐
│           HMS.API (Presentation)        │
│  Controllers, Middleware, Filters       │
└─────────────┬───────────────────────────┘
              │
┌─────────────▼───────────────────────────┐
│        HMS.Business (Application)       │
│  Services, DTOs, Validators, Mappings   │
└─────────────┬───────────────────────────┘
              │
┌─────────────▼───────────────────────────┐
│      HMS.DataAccess (Infrastructure)    │
│  Repositories, UnitOfWork, DbContext    │
└─────────────┬───────────────────────────┘
              │
┌─────────────▼───────────────────────────┐
│          HMS.Domain (Core)              │
│         Entities, Interfaces            │
└─────────────────────────────────────────┘
Design Patterns

🎨 Clean Architecture
🏪 Repository Pattern
🔄 Unit of Work
🗺️ DTO Pattern
🏭 Factory Pattern
💉 Dependency Injection


🔑 Default Users
UsernamePasswordRoleDescriptionadminAdmin@123AdminFull system accessdoctor1Doctor@123DoctorMedical operationsreceptionistReception@123ReceptionistAppointments & billingpatient1Patient@123PatientPersonal data access

📚 API Documentation
Authentication Endpoints
MethodEndpointDescriptionPOST/api/Auth/loginUser loginPOST/api/Auth/registerRegister new userGET/api/Auth/meGet current userPOST/api/Auth/change-passwordChange password
Main Endpoints Summary

Departments: 7 endpoints (CRUD + pagination + soft delete)
Doctors: 8 endpoints (CRUD + filters + pagination)
Patients: 8 endpoints (CRUD + search + pagination)
Appointments: 10 endpoints (CRUD + filters + cancel)
Medical Records: 8 endpoints (CRUD + filters)
Prescriptions: 5 endpoints (CRUD + filters)
Billings: 8 endpoints (CRUD + payment processing)

Total: 61 Endpoints
🛠️ Tech Stack
CategoryTechnologiesFrameworkASP.NET Core 8.0LanguageC# 12ORMEntity Framework Core 8.0DatabaseSQL ServerAuthenticationJWT BearerValidationFluentValidationMappingAutoMapperLoggingSerilogDocumentationSwagger/OpenAPIPassword HashingBCrypt.Net


👨‍💻 Author
Moaz Tamer

GitHub: @MoazTamer
LinkedIn: moaz-tamer-8365591bb/
Email: moaztamer390@gmail.com


<div align="center">
⭐ Star this repository if you find it helpful! ⭐
Made with ❤️ using ASP.NET Core
</div>
