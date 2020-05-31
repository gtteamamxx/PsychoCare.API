using PsychoCare.Common.Errors;
using PsychoCare.Common.Exceptions;

namespace PsychoCare.Logic.Exceptions
{
    /// <summary>
    /// Exception throwed when email is invalid
    /// </summary>
    public class InvalidEmailException : MessageException
    {
        public InvalidEmailException() : base(Errors.InvalidEmail)
        {
        }
    }
}