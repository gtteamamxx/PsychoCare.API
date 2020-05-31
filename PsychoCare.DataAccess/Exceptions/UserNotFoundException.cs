using PsychoCare.Common.Errors;
using PsychoCare.Common.Exceptions;

namespace PsychoCare.Logic.Exceptions
{
    /// <summary>
    /// Exception throwed when user was not found
    /// </summary>
    public class UserNotFoundException : MessageException
    {
        public UserNotFoundException() : base(Errors.UserNotFound)
        {
        }
    }
}