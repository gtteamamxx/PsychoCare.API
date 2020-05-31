using Moq;
using PsychoCare.API.Confiugration;
using PsychoCare.API.Services;
using PsychoCare.DataAccess.Interfaces;
using PsychoCare.DataAccess.Repositories;
using PsychoCare.DataAccess.Tests.Helpers;
using PsychoCare.Logic.Interfaces;
using PsychoCare.Logic.Services;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Text;

namespace PsychoCare.IntegrationTests.Helpers
{
    public class TestContainerHolder
    {
        public static Container Container => ContainerHolder.Container;

        public static void RegisterBaseDependencies()
        {
            ResetConatiner();

            ContainerHolder.RegisterCommonDependencies();
            Container.Options.AllowOverridingRegistrations = true;

            var contextBuilderMock = new Mock<IDbContextBuilder>();
            contextBuilderMock.Setup(x => x.GetContext())
                .Returns(() => new TestPsychoCareContext());

            ContainerHolder.Container.Register(typeof(IDbContextBuilder), () => contextBuilderMock.Object, Lifestyle.Transient);
        }

        private static void ResetConatiner()
        {
            ContainerHolder.Container.Dispose();
            ContainerHolder.Container = new Container();
        }
    }
}