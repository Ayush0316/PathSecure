using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class User : BaseEntity
{
    [Required]
    public string Username { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    public string? PasswordHash { get; set; }

    public UserType Type { get; set; }

    public UserStatus Status { get; set; }
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<PasswordSetLinks> PasswordSetLinks { get; set; } = new List<PasswordSetLinks>();
}

public enum UserType
{
    REGULAR,
    OAUTH2,
    SILENT
}

public enum UserStatus
{
    NOT_VERIFIED,
    VERIFIED,
    IN_ACTIVE
}