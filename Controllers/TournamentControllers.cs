using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Chess.Databases;
using Chess.Models;

[ApiController]
[Route("tournaments")]
public class TournamentController : ControllerBase
{
    private MainContext db;

    public struct CreateTournamentRequest
    {
        public long Id;
        public string Name;
        public string OrganizerId;
    }

    public TournamentController(MainContext _db)
    {
        db = _db;
    }

    [HttpGet("/tournament", Name = "GetAllTournament")]
    [Authorize]
    public Tournament[] GetAllTournament()
    {
        return db.Tournament.ToArray();
    }

    [HttpGet("/Tournament/{id}", Name = "GetClub")]
    [Authorize]
    public ActionResult GetTournament(long id)
    {
        Tournament? tournament = db.Tournament.Find(id);

        if (Tournament == null)
        {
            return NotFound();
        }
        return Ok(match);
    }

    [HttpPost("/tournament/create", Name = "CreateTournament")]
    // [Authorize]
    public ActionResult CreateTournament(CreateTournamentRequest request)
    {
        try
        {
           Tournament tournament = new tournament(DateTime.UtcNow, request.Name);

            db.ChangeTracker.Clear();
            db.Tournament.Add(tournament);
            db.SaveChanges();

            return StatusCode(201);
        }
        catch (DbUpdateException e)
        {
            return BadRequest(new { error = e.ToString() });
        }
    }



}