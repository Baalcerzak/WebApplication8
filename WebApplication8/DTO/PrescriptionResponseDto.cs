namespace WebApplication8.DTO;

public class PrescriptionResponseDto
{
    public int IdPrescription { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }

    public string PatientName { get; set; }
    public string DoctorName { get; set; }

    public List<PrescriptionMedicamentDetailsDto> Medications { get; set; }
}




