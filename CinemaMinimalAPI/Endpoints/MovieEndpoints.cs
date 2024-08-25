using AutoMapper;
using CinemaMinimalAPI.Data;
using CinemaMinimalAPI.DTO;
using CinemaMinimalAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace CinemaMinimalAPI.Endpoints;

public static class MovieEndpoints
{
    public static void AddMovieEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoint = app.MapGroup("/api/movies");

        /*
         // Get all movies with filtration and pagination
        endpoint.MapGet("/", async (
            CinemaContext context,
            IMapper mapper,
            [FromQuery] int page = 0,
            [FromQuery] int size = 10,
            [FromQuery] string? search = null,
            [FromQuery] int? genreId = null,
            [FromQuery] int? directorId = null) =>
        {
            var moviesQuery = context.Movies
                .Include(m => m.Genre)
                .Include(m => m.Director)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                moviesQuery = moviesQuery.Where(m => m.Title.Contains(search));
            }

            if (genreId.HasValue)
            {
                moviesQuery = moviesQuery.Where(m => m.GenreId == genreId.Value);
            }

            if (directorId.HasValue)
            {
                moviesQuery = moviesQuery.Where(m => m.DirectorId == directorId.Value);
            }

            var movies = await moviesQuery
                .Skip(page * size)
                .Take(size)
                .ToListAsync();

            var movieDtos = mapper.Map<IEnumerable<MovieDTO>>(movies);
            return Results.Ok(movieDtos);
        })
            .WithName("Get All Movies")
            .WithDescription("Returns all movies with pagination and search");
        */

        // Get all movies with pagination
        endpoint.MapGet("/all", async (
            CinemaContext context,
            IMapper mapper,
            [FromQuery] int page = 0,
            [FromQuery] int size = 10) =>
        {
            var totalMovies = await context.Movies.CountAsync();
            var movies = await context.Movies
                .Include(m => m.Genre)
                .Include(m => m.Director)
                .Skip(page * size)
                .Take(size)
                .ToListAsync();

            var movieDtos = mapper.Map<IEnumerable<MovieDTO>>(movies);

            return Results.Ok(movieDtos);
        })
        .WithName("Get All Movies")
        .WithDescription("Returns all movies with pagination");

        // Search movies with filters
        endpoint.MapGet("/", async (
            CinemaContext context,
            IMapper mapper,
            [FromQuery] string? search = null,
            [FromQuery] int? genreId = null,
            [FromQuery] int? directorId = null,
            [FromQuery] int page = 0,
            [FromQuery] int size = 10) =>
        {
            var moviesQuery = context.Movies
                .Include(m => m.Genre)
                .Include(m => m.Director)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                moviesQuery = moviesQuery.Where(m => m.Title.Contains(search));
            }

            if (genreId.HasValue)
            {
                moviesQuery = moviesQuery.Where(m => m.GenreId == genreId.Value);
            }

            if (directorId.HasValue)
            {
                moviesQuery = moviesQuery.Where(m => m.DirectorId == directorId.Value);
            }

            var movies = await moviesQuery
                .Skip(page * size)
                .Take(size)
                .ToListAsync();

            var movieDtos = mapper.Map<IEnumerable<MovieDTO>>(movies);

            return Results.Ok(movieDtos);
        })
        .WithName("Search Movies")
        .WithDescription("Returns movies with filters and pagination");


        // Get movie by id
        endpoint.MapGet("/{id:int}", async (CinemaContext context, IMapper mapper, [FromRoute]int id) =>
        {
            var movie = await context.Movies
                .Where(m => m.Id == id)
                .Include(m => m.Genre)
                .Include(m => m.Director)
                .FirstOrDefaultAsync();

            if (movie is null)
            {
                return Results.NotFound();
            }

            var movieDto = mapper.Map<MovieDTO>(movie);
            return Results.Ok(movieDto);
        })
            .WithName("Get Movie By Id")
            .WithDescription("Returns movie by ID");

        // Create new movie
        endpoint.MapPost("/", async (CinemaContext context, IMapper mapper, [FromBody]AddMovieDTO movieDto) =>
        {
            var genre = await context.Genres.FindAsync(movieDto.GenreId);
            if (genre is null)
            {
                return Results.BadRequest($"Genre with ID {movieDto.GenreId} not found.");
            }

            var director = await context.Directors.FindAsync(movieDto.DirectorId);
            if (director is null)
            {
                return Results.BadRequest($"Director with ID {movieDto.DirectorId} not found.");
            }

            var movie = mapper.Map<Movie>(movieDto);
            movie.Genre = genre;
            movie.Director = director;

            context.Movies.Add(movie);
            await context.SaveChangesAsync();

            return Results.Created($"/api/movies/{movie.Id}", movie.Id);
        })
            .WithName("Create movie")
            .WithDescription("Adds new movie to database");

        // Update movie
        endpoint.MapPut("/{id:int}", async (CinemaContext context, IMapper mapper, [FromRoute]int id, [FromBody]AddMovieDTO movieDto) =>
        {
            var movie = await context.Movies.FindAsync(id);
            if (movie is null)
            {
                return Results.NotFound();
            }

            var genre = await context.Genres.FindAsync(movieDto.GenreId);
            if (genre is null)
            {
                return Results.BadRequest($"Genre with ID {movieDto.GenreId} not found.");
            }

            var director = await context.Directors.FindAsync(movieDto.DirectorId);
            if (director is null)
            {
                return Results.BadRequest($"Director with ID {movieDto.DirectorId} not found.");
            }

            movie.Title = movieDto.Title;
            movie.DurationInMinutes = movieDto.DurationInMinutes;
            movie.GenreId = movieDto.GenreId;
            movie.DirectorId = movieDto.DirectorId;

            await context.SaveChangesAsync();

            return Results.Ok(mapper.Map<MovieDTO>(movie));
        })
            .WithName("Update Movie")
            .WithDescription("Updates existing movie data");

        // Delete movie
        endpoint.MapDelete("/{id:int}", async (CinemaContext context, [FromRoute]int id) =>
        {
            var movie = await context.Movies.FindAsync(id);
            if (movie is null)
            {
                return Results.NotFound();
            }

            context.Movies.Remove(movie);
            await context.SaveChangesAsync();

            return Results.NoContent();
        })
            .WithName("Delete movie")
            .WithDescription("Deletes movie using movie ID");
    }
}
