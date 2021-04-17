using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRS.Models
{
    [Table("Role", Schema = "FamilyRelationship")]
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

        [NotMapped]//不映射到数据库中
        public int? Age { get; set; }

        public int RoleRefId { get; set; }

        [ForeignKey("RoleRefId")]//外键 [ForeignKey(导航属性名)] 依赖实体中在导航属性上指定属性名
        public Role Role { get; set; }
    }
}
