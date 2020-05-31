using System;
using PsychoCare.DataAccess.Interfaces;
using PsychoCare.DataAccess.Entities;
using PsychoCare.DataAccess.Exceptions;
using PsychoCare.Logic.Interfaces;
using PsychoCare.Logic.Services;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using TestStack.BDDfy;

namespace PsychoCare.Logic.Tests.ServicesTests
{
    [TestFixture]
    class EnvironmentgroupsServiceTests
    {
        private IEnvironmentGroupsService _environmentGroupsService;
        private Exception _thrownException;
        private EnvironmentGroup _environmentGroup;
        private int _environmentGroupId;
        private IEnvironmentGroupsRepository _environmentGropusRepositiory;

        [OneTimeSetUp]
        public void Init()
        {
            var environmentGroupsServiceMock = new Mock<IEnvironmentGroupsService>();

            environmentGroupsServiceMock.Setup(x => x.AddPredefinedGroupsToUser(It.IsAny<int>()))
                .Callback(() => { });

            _environmentGroupsService = environmentGroupsServiceMock.Object;
        }

        [Test]
        public void Adding_Environment_Group_With_Null_Value_Should_Throw_Exception()
        {
            this.Given(x => Environment_Group_Is_Null())
                    .And(x => Environmnet_Group_Is_Not_In_Db())
                .When(x => Add_Environment_Group())
                .Then(x => Argument_Null_Exception_Should_Be_Thrown())
                .BDDfy();
        }

        [Test]
        public void Adding_Valid_Environment_Group_Should_Not_Throw_Exception()
        {
            this.Given(x => Environment_Group_With_With_Valid_Name_And_UserId())
                    .And(x => Environmnet_Group_Is_Not_In_Db())
                .When(x => Add_Environment_Group())
                .Then(x => No_Exception_Should_Be_Thrown())
                .BDDfy();
        }

        [Test]
        public void Adding_Environment_Group_With_Short_Name_Should_Throw_Exception()
        {
            this.Given(x => Environment_Group_With_To_Short_Name())
                .When(x => Add_Environment_Group())
                .Then(x => Invalid_Name_Exception_Should_Be_Throwed())
                .BDDfy();
        }

        [Test]
        public void Adding_Environment_Group_With_Long_Name_Should_Throw_Exception()
        {
            this.Given(x => Environment_Group_With_To_Long_Name())
                .When(x => Add_Environment_Group())
                .Then(x => Invalid_Name_Exception_Should_Be_Throwed())
                .BDDfy();
        }

        [Test]
        public void Adding_Environment_Group_With_Ivalid_User_Id_Should_Throw_Exception()
        {
            this.Given(x => Environment_Group_With_Invalid_UserId())
                .When(x => Add_Environment_Group())
                .Then(x => Invalid_Argument_Exception_Should_Be_Throwed())
                .BDDfy();
        }

        [Test]
        public void Adding_Environment_Group_Already_In_Db_Should_Throw_Exception()
        {
            this.Given(x => Environment_Group_With_With_Valid_Name_And_UserId())
                    .And(x => Environmnet_Group_Is_In_Db())
                .When(x => Add_Environment_Group())
                .Then(x => Environment_Group_Already_Exists_Exception_Should_Be_Throwed())
                .BDDfy();
        }

        [Test]
        public void Deleting_Environment_Group_Should_Not_Throw_Exception()
        {
            this.Given(x => Delete_Environment_Group_With_Valid_Id())
                    .And(x => Environmnet_Group_Found_In_Db())
                .When(x => Delete_Environment_Group())
                .Then(x => No_Exception_Should_Be_Thrown())
                .BDDfy();
        }

        [Test]
        public void Deleting_Environment_Group_That_Not_In_Db_Should_Throw_Exception()
        {
            this.Given(x => Delete_Environment_Group_With_Invalid_Id())
                    .And(x => Environmnet_Group_Not_Found_In_Db())
                .When(x => Delete_Environment_Group())
                .Then(x => Environment_Group_Not_Found_Exception_Should_Be_Throwed())
                .BDDfy();
        }

        [Test]
        public void Editing_Environment_Group_With_Null_Value_Should_Throw_Exception()
        {
            this.Given(x => Environment_Group_Is_Null())
                    .And(x => Environmnet_Group_Is_Not_In_Db())
                .When(x => Edit_Environment_Group())
                .Then(x => Argument_Null_Exception_Should_Be_Thrown())
                .BDDfy();
        }

        [Test]
        public void Editing_Valid_Environment_Group_Should_Not_Throw_Exception()
        {
            this.Given(x => Environment_Group_With_With_Valid_Name_And_UserId())
                    .And(x => Environmnet_Group_Is_Not_In_Db())
                .When(x => Edit_Environment_Group())
                .Then(x => No_Exception_Should_Be_Thrown())
                .BDDfy();
        }

        [Test]
        public void Editing_Environment_Group_With_Short_Name_Should_Throw_Exception()
        {
            this.Given(x => Environment_Group_With_To_Short_Name())
                .When(x => Edit_Environment_Group())
                .Then(x => Invalid_Name_Exception_Should_Be_Throwed())
                .BDDfy();
        }

