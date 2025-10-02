using Domain.Entities;
using Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository;

public interface IUserRoleRepository: IRepository<UserRole>
{
    Task AssignUserRole(string userId, ICollection<string> roleId, bool saveChanges = true);
    Task RemoveUserRole(string userId, ICollection<string> roleId, bool saveChanges = true);
}
