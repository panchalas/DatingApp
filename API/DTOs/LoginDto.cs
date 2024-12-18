using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class LoginDto
{
    [Required(ErrorMessage = "Username mandatory.")]
    public required string Username {get; set;}
    [Required(ErrorMessage = "Password mandatory")]
    public required string Password {get;set;}
}
