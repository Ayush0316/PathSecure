using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class PasswordSetLinks : BaseEntity
{
    public string UserId { get; set; } = null!;
    public string TokenHash { get; set; } = null!;
    public DateTime Expiry { get; set; }
    public bool Used { get; set; } = false;

    [ForeignKey("UserId")]
    public User User { get; set; } = null!;
}