        [Test]
        public void Editing_Environment_Group_With_Long_Name_Should_Throw_Exception()
        {
            this.Given(x => Environment_Group_With_To_Long_Name())
                .When(x => Edit_Environment_Group())
                .Then(x => Invalid_Name_Exception_Should_Be_Throwed())
                .BDDfy();
        }

        [Test]
        public void Editing_Environment_Group_With_Ivalid_User_Id_Should_Throw_Exception()
        {
            this.Given(x => Environment_Group_With_Invalid_UserId())
                .When(x => Edit_Environment_Group())
                .Then(x => Invalid_Argument_Exception_Should_Be_Throwed())
                .BDDfy();
        }

        [Test]
        public void Editing_Environment_Group_That_Already_In_Db_Should_Throw_Exception()
        {
            this.Given(x => Environment_Group_With_With_Valid_Name_And_UserId())
                    .And(x => Environmnet_Group_Is_In_Db())
                .When(x => Edit_Environment_Group())
                .Then(x => Environment_Group_Already_Exists_Exception_Should_Be_Throwed())
                .BDDfy();
        }

        [SetUp]
        public void Reset()
        {
            _environmentGroup = null;
            _environmentGroupId = 0;
            _thrownException = null;
            _environmentGropusRepositiory = null;
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

        private void No_Exception_Should_Be_Thrown()
        {
            _thrownException.Should().BeNull();
        }

        private void Argument_Null_Exception_Should_Be_Thrown()
        {
            _thrownException.Should().BeOfType(typeof(ArgumentNullException));
        }

        private void Invalid_Name_Exception_Should_Be_Throwed()
        {
            if (_thrownException != null)
            {
                _thrownException.Should().BeOfType(typeof(EnvironmentGroupInvalidNameException));
            }
        }

        private void Invalid_Argument_Exception_Should_Be_Throwed()
        {
            if (_thrownException != null)
            {
                _thrownException.Should().BeOfType(typeof(ArgumentException));
            }
        }

        private void Environment_Group_Already_Exists_Exception_Should_Be_Throwed()
        {
            if (_thrownException != null)
            {
                _thrownException.Should().BeOfType(typeof(EnvironmentGroupAlreadyExistsException));
            }
        }

        private void Environment_Group_Not_Found_Exception_Should_Be_Throwed()
        {
            if (_thrownException != null)
            {
                _thrownException.Should().BeOfType(typeof(EnvironmentGroupNotFoundException));
            }
        }

        private void Add_Environment_Group()
        {
            Exception_Function(() => new EnvironmentGroupsService(_environmentGropusRepositiory).AddEnvironmentGroup(_environmentGroup));
        }

        private void Delete_Environment_Group()
        {
            Exception_Function(() => new EnvironmentGroupsService(_environmentGropusRepositiory).DeleteEnvironmentGroup(_environmentGroupId));
        }

        private void Edit_Environment_Group()
        {
            Exception_Function(() => new EnvironmentGroupsService(_environmentGropusRepositiory).AddEnvironmentGroup(_environmentGroup));
        }

        private void Environmnet_Group_Is_In_Db()
        {
            var mock = new Mock<IEnvironmentGroupsRepository>();

            mock.Setup(x => x.GetEnvironmentGroupByUserAndName(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(new EnvironmentGroup());

            _environmentGropusRepositiory = mock.Object;
        }

        private void Environmnet_Group_Is_Not_In_Db()
        {
            var mock = new Mock<IEnvironmentGroupsRepository>();

            mock.Setup(x => x.GetEnvironmentGroupByUserAndName(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(null as EnvironmentGroup);

            _environmentGropusRepositiory = mock.Object;
        }

        private void Environmnet_Group_Not_Found_In_Db()
        {
            var mock = new Mock<IEnvironmentGroupsRepository>();

            mock.Setup(x => x.GetEnvironmentGroupById(It.IsAny<int>()))
                .Returns(null as EnvironmentGroup);

            _environmentGropusRepositiory = mock.Object;
        }

        private void Environmnet_Group_Found_In_Db()
        {
            var mock = new Mock<IEnvironmentGroupsRepository>();

            mock.Setup(x => x.GetEnvironmentGroupById(It.IsAny<int>()))
                .Returns(new EnvironmentGroup());

            _environmentGropusRepositiory = mock.Object;
        }

        private void Environment_Group_Is_Null()
        {
            _environmentGroup = null;
        }

        private void Environment_Group_With_With_Valid_Name_And_UserId()
        {
            _environmentGroup = new EnvironmentGroup()
            {
                Name = "Test",
                UserId = 2,
            };
        }

        private void Environment_Group_With_To_Short_Name()
        {
            _environmentGroup = new EnvironmentGroup()
            {
                Name = "Tt",
                UserId = 2,
            };
        }

        private void Environment_Group_With_To_Long_Name()
        {
            _environmentGroup = new EnvironmentGroup()
            {
                Name = new string('*', 100),
                UserId = 2,
            };
        }

        private void Environment_Group_With_Invalid_UserId()
        {
            _environmentGroup = new EnvironmentGroup()
            {
                Name = "Test",
                UserId = 0,
            };
        }

        private void Delete_Environment_Group_With_Invalid_Id()
        {
             _environmentGroupId = 99;
        }

        private void Delete_Environment_Group_With_Valid_Id()
        {
            _environmentGroupId = 1;
        }
    }
}
