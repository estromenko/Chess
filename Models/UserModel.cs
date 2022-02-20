using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Chess.Models;

[Table("users")]
[Index(nameof(Email), IsUnique = true)]
public class User
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    [SwaggerSchema(ReadOnly = true)]
    [Column("id")]
    public long Id { get; set; }

    [StringLength(255, MinimumLength = 6)]
    [Required]
    [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Email is not valid")]
    [Column("email")]
    public string Email { get; set; }

    [StringLength(255, MinimumLength = 3)]
    [Required]
    [Column("username")]
    public string Username { get; set; }

    [Required]
    [StringLength(255, MinimumLength = 8)]
    [Column("password")]
    public string Password { get; set; }

    [NotMapped]
    [Compare(nameof(Password))]
    public string? Password2 { get; set; }

    [Column("rating")]
    public uint Rating { get; set; } = 1500;

    public User(string email, string username, string password)
    {
        Email = email;
        Username = username;
        Password = password;
    }
}
