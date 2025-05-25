using Apbd11.Properties.models;
using Apbd11.Properties.repository;
using Apbd11.Properties.services;
using Microsoft.EntityFrameworkCore;

namespace Apbd11;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
        builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();
        
        builder.Services.AddControllers();
        builder.Services.AddAuthorization();

        builder.Services.AddOpenApi();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                try
                {
                    dbContext.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Migration failed: {ex.Message}");
                    throw;
                }
            }
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}