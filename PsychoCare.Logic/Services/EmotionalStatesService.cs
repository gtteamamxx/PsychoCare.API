using System;
using System.Collections.Generic;
using PsychoCare.DataAccess.Entities;
using PsychoCare.DataAccess.Interfaces;
using PsychoCare.Logic.Interfaces;

namespace PsychoCare.Logic.Services
{
    public class EmotionalStatesService : IEmotionalStatesService
    {
        private readonly IEmotionalStatesRepository _emotionalStatesRepository;

        public EmotionalStatesService(IEmotionalStatesRepository emotionalStatesRepository)
        {
            _emotionalStatesRepository = emotionalStatesRepository;
        }

        /// <summary>
        /// Validates and adds emotional state
        /// if added model has not vlaid datetime then uses server datetime
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        public void AddEmotionalState(EmotionalState emotionalState)
        {
            if (emotionalState is null) throw new ArgumentNullException(nameof(emotionalState));

            emotionalState.Validate();

            if (emotionalState.CreationDate == DateTime.MinValue)
            {
                emotionalState.CreationDate = DateTime.Now;
            }

            _emotionalStatesRepository.AddEmotionalState(emotionalState);
        }

        /// <summary>
        /// Gets user all emotional states
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<EmotionalState> GetEmotionalStates(int userId)
        {
            List<EmotionalState> emotionalStates = _emotionalStatesRepository.GetEmotionalStates(userId);
            return emotionalStates;
        }
    }
}