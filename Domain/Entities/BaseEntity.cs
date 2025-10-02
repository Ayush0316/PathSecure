using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class BaseEntity
{
    [Key]
    [Required]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    [DefaultValue(false)]
    public bool IsDeleted { get; set; } = false;

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public DateTime? UpdatedAt { get; set; } = DateTime.Now;
}
