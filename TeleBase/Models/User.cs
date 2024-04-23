using System.ComponentModel.DataAnnotations;

namespace TeleBase.Models;

public class User
{
    [Key] public int Id { get; set; }

    public string Name { get; set; }

    public User()
    {
        Name = $"user{Id}";
    }
}