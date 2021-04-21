using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FRS.Models
{
    /// <summary>
    /// Faily member class, 
    /// this class is a member in only one family, 
    /// </summary>
    [Table("FamilyMember")]
    public class FamilyMember
    {
        public FamilyMember() { }

        /// <summary>
        /// 
        /// </summary>
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
        /// Family info
        /// </summary>
        [ForeignKey("FamilyId")]
        public Family Family { get; set; }

        /// <summary>
        /// member's generation of a family in the history
        /// exp:  jack's generationIndex is 20, his son generationIndex is 21
        /// maybe, in same culture, mother have no generationIndex in this family.
        /// </summary>
        public int? GenerationIndex { get; set; }


        /// <summary>
        /// the Certificate type
        /// </summary>
        public int CertificateTypeId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ForeignKey("CertificateTypeId")]//外键 [ForeignKey(导航属性名)] 依赖实体中在导航属性上指定属性名
        public CertificateType CertificateType { get; set; }

        /// <summary>
        /// number of certificate
        /// </summary>
        [MaxLength(100)]
        public string CertificateNumber { get; set; }

        /// <summary>
        /// first name 
        /// </summary>
        [MaxLength(100)]
        public string GivenName { get; set; }

        /// <summary>
        /// last name 
        /// </summary>
        [MaxLength(100)]
        public string LastName  { get; set; }

        /// <summary>
        /// middle name
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
        /// sex
        /// </summary>
        public SexClass Sex { get; set; }

        /// <summary>
        /// exp: han,  Russian, Korean
        /// </summary>
        [MaxLength(100)]
        public string Nationality { get; set; }

        /// <summary>
        /// exp: SUA , china, 
        /// </summary>
        [MaxLength(100)]
        public string Country { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MaxLength(200)]
        public string Hometown { get; set; }

        /// <summary>
        /// mailing address
        /// </summary>
        [MaxLength(200)]
        public string Address { get; set; }

        /// <summary>
        /// some member or ancestor could't known the day
        /// </summary>
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? Deathday { get; set; }

        /// <summary>
        /// if the member alive?
        /// </summary>        
        public bool Alive { get; set; } = true;

        /// <summary>
        /// the index of your brothers or sisters
        /// </summary>
        public int? Ranking{ get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MaxLength(100)]
        public string Education { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MaxLength(100)]
        public string Job { get; set; }


        /// <summary>
        /// Introduction this member's life
        /// </summary>
        [MaxLength(5000)]
        public string Description { get; set; }

        /// <summary>
        /// ico or head image of this member
        /// </summary>
        public byte[] PhotoImage { get; set; }

        /// <summary>
        /// files of this member
        /// it can be a package to save in database
        /// </summary>
        public byte[] File { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? Spouse_FamilyMemberId { get; set; }

        /// <summary>
        /// Spouse
        /// </summary>
        [ForeignKey("Spouse_FamilyMemberId")]
        public FamilyMember Spouse { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? Father_FamilyMemberId { get; set; }

        /// <summary>
        /// Father
        /// </summary>
        [ForeignKey("Father_FamilyMemberId")]
        public FamilyMember Father { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? Mother_FamilyMemberId { get; set; }

        /// <summary>
        /// Mother
        /// </summary>
        [ForeignKey("Mother_FamilyMemberId")]
        public FamilyMember Mother { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Timestamp]
        public byte[] RowVersion { get; set; }

        private int? age;

        /// <summary>
        /// Age, not save to database, it changed year by year if alive.
        /// </summary>
        [NotMapped]
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
