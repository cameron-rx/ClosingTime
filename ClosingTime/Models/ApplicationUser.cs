using System;
using System.ComponentModel.DataAnnotations;

namespace ClosingTime.Models;

public class ApplicationUser
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public ulong DiscordId { get; set; } 
    public string? Username { get; set; }    
    public string? DisplayName { get; set; } 
    public string? AvatarHash { get; set; }
    
    public bool IsWhitelisted { get; set; }
    public bool IsAdmin { get; set; }
}
