using System;
using PsychoCare.DataAccess.Entities;
using System.Collections.Generic;
using System.Text;

namespace PsychoCare.DataAccess.Interfaces
{
    public interface IEmotionalStatesRepository
    {
        void AddEmotionalState(EmotionalState emotionalState);

        List<EmotionalState> GetEmotionalStates(int userId);
    }
}