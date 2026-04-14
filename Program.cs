using ProductAPI.Data;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Adding services to the container.
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

// PostgreSQL connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();
var connStr = app.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine($">>> CONNECTION STRING: {connStr}");


app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.MapControllers();

try
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var retries = 5;
    while (retries > 0)
    {
        try
        {
            db.Database.Migrate();
            Console.WriteLine(">>> Migrations applied successfully.");
            break;
        }
        catch (Exception ex)
        {
            retries--;
            Console.WriteLine($">>> Migration failed, retries left: {retries}. Error: {ex.Message}");
            if (retries == 0) throw;
            Thread.Sleep(3000); // wait 3 seconds 
        }
        db.Database.Migrate();  // For pending data migrations
    }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error applying migrations: {ex.Message}");
}


app.Run();




