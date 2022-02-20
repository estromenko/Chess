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
    private MainContext db;
    private AuthService authService;

    public struct LoginUserRequest
    {
        public string Email;
        public string Password;
    }

    public struct RegisterUserRequest
    {
        public string Email;
        public string Username;
        public string Password;
        public string Password2;
    }

    public UserController(MainContext _db, AuthService _authService)
    {
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
    public ActionResult RegisterUser(RegisterUserRequest request)
    {
        try
        {
            if (request.Password != request.Password2)
            {
                return BadRequest(new { error = "Passwords do not match" });
            }

            User user = new User(0, request.Email, request.Username, request.Password);

            db.Users.Add(user);
            db.SaveChanges();

            string token = authService.GenerateToken(user.Username);

            return Ok(new { token = token });
        }
        catch (Microsoft.EntityFrameworkCore.DbUpdateException)
        {
            return BadRequest();
        }
    }

    [HttpPost("/users/login", Name = "LoginUser")]
    public ActionResult LoginUser(LoginUserRequest request)
    {
        try
        {
            User existentUser = db.Users.Single(u => u.Email == request.Email && u.Password == request.Password);

            string token = authService.GenerateToken(existentUser.Username);

            return Ok(new { token = token });
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(new { error = "Wrong email or password", message = e.ToString() });
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
