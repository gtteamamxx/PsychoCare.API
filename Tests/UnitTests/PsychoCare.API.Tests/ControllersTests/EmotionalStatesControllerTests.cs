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
    public class EmotionalStatesControllerTests
    {
        private EmotionalState _emotionalState;
        private string _targetUserToken;
        private Exception _thrownException;

        [Test]
        public void Add_Emotional_State_ShouldNot_Accept_Null_Token()
        {
            this.Given(x => Emotional_State_Is_Null())
                .When(x => Add_Emotional_State())
                .Then(x => Exception_Should_Be_Thrown())
                .BDDfy();
        }

        [Test]
        public void Getting_All_Emotional_States_For_User_Should_Not_Accept_Null_Token()
        {
            this.Given(x => Target_User_Token_Is_Null())
                .When(x => Get_All_Emotional_States_For_User())
                .Then(x => Exception_Should_Be_Thrown())
                .BDDfy();
        }

        private void Add_Emotional_State()
        {
            Exception_Function(() => new EmotionalStatesController(null, null, null).AddEmotionalState(_emotionalState));
        }

        private void Emotional_State_Is_Null()
        {
            _emotionalState = null;
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

        private void Get_All_Emotional_States_For_User()
        {
            Exception_Function(() => new EmotionalStatesController(null, null, null).GetAllEmotionalStatesForUser(_targetUserToken));
        }

        private void Target_User_Token_Is_Null()
        {
            _targetUserToken = null;
        }
    }
}