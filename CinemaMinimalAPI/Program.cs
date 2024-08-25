using CinemaMinimalAPI.Data;
using CinemaMinimalAPI.Endpoints;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CinemaContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Local")).UseLazyLoadingProxies());
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.AddMovieEndpoints();
app.AddGenreEndpoints();
app.AddDirectorEndpoints();
app.AddCinemaEndpoints();

app.Run();
