using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRS.Models
{
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
        /// 用户名
        /// </summary>
        [Required]
        [MinLength(4)]
        [MaxLength(20)]
        [ConcurrencyCheck]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
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
        [ForeignKey("RoleId")]//外键 [ForeignKey(导航属性名)] 依赖实体中在导航属性上指定属性名
        public Role Role { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MaxLength(50)]
        public string FamilyId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ForeignKey("FamilyId")]
        public Family Family { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
