using FluentAssertions;
using NUnit.Framework;
using PsychoCare.API.Controllers;
using PsychoCare.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using TestStack.BDDfy;

namespace PsychoCare.API.Tests.ControllersTests
{
    [TestFixture]
    public class EnvironmentGroupsControllerTests
    {
        private EnvironmentGroup _environmentGroup;
        private int _environmentGroupId;
        private Exception _thrownException;

        [Test]
        public void Add_Environment_Group_Should_Not_Accept_Null_Body()
        {
            this.Given(x => Environment_Group_Is_Null())
                .When(x => Add_Environment_Group())
                .Then(x => Exception_Should_Be_Thrown())
                .BDDfy();
        }

        [Test]
        public void Delete_Environment_Group_Should_Not_Accept_Id_Zero()
        {
            this.Given(x => Environment_Group_Id_Is_Zero())
                .When(x => Delete_Environment_Group())
                .Then(x => Exception_Should_Be_Thrown())
                .BDDfy();
        }

        [Test]
        public void Edit_Environment_Group_Should_Not_Accept_Null_Body()
        {
            this.Given(x => Environment_Group_Is_Null())
                .When(x => Edit_Environment_Group())
                .Then(x => Exception_Should_Be_Thrown())
                .BDDfy();
        }

        private void Add_Environment_Group()
        {
            Exception_Function(() => new EnvironmentGroupsController(null, null).AddEnvironmentGroup(_environmentGroup));
        }

        private void Delete_Environment_Group()
        {
            Exception_Function(() => new EnvironmentGroupsController(null, null).DeleteEnvironmentGroup(_environmentGroupId));
        }

        private void Edit_Environment_Group()
        {
            Exception_Function(() => new EnvironmentGroupsController(null, null).EditEnvironmentGroupName(_environmentGroup));
        }

        private void Environment_Group_Id_Is_Zero()
        {
            _environmentGroupId = 0;
        }

        private void Environment_Group_Is_Null()
        {
            _environmentGroup = null;
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
    }
}