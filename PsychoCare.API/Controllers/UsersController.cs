using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PsychoCare.API.Models;
using PsychoCare.Common.Constants;
using PsychoCare.Logic.DataAccess;
using PsychoCare.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using PsychoCare.Logic.Exceptions;
using PsychoCare.API.Services;

namespace PsychoCare.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IUsersService _usersService;

        public UsersController(
            ITokenService tokenService,
            IUsersService usersService)
        {
            _tokenService = tokenService;
            _usersService = usersService;
        }

        /// <summary>
        /// Gets token for user session
        /// </summary>
        /// <param name="authModel">User credentials</param>
        /// <returns></returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] AuthModel authModel)
        {
            if (authModel == null) throw new ArgumentNullException(nameof(authModel));

            User user = _usersService.GetUser(authModel.Email, authModel.Password);

            if (user == null)
            {
                throw new UserNotFoundException();
            }

            if (user.Password != authModel.Password)
            {
                throw new InvalidPasswordException();
            }

            DateTime expiresDate = DateTime.Now.AddDays(999);

            string token = _tokenService.GenerateTokenForUser(expiresDate, user.Id);

            return Ok(token);
        }

        /// <summary>
        /// Register user
        /// </summary>
        /// <param name="authModel">User credentials</param>
        /// <returns></returns>
        [HttpPost("register")]
        [AllowAnonymous]
        public IActionResult Register([FromBody] AuthModel authModel)
        {
            if (authModel == null) throw new ArgumentNullException(nameof(authModel));

            User user = new User()
            {
                Email = authModel.Email,
                Password = authModel.Password
            };

            _usersService.Register(user);

            return Ok();
        }
    }
}