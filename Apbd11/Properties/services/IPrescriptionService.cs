using Apbd11.Properties.models;

namespace Apbd11.Properties.services;

public interface IPrescriptionService
{
    Task AddPrescriptionAsync(PrescriptionRequestDto dto);
    Task<PatientDetailsDto?> GetPatientDetailsAsync(int patientId);
}
