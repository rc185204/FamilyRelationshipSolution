using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRS.Models
{
    [Table("Family")]
    public class Family
    {
        [Key]
        [Required]
        [MaxLength(50)]
        public string FamilyId { get; set; }

        /// <summary>
        /// 证件类型名称
        /// </summary>
        [Required]
        [MaxLength(100)]
        [ConcurrencyCheck]
        public string FamilyName { get; set; }

        /// <summary>
        /// 家族发源地
        /// </summary>
        [MaxLength(100)]
        public string FamilyOrigin { get; set; }

        /// <summary>
        /// 家族历史
        /// </summary>
        [MaxLength(1000)]
        public string FamilyHistory { get; set; }

        /// <summary>
        /// 家族传承的辈份依据，
        /// </summary>
        [MaxLength(500)]
        public string GenerationInfo { get; set; }

        /// <summary>
        /// 预留1
        /// </summary>
        [MaxLength(100)]
        public string Other1 { get; set; }

        /// <summary>
        /// 预留2
        /// </summary>
        [MaxLength(100)]
        public string Other2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Timestamp]
        public byte[] RowVersion { get; set; }

        ///// <summary>
        ///// 家族成员集合
        ///// </summary>
        //[NotMapped]//不映射到数据库中
        //public List<FamilyMember> FamilyMembers { get; set; }
    }
}
