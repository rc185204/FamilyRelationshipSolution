using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRS.Models
{
    [Table("Role", Schema = "FamilyRelationship")]//数据库名为FamilyRelationship.Role.用于实体，配置实体对应的数据库表名和表结构
    public class Role
    {

        public Role() { }

        [Key]//数据库中对应列为主键
        [Required]//属性不为空，数据中对应列
        public int RoleId { get; set; }

        [Required]
        [MinLength(4)]//属性和数据库中的最小的string长度
        [MaxLength(20)]//属性和数据库中的最大的string长度
        [ConcurrencyCheck]//数据库中对应列进行乐观并发检测，主要用于解决高并发问题
        public string RoleName { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// 在update操作时，包含在where子句中
        /// 数据库中对应列为timestamp类型，主要用于解决高并发问题.
        /// 注：一个类只能用一次，且修饰的属性必须为byte[] 类型
        /// </summary>
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
