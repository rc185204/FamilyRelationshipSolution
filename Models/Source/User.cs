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

        [Key]
        [Required]
        public int UserId { get; set; }

        [Required]
        [MinLength(4)]
        [MaxLength(20)]
        [ConcurrencyCheck]
        public string UserName { get; set; }

        [Required]
        [MinLength(6)]
        [MaxLength(10)]
        public string Password { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(20)]
        [ConcurrencyCheck]
        public string UserTrueName { get; set; }

        public byte[] HeaderImage { get; set; }


        public int RoleId { get; set; }


        [ForeignKey("RoleId")]//外键 [ForeignKey(导航属性名)] 依赖实体中在导航属性上指定属性名
        public Role Role { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? FamilyId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ForeignKey("FamilyId")]
        public Family? Family { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
