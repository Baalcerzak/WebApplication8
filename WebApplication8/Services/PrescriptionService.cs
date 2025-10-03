using Microsoft.EntityFrameworkCore;
using WebApplication8.Data;
using WebApplication8.DTO;
using WebApplication8.Models;

namespace WebApplication8.Services;

public class PrescriptionService : IPrescriptionService
{
    private readonly AppDbContext _context;

    public PrescriptionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PrescriptionResponseDto?> GetPrescriptionAsync(int id)
    {
        var prescription = await _context.Prescriptions
            .Include(p => p.Patient)
            .Include(p => p.Doctor)
            .Include(p => p.PMs)
                .ThenInclude(pm => pm.Medicament)
            .FirstOrDefaultAsync(p => p.IdPrescription == id);

        if (prescription == null) return null;

        return new PrescriptionResponseDto
        {
            IdPrescription = prescription.IdPrescription,
            Date = prescription.Date,
            DueDate = prescription.DueDate,
            PatientName = $"{prescription.Patient.FirstName} {prescription.Patient.LastName}",
            DoctorName = $"{prescription.Doctor.FirstName} {prescription.Doctor.LastName}",
            Medications = prescription.PMs.Select(pm => new PrescriptionMedicamentDetailsDto
            {
                MedicamentName = pm.Medicament.Name,
                Dose = pm.Dose,
                Details = pm.Details
            }).ToList()
        };
    }

    public async Task<PrescriptionResponseDto> AddPrescriptionAsync(CreatePrescriptionRequestDto dto)
    {
        if (dto.DueDate <= dto.Date)
            throw new ArgumentException("DueDate must be after Date");

        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var prescription = new Prescription
            {
                Date = dto.Date,
                DueDate = dto.DueDate,
                IdPatient = dto.IdPatient,
                IdDoctor = dto.IdDoctor
            };

            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();

            foreach (var med in dto.Medications)
            {
                var prescriptionMed = new Prescription_Medicament
                {
                    IdPrescription = prescription.IdPrescription,
                    IdMedicament = med.IdMedicament,
                    Dose = med.Dose,
                    Details = med.Details
                };
                _context.PMs.Add(prescriptionMed);
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return await GetPrescriptionAsync(prescription.IdPrescription) ?? throw new Exception("Prescription not found");
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
