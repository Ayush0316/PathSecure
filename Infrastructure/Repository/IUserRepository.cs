using Domain.Entities;
using Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository;

public interface IUserRepository : IRepository<User>
{
    Task<User?> FindByUsernameAsync(string username);
}
