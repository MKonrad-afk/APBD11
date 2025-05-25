using Apbd11.Properties.models;
using Apbd11.Properties.services;
using Microsoft.AspNetCore.Mvc;

namespace Apbd11.Properties.controllers;

[ApiController]
[Route("api/prescriptions")]
public class PrescriptionsController : ControllerBase
{
    private readonly IPrescriptionService _service;

    public PrescriptionsController(IPrescriptionService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> AddPrescription([FromBody] PrescriptionRequestDto dto)
    {
        try
        {
            await _service.AddPrescriptionAsync(dto);
            return Ok("Prescription added.");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{patientId}")]
    public async Task<IActionResult> GetPatientDetails(int patientId)
    {
        var result = await _service.GetPatientDetailsAsync(patientId);
        if (result == null) return NotFound();
        return Ok(result);
    }
}
