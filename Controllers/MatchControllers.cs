using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

using Chess.Databases;
using Chess.Models;

[ApiController]
[Route("matches")]
public class MatchController : ControllerBase
{
    private MainContext db;

    public struct CreateMatchRequest
    {
        public long whiteId;
        public long blackId;
    }

    public MatchController(MainContext _db)
    {
        db = _db;
    }

    [HttpGet("/matches", Name = "GetAllMatches")]
    [Authorize]
    public Match[] GetAllMatches()
    {
        return db.Matches.ToArray();
    }

    [HttpGet("/matches/{id}", Name = "GetMatch")]
    [Authorize]
    public ActionResult GetMatch(long id)
    {
        Match? match = db.Matches.Find(id);

        if (match == null)
        {
            return NotFound();
        }
        return Ok(match);
    }

    [HttpPost("/matches/create", Name = "CreateMatch")]
    // [Authorize]
    public ActionResult CreateMatch(CreateMatchRequest request)
    {
        try
        {
            Match match = new Match(DateTime.UtcNow, request.whiteId, request.blackId);

            db.ChangeTracker.Clear();
            db.Matches.Add(match);
            db.SaveChanges();

            return StatusCode(201);
        }
        catch (DbUpdateException e)
        {
            return BadRequest(new { error = e.ToString() });
        }
    }
}
