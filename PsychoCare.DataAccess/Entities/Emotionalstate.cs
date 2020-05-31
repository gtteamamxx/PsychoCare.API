using System;
using PsychoCare.Logic.DataAccess;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PsychoCare.Common.Enums;

namespace PsychoCare.DataAccess.Entities
{
    /// <summary>
    /// Emotional state DB Model
    /// </summary>
    public class EmotionalState
    {
        public string Comment { get; set; }

        /// <summary>
        /// Date of created emtional state
        /// </summary>
        [Required]
        public DateTime CreationDate { get; set; }

        [ForeignKey(nameof(EnvironmentGroupId))]
        public virtual EnvironmentGroup EnvironmentGroup { get; set; }

        /// <summary>
        /// Relation to environment group
        /// </summary>
        [Required]
        public int EnvironmentGroupId { get; set; }

        [Key]
        public int Id { get; set; }

        /// <summary>
        /// State ID. See <see cref="EmotionalStatesEnum"/>
        /// </summary>
        [Required]
        public int State { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        /// <summary>
        /// Relation to owner's user
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// Check's if model is valid
        ///
        /// Specially if some of it's relations are invalid
        /// </summary>
        public void Validate()
        {
            if (UserId == 0) throw new ArgumentException(nameof(UserId));
            if (EnvironmentGroupId == 0) throw new ArgumentException(nameof(EnvironmentGroupId));
        }
    }
}