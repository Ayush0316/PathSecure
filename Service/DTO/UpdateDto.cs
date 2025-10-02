using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTO;

public record UpdateDto(string UserId, string? Username, string? Email, string? Password)
{
}
