using Infrastructure.Repository;
using Infrastructure.Security;
using Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;
    private readonly IRbacService _rbacService;

    public RoleService(IRoleRepository roleRepository, IRbacService service)
    {
        _roleRepository = roleRepository;
        _rbacService = service;
    }

    public async Task<List<RoleDto>> GetAllRoles()
    {
        var roleDtos = new List<RoleDto>();
        await foreach (var r in _roleRepository.FindAll())
        {
            roleDtos.Add(new RoleDto(r.Id, r.Name));
        }
        return roleDtos;
    }

    public async Task HasPagePermission(string ResourceName)
    {
        if(String.IsNullOrEmpty(ResourceName))
        {
            throw new ArgumentNullException(nameof(ResourceName));
        }

        var permission = await _rbacService.IsAllowedAsync(RbacResource.PAGE(ResourceName), RbacAction.PAGE);

        if (!permission)
        {
            throw new UnauthorizedAccessException($"No permission to access page: {ResourceName}");
        }
    }
}
