using System;
using System.Collections.Generic;
using System.Text;

namespace PsychoCare.DataAccess.Interfaces
{
    public interface IDbContextBuilder
    {
        PsychoCareContext GetContext();
    }
}