 using PsychoCare.DataAccess.Entities;
using PsychoCare.DataAccess.Exceptions;
using PsychoCare.DataAccess.Interfaces;
using PsychoCare.Logic.Interfaces;
using PsychoCare.Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PsychoCare.Logic.Services
{
    public class EnvironmentGroupsService : IEnvironmentGroupsService
    {
        private readonly IEnvironmentGroupsRepository _environmentGroupsRepository;

        public EnvironmentGroupsService(IEnvironmentGroupsRepository environmentGroupsRepository)
        {
            _environmentGroupsRepository = environmentGroupsRepository;
        }

        /// <summary>
        /// Validates and adds user environment group
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="EnvironmentGroupInvalidNameException"/>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="EnvironmentGroupAlreadyExistsException"/>
        public int AddEnvironmentGroup(EnvironmentGroup environmentGroup)
        {
            if (environmentGroup == null) throw new ArgumentNullException(nameof(environmentGroup));

            environmentGroup.Name = environmentGroup.Name?.Trim();
            environmentGroup.Validate();

            CheckIfSimilarEnvironmentGroupAlreadyExist(environmentGroup);

            int id = _environmentGroupsRepository.AddEnvironmentGroup(environmentGroup);
            return id;
        }

        /// <summary>
        /// Adds predefined groups to user
        /// </summary>
        public void AddPredefinedGroupsToUser(int userId)
        {
            List<EnvironmentGroup> predefinedEnvironmentGroups = PredefinedGroups.GetPredefinedEnvironmentGroups(userId);

            foreach (EnvironmentGroup predefinedGroup in predefinedEnvironmentGroups)
            {
                _environmentGroupsRepository.AddEnvironmentGroup(predefinedGroup);
            }
        }

        /// <summary>
        /// Deletes environment group by id
        /// </summary>
        /// <exception cref="EnvironmentGroupNotFoundException"/>
        public void DeleteEnvironmentGroup(int environmentGroupId)
        {
            CheckIfEnvironmentGroupExists(environmentGroupId);

            _environmentGroupsRepository.DeleteEnvironmentGroup(environmentGroupId);
        }

        /// <summary>
        /// Validates and edits environment group
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="EnvironmentGroupInvalidNameException"/>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="EnvironmentGroupNotFoundException"/>
        /// <exception cref="EnvironmentGroupAlreadyExistsException"/>
        public void EditEnvironmentGroup(EnvironmentGroup environmentGroup)
        {
            if (environmentGroup == null) throw new ArgumentNullException(nameof(environmentGroup));
            if (environmentGroup.Id == 0) throw new ArgumentException(nameof(environmentGroup.Id));

            environmentGroup.Name = environmentGroup.Name?.Trim();
            environmentGroup.Validate();

            CheckIfEnvironmentGroupExists(environmentGroup.Id);
            CheckIfSimilarEnvironmentGroupAlreadyExist(environmentGroup);

            _environmentGroupsRepository.EditEnvironmentGroupName(environmentGroup);
        }

        /// <summary>
        /// Gets user all environment groups
        /// </summary>
        public List<EnvironmentGroup> GetUserEnvironmentGroups(int userId)
        {
            List<EnvironmentGroup> environmentGroups = _environmentGroupsRepository.GetUserEnvironmentGroups(userId);
            return environmentGroups;
        }

        /// <summary>
        /// Checks if environment gorup already exists.
        /// If false then exception is thrown
        /// </summary>
        /// <exception cref="EnvironmentGroupNotFoundException"/>
        private void CheckIfEnvironmentGroupExists(int environmentGroupId)
        {
            EnvironmentGroup environmentGroupInDb = _environmentGroupsRepository.GetEnvironmentGroupById(environmentGroupId);

            if (environmentGroupInDb == null) throw new EnvironmentGroupNotFoundException();
        }

        /// <summary>
        /// Checks if similar environment group already exist.
        /// (matches user id and name)
        /// If true then exception is thrown
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="EnvironmentGroupAlreadyExistsException"/>
        private void CheckIfSimilarEnvironmentGroupAlreadyExist(EnvironmentGroup environmentGroupToFind)
        {
            if (environmentGroupToFind == null) throw new ArgumentNullException(nameof(environmentGroupToFind));

            EnvironmentGroup environmentGroupInDb
                = _environmentGroupsRepository.GetEnvironmentGroupByUserAndName(
                    environmentGroupToFind.UserId,
                    environmentGroupToFind.Name?.ToLower());

            if (environmentGroupInDb != null) throw new EnvironmentGroupAlreadyExistsException();
        }
    }
}