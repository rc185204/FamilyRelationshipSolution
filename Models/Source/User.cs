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
    /// user info
    /// </summary>
    [Table("User")]
    public class User
    {
        public User() { }

        /// <summary>
        /// 
        /// </summary>
        [Key]
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// user login name
        /// </summary>
        [Required]
        [MinLength(4)]
        [MaxLength(20)]
        [ConcurrencyCheck]
        public string UserName { get; set; }

        /// <summary>
        /// password
        /// </summary>
        [Required]
        [MinLength(6)]
        [MaxLength(20)]
        public string Password { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [MinLength(2)]
        [MaxLength(20)]
        [ConcurrencyCheck]
        public string UserTrueName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public byte[] HeaderImage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ForeignKey("RoleId")]
        public Role Role { get; set; }

        /// <summary>
        /// a user can link a faily
        /// </summary>
        [MaxLength(50)]
        public string FamilyId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ForeignKey("FamilyId")]
        public Family Family { get; set; }

        ///// <summary>
        ///// ，
        ///// </summary>
        //public int? FamilyMemberId { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        //[ForeignKey("FamilyMemberId")]
        //public FamilyMember FamilyMember { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
