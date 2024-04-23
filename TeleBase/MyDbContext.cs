using Microsoft.EntityFrameworkCore;

namespace TeleBase;

public class MyDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.UseNpgsql("Host=172.17.0.3;Database=TelegramBase;Username=postgres;Password=123");
    }
}