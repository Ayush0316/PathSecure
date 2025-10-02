using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class RefreshToken : BaseEntity
{
    [Required]
    public string UserId { get; set; } = null!;

    [Required]
    public string TokenHash { get; set; } = null!;       // store hashed token

    [Required]
    public DateTime ExpiresAt { get; set; }

    public bool Used { get; set; } = false;              // one-time use
    public bool Revoked { get; set; } = false;
    public string? ReplacedByHash { get; set; } = null;  // chain for rotation
}
