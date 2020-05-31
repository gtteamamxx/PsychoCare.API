using PsychoCare.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PsychoCare.DataAccess.Interfaces
{
    public interface IEnvironmentGroupsRepository
    {
        int AddEnvironmentGroup(EnvironmentGroup environmentGroup);

        void DeleteEnvironmentGroup(int environmentGroupId);

        void EditEnvironmentGroupName(EnvironmentGroup environmentGroup);

        EnvironmentGroup GetEnvironmentGroupById(int environmentGroupId);

        EnvironmentGroup GetEnvironmentGroupByUserAndName(int userId, string name);

        List<EnvironmentGroup> GetUserEnvironmentGroups(int userId);
    }
}