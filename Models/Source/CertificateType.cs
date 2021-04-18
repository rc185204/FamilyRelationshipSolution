using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRS.Models
{
    [Table("CertificateType")]
    public class CertificateType
    {

        /// <summary>
        /// 当key在数据库中是自增模式，即使外部对象给CertificateTypeId赋值，也不会生效
        /// 由于在EF操作的时候，需要使用set对象，因此这里不能省却 set 处理
        /// </summary>
        [Key]
        [Required]
        public int CertificateTypeId { get; set; }

        /// <summary>
        /// 证件类型名称
        /// </summary>
        [Required]
        [MaxLength(50)]
        [ConcurrencyCheck]
        public string CertificateTypeName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
