using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class UserRole
{
    [Required]
    public string UserId { get; set; } = null!;

    [ForeignKey("UserId")]
    public User User { get; set; } = null!;

    [Required]
    public string RoleId { get; set; } = null!;

    [ForeignKey("RoleId")]
    public Role Role { get; set; } = null!;
}