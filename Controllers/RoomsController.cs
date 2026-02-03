using Microsoft.AspNetCore.Mvc;
using ReservationApi.Domain;
using ReservationApi.Services;

namespace ReservationApi.Controllers;

[ApiController]
[Route("api/rooms")]
public class RoomsController : ControllerBase
{
    private readonly IRoomService _service;

    public RoomsController(IRoomService service)
    {
        _service = service;
    }

    // GET /api/rooms
    [HttpGet]
    public async Task<ActionResult<List<RoomResponse>>> GetAll(CancellationToken ct)
    {
        var rooms = await _service.GetAllAsync(ct);
        return Ok(rooms);
    }

    // GET /api/rooms/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RoomResponse>> GetById(Guid id, CancellationToken ct)
    {
        var room = await _service.GetByIdAsync(id, ct);
        if (room is null) return NotFound(new { message = "Room not found" });
        return Ok(room);
    }

    // POST /api/rooms
    [HttpPost]
    public async Task<ActionResult<RoomResponse>> Create([FromBody] CreateRoomRequest req, CancellationToken ct)
    {
        var (ok, error, room) = await _service.CreateAsync(req, ct);
        if (!ok) return BadRequest(new { message = error });

        return CreatedAtAction(nameof(GetById), new { id = room!.Id }, room);
    }

    // PUT /api/rooms/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRoomRequest req, CancellationToken ct)
    {
        var (ok, error) = await _service.UpdateAsync(id, req, ct);
        if (!ok)
        {
            if (error == "Room not found.") return NotFound(new { message = error });
            return BadRequest(new { message = error });
        }

        return NoContent();
    }

    // DELETE /api/rooms/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var deleted = await _service.DeleteAsync(id, ct);
        if (!deleted) return NotFound(new { message = "Room not found" });
        return NoContent();
    }
}
