using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class ResourcePermission
{
    [Required]
    public string ResourceId { get; set; }

    [ForeignKey("ResourceId")]
    public Resource Resource { get; set; } = null!;

    [Required]
    public string PermissionId { get; set; }

    [ForeignKey("PermissionId")]
    public Permission Permission { get; set; } = null!;
}