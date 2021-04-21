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
    [Table("CertificateType")]
    public class CertificateType
    {

        /// <summary>
        /// 
        /// </summary>
        [Key]
        [Required]
        public int CertificateTypeId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [MaxLength(50)]
        [ConcurrencyCheck]
        public string CertificateTypeName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
