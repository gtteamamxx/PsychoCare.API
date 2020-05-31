using PsychoCare.API.Services;
using PsychoCare.DataAccess;
using PsychoCare.DataAccess.Interfaces;
using PsychoCare.DataAccess.Repositories;
using PsychoCare.Logic.Interfaces;
using PsychoCare.Logic.Services;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PsychoCare.API.Confiugration
{
    public static class ContainerHolder
    {
        public static Container Container = new Container();

        /// <summary>
        /// IoC common dependencies
        /// </summary>
        public static void RegisterCommonDependencies()
        {
            Container.Register<IUsersRepository, UsersRepository>();
            Container.Register<IUsersService, UsersService>();

            Container.Register<IEnvironmentGroupsRepository, EnvironmentGroupsRepository>();
            Container.Register<IEnvironmentGroupsService, EnvironmentGroupsService>();

            Container.Register<IEmotionalStatesRepository, EmotionalStatesRepository>();
            Container.Register<IEmotionalStatesService, EmotionalStatesService>();

            Container.Register<ITokenService, TokenService>();

            Container.Register<IDbContextBuilder, PsychoCareContextBuilder>();
        }
    }
}