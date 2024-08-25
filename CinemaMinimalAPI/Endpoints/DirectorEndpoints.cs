using CinemaMinimalAPI.Data;
using CinemaMinimalAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaMinimalAPI.Endpoints;

public static class DirectorEndpoints
{
    public static void AddDirectorEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoint = app.MapGroup("/api/directors");

        // Get all directors
        endpoint.MapGet("/", async (CinemaContext context) =>
        {
            var directors = await context.Directors.ToListAsync();
            return Results.Ok(directors);
        })
            .WithName("Get all directors")
            .WithDescription("Returns all directors");

        // Get director by ID
        endpoint.MapGet("/{id:int}", async (CinemaContext context, [FromRoute]int id) =>
        {
            var director = await context.Directors.FindAsync(id);
            if (director is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(director);
        })
            .WithName("Get director by ID")
            .WithDescription("Returns director by ID");

        // Create durector
        endpoint.MapPost("/", async (CinemaContext context, [FromBody]Director director) =>
        {
            context.Directors.Add(director);
            await context.SaveChangesAsync();
            return Results.Created($"/api/directors/{director.Id}", director);
        })
            .WithName("Create director")
            .WithDescription("Adds new director to DB");

        // Update director
        endpoint.MapPut("/{id:int}", async (CinemaContext context, [FromRoute]int id, [FromBody]Director updatedDirector) =>
        {
            var director = await context.Directors.FindAsync(id);
            if (director is null)
            {
                return Results.NotFound();
            }

            director.Name = updatedDirector.Name;
            await context.SaveChangesAsync();
            return Results.Ok(director);
        })
            .WithName("Update director")
            .WithDescription("Updates existing director");

        // Delete director
        endpoint.MapDelete("/{id:int}", async (CinemaContext context, [FromRoute]int id) =>
        {
            var director = await context.Directors.FindAsync(id);
            if (director is null)
            {
                return Results.NotFound();
            }

            context.Directors.Remove(director);
            await context.SaveChangesAsync();
            return Results.NoContent();
        })
            .WithName("Delete director")
            .WithDescription("Deletes director");
    }
}

