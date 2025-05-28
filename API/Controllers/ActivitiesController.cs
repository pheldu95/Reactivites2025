using System;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers;

// adding (AppDbContext context) makes AppDbContext a primary constructor
public class ActivitiesController(AppDbContext context) : BaseApiController //can derive from the base controller we made. so we don't need to repeat code
{
    [HttpGet]
    public async Task<ActionResult<List<Activity>>> GetActivities() //when we access the db, best practice to use async Task
    {
        return await context.Activities.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Activity>> GetActivityDetail(string id)
    {
        var activity = await context.Activities.FindAsync(id);

        if (activity == null) return NotFound();

        return activity;
    }
}
