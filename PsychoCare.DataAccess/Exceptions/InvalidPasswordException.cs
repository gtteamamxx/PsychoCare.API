using PsychoCare.Common.Errors;
using PsychoCare.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PsychoCare.Logic.Exceptions
{
    /// <summary>
    /// Exception throwed when password is invalid. (NOT SHA256)
    /// </summary>
    public class InvalidPasswordException : MessageException
    {
        public InvalidPasswordException() : base(Errors.InvalidPassword)
        {
        }
    }
}