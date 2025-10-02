using Domain.Entities;
using Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository;

public class PasswordSetLinkRepository(BaseDbContext dc): BaseRepository<PasswordSetLinks>(dc), IPasswordSetLinkRepository
{
}
