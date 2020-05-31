using PsychoCare.Common.Errors;
using PsychoCare.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PsychoCare.DataAccess.Exceptions
{
    /// <summary>
    /// Exception throwed when environment group arleady exists
    /// </summary>
    public class EnvironmentGroupAlreadyExistsException : MessageException
    {
        public EnvironmentGroupAlreadyExistsException() : base(Errors.EnvironmentGroupAlreadyExists)
        {
        }
    }
}