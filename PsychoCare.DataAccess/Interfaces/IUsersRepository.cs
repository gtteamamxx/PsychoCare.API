using PsychoCare.Logic.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace PsychoCare.DataAccess.Interfaces
{
    public interface IUsersRepository
    {
        User FindUserByEmail(string email);

        void Register(User user);
    }
}