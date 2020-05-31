using PsychoCare.Logic.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace PsychoCare.Logic.Interfaces
{
    public interface IUsersService
    {
        void Register(User user);
        User GetUser(string email, string password);
    }
}