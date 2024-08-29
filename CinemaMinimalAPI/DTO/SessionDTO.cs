﻿using CinemaMinimalAPI.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace CinemaMinimalAPI.DTO;

public class SessionDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "MovieId is required.")]
    public int MovieId { get; set; }

    [Required(ErrorMessage = "CinemaId is required.")]
    public int CinemaId { get; set; }

    [Required(ErrorMessage = "StartTime is required.")]
    public DateTime StartTime { get; set; }

    [Required(ErrorMessage = "EndTime is required.")]
    [DateGreaterThan("StartTime", ErrorMessage = "EndTime must be greater than StartTime.")]
    public DateTime EndTime { get; set; }
}
