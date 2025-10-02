using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Resource : BaseEntity
{
    [Required]
    public string ResourceName { get; set; } = null!; // e.g., "Page_123" or "API:GetOrders"

    [Required]
    public string ResourceType { get; set; } = null!; // e.g., "Page" or "API"

    [Required]
    public string ResourceMethod { get; set; } = null!; // e.g., "GET", "POST", "VIEW"
    public string? ParentResourceId { get; set; } // null if root

    [ForeignKey("ParentResourceId")]
    public Resource? ParentResource { get; set; }

    public ICollection<Resource> ChildResources { get; set; } = new List<Resource>();

    public ICollection<ResourcePermission> ResourcePermissions { get; set; } = new List<ResourcePermission>();
}