using AutoMapper;
using CinemaMinimalAPI.Data;
using CinemaMinimalAPI.DTO;
using CinemaMinimalAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaMinimalAPI.Endpoints;

public static class CinemaEndpoints
{
    public static void AddCinemaEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoint = app.MapGroup("/api/cinemas");

        // Create cinema
        endpoint.MapPost("/", async (CinemaContext context, [FromBody]CinemaDTO cinemaDto) =>
        {
            var cinema = new Cinema
            {
                Name = cinemaDto.Name,
                Location = cinemaDto.Location
            };

            context.Cinemas.Add(cinema);
            await context.SaveChangesAsync();

            return Results.Created($"/api/cinemas/{cinema.Id}", cinema);
        })
            .WithName("Create cinema")
            .WithDescription("Adds new cinema to DB");

        // Get all cinemas
        endpoint.MapGet("/", async (CinemaContext context) =>
        {
            var cinemas = await context.Cinemas.ToListAsync();
            return Results.Ok(cinemas);
        })
            .WithName("Get all cinemas")
            .WithDescription("Returns all cinemas");

        // Get cinema by ID
        endpoint.MapGet("/{id:int}", async (CinemaContext context, [FromRoute]int id) =>
        {
            var cinema = await context.Cinemas.FindAsync(id);
            if (cinema is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(cinema);
        })
            .WithName("Get cinema by ID")
            .WithDescription("Returns cinema by ID");

        // Update cinema
        endpoint.MapPut("/{id:int}", async (CinemaContext context, [FromRoute]int id, [FromBody]CinemaDTO updatedCinema) =>
        {
            var cinema = await context.Cinemas.FindAsync(id);
            if (cinema is null)
            {
                return Results.NotFound();
            }

            cinema.Name = updatedCinema.Name;
            cinema.Location = updatedCinema.Location;

            await context.SaveChangesAsync();
            return Results.Ok(cinema);
        })
            .WithName("Update cinema")
            .WithDescription("Updates existing cinema data");

        // Delete cinema
        endpoint.MapDelete("/{id:int}", async (CinemaContext context, [FromRoute]int id) =>
        {
            var cinema = await context.Cinemas.FindAsync(id);
            if (cinema is null)
            {
                return Results.NotFound();
            }

            context.Cinemas.Remove(cinema);
            await context.SaveChangesAsync();
            return Results.NoContent();
        })
            .WithName("Delete cinema")
            .WithDescription("Deletes cinema");

        // Create session
        endpoint.MapPost("/{cinemaId:int}/sessions", async (
            CinemaContext context,
            IMapper mapper,
            [FromRoute] int cinemaId,
            [FromBody] AddSessionDTO createSessionDTO) =>
        {
            var cinema = await context.Cinemas.FindAsync(cinemaId);
            if (cinema is null)
            {
                return Results.BadRequest($"Cinema with ID {cinemaId} not found.");
            }

            var movie = await context.Movies.FindAsync(createSessionDTO.MovieId);
            if (movie is null)
            {
                return Results.BadRequest($"Movie with ID {createSessionDTO.MovieId} not found.");
            }

            var session = new Session
            {
                StartTime = createSessionDTO.StartTime,
                EndTime = createSessionDTO.EndTime,
                Cinema = cinema,
                Movie = movie
            };

            context.Sessions.Add(session);
            await context.SaveChangesAsync();

            var sessionDTO = mapper.Map<SessionDTO>(session);
            return Results.Created($"/api/cinemas/{cinemaId}/sessions/{sessionDTO.Id}", sessionDTO);
        })
        .WithName("Create session")
        .WithDescription("Creates a session for a specific cinema");


        // Get all sessions from cinema
        endpoint.MapGet("/{cinemaId:int}/sessions", async (CinemaContext context, IMapper mapper, [FromRoute]int cinemaId) =>
        {
            var cinema = await context.Cinemas.Where(c => c.Id == cinemaId).Include(c => c.Sessions).ThenInclude(s => s.Movie).FirstOrDefaultAsync();
            if (cinema is null)
            {
                return Results.NotFound();
            }

            var sessions = cinema.Sessions;
            var sessionDtos = mapper.Map<IEnumerable<SessionDTO>>(cinema.Sessions);
            return Results.Ok(sessionDtos);
        })
            .WithName("Get all sessions from cinema")
            .WithDescription("Returns all sessions from a specific cinema");

        // Get session by id
        endpoint.MapGet("/{cinemaId:int}/sessions/{sessionId:int}", async (CinemaContext context, IMapper mapper, [FromRoute]int cinemaId, [FromRoute] int sessionId) =>
        {
            var session = await context.Sessions
                .Where(s => s.Id == sessionId && s.CinemaId == cinemaId)
                .Include(s => s.Movie)
                .FirstOrDefaultAsync();

            if (session is null)
            {
                return Results.NotFound();
            }
            var sessionDto = mapper.Map<SessionDTO>(session);
            return Results.Ok(sessionDto);
        })
            .WithName("Get session by ID")
            .WithDescription("Returns session by ID");

        // Update session
        endpoint.MapPut("/{cinemaId:int}/sessions/{sessionId:int}", async (
            CinemaContext context,
            [FromRoute]int cinemaId,
            [FromRoute]int sessionId,
            [FromBody]AddSessionDTO updatedSession) =>
        {
            var session = await context.Sessions
                .Where(s=>s.Id == sessionId && s.CinemaId == cinemaId)
                .Include(s => s.Movie)
                .FirstOrDefaultAsync();

            if (session is null)
            {
                return Results.NotFound();
            }

            var movie = await context.Movies.FindAsync(updatedSession.MovieId);
            if (movie is null)
            {
                return Results.BadRequest($"Movie with ID {updatedSession.MovieId} not found.");
            }

            session.MovieId = updatedSession.MovieId;
            session.StartTime = updatedSession.StartTime;
            session.EndTime = updatedSession.EndTime;

            await context.SaveChangesAsync();

            return Results.Ok();
        })
            .WithName("Update session")
            .WithDescription("Updates session");

        // Delete session
        endpoint.MapDelete("/{cinemaId:int}/sessions/{sessionId:int}", async (CinemaContext context, [FromRoute]int cinemaId, [FromRoute]int sessionId) =>
        {
            var session = await context.Sessions.Where(s=>s.Id == sessionId && s.CinemaId==cinemaId).FirstOrDefaultAsync();
            if (session is null)
            {
                return Results.NotFound();
            }

            context.Sessions.Remove(session);
            await context.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}

