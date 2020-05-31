using FluentAssertions;
using Moq;
using NUnit.Framework;
using PsychoCare.DataAccess.Exceptions;
using PsychoCare.DataAccess.Interfaces;
using PsychoCare.Logic.DataAccess;
using PsychoCare.Logic.Exceptions;
using PsychoCare.Logic.Interfaces;
using PsychoCare.Logic.Services;
using System;
using System.Collections.Generic;
using System.Text;
using TestStack.BDDfy;
using TestStack.BDDfy.Annotations;

namespace PsychoCare.Logic.Tests.ServicesTests
{
    [TestFixture]
    public class UsersServiceTests
    {
        private IEnvironmentGroupsService _environmentGroupsService;
        private Exception _thrownException;
        private User _user;
        private IUsersRepository _usersRepository;

        [OneTimeSetUp]
        public void Init()
        {
            var environmentGroupsServiceMock = new Mock<IEnvironmentGroupsService>();

            environmentGroupsServiceMock.Setup(x => x.AddPredefinedGroupsToUser(It.IsAny<int>()))
                .Callback(() => { });

            _environmentGroupsService = environmentGroupsServiceMock.Object;
        }

        [Test]
        public void Registering_New_User_Should_Not_Throw_Exception()
        {
            this.Given(x => User_With_With_Valid_Email_And_Password())
                    .And(x => User_Is_Not_In_Db())
                .When(x => Register_User())
                .Then(x => No_Exception_Should_Be_Thrown())
                .BDDfy();
        }

        [Test]
        public void Registering_User_Already_Existed_Should_Throw_Exception()
        {
            this.Given(x => User_With_With_Valid_Email_And_Password())
                    .And(x => User_Is_In_Db())
                .When(x => Register_User())
                .Then(x => User_Already_Exist_Exception_Should_Be_Thrown())
                .BDDfy();
        }

        [Test]
        public void Registering_User_With_Emails_Length_150_Should_Throw_Exception()
        {
            this.Given(x => User_Is_With_Emails_Length_150())
                .When(x => Register_User())
                .Then(x => Invalid_Email_Exception_Should_Be_Thrown())
                .BDDfy();
        }

        [Test]
        public void Registering_User_With_Empty_Email_Should_Throw_Exception()
        {
            this.Given(x => User_Is_With_Empty_Email())
                .When(x => Register_User())
                .Then(x => Invalid_Email_Exception_Should_Be_Thrown())
                .BDDfy();
        }

        [Test]
        public void Registering_User_With_Invalid_Email_Should_Throw_Exception()
        {
            this.Given(x => User_Is_With_Invalid_Email())
                .When(x => Register_User())
                .Then(x => Invalid_Email_Exception_Should_Be_Thrown())
                .BDDfy();
        }

        [Test]
        public void Registering_User_With_Null_Email_Should_Throw_Exception()
        {
            this.Given(x => User_Is_With_Null_Email())
                .When(x => Register_User())
                .Then(x => Invalid_Email_Exception_Should_Be_Thrown())
                .BDDfy();
        }

        [Test]
        public void Registering_User_With_Null_Password_Should_Throw_Exception()
        {
            this.Given(x => User_Is_With_Null_Password())
                .When(x => Register_User())
                .Then(x => Invalid_Password_Exception_Should_Be_Thrown())
                .BDDfy();
        }

        [Test]
        public void Registering_User_With_Null_Value_Should_Throw_Exception()
        {
            this.Given(x => User_Is_Null())
                .When(x => Register_User())
                .Then(x => Argument_Null_Exception_Should_Be_Thrown())
                .BDDfy();
        }

        [Test]
        public void Registering_User_With_Password_Length_Less_Greather_Than_64_Chars_Should_Throw_Exception()
        {
            this.Given(x => User_Is_With_Password_Greather_Than_64_Chars())
                .When(x => Register_User())
                .Then(x => Invalid_Password_Exception_Should_Be_Thrown())
                .BDDfy();
        }

        [Test]
        public void Registering_User_With_Password_Length_Less_Than_64_Chars_Should_Throw_Exception()
        {
            this.Given(x => User_Is_With_Password_Less_Than_64_Chars())
                .When(x => Register_User())
                .Then(x => Invalid_Password_Exception_Should_Be_Thrown())
                .BDDfy();
        }

        [Test]
        public void Registering_User_With_Valid_Email_Not_Throw_Invalid_Email_Exception()
        {
            this.Given(x => User_Is_With_Valid_Email())
                .When(x => Register_User())
                .Then(x => Invalid_Email_Exception_Should_Not_Be_Throwed())
                .BDDfy();
        }

