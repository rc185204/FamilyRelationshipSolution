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
    /// role
    /// </summary>
    [Table("Role"/*, Schema = "FamilyRelationship"*/)]//FamilyRelationship.Role.
    public class Role
    {

        public Role() { }

        /// <summary>
        /// 
        /// </summary>
        [Key]
        [Required]// must not null
        public int RoleId { get; set; }

        /// <summary>
        /// RoleName
        /// exp: System admin; Family admin; Family member
        /// </summary>
        [Required]
        [MinLength(4)]
        [MaxLength(20)]
        [ConcurrencyCheck]
        public string RoleName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// update sql , include where RowVersion=0xXXXXX
        /// database timestamp type，
        /// note：one class only have a timestamp, and it must typeof byte[]
        /// </summary>
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
