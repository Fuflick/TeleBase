using System.ComponentModel.DataAnnotations;

namespace TeleBase.Models;

public class Chat
{
    [Key] public int Id { get; set; }

    public string Name { get; set; }

    public Chat()
    {
        Name = $"chat{Id}";
    }
}