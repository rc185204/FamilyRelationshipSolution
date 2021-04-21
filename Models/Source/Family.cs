using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRS.Models
{
    /// <summary>
    /// 
    /// </summary>
    [Table("Family")]
    public class Family
    {
        [Key]
        [Required]
        [MaxLength(50)]
        public string FamilyId { get; set; }

        /// <summary>
        /// family name
        /// exp: jack's family
        /// </summary>
        [Required]
        [MaxLength(100)]
        [ConcurrencyCheck]
        public string FamilyName { get; set; }

        /// <summary>
        /// Origin
        /// exp: Birthplace of George I
        /// </summary>
        [MaxLength(100)]
        public string FamilyOrigin { get; set; }

        /// <summary>
        /// the history of this family
        /// famous story, mission or important historical event of this family
        /// </summary>
        [MaxLength(1000)]
        public string FamilyHistory { get; set; }

        /// <summary>
        /// A word or famous quote from the family
        /// </summary>
        [MaxLength(500)]
        public string GenerationInfo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MaxLength(100)]
        public string Other1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MaxLength(100)]
        public string Other2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Timestamp]
        public byte[] RowVersion { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        //[NotMapped]
        //public List<FamilyMember> FamilyMembers { get; set; }
    }
}
