using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PsychoCare.API.Models
{
    /// <summary>
    /// Error response model returned by ErrorFilter
    /// </summary>
    public class ErrorFilterResponse
    {
        public string Error { get; set; }

        public bool IsError { get; set; }

        public string Message { get; set; }
    }
}