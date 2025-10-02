using Domain.Entities;
using Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository;

public class UserRoleRepository(BaseDbContext dc) : BaseRepository<UserRole>(dc), IUserRoleRepository
{
    public async Task AssignUserRole(string userId, ICollection<string> roleId, bool saveChanges = true)
    {
        foreach (var r in roleId)
        {
            var ur = new UserRole
            {
                UserId = userId,
                RoleId = r
            };
            await Create(ur,false);
        }
        if (saveChanges)
        {
            await SaveChanges();
        }
    }

    public async Task RemoveUserRole(string userId, ICollection<string> roleId, bool saveChanges = true)
    {
        var urs = Query.Where(ur => ur.UserId == userId && roleId.Contains(ur.RoleId)).ToList();
        foreach (var ur in urs)
        {
            await Delete(ur, false);
        }
        if (saveChanges)
        {
            await SaveChanges();
        }
    }
}
