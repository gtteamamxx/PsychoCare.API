using PsychoCare.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PsychoCare.Logic.Interfaces
{
    public interface IEnvironmentGroupsService
    {
        int AddEnvironmentGroup(EnvironmentGroup environmentGroup);

        void AddPredefinedGroupsToUser(int userId);

        void DeleteEnvironmentGroup(int environmentGroupId);

        void EditEnvironmentGroup(EnvironmentGroup environmentGroup);

        List<EnvironmentGroup> GetUserEnvironmentGroups(int userId);
    }
}