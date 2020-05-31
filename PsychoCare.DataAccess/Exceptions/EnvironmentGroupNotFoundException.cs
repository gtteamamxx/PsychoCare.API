using PsychoCare.Common.Errors;
using PsychoCare.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PsychoCare.DataAccess.Exceptions
{
    /// <summary>
    /// Exception throwed when environemnt group is not exist
    /// </summary>
    public class EnvironmentGroupNotFoundException : MessageException
    {
        public EnvironmentGroupNotFoundException() : base(Errors.EnvironmentGroupNotFound)
        {
        }
    }
}