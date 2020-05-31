using PsychoCare.Common.Errors;
using PsychoCare.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PsychoCare.DataAccess.Exceptions
{
    /// <summary>
    /// Exception throwed when environment group has invalid name
    /// </summary>
    public class EnvironmentGroupInvalidNameException : MessageException
    {
        public EnvironmentGroupInvalidNameException() : base(Errors.EnvironmentGroupInvalidName)
        {
        }
    }
}