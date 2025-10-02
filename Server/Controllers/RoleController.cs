using Microsoft.AspNetCore.Mvc;
using Service.DTO;
using Service.Services;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;
    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet("special/get")]
    public async Task<ResponseDto<List<RoleDto>>> GetAll()
    {
        var roles = await _roleService.GetAllRoles();

        return new ResponseDto<List<RoleDto>>(Body: roles);
    }

    [HttpPost("permission/check")]
    public async Task<ResponseDto> CheckPermission([FromBody] PermissionCheckDto dto)
    {
        await _roleService.HasPagePermission(dto.Resource);
        return new ResponseDto(message: "Has permission");
    }
}
