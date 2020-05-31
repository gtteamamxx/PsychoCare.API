using PsychoCare.Common.Errors;
using PsychoCare.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PsychoCare.DataAccess.Exceptions
{
    /// <summary>
    /// Exception throwed when user already exist
    /// </summary>
    public class UserAlreadyExistsException : MessageException
    {
        public UserAlreadyExistsException() : base(Errors.UserAlreadyExists)
        {
        }
    }
}