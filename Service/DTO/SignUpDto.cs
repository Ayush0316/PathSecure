using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTO;

public record SignUpDto(string username, string email, string type)
{
}
