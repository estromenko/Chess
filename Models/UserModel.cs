using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;

namespace Chess.Models;

[Index(nameof(Email), IsUnique = true)]
public class User
{
    public long Id { get; set; }

    [StringLength(255)]
    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public uint Rating { get; set; } = 1500;

    public User(long id, string email, string username, string password)
    {
        Id = id;
        Email = email;
        Username = username;
        Password = password;
    }
}
