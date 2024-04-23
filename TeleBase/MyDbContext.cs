using Microsoft.EntityFrameworkCore;
using TeleBase.Models;

namespace TeleBase;

public class MyDbContext : DbContext
{
    public DbSet<Bot> Bots { get; set; } = null!;

    public DbSet<Chat> Chats { get; set; } = null!;

    public DbSet<Command> Commands { get; set; } = null!;

    public DbSet<User> Users { get; set; } = null!;

    public DbSet<BotChat> BotChats { get; set; } = null!;

    public DbSet<BotCommand> BotCommands { get; set; } = null!;

    public DbSet<UserChat> UserChats { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.UseNpgsql("Host=172.17.0.2;Database=TelegramBase;Username=postgres;Password=123");
    }
}