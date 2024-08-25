using CinemaMinimalAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CinemaMinimalAPI.Data;

public class CinemaContext : DbContext
{
    public CinemaContext(DbContextOptions<CinemaContext> options) : base(options) 
    { 
    }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Director> Directors { get; set; }
    public DbSet<Cinema> Cinemas { get; set; }
    public DbSet<Session> Sessions { get; set; }

}
