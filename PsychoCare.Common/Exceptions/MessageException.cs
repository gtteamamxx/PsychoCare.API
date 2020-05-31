using System;
using System.Collections.Generic;
using System.Text;

namespace PsychoCare.Common.Exceptions
{
    /// <summary>
    /// Exception throwed by all services
    ///
    /// When throwed UI clients should show message and treat it like intercepted error
    /// </summary>
    public class MessageException : Exception
    {
        public MessageException(string message) : base(message)
        {
        }
    }
}