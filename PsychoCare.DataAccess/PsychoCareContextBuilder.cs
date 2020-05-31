using PsychoCare.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PsychoCare.DataAccess
{
    /// <summary>
    /// This class provides access to production db
    /// </summary>
    public class PsychoCareContextBuilder : IDbContextBuilder
    {
        public PsychoCareContext GetContext() => new PsychoCareContext();
    }
}