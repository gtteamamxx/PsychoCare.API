using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.IdentityModel.Tokens;
using PsychoCare.Common.Constants;
using PsychoCare.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PsychoCare.API.Services
{
    public class TokenService : ITokenService
    {
        /// <summary>
        /// Generate token for concrete user
        /// </summary>
        /// <param name="expiresDate">Token expire date</param>
        /// <param name="userId">User id</param>
        /// <returns></returns>
        public string GenerateTokenForUser(DateTime expiresDate, int userId)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, userId.ToString())
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.SECRET_KEY));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenSecurity = new JwtSecurityToken(Constants.AUDIENCE_KEY,
                         Constants.AUDIENCE_KEY,
                         claims,
                         expires: expiresDate,
                         signingCredentials: credentials);
            string token = new JwtSecurityTokenHandler().WriteToken(tokenSecurity);
            return token;
        }

        /// <summary>
        /// Decodes userId from provided token
        ///
        /// If token is invalid then exception is thrown
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public int GetUserIdFromToken(string token)
        {
            if (token == null) throw new ArgumentNullException(nameof(token));

            try
            {
                JwtSecurityToken tokenSecurity = new JwtSecurityTokenHandler().ReadJwtToken(token);

                Claim sidClaim = tokenSecurity.Claims.First(x => x.Type.Equals(ClaimTypes.Sid));

                int userId = int.Parse(sidClaim.Value);

                return userId;
            }
            catch
            {
                throw new MessageException("Nie udało się odczytać podanego użytkownika");
            }
        }
    }
}