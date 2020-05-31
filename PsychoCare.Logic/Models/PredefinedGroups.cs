using PsychoCare.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PsychoCare.Logic.Models
{
    /// <summary>
    /// This model provides all user predefined groups
    /// </summary>
    public static class PredefinedGroups
    {
        public static List<EnvironmentGroup> GetPredefinedEnvironmentGroups(int userId)
        {
            var result = new List<EnvironmentGroup>();

            result.Add(new EnvironmentGroup()
            {
                UserId = userId,
                Name = "Ogólna",
            });

            result.Add(new EnvironmentGroup()
            {
                UserId = userId,
                Name = "Rodzina",
            });

            result.Add(new EnvironmentGroup()
            {
                UserId = userId,
                Name = "Praca",
            });

            result.Add(new EnvironmentGroup()
            {
                UserId = userId,
                Name = "Szkoła",
            });

            result.Add(new EnvironmentGroup()
            {
                UserId = userId,
                Name = "Znajomi",
            });

            result.Add(new EnvironmentGroup()
            {
                UserId = userId,
                Name = "Związek",
            });

            return result;
        }
    }
}