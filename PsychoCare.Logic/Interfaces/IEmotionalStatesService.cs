using PsychoCare.DataAccess.Entities;
using System.Collections.Generic;

namespace PsychoCare.Logic.Interfaces
{
    public interface IEmotionalStatesService
    {
        void AddEmotionalState(EmotionalState emotionalState);

        List<EmotionalState> GetEmotionalStates(int userId);
    }
}