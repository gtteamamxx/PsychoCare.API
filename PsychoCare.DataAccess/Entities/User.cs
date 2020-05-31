using PsychoCare.DataAccess.Entities;
using PsychoCare.Logic.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PsychoCare.Logic.DataAccess
{
    public class User
    {
        public User()
        {
            EnvironmentGroups = new HashSet<EnvironmentGroup>();
            EmotionalStates = new HashSet<EmotionalState>();
        }

        [Required]
        [StringLength(maximumLength: 128)]
        public string Email { get; set; }

        public virtual ICollection<EmotionalState> EmotionalStates { get; set; }

        public virtual ICollection<EnvironmentGroup> EnvironmentGroups { get; set; }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength: 64)]
        public string Password { get; set; }

        /// <summary>
        /// Checks if model state is valid
        ///
        /// Specially checks if email is valid or if password is valid SHA256 password
        /// </summary>
        public void Validate()
        {
            if (Email == null || !new EmailAddressAttribute().IsValid(Email) || Email.Length > 128)
            {
                throw new InvalidEmailException();
            }

            // User password should be SHA256, so length should be exactly 64.
            if (Password == null || Password.Trim().Length > 64 || Password.Trim().Length < 64)
            {
                throw new InvalidPasswordException();
            }
        }
    }
}