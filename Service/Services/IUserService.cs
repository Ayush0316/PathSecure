using Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services;

public interface IUserService
{
    Task<List<UserDto>> GetAll();
    Task<string> CreateUser(string username, string email, string type);
    Task AssignRoles(string userId, ICollection<string> roleIds);
    Task SpecialUpdateUser(string userId, string? username, string? email);
    Task UpdateUser(string userId, string? username, string? email, string? password);
    Task DeleteUser(string userId);
    Task<UserDto> GetUser(string userId);
}
