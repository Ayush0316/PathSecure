using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTO;

public class TokenDto
{
    public string Token { get; set; } = null!;
    public DateTime expiresAt { get; set; }
}
