using System;
using PsychoCare.DataAccess.Interfaces;
using PsychoCare.DataAccess.Entities;
using PsychoCare.Logic.Services;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using TestStack.BDDfy;

namespace PsychoCare.Logic.Tests.ServicesTests
{
    [TestFixture]
    class EmotionalStatesServiceTests
    {
        private Exception _thrownException;
        private EmotionalState _emotionalState;
        private IEmotionalStatesRepository _emotionalStatesRepository;

        [OneTimeSetUp]
        public void Init()
        {
            var emotionalSattesRepositoryMock = new Mock<IEmotionalStatesRepository>();

            emotionalSattesRepositoryMock.Setup(x => x.AddEmotionalState(It.IsAny<EmotionalState>()))
                .Callback(() => { });

            _emotionalStatesRepository = emotionalSattesRepositoryMock.Object;
        }

        [Test]
        public void Adding_Valid_Emotional_State_Should_Not_Throw_Exception()
        {
            this.Given(x => Valid_Emotional_State())
                .When(x => Add_Emotional_State())
                .Then(x => No_Exception_Should_Be_Thrown())
                .BDDfy();
        }

        [Test]
        public void Adding_Null_Emotional_State_Should_Throw_Exception()
        {
            this.Given(x => Emotional_State_Is_Null())
                .When(x => Add_Emotional_State())
                .Then(x => Argument_Null_Exception_Should_Be_Thrown())
                .BDDfy();
        }

        [Test]
        public void Adding_Emotional_State_Without_UserId_Should_Throw_Exception()
        {
            this.Given(x => Emotional_State_With_0_UserId())
                .When(x => Add_Emotional_State())
                .Then(x => Argument_Exception_Should_Be_Thrown())
                .BDDfy();
        }

        [Test]
        public void Adding_Emotional_State_Without_Environment_Group_Id_Should_Throw_Exception()
        {
            this.Given(x => Emotional_State_With_0_Environment_Group_Id())
                .When(x => Add_Emotional_State())
                .Then(x => Argument_Exception_Should_Be_Thrown())
                .BDDfy();
        }

        [SetUp]
        public void Reset()
        {
            _emotionalState = null;
            _thrownException = null;
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

        private void Argument_Exception_Should_Be_Thrown()
        {
            _thrownException.Should().BeOfType(typeof(ArgumentException));
        }

        private void Add_Emotional_State()
        {
            Exception_Function(() => new EmotionalStatesService(_emotionalStatesRepository).AddEmotionalState(_emotionalState));
        }


        private void Valid_Emotional_State()
        {
            _emotionalState = new EmotionalState()
            {
                EnvironmentGroupId = 3,
                UserId = 2,
                CreationDate = DateTime.Now,
                State = 1,
            };
        }

        private void Emotional_State_With_0_UserId()
        {
            _emotionalState = new EmotionalState()
            {
                EnvironmentGroupId = 3,
                UserId = 0,
                CreationDate = DateTime.Now,
                State = 1,
            };
        }

        private void Emotional_State_With_0_Environment_Group_Id()
        {
            _emotionalState = new EmotionalState()
            {
                EnvironmentGroupId = 0,
                UserId = 3,
                CreationDate = DateTime.Now,
                State = 1,
            };
        }

        private void Emotional_State_Is_Null()
        {
            _emotionalState = null;
        }
    }
}
