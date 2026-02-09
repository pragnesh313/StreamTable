using StreamTable.Data;
using Microsoft.EntityFrameworkCore;

namespace StreamTable.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        try
        {
            await context.Database.MigrateAsync();
            // 2. Check if we need to seed
            if (!await context.Customers.AnyAsync())
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "SampleCustomers.sql");
                string script = await File.ReadAllTextAsync(filePath);
                await context.Database.ExecuteSqlRawAsync(script);
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}