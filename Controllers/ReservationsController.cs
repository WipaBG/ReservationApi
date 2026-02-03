using Microsoft.AspNetCore.Mvc;
using ReservationApi.Domain;
using ReservationApi.Services;

namespace ReservationApi.Controllers;

[ApiController]
[Route("/api/reservations")]
public class ReservationController(IReservationService service) : ControllerBase
{
    private readonly IReservationService _service = service;

    [HttpGet]
    public async Task<ActionResult<List<ReservationResponse>>> GetAll(CancellationToken ct)
    {
        var rooms = await _service.GetAllAsync(ct);
        return Ok(rooms);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ReservationResponse>> GetById(Guid id, CancellationToken ct)
    {
        var reservation = await _service.GetByIdAsync(id, ct);
        if (reservation is null) return NotFound(new { message = "Reservation not found" });
        return Ok(reservation);
    }

    [HttpPost]
    public async Task<ActionResult<ReservationResponse>> CreateAsync(CreateReservationRequest req, CancellationToken ct)
    {
        var (ok, error, reservation) = await _service.CreateAsync(req, ct);
        if (!ok)
        {
            if (error == "reservation already exists")
            {
                return Conflict(new { message = error });
            }
            return BadRequest(new { message = error });
        }
        return CreatedAtAction(nameof(GetById), new { id = reservation!.Id }, reservation);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateReservationRequest req, CancellationToken ct)
    {
        var (ok, error) = await _service.UpdateAsync(id, req, ct);
        if (!ok)
        {
            if (error == "reservation does not exist")
            {
                return NotFound(new { message = error });
            }
            if (error == "room number is required")
            {
                return BadRequest(new { message = error });
            }
            if (error == "check-in date should be before check-out date")
            {
                return Conflict(new { message = error });
            }

        }
        return NoContent();

    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var deleted = await _service.DeleteAsync(id, ct);
        if (!deleted)
        {
            return NotFound(new { message = "Reservation with such ID not found" });
        }
        return NoContent();
    }
}