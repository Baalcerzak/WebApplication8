using WebApplication8.DTO;

namespace WebApplication8.Services;

public interface IPrescriptionService
{
    Task<PrescriptionResponseDto?> GetPrescriptionAsync(int id);
    Task<PrescriptionResponseDto> AddPrescriptionAsync(CreatePrescriptionRequestDto dto);
}

