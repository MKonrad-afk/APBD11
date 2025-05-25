using Apbd11.Properties.models;
using Microsoft.EntityFrameworkCore;

namespace Apbd11.Properties.repository;

public class PrescriptionRepository : IPrescriptionRepository
{
    private readonly AppDbContext _context;

    public PrescriptionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Patient?> GetOrCreatePatientAsync(PatientDto dto)
    {
        var patient = await _context.Patients
            .FirstOrDefaultAsync(p => p.FirstName == dto.FirstName && p.LastName == dto.LastName && p.Birthdate == dto.Birthdate);

        if (patient == null)
        {
            patient = new Patient
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Birthdate = dto.Birthdate
            };
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
        }

        return patient;
    }

    public async Task<bool> MedicamentsExistAsync(IEnumerable<int> ids)
    {
        var count = await _context.Medicaments.CountAsync(m => ids.Contains(m.IdMedicament));
        return count == ids.Count();
    }

    public async Task AddPrescriptionAsync(Prescription prescription)
    {
        _context.Prescriptions.Add(prescription);
        await _context.SaveChangesAsync();
    }

    public async Task<Patient?> GetPatientDetailsAsync(int patientId)
    {
        return await _context.Patients
            .Include(p => p.Prescriptions)
            .ThenInclude(pr => pr.Doctor)
            .Include(p => p.Prescriptions)
            .ThenInclude(pr => pr.PrescriptionMedicaments)
            .ThenInclude(pm => pm.Medicament)
            .FirstOrDefaultAsync(p => p.IdPatient == patientId);
    }
}
