using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PsychoCare.API.Services
{
    public interface ITokenService
    {
        string GenerateTokenForUser(DateTime expiresDate, int userId);

        int GetUserIdFromToken(string token);
    }
}