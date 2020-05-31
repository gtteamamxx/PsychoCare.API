using System;
using System.Collections.Generic;
using PsychoCare.Common.Interfaces;
using PsychoCare.DataAccess.Entities;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using TestStack.BDDfy;
using PsychoCare.API.Controllers;
using PsychoCare.DataAccess.Tests.Helpers;
using PsychoCare.IntegrationTests.Helpers;
using System.Linq;

namespace PsychoCare.IntegrationTests
{
    [TestFixture]
    public class EnvironmentGroupsControllerTests
    {
        private EnvironmentGroup _environmentGroup;
        private EnvironmentGroupsController _environmentGroupController;
        private int _environmentGroupId;
        private List<EnvironmentGroup> _environmentGroupsList;
        private Exception _thrownException;
        private Mock<IWebPrincipal> _webPricipalMock = new Mock<IWebPrincipal>();

        [Test]
        [Order(1)]
        public void Adding_Valid_Environment_Group_Should_Put_Her_To_DB()
        {
            this.Given(x => Environment_Group_With_With_Valid_Name_And_UserId(), "Name: Test")
                .When(x => Add_Environment_Group_Request_Incoming())
                .Then(x => No_Exception_Should_Be_Thrown())
                    .And(x => Environment_Group_Should_Be_In_Db("Test"))
                .BDDfy();
        }

        [Test]
        [Order(4)]
        public void Delete_Environment_Group_Should_Remove_Her_From_DB()
        {
            this.Given(x => Evironment_Group_To_Delete_Id("Test Edited"))
                .When(x => Delete_Environment_Group_Request_Incoming())
                .Then(x => No_Exception_Should_Be_Thrown())
                    .And(x => Environment_Group_Shouldynt_Be_In_Db(3))
                .BDDfy();
        }

        [OneTimeTearDown]
        public void Dispose()
        {
            using var db = new TestPsychoCareContext();
            db.Database.EnsureDeleted();
        }

        [Test]
        [Order(2)]
        public void Editing_Valid_Environment_Group_Should_Change_Her_In_DB()
        {
            this.Given(x => Environment_Group_To_Edit_With_With_Valid_Name_And_UserId(), "NewName: Test Edited")
                .When(x => Edit_Environment_Group_Name_Request_Incoming())
                .Then(x => No_Exception_Should_Be_Thrown())
                    .And(x => Environment_Group_Should_Be_In_Db("Test Edited"))
                .BDDfy();
        }

        [Test]
        [Order(3)]
        public void Get_Environment_Groups_Should_Return_Groups_From_DB()
        {
            this.Given(x => Valid_UserId(), "UserId: 3")
                .When(x => Get_Environment_Groups_Request_Incoming())
                .Then(x => No_Exception_Should_Be_Thrown())
                    .And(x => Environment_Group_Should_Equal_Inseterd(3))
                .BDDfy();
        }

        [OneTimeSetUp]
        public void Initialize()
        {
            TestContainerHolder.RegisterBaseDependencies();
            TestContainerHolder.Container.Register<IWebPrincipal>(() => _webPricipalMock.Object);
            TestContainerHolder.Container.Verify();
        }

        [SetUp]
        public void Reset()
        {
            _thrownException = null;
            _environmentGroup = null;
            _webPricipalMock = new Mock<IWebPrincipal>();
        }

        private void Add_Environment_Group_Request_Incoming()
        {
            _environmentGroupController = TestContainerHolder.Container.GetInstance<EnvironmentGroupsController>();

            Exception_Function(() => _environmentGroupId = _environmentGroupController.AddEnvironmentGroup(_environmentGroup));
        }

        private void Delete_Environment_Group_Request_Incoming()
        {
            _environmentGroupController = TestContainerHolder.Container.GetInstance<EnvironmentGroupsController>();

            Exception_Function(() => _environmentGroupController.DeleteEnvironmentGroup(_environmentGroupId));
        }

        private void Edit_Environment_Group_Name_Request_Incoming()
        {
            _environmentGroupController = TestContainerHolder.Container.GetInstance<EnvironmentGroupsController>();

            Exception_Function(() => _environmentGroupController.EditEnvironmentGroupName(_environmentGroup));
        }

        private void Environment_Group_Should_Be_In_Db(string name)
        {
            using (var db = new TestPsychoCareContext())
            {
                EnvironmentGroup environmentGroup = db.EnvironmentGroups.SingleOrDefault(x => x.Name == name);
                environmentGroup.Should().NotBeNull();
            }
        }

        private void Environment_Group_Should_Equal_Inseterd(int id)
        {
            using (var db = new TestPsychoCareContext())
            {
                List<EnvironmentGroup> environmentGroups = db.EnvironmentGroups.Where(x => x.UserId == id)
                        .ToList();
                environmentGroups.Should().BeEquivalentTo(_environmentGroupsList);
            }
        }

        private void Environment_Group_Shouldynt_Be_In_Db(int id)
        {
            using (var db = new TestPsychoCareContext())
            {
                EnvironmentGroup environmentGroup = db.EnvironmentGroups.SingleOrDefault(x => x.Id == id);
                environmentGroup.Should().BeNull();
            }
        }

        private void Environment_Group_To_Edit_With_With_Valid_Name_And_UserId()
        {
            using (var db = new TestPsychoCareContext())
            {
                EnvironmentGroup environmentGroup = db.EnvironmentGroups.SingleOrDefault(x => x.Id == _environmentGroupId);
                _environmentGroup = new EnvironmentGroup()
                {
                    Name = "Test Edited",
                    UserId = 3,
                    Id = environmentGroup.Id,
                };
            }

            _webPricipalMock.Setup(x => x.UserId).Returns(3);
        }

        private void Environment_Group_With_With_Valid_Name_And_UserId()
        {
            _environmentGroup = new EnvironmentGroup()
            {
                Name = "Test",
                UserId = 0,
            };

            _webPricipalMock.Setup(x => x.UserId).Returns(3);
        }

        private void Evironment_Group_To_Delete_Id(string name)
        {
            using (var db = new TestPsychoCareContext())
            {
                EnvironmentGroup environmentGroup = db.EnvironmentGroups.SingleOrDefault(x => x.Name == name);
                _environmentGroupId = environmentGroup.Id;
            }
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

        private void Get_Environment_Groups_Request_Incoming()
        {
            _environmentGroupController = TestContainerHolder.Container.GetInstance<EnvironmentGroupsController>();

            Exception_Function(() => _environmentGroupsList = _environmentGroupController.GetUserEnvironmentGroups());
        }

        private void No_Exception_Should_Be_Thrown()
        {
            _thrownException.Should().BeNull();
        }

        private void Valid_UserId()
        {
            _webPricipalMock.Setup(x => x.UserId).Returns(3);
        }
    }
}