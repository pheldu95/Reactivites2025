using Microsoft.EntityFrameworkCore;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapControllers();

//creates a scope that will be disposed of after app.Run() happens
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    //get access to db context so that we can use it to query our db
    var context = services.GetRequiredService<AppDbContext>();
    await context.Database.MigrateAsync(); //runs any migrations. or creates the db if it doesn't exist yet
    //seed data in. because DbInitializer is static, don't need to create a new instance of it. just have to initialize it
    await DbInitializer.SeedData(context);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration.");
}

app.Run();
