using Microsoft.EntityFrameworkCore;

using System.Diagnostics.CodeAnalysis;

using Chess.Models;


namespace Chess.Databases;

public class MainContext : DbContext
{

    [NotNull]
    public DbSet<User>? Users { get; set; }

    [NotNull]
    public DbSet<Match>? Matches { get; set; }

    [NotNull]
    public DbSet<Tournament>? Tournaments { get; set; }

    [NotNull]
    public DbSet<UsersTournaments>? UsersTournaments { get; set; }

    [NotNull]
    public DbSet<Club>? Clubs { get; set; }

    [NotNull]
    public DbSet<UsersClubs>? UsersClubs { get; set; }

    public MainContext() { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string? name = Environment.GetEnvironmentVariable("PG_NAME");
        string? user = Environment.GetEnvironmentVariable("PG_USER");
        string? password = Environment.GetEnvironmentVariable("PG_PASSWORD");
        string? host = Environment.GetEnvironmentVariable("PG_HOST");
        string? port = Environment.GetEnvironmentVariable("PG_PORT");

        optionsBuilder.UseNpgsql(
            $"Host={host};Port={port};Database={name};Username={user};Password={password}"
        );
    }
}
