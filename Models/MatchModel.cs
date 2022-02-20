using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Chess.Models;

[Table("matches")]
[Index(nameof(WhiteId), nameof(BlackId), IsUnique = true)]
public class Match
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    [SwaggerSchema(ReadOnly = true)]
    [Column("id")]
    public long Id { get; set; }

    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    [Column("started_at")]
    public DateTime StartedAt { get; set; }

    [Required]
    [Column("white_id")]
    public long WhiteId { get; set; }
    public virtual User? White { get; set; }

    [Required]
    [Column("black_id")]
    public long BlackId { get; set; }
    public virtual User? Black { get; set; }

    public Match(DateTime startedAt, long whiteId, long blackId)
    {
        StartedAt = startedAt;
        WhiteId = whiteId;
        BlackId = blackId;
    }
}

[Table("match_results")]
[Index(nameof(MatchId), IsUnique = true)]
public class MatchResult
{
    [Required]
    [Column("match_id")]
    public long MatchId { get; set; }
    public virtual Match? Match { get; set; }

    [Required]
    [Column("winner_id")]
    public long WinnerId { get; set; }
    public virtual User? Winner { get; set; }

    [Required]
    public DateTime Duration { get; set; }

    public MatchResult(long matchId, long winnerId, DateTime duration)
    {
        MatchId = matchId;
        WinnerId = winnerId;
        Duration = duration;
    }
}
