using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using PsychoCare.API.Controllers;
using PsychoCare.API.Models;
using PsychoCare.API.Services;
using PsychoCare.DataAccess.Interfaces;
using PsychoCare.DataAccess.Tests.Helpers;
using PsychoCare.IntegrationTests.Helpers;
using PsychoCare.Logic.DataAccess;
using System;
using System.Linq;
using TestStack.BDDfy;

namespace PsychoCare.IntegrationTests
{
    [TestFixture]
    public class UsersControllerTests
    {
        private AuthModel _authModel;
        private Exception _thrownException;
        private string _token;
        private UsersController _usersController;

        [OneTimeTearDown]
        public void Dispose()
        {
            using var db = new TestPsychoCareContext();
            db.Database.EnsureDeleted();
        }

        [OneTimeSetUp]
        public void Initialize()
        {
            TestContainerHolder.RegisterBaseDependencies();
            TestContainerHolder.Container.Verify();

            _usersController = TestContainerHolder.Container.GetInstance<UsersController>();
        }

        [Test]
        [Order(2)]
        public void Logging_With_User_Should_Return_Token_With_User()
        {
            this.Given(x => Auth_Model_With_Email_And_Password(), "Email: test@test.com and password SHA256")
                .When(x => Login_User_Request_Incoming())
                .Then(x => No_Exception_Should_Be_Thrown())
                    .And(x => Token_Should_Be_Returned())
                    .And(x => Decoded_Token_Should_Return_Valid_User_Id("test@test.com"))
                .BDDfy();
        }

        [Test]
        [Order(1)]
        public void Registering_Valid_User_Should_Add_Him_Into_Db()
        {
            this.Given(x => Auth_Model_With_Email_And_Password(), "Email: test@test.com and password SHA256")
                .When(x => Register_User_Request_Incoming())
                .Then(x => No_Exception_Should_Be_Thrown())
                    .And(x => User_Should_Be_In_Db("test@test.com"))
                .BDDfy();
        }

        [SetUp]
        public void Reset()
        {
            _thrownException = null;
            _token = null;
        }

        private void Auth_Model_With_Email_And_Password()
        {
            _authModel = new AuthModel()
            {
                Email = "test@test.com",
                Password = "741f67765bef6f01f37bf5cb1724509a83409324efa6ad2586d27f4e3edea296"
            };
        }

        private void Decoded_Token_Should_Return_Valid_User_Id(string email)
        {
            ITokenService tokenService = TestContainerHolder.Container.GetInstance<ITokenService>();
            IUsersRepository usersRepository = TestContainerHolder.Container.GetInstance<IUsersRepository>();

            Assert.DoesNotThrow(() =>
            {
                int userId = tokenService.GetUserIdFromToken(_token);

                User user = usersRepository.FindUserByEmail(email);

                user.Id.Should().Be(userId);
            });
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

        private void Login_User_Request_Incoming()
        {
            Exception_Function(() => _token = ((OkObjectResult)_usersController.Login(_authModel)).Value as string);
        }

        private void No_Exception_Should_Be_Thrown()
        {
            _thrownException.Should().BeNull();
        }

        private void Register_User_Request_Incoming()
        {
            Exception_Function(() => _usersController.Register(_authModel));
        }

        private void Token_Should_Be_Returned()
        {
            _token.Should().NotBeNull();
        }

        private void User_Should_Be_In_Db(string email)
        {
            using (var db = new TestPsychoCareContext())
            {
                User user = db.Users.SingleOrDefault(x => x.Email == email);
                user.Should().NotBeNull();
            }
        }
    }
}