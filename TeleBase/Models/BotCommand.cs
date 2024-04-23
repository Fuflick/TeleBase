using System.ComponentModel.DataAnnotations;

namespace TeleBase.Models;

public class BotCommand
{
    [Key] public int Id { get; set; }

    public int? BotId { get; set; }

    public int? CommandId { get; set; }
}