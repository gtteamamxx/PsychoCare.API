using FluentAssertions;
using Moq;
using NUnit.Framework;
using PsychoCare.API.Controllers;
using PsychoCare.API.Models;
using PsychoCare.Logic.DataAccess;
using PsychoCare.Logic.Exceptions;
using PsychoCare.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using TestStack.BDDfy;

namespace PsychoCare.API.Tests.ControllersTests
{
    [TestFixture]
    public class UserControllerTests
    {
        private AuthModel _authModel;
        private Exception _thrownException;
        private IUsersService _usersService;

        [Test]
        public void Login_Should_Not_Accept_Null_Body()
        {
            this.Given(x => AuthModel_Is_Null())
                .When(x => Login())
                .Then(x => Exception_Should_Be_Thrown())
                .BDDfy();
        }

        [Test]
        public void Login_Using_Email_Which_Doesnt_Exist_Should_Throw_Exception()
        {
            this.Given(x => AuthModel_Is_With_Not_Existed_Email())
                    .And(x => User_Is_Not_In_Db())
                .When(x => x.Login())
                    .Then(x => Exception_Should_Be_Thrown_Of_Type(typeof(UserNotFoundException)))
                .BDDfy();
        }

        [Test]
        public void Login_Valid_Email_Bad_Password_Should_Throw_Exception()
        {
            this.Given(x => AuthModel_Is_With_Valid_Email_Bad_Password())
                    .And(x => User_Is_In_Db())
                .When(x => Login())
                    .Then(x => Exception_Should_Be_Thrown_Of_Type(typeof(InvalidPasswordException)))
                .BDDfy();
        }

        [Test]
        public void Register_Should_Not_Accept_Null_Body()
        {
            this.Given(x => AuthModel_Is_Null())
                .When(x => Register())
                .Then(x => Exception_Should_Be_Thrown())
                .BDDfy();
        }

        private void AuthModel_Is_Null()
        {
            _authModel = null;
        }

        private void AuthModel_Is_With_Not_Existed_Email()
        {
            _authModel = new AuthModel() { Email = "notexist@email.com" };
        }

        private void AuthModel_Is_With_Valid_Email_Bad_Password()
        {
            _authModel = new AuthModel()
            {
                Email = "exist@email.com",
                Password = "5e884898da280471"
            };
        }

        private void Exception_Function(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                _thrownException = ex;
            }
        }

        private void Exception_Should_Be_Thrown()
        {
            _thrownException.Should().NotBeNull();
        }

        private void Exception_Should_Be_Thrown_Of_Type(Type type)
        {
            Exception_Should_Be_Thrown();
            _thrownException.Should().BeOfType(type);
        }

        private void Login()
        {
            Exception_Function(() => new UsersController(default, _usersService).Login(_authModel));
        }

        private void Register()
        {
            Exception_Function(() => new UsersController(default, default).Register(_authModel));
        }

        private void User_Is_In_Db()
        {
            var mock = new Mock<IUsersService>();
            mock.Setup(x => x.GetUser(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new User()
                {
                    Password = "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8"
                });

            _usersService = mock.Object;
        }

        private void User_Is_Not_In_Db()
        {
            var mock = new Mock<IUsersService>();
            mock.Setup(x => x.GetUser(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(null as User);

            _usersService = mock.Object;
        }
    }
}