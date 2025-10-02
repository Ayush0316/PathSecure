using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTO;

public record SilentLoginDto(string? Username, string? Password, string? UserId, string? Token)
{
}
