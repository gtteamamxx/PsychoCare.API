using Microsoft.EntityFrameworkCore;
using PsychoCare.DataAccess.Interfaces;
using PsychoCare.Logic.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PsychoCare.DataAccess.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly IDbContextBuilder _contextBuilder;

        public UsersRepository(IDbContextBuilder contextBuilder)
        {
            _contextBuilder = contextBuilder;
        }

        /// <summary>
        /// Gets user by email adress otherwise null
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public User FindUserByEmail(string email)
        {
            using (PsychoCareContext db = _contextBuilder.GetContext())
            {
                User user = db.Users.AsNoTracking()
                    .FirstOrDefault(x => x.Email == email);
                return user;
            }
        }

        /// <summary>
        /// Adds user to DB (Registering a user)
        /// </summary>
        /// <param name="user"></param>
        public void Register(User user)
        {
            using (PsychoCareContext db = _contextBuilder.GetContext())
            {
                db.Users.Add(user);
                db.SaveChanges();
            }
        }
    }
}