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
            if (!await context.Customers.AnyAsync())
            {
                for (int i = 0; i < 10; i++)
                {
                    await InsertData(context);
                }
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private static async Task InsertData(ApplicationDbContext context)
    {
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "SampleCustomers.sql");
        string script = await File.ReadAllTextAsync(filePath);
        await context.Database.ExecuteSqlRawAsync(script);
    }
}