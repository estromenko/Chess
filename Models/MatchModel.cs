namespace Chess.Models;

public class Match
{
    public long Id { get; set; }
    public long WhiteId { get; set; }
    public long BlackId { get; set; }
}

public class MatchResult
{
    public long MatchId { get; set; }
    public string? Winner { get; set; }
    public DateTime? Duration { get; set; }
}

public class Club
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Logo { get; set; }
    public long Leader { get; set; }
}

public class UsersClubs
{
    public long UserId { get; set; }
    public long ClubId { get; set; }
}

public class Tournament
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public long OrganizerId { get; set; }
}

public class UsersTournaments
{
    public long UserId { get; set; }
    public long TournamentId { get; set; }
}
