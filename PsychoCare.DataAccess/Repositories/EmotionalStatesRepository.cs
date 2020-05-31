using System;
using System.Collections.Generic;
using System.Text;
using PsychoCare.DataAccess.Interfaces;
using PsychoCare.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace PsychoCare.DataAccess.Repositories
{
    public class EmotionalStatesRepository : IEmotionalStatesRepository
    {
        private readonly IDbContextBuilder _contextBuilder;

        public EmotionalStatesRepository(IDbContextBuilder contextBuilder)
        {
            _contextBuilder = contextBuilder;
        }

        /// <summary>
        /// Adds to DB emotional state
        /// </summary>
        /// <param name="emotionalState"></param>
        public void AddEmotionalState(EmotionalState emotionalState)
        {
            using (PsychoCareContext db = _contextBuilder.GetContext())
            {
                db.EmotionalStates.Add(emotionalState);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Downloads all user emotional states
        /// and includes them environment groups
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<EmotionalState> GetEmotionalStates(int userId)
        {
            List<EmotionalState> emotionalStates = null;

            using (PsychoCareContext db = _contextBuilder.GetContext())
            {
                emotionalStates = db.EmotionalStates.AsNoTracking()
                    .Where(x => x.UserId == userId)
                    .Include(x => x.EnvironmentGroup)
                    .ToList();
            }

            return emotionalStates;
        }
    }
}