using System.ComponentModel.DataAnnotations;

namespace TeleBase.Models;

public class Bot
{
    [Key] public int Id { get; set; }

    public string Name { get; set; }

    public Bot()
    {
        Name = $"bot{Id}";
    }
}