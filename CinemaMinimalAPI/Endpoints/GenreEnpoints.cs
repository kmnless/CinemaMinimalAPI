﻿using CinemaMinimalAPI.Data;
using CinemaMinimalAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaMinimalAPI.Endpoints;

public static class GenreEndpoints
{
    public static void AddGenreEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoint = app.MapGroup("/api/genres");

        // Get all genres with optional name filter and pagination
        app.MapGet("/api/genres", async (
            CinemaContext context,
            [FromQuery] string? name = null,
            [FromQuery] int page = 0,
            [FromQuery] int size = 10) =>
        {
            var genresQuery = context.Genres.AsQueryable();

            if (!string.IsNullOrEmpty(name))
                genresQuery = genresQuery.Where(g => g.Name.Contains(name));

            var genres = await genresQuery
                .Skip(page * size)
                .Take(size)
                .ToListAsync();

            return Results.Ok(genres);
        })
        .WithName("Get all genres with filters and pagination")
        .WithDescription("Returns all genres with optional name filter and pagination");

        // Get genre by ID
        endpoint.MapGet("/{id:int}", async (CinemaContext context, [FromRoute]int id) =>
        {
            var genre = await context.Genres.FindAsync(id);
            if (genre is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(genre);
        })
            .WithName("Get genre by ID")
            .WithDescription("Returns genre by ID");

        // Create genre
        endpoint.MapPost("/", async (CinemaContext context, [FromBody]Genre genre) =>
        {
            context.Genres.Add(genre);
            await context.SaveChangesAsync();
            return Results.Created($"/api/genres/{genre.Id}", genre);
        })
            .WithName("Create genre")
            .WithDescription("Adds new genre to DB");

        // Update genre
        endpoint.MapPut("/{id:int}", async (CinemaContext context, [FromRoute]int id, [FromBody]Genre updatedGenre) =>
        {
            var genre = await context.Genres.FindAsync(id);
            if (genre is null)
            {
                return Results.NotFound();
            }

            genre.Name = updatedGenre.Name;
            await context.SaveChangesAsync();
            return Results.Ok(genre);
        })
            .WithName("Update genre")
            .WithDescription("Updates existing genre data");

        // Delete genre
        endpoint.MapDelete("/{id:int}", async (CinemaContext context, [FromRoute]int id) =>
        {
            var genre = await context.Genres.FindAsync(id);
            if (genre is null)
            {
                return Results.NotFound();
            }

            context.Genres.Remove(genre);
            await context.SaveChangesAsync();
            return Results.NoContent();
        })
            .WithName("Delete genre")
            .WithDescription("Deletes genre by id");
    }
}
