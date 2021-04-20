using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FRS.Models
{
    [Table("FamilyMember")]
    public class FamilyMember
    {
        public FamilyMember() { }

        [Key]
        [Required]
        [Column(Order = 1)]
        public int FamilyMemberId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string FamilyId { get; set; }

        /// <summary>
        /// 家族信息
        /// </summary>
        [ForeignKey("FamilyId")]
        public Family Family { get; set; }

        /// <summary>
        /// 在本家族中世代号（eg:二十一世）
        /// 外戚是没有世号的，所以可以为空
        /// </summary>
        public int? GenerationIndex { get; set; }

        ///// <summary>
        ///// 因为同一个人有可能会出现在不同的族谱中，所以对同一个自然人的信息需求不一定相同，为避免不同家族对同一个人的定位不同需求
        ///// 这里不适用唯一自然人存在，而是不同的family根据自己的信息，填写或者调整自然新人信息，所以通过PersonInfoId做主键
        ///// </summary>
        //[Key]
        //[Required]
        //public int PersonInfoId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int CertificateTypeId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ForeignKey("CertificateTypeId")]//外键 [ForeignKey(导航属性名)] 依赖实体中在导航属性上指定属性名
        public CertificateType CertificateType { get; set; }

        /// <summary>
        /// 证件编号
        /// </summary>
        [MaxLength(100)]
        public string CertificateNumber { get; set; }

        /// <summary>
        /// first name 名
        /// </summary>
        [MaxLength(100)]
        public string GivenName { get; set; }

        /// <summary>
        /// last name 姓
        /// </summary>
        [MaxLength(100)]
        public string LastName  { get; set; }

        /// <summary>
        /// 中间名
        /// </summary>
        [MaxLength(100)]
        public string MiddleName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MaxLength(100)]
        public string OtherName1 { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [MaxLength(100)]
        public string OtherName2 { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public SexClass Sex { get; set; }

        /// <summary>
        /// 民族
        /// </summary>
        [MaxLength(100)]
        public string Nationality { get; set; }

        /// <summary>
        /// 国籍
        /// </summary>
        [MaxLength(100)]
        public string Country { get; set; }

        /// <summary>
        /// 籍贯，出生地
        /// </summary>
        [MaxLength(200)]
        public string Hometown { get; set; }

        /// <summary>
        /// 居住地
        /// </summary>
        [MaxLength(200)]
        public string Address { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// 死亡日
        /// </summary>
        public DateTime? Deathday { get; set; }

        /// <summary>
        /// 活着
        /// </summary>        
        public bool Alive { get; set; } = true;

        /// <summary>
        /// 家中排行
        /// </summary>
        public int? Ranking{ get; set; }

        /// <summary>
        /// 教育经历
        /// </summary>
        [MaxLength(100)]
        public string Education { get; set; }

        /// <summary>
        /// 职业
        /// </summary>
        [MaxLength(100)]
        public string Job { get; set; }


        /// <summary>
        /// 生平描述
        /// </summary>
        [MaxLength(5000)]
        public string Description { get; set; }

        /// <summary>
        /// 人脸或简照
        /// </summary>
        public byte[] PhotoImage { get; set; }

        /// <summary>
        /// 生平文件包
        /// </summary>
        public byte[] File { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? Spouse_FamilyMemberId { get; set; }

        /// <summary>
        /// 配偶
        /// </summary>
        [ForeignKey("Spouse_FamilyMemberId")]
        public FamilyMember Spouse { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? Father_FamilyMemberId { get; set; }

        /// <summary>
        /// 父亲
        /// </summary>
        [ForeignKey("Father_FamilyMemberId")]
        public FamilyMember Father { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? Mother_FamilyMemberId { get; set; }

        /// <summary>
        /// 母亲
        /// </summary>
        [ForeignKey("Mother_FamilyMemberId")]
        public FamilyMember Mother { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Timestamp]
        public byte[] RowVersion { get; set; }

        private int? age;

        [NotMapped]//不映射到数据库中
        public int? Age 
        {
            get 
            {
                if (!Alive)
                {
                    if (Birthday != null && Deathday != null)
                    {
                        int age = Deathday.Value.Year - Birthday.Value.Year;
                        if (Deathday.Value.Month < Birthday.Value.Month || (Deathday.Value.Month == Birthday.Value.Month && Deathday.Value.Day <= Birthday.Value.Day))
                            age--;
                        return age;
                    }
                }
                else
                {
                    if (Birthday != null)
                    {
                        DateTime nowDateTime = DateTime.Now;
                        int age = nowDateTime.Year - Birthday.Value.Year;
                        if (nowDateTime.Month < Birthday.Value.Month || (nowDateTime.Month == Birthday.Value.Month && nowDateTime.Day <= Birthday.Value.Day))
                            age--;
                        return age;
                    }                    
                }                
                return null;

            }
            set { age = value; }
        }
    }

    public enum SexClass
    {
        Man,
        Woman,
    }
}