        [Test]
        public void Registering_User_With_Valid_Password_Should_Not_Throw_Exception()
        {
            this.Given(x => User_Is_With_Valid_Password())
                .When(x => Register_User())
                .Then(x => Invalid_Password_Exception_Should_Not_Be_Throwed())
                .BDDfy();
        }

        [SetUp]
        public void Reset()
        {
            _user = null;
            _thrownException = null;
            _usersRepository = null;
        }

        private void Argument_Null_Exception_Should_Be_Thrown()
        {
            _thrownException.Should().BeOfType(typeof(ArgumentNullException));
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

        private void Invalid_Email_Exception_Should_Be_Thrown()
        {
            _thrownException.Should().BeOfType(typeof(InvalidEmailException));
        }

        private void Invalid_Email_Exception_Should_Not_Be_Throwed()
        {
            if (_thrownException != null)
            {
                _thrownException.Should().NotBeOfType(typeof(InvalidEmailException));
            }
        }

        private void Invalid_Password_Exception_Should_Be_Thrown()
        {
            _thrownException.Should().BeOfType(typeof(InvalidPasswordException));
        }

        private void Invalid_Password_Exception_Should_Not_Be_Throwed()
        {
            if (_thrownException != null)
            {
                _thrownException.Should().NotBeOfType(typeof(InvalidPasswordException));
            }
        }

        private void No_Exception_Should_Be_Thrown()
        {
            _thrownException.Should().BeNull();
        }

        private void Register_User()
        {
            Exception_Function(() => new UsersService(_usersRepository, _environmentGroupsService).Register(_user));
        }

        private void User_Already_Exist_Exception_Should_Be_Thrown()
        {
            _thrownException.Should().BeOfType(typeof(UserAlreadyExistsException));
        }

        private void User_Is_In_Db()
        {
            var mock = new Mock<IUsersRepository>();

            mock.Setup(x => x.FindUserByEmail(It.IsAny<string>()))
                .Returns(new User());

            _usersRepository = mock.Object;
        }

        private void User_Is_Not_In_Db()
        {
            var mock = new Mock<IUsersRepository>();

            mock.Setup(x => x.FindUserByEmail(It.IsAny<string>()))
                .Returns(null as User);

            _usersRepository = mock.Object;
        }

        private void User_Is_Null()
        {
            _user = null;
        }

        private void User_Is_With_Emails_Length_150()
        {
            _user = new User()
            {
                Email = "this_is_sample_email_which_length_can_be_longer_than_one_hundred_fifty_chars_and_some_longer_test_additional@email.domain.google_facebook.net.com.swe"
            };
        }

        private void User_Is_With_Empty_Email()
        {
            _user = new User()
            {
                Email = string.Empty
            };
        }

        private void User_Is_With_Invalid_Email()
        {
            _user = new User()
            {
                Email = "invalid_email.net.com"
            };
        }

        private void User_Is_With_Null_Email()
        {
            _user = new User()
            {
                Email = null
            };
        }

        private void User_Is_With_Null_Password()
        {
            _user = new User()
            {
                Email = "valid_email@net.com",
                Password = null
            };
        }

        private void User_Is_With_Password_Greather_Than_64_Chars()
        {
            _user = new User()
            {
                Email = "valid_email@net.com",
                Password = "DK2JJ3107V80CXVNSLDNTL23HK501U90DAJSKLD23I1JO2IDJZKLXFJSDOIRH32OIRHFJLDXVNXJDVHIUWFNJFEWNJKFNJXCNVXCJKVNKJB4JKRBKJXBVK"
            };
        }

        private void User_Is_With_Password_Less_Than_64_Chars()
        {
            _user = new User()
            {
                Email = "valid_email@net.com",
                Password = "DK2JJ3107V80CXVNSLDNTL23HK501U90DAJSKLD"
            };
        }

        private void User_Is_With_Valid_Email()
        {
            _user = new User()
            {
                Email = "some_valid_email@microsoft.com"
            };
        }

        private void User_Is_With_Valid_Password()
        {
            _user = new User()
            {
                Email = "valid_email@net.com",
                Password = "42a9798b99d4afcec9995e47a1d246b98ebc96be7a732323eee39d924006ee1d"
            };
        }

        private void User_With_With_Valid_Email_And_Password()
        {
            _user = new User()
            {
                Email = "valid_email@net.com",
                Password = "42a9798b99d4afcec9995e47a1d246b98ebc96be7a732323eee39d924006ee1d"
            };
        }
    }
}