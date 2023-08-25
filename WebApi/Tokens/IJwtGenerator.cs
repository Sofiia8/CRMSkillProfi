using System.Collections.Generic;
using System.Security.Claims;

namespace WebApi.Tokens
{
    public interface IJwtGenerator
    {
        string GenerateToken(List<Claim> claims);
    }
}
