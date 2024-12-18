using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class RegisterDto
{
    [Required(ErrorMessage = "User name is mandatory")]
    [MaxLength(10)]
    public required string Username { get; set; }

    [Required(ErrorMessage = "Password is mandatory")]
    [MaxLength(20)]
    [MinLength(8)]
    public required string Password { get; set; }
}
