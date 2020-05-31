using Microsoft.AspNetCore.Http;
using PsychoCare.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PsychoCare.API.Models
{
    /// <summary>
    /// Web principal model which provides access to decoded
    /// user identity from token
    /// </summary>
    public class WebPrincipal : IWebPrincipal
    {
        private readonly HttpContext _httpContext;

        public WebPrincipal(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor?.HttpContext;
        }

        public ClaimsIdentity Identity => _httpContext.User.Identity as ClaimsIdentity;

        public int UserId
        {
            get
            {
                IEnumerable<Claim> claims = Identity.Claims;
                return int.Parse(claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value);
            }
        }
    }
}