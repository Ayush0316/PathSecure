using Domain.Entities;
using Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository;

public class OAuthClientRepository(BaseDbContext dc) : BaseRepository<OAuthClient>(dc), IOAuthClientRepository
{
}
