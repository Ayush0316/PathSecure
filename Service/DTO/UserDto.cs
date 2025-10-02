using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTO;

public class UserDto
{
    public string Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Type { get; set; } = null!;
    public ICollection<RoleDto> Roles { get; set; } = null!;
}
