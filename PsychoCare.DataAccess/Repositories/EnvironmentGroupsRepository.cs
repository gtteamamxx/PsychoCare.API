using Microsoft.EntityFrameworkCore;
using PsychoCare.DataAccess.Entities;
using PsychoCare.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PsychoCare.DataAccess.Repositories
{
    public class EnvironmentGroupsRepository : IEnvironmentGroupsRepository
    {
        private readonly IDbContextBuilder _contextBuilder;

        public EnvironmentGroupsRepository(IDbContextBuilder contextBuilder)
        {
            _contextBuilder = contextBuilder;
        }

        /// <summary>
        /// Adds environment group
        /// </summary>
        /// <param name="environmentGroup"></param>
        /// <returns></returns>
        public int AddEnvironmentGroup(EnvironmentGroup environmentGroup)
        {
            using (PsychoCareContext db = _contextBuilder.GetContext())
            {
                db.EnvironmentGroups.Add(environmentGroup);
                db.SaveChanges();
            }

            return environmentGroup.Id;
        }

        /// <summary>
        /// Deletes environment group
        /// </summary>
        /// <param name="environmentGroupId"></param>
        public void DeleteEnvironmentGroup(int environmentGroupId)
        {
            using (PsychoCareContext db = _contextBuilder.GetContext())
            {
                EnvironmentGroup environmentGroup = db.EnvironmentGroups.Find(environmentGroupId);

                db.EnvironmentGroups.Remove(environmentGroup);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Edits environment group
        ///
        /// Speccialy edits only name
        /// </summary>
        /// <param name="environmentGroup"></param>
        public void EditEnvironmentGroupName(EnvironmentGroup environmentGroup)
        {
            using (PsychoCareContext db = _contextBuilder.GetContext())
            {
                EnvironmentGroup environmentGroupDb = db.EnvironmentGroups
                    .FirstOrDefault(x => x.Id == environmentGroup.Id);

                environmentGroupDb.Name = environmentGroup.Name;
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Gets environment group by id
        /// </summary>
        /// <param name="environmentGroupId"></param>
        /// <returns></returns>
        public EnvironmentGroup GetEnvironmentGroupById(int environmentGroupId)
        {
            using (PsychoCareContext db = _contextBuilder.GetContext())
            {
                EnvironmentGroup environmentGroup
                    = db.EnvironmentGroups.AsNoTracking()
                        .FirstOrDefault(x => x.Id == environmentGroupId);

                return environmentGroup;
            }
        }

        /// <summary>
        /// Gets environment group by user id and environment group name
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public EnvironmentGroup GetEnvironmentGroupByUserAndName(int userId, string name)
        {
            using (PsychoCareContext db = _contextBuilder.GetContext())
            {
                EnvironmentGroup environmentGroup
                    = db.EnvironmentGroups.AsNoTracking()
                        .FirstOrDefault(x => x.UserId == userId && x.Name.ToLower() == name);

                return environmentGroup;
            }
        }

        /// <summary>
        /// Gets all user environment groups
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<EnvironmentGroup> GetUserEnvironmentGroups(int userId)
        {
            using (PsychoCareContext db = _contextBuilder.GetContext())
            {
                List<EnvironmentGroup> environmentGroups
                    = db.EnvironmentGroups.AsNoTracking()
                        .Where(x => x.UserId == userId)
                        .ToList();

                return environmentGroups;
            }
        }
    }
}