using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Chess.Models;

[Table("tournaments")]
public class Tournament
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    [SwaggerSchema(ReadOnly = true)]
    [Column("id")]
    public long Id { get; set; }

    [Required]
    [Column("name")]
    public string? Name { get; set; }

    [Required]
    [Column("organizer_id")]
    public long OrganizerId { get; set; }
    public virtual User? Organizer { get; set; }

    public Tournament(string name, long organizerId)
    {
        Name = name;
        OrganizerId = organizerId;
    }
}

[Table("users_tournaments")]
[Index(nameof(UserId), nameof(TournamentId), IsUnique = true)]
public class UsersTournaments
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
    [Column("tournament_id")]
    public long TournamentId { get; set; }
    public virtual Tournament? Tournament { get; set; }

    public UsersTournaments(long userId, long tournamentId)
    {
        UserId = userId;
        TournamentId = tournamentId;
    }
}
