using Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services;

public interface IRoleService
{
    Task<List<RoleDto>> GetAllRoles();

    Task HasPagePermission(string ResourceName);
}
