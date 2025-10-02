using Domain.Entities;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository;

public class UserRepository(BaseDbContext dc) : BaseRepository<User>(dc), IUserRepository
{
    public override Task<User?> FindByPrimaryKey(params object[] pkVals)
    {
        ArgumentNullException.ThrowIfNull(pkVals, nameof(pkVals));
        return Query.Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .ThenInclude(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .Where(u => u.Id == (string)pkVals[0])
            .FirstOrDefaultAsync();
    }
    public Task<User?> FindByUsernameAsync(string username)
    {
        return Query
            .Where(u => u.Username == username)
            .FirstOrDefaultAsync();
    }
}
