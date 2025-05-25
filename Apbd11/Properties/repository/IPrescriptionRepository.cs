using Apbd11.Properties.models;

namespace Apbd11.Properties.repository;

public interface IPrescriptionRepository
{
    Task<Patient?> GetOrCreatePatientAsync(PatientDto dto);
    Task<bool> MedicamentsExistAsync(IEnumerable<int> ids);
    Task AddPrescriptionAsync(Prescription prescription);
    Task<Patient?> GetPatientDetailsAsync(int patientId);
}
