using System.ComponentModel.DataAnnotations;

namespace TeleBase.Models;

public class Command
{
    [Key] public int Id { get; set; }

    public string Name { get; set; }

    public Command()
    {
        Name = $"command{Id}";
    }
}