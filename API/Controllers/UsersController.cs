using System;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

//below two are specified in BaseAPIContoller class, and this is inheriting BaseAPIContoller.
//[ApiController]
//[Route("api/[controller]")]
[Authorize]
public class UsersController(DataContext dataContext) : BaseAPIController
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers() {
        var users = await dataContext.Users.ToListAsync();

        if (users is null) return NotFound();

        return Ok(users);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<AppUser>> GetUser(int id) {
        var user = await dataContext.Users.FindAsync(id);

        if(user is null) return NotFound();
        
        return Ok(user);
    }
}
