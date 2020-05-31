using PsychoCare.DataAccess.Exceptions;
using PsychoCare.DataAccess.Interfaces;
using PsychoCare.Logic.DataAccess;
using PsychoCare.Logic.Exceptions;
using PsychoCare.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace PsychoCare.Logic.Services
{
    public class UsersService : IUsersService
    {
        private readonly IEnvironmentGroupsService _environmentGroupsService;
        private readonly IUsersRepository _usersRepository;

        public UsersService(IUsersRepository usersRepository, IEnvironmentGroupsService environmentGroupsService)
        {
            _usersRepository = usersRepository;
            _environmentGroupsService = environmentGroupsService;
        }

        /// <summary>
        /// Gets user by email and password
        /// </summary>
        public User GetUser(string email, string password)
        {
            User user = _usersRepository.FindUserByEmail(email);
            return user;
        }

        /// <summary>
        /// This method is registering a user
        /// Specially it validates, check if user exists, registers and adds predefined groups
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidEmailException"/>
        /// <exception cref="InvalidPasswordException"/>
        /// <exception cref="UserAlreadyExistsException"/>
        public void Register(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            user.Validate();

            CheckIfEmailIsInUse(user.Email);

            _usersRepository.Register(user);

            _environmentGroupsService.AddPredefinedGroupsToUser(user.Id);
        }

        /// <summary>
        /// Checks if user with provided email already exists
        /// If true then exceptio nis thrown
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="UserAlreadyExistsException"/>
        private void CheckIfEmailIsInUse(string email)
        {
            if (email == null) throw new ArgumentNullException(nameof(email));

            User user = _usersRepository.FindUserByEmail(email);

            if (user != null) throw new UserAlreadyExistsException();
        }
    }
}