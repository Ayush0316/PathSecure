using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Token;

public interface IKeyManagementService { 
    Task<SigningCredentials> GetActiveSigningCredentialsAsync(); 
    Task<SecurityKey?> GetPublicKeyByKidAsync(string kid); 
    Task<string> CreateAndActivateNewRsaKeyAsync(); // returns new kid
    Task RotateKeysAsync(); // admin triggered
}