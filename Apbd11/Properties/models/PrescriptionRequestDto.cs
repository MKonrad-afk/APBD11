namespace Apbd11.Properties.models;

public class PrescriptionRequestDto
{
    public int IdDoctor { get; set; }

    public PatientDto Patient { get; set; } = null!;
    public List<MedicamentDto> Medicaments { get; set; } = new();
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
}

public class PatientDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime Birthdate { get; set; }
}

public class MedicamentDto
{
    public int IdMedicament { get; set; }
    public int Dose { get; set; }
    public string Details { get; set; } = null!;
}
