using System;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers;

public class AccountController(DataContext context, ITokenService tokenService) : BaseAPIController
{
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto) 
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.UserName.ToLower() == loginDto.Username.ToLower());
        if (user == null) return Unauthorized("Invalid Username");

        using var hmac = new HMACSHA512( user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        for(int i=0; i<computedHash.Length; i++) {
            if(computedHash[i] != user.PasswordHash[i]) {
                return Unauthorized("Invalid Password");
            }
        }
        //return tokenService.CreateToken(user);

        return new UserDto {
            Username = user.UserName,
            token = tokenService.CreateToken(user)
        };
    }


    [HttpPost("register")]
    public async Task<ActionResult<AppUser>> RegisterUser(RegisterDto registerDto) 
    {
        if(await UserExists(registerDto.Username)) return BadRequest("User already exists.");

        using var hmac = new HMACSHA512();
        var user = new AppUser 
        {
            UserName = registerDto.Username,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return user;
    }


    [HttpPost("register2")]
    public async Task<ActionResult<AppUser>> RegisterUser2(string username, string password) 
    {
        using var hmac = new HMACSHA512();
        
        var user = new AppUser {
            UserName = username,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
            PasswordSalt = hmac.Key
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return user;
    }

    private Task<bool> UserExists(string username) 
    {
        return context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
    }
}
