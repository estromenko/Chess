using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Chess.Databases;
using Chess.Models;
using Chess.Services;

namespace Chess.Controllers;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    private ILogger logger;
    private MainContext db;
    private AuthService authService;

    public struct LoginUserRequest
    {
        public string Email;
        public string Password;
    }

    public UserController(MainContext _db, AuthService _authService, ILogger<User> _logger)
    {
        logger = _logger;
        db = _db;
        authService = _authService;
    }

    [HttpGet("/users", Name = "GetAllUsers")]
    [Authorize]
    public User[] GetAllUsers()
    {
        return db.Users.ToArray();
    }

    [HttpPost("/users/registration", Name = "RegisterUser")]
    public ActionResult RegisterUser(User userData)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { errors = ModelState.Values.SelectMany(v => v.Errors).ToArray() });
            }

            Array existentUser = db.Users.Where(u => u.Email == userData.Email).ToArray();
            if (existentUser.Length > 0)
            {
                return BadRequest(new { message = "Such user already exists" });
            }

            User user = new User(userData.Email, userData.Username, userData.Password);

            string hashedPassword = authService.HashPassword(user.Password);
            user.Password = hashedPassword;

            db.ChangeTracker.Clear();
            db.Users.Add(user);
            db.SaveChanges();

            string token = authService.GenerateToken(user.Username);

            return Ok(new { token = token });
        }
        catch (Microsoft.EntityFrameworkCore.DbUpdateException e)
        {
            logger.LogWarning(e.InnerException!.Message, userData.Email);
            return BadRequest(new { message = $"User with such email ({userData.Email}) already exists" });
        }
    }

    [HttpPost("/users/login", Name = "LoginUser")]
    public ActionResult LoginUser(LoginUserRequest request)
    {
        try
        {
            string hashedPassword = authService.HashPassword(request.Password);
            User existentUser = db.Users.Single(u => u.Email == request.Email);

            if (existentUser.Password != hashedPassword)
            {
                return BadRequest(new { message = "Wrong email or password" });
            }

            string token = authService.GenerateToken(existentUser.Username);

            return Ok(new { token = token });
        }
        catch (InvalidOperationException e)
        {
            logger.LogTrace(e.InnerException!.Message);
            return BadRequest(new { message = "Wrong email or password" });
        }
    }

    [HttpGet("/users/{id}", Name = "GetUserById")]
    public ActionResult GetUserById(long id)
    {
        User? user = db.Users.Find(id);

        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }
}
