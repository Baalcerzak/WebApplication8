using Microsoft.AspNetCore.Mvc;
using WebApplication8.Services;
using WebApplication8.DTO;


namespace WebApplication8.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrescriptionsController : ControllerBase
{
    private readonly IPrescriptionService _service;

    public PrescriptionsController(IPrescriptionService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _service.GetPrescriptionAsync(id);
        if (result == null)
            return NotFound($"Prescription with ID {id} not found");
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreatePrescriptionRequestDto dto)
    {
        try
        {
            var created = await _service.AddPrescriptionAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.IdPrescription }, created);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
    }
}