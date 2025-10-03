namespace WebApplication8.DTO;

public class CreatePrescriptionRequestDto
{
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public int IdPatient { get; set; }
    public int IdDoctor { get; set; }

    public List<PrescriptionMedicamentDto> Medications { get; set; }
}