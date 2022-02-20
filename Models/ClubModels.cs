using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Chess.Models;

[Table("clubs")]
public class Club
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    [SwaggerSchema(ReadOnly = true)]
    [Column("id")]
    public long Id { get; set; }

    [Required]
    [Column("name")]
    public string Name { get; set; }

    [Required]
    [Column("logo")]
    public string Logo { get; set; }

    [Required]
    [Column("leader_id")]
    public long LeaderId { get; set; }
    public virtual User? Leader { get; set; }

    public Club(string name, string logo, long leaderId)
    {
        Name = name;
        Logo = logo;
        LeaderId = leaderId;
    }
}

[Table("users_clubs")]
[Index(nameof(UserId), nameof(ClubId), IsUnique = true)]
public class UsersClubs
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    [SwaggerSchema(ReadOnly = true)]
    [Column("id")]
    public long Id { get; set; }

    [Required]
    [Column("user_id")]
    public long UserId { get; set; }
    public virtual User? User { get; set; }

    [Required]
    [Column("club_id")]
    public long ClubId { get; set; }
    public virtual Club? Club { get; set; }

    public UsersClubs(long userId, long clubId)
    {
        UserId = userId;
        ClubId = clubId;
    }
}
