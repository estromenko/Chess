using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Chess.Databases;
using Chess.Models;

[ApiController]
[Route("clubs")]
public class ClubController : ControllerBase
{
    private MainContext db;

    public struct CreateClubRequest
    {
        public long Id;
        public long Name;
        public string description;
        public string logo;
        public string leader_id;
    }

    public struct DeleteClubRequest
    {
        public long Id;
    }

    public ClubController(MainContext _db)
    {
        db = _db;
    }

    [HttpGet("/clubs", Name = "GetAllClub")]
    [Authorize]
    public Club[] GetAllClub()
    {
        return db.Club.ToArray();
    }

    [HttpGet("/club/{id}", Name = "GetClub")]
    [Authorize]
    public ActionResult GetClub(long id)
    {
        Club? club = db.Club.Find(id);

        if (club == null)
        {
            return NotFound();
        }
        return Ok(match);
    }

    [HttpPost("/club/create", Name = "CreateClub")]
    // [Authorize]
    public ActionResult CreateClub(CreateClubRequest request)
    {
        try
        {
            Club club = new Club(DateTime.UtcNow, request.Name, request.description, request.logo, request.leader_id);

            db.ChangeTracker.Clear();
            db.Club.Add(club);
            db.SaveChanges();

            return StatusCode(201);
        }
        catch (DbUpdateException e)
        {
            return BadRequest(new { error = e.ToString() });
        }
    }
    public ActionResult DeleteClub(DeleteClubRequest request)
    {
        try
        {
            Club club = new Club(DateTime.UtcNow, request.Id);

            db.ChangeTracker.Clear();
            db.Club.Delete(club);
            db.SaveChanges();

            return StatusCode(201);
        }
        catch (DbUpdateException e)
        {
            return BadRequest(new { error = e.ToString() });
        }
    }
}
