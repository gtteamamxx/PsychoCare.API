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
using PsychoCare.Common.Enums;

namespace PsychoCare.IntegrationTests
{
    public class EmotionalStatesServiceControllerTests
    {
        private readonly Mock<IWebPrincipal> _webPricipalMock = new Mock<IWebPrincipal>();
        private EmotionalState _emotionalState;
        private EmotionalStatesController _emotionalStatesController;
        private List<EmotionalState> _emotionalStatesList;
        private Exception _thrownException;
        private int _userId;

        [Test]
        [Order(1)]
        public void Adding_Valid_Emotional_State_Should_Put_It_To_DB()
        {
            this.Given(x => Valid_Emotional_State())
                .When(x => Add_Emotional_State_Request_Incoming())
                .Then(x => No_Exception_Should_Be_Thrown())
                    .And(x => Emotional_State_Should_Be_In_Db())
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
        public void Get_Users_Emotional_States_From_DB()
        {
            this.Given(x => Valid_UserId(3))
                .When(x => User_Has_Added_Emotional_States_Before())
                    .And(x => Get_Emotional_States_Request_Incoming())
                .Then(x => No_Exception_Should_Be_Thrown())
                    .And(x => Emotional_States_Should_Equal_Inserted())
                .BDDfy();
        }

        [OneTimeSetUp]
        public void Initialize()
        {
            TestContainerHolder.RegisterBaseDependencies();
            TestContainerHolder.Container.Register<IWebPrincipal>(() => _webPricipalMock.Object);
            TestContainerHolder.Container.Verify();
        }

        private void Add_Emotional_State_Request_Incoming()
        {
            _emotionalStatesController = TestContainerHolder.Container.GetInstance<EmotionalStatesController>();

            Exception_Function(() => _emotionalStatesController.AddEmotionalState(_emotionalState));
        }

        private void Emotional_State_Should_Be_In_Db()
        {
            using (var db = new TestPsychoCareContext())
            {
                EmotionalState emotionalState = db.EmotionalStates.SingleOrDefault(x => x.UserId == 3);
                emotionalState.Should().NotBeNull();
            }
        }

        private void Emotional_States_Should_Equal_Inserted()
        {
            using (var db = new TestPsychoCareContext())
            {
                List<EmotionalState> emotionalStatesDb = db.EmotionalStates.Where(x => x.UserId == _userId).ToList();

                foreach (EmotionalState expectedEmotionalState in _emotionalStatesList)
                {
                    EmotionalState emotioanlStateDb = emotionalStatesDb.First(x => x.Id == expectedEmotionalState.Id);
                    emotioanlStateDb.Should().NotBeNull();
                }
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

        private void Get_Emotional_States_Request_Incoming()
        {
            _emotionalStatesController = TestContainerHolder.Container.GetInstance<EmotionalStatesController>();

            Exception_Function(() => _emotionalStatesList = _emotionalStatesController.GetAllEmotionalStates());
        }

        private void No_Exception_Should_Be_Thrown()
        {
            _thrownException.Should().BeNull();
        }

        private void User_Has_Added_Emotional_States_Before()
        {
            using (var db = new TestPsychoCareContext())
            {
                db.EnvironmentGroups.Add(new EnvironmentGroup()
                {
                    UserId = _userId,
                    Name = string.Empty,
                });

                db.EmotionalStates.Add(new EmotionalState()
                {
                    UserId = _userId,
                    CreationDate = DateTime.Now,
                    EnvironmentGroupId = 1,
                    State = (int)EmotionalStatesEnum.Angry,
                });

                db.EmotionalStates.Add(new EmotionalState()
                {
                    UserId = _userId,
                    CreationDate = DateTime.Now,
                    EnvironmentGroupId = 1,
                    State = (int)EmotionalStatesEnum.Bored,
                });

                db.SaveChanges();
            }
        }

        private void Valid_Emotional_State()
        {
            _emotionalState = new EmotionalState()
            {
                CreationDate = DateTime.Now,
                EnvironmentGroupId = 2,
                UserId = 3,
                State = 2,
            };

            _webPricipalMock.Setup(x => x.UserId).Returns(3);
        }

        private void Valid_UserId(int userId)
        {
            _userId = userId;
            _webPricipalMock.Setup(x => x.UserId).Returns(userId);
        }
    }
}