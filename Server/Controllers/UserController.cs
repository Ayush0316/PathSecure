using Infrastructure.Session;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Bcpg;
using Service.DTO;
using Service.Services;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ISessionContext _sessionContext;

    public UserController(IUserService userService, ISessionContext context)
    {
        _userService = userService;
        _sessionContext = context;
    }

    [HttpPost("special/create")]
    public async Task<IActionResult> SignUp(SignUpDto signUp)
    {
        var userId = await _userService.CreateUser(signUp.username, signUp.email, signUp.type);
        return CreatedAtAction(nameof(GetUser), new { userId = userId }, new ResponseDto(message: "User Created Successfully"));
    }

    [HttpPost("assign-role")]
    public async Task<ResponseDto> AssignRoleToUser(AssignRoleDto assignRole)
    {
        await _userService.AssignRoles(assignRole.userId, assignRole.roleId);
        return new ResponseDto(message: "Role assigned to user successfully");
    }

    [HttpPut("special/update")]
    public async Task<ResponseDto> SpecialUpdate(SpecialUpdateDto dto)
    {
        await _userService.SpecialUpdateUser(dto.UserId, dto.Username, dto.Email);
        return new ResponseDto(message: "User updated successfully");
    }

    [HttpPut("update")]
    public async Task<ResponseDto> UpdateUser(UpdateDto dto)
    {
        await _userService.UpdateUser(dto.UserId, dto.Username, dto.Email, dto.Password);
        return new ResponseDto(message: "User updated successfully");
    }

    [HttpGet("get/{userId?}")]
    public async Task<ResponseDto<UserDto>> GetUser([FromRoute] string? userId)
    {
        if (userId == null)
        {
            var sessionUserId = _sessionContext.UserContext?.UserId;
            if (sessionUserId == null)
                throw new InvalidOperationException("User not found");
            userId = sessionUserId;
        }
        var user = await _userService.GetUser(userId);
        return new ResponseDto<UserDto>(Body: user);
    }

    [HttpDelete("special/delete/{userId}")]
    public async Task<ResponseDto> DeleteUser([FromRoute] string userId)
    {
        await _userService.DeleteUser(userId);
        return new ResponseDto(message: "User deleted successfully");
    }

    [HttpGet("special/get")]
    public async Task<ResponseDto<List<UserDto>>> GetAllUsers()
    {
        var users = await _userService.GetAll();
        return new ResponseDto<List<UserDto>>(Body: users);
    }
}
