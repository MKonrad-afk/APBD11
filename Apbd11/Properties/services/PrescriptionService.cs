using Apbd11.Properties.models;
using Apbd11.Properties.repository;

namespace Apbd11.Properties.services;

public class PrescriptionService : IPrescriptionService
{
    private readonly IPrescriptionRepository _repository;

    public PrescriptionService(IPrescriptionRepository repository)
    {
        _repository = repository;
    }

    public async Task AddPrescriptionAsync(PrescriptionRequestDto dto)
    {
        if (dto.Medicaments.Count > 10)
            throw new ArgumentException("Prescription cannot have more than 10 medicaments.");

        if (dto.DueDate < dto.Date)
            throw new ArgumentException("Due date must be later than or equal to start date.");

        var patient = await _repository.GetOrCreatePatientAsync(dto.Patient);

        var medicamentIds = dto.Medicaments.Select(m => m.IdMedicament);
        var medicamentsExist = await _repository.MedicamentsExistAsync(medicamentIds);

        if (!medicamentsExist)
            throw new ArgumentException("One or more medicaments do not exist.");

        var prescription = new Prescription
        {
            Date = dto.Date,
            DueDate = dto.DueDate,
            IdPatient = patient!.IdPatient,
            IdDoctor = dto.IdDoctor,
            PrescriptionMedicaments = dto.Medicaments.Select(m => new PrescriptionMedicament
            {
                IdMedicament = m.IdMedicament,
                Dose = m.Dose,
                Details = m.Details
            }).ToList()
        };

        await _repository.AddPrescriptionAsync(prescription);
    }

    public async Task<PatientDetailsDto?> GetPatientDetailsAsync(int patientId)
    {
        var patient = await _repository.GetPatientDetailsAsync(patientId);
        if (patient == null) return null;

        return new PatientDetailsDto
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            Birthdate = patient.Birthdate,
            Prescriptions = patient.Prescriptions
                .OrderBy(p => p.DueDate)
                .Select(p => new PrescriptionDto
                {
                    IdPrescription = p.IdPrescription,
                    Date = p.Date,
                    DueDate = p.DueDate,
                    Doctor = new DoctorDto
                    {
                        FirstName = p.Doctor.FirstName,
                        LastName = p.Doctor.LastName
                    },
                    Medicaments = p.PrescriptionMedicaments.Select(pm => new MedicamentSummaryDto
                    {
                        Name = pm.Medicament.Name,
                        Description = pm.Medicament.Description,
                        Dose = pm.Dose
                    }).ToList()
                }).ToList()
        };
    }
}
