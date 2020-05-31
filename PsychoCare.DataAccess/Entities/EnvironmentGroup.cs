using Newtonsoft.Json;
using PsychoCare.DataAccess.Exceptions;
using PsychoCare.Logic.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PsychoCare.DataAccess.Entities
{
    /// <summary>
    /// Environemnt group model
    /// </summary>
    public class EnvironmentGroup
    {
        public EnvironmentGroup()
        {
            EmotionalStates = new HashSet<EmotionalState>();
        }

        [JsonIgnore]
        public virtual ICollection<EmotionalState> EmotionalStates { get; set; }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength: 64)]
        public string Name { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// Checks if model is valid
        ///
        /// Speccialy checks if some of it's relations are invalid
        /// or provided name is not provied or it's length is less than 3 or longest than 64 chars
        /// </summary>
        public void Validate()
        {
            if (Name == null || Name.Length <= 3 || Name.Length > 64) throw new EnvironmentGroupInvalidNameException();
            if (UserId == 0) throw new ArgumentException(nameof(UserId));
        }
    }
}