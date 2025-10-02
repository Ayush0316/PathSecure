using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class RolePermission
{
    [Required]
    public string RoleId { get; set; } = null!;

    [ForeignKey("RoleId")]
    public Role Role { get; set; } = null!;

    [Required]
    public string PermissionId { get; set; } = null!;

    [ForeignKey("PermissionId")]
    public Permission Permission { get; set; } = null!;
}