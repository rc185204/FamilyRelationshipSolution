using FRS.DatabaseContext;
using FRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FRS.Common;

namespace FRS.BusinessLayer
{
    public class BLFamilyMember
    {
        /// <summary>
        /// 获取单一家庭成员
        /// </summary>
        /// <param name="FamilyMemberId"></param>
        /// <returns></returns>
        public static FamilyMember Get(int FamilyMemberId)
        {
            FamilyMember PersonInfo = null;
            using (FamilyRelationshipContext dbContext = new FamilyRelationshipContext())
            {
                PersonInfo = dbContext.FamilyMember.Where<FamilyMember>(c => c.FamilyMemberId == FamilyMemberId).FirstOrDefault();
                if (PersonInfo != null)
                {
                    PersonInfo.CertificateType = dbContext.CertificateType.Where<CertificateType>(c => c.CertificateTypeId == PersonInfo.CertificateTypeId).FirstOrDefault();
                    PersonInfo.Family = dbContext.Family.Where<Family>(c => c.FamilyId == PersonInfo.FamilyId).FirstOrDefault();
                    PersonInfo.Father = dbContext.FamilyMember.Where<FamilyMember>(c => c.FamilyMemberId == PersonInfo.Father_FamilyMemberId).FirstOrDefault();
                    PersonInfo.Mother = dbContext.FamilyMember.Where<FamilyMember>(c => c.FamilyMemberId == PersonInfo.Mother_FamilyMemberId).FirstOrDefault();
                    PersonInfo.Spouse = dbContext.FamilyMember.Where<FamilyMember>(c => c.FamilyMemberId == PersonInfo.Spouse_FamilyMemberId).FirstOrDefault();
                }
            }            
            return PersonInfo;
        }

        /// <summary>
        /// 根据家族编号，获取所有成员
        /// </summary>
        /// <param name="FamilyId"></param>
        /// <returns></returns>
        public static List<FamilyMember> GetMembers(int FamilyId)
        {
            List<FamilyMember> list = null;
            using (FamilyRelationshipContext dbContext = new FamilyRelationshipContext())
            {
                list = dbContext.FamilyMember.Where<FamilyMember>(c => c.FamilyId == FamilyId).ToList();
                if (list != null && list.Count > 0)
                {
                    foreach (FamilyMember e in list)
                    {
                        e.CertificateType = dbContext.CertificateType.Where<CertificateType>(c => c.CertificateTypeId == e.CertificateTypeId).FirstOrDefault();
                        e.Family = dbContext.Family.Where<Family>(c => c.FamilyId == e.FamilyId).FirstOrDefault();
                        e.Father = dbContext.FamilyMember.Where<FamilyMember>(c => c.FamilyMemberId == e.Father_FamilyMemberId).FirstOrDefault();
                        e.Mother = dbContext.FamilyMember.Where<FamilyMember>(c => c.FamilyMemberId == e.Mother_FamilyMemberId).FirstOrDefault();
                        e.Spouse = dbContext.FamilyMember.Where<FamilyMember>(c => c.FamilyMemberId == e.Spouse_FamilyMemberId).FirstOrDefault();
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Member"></param>
        /// <returns></returns>
        public static ErrorCode Add(FamilyMember Member)
        {
            ErrorCode code = ErrorCode.Unknown_Error;
            using (FamilyRelationshipContext dbContext = new FamilyRelationshipContext())
            {
                if (dbContext.FamilyMember.Find(Member.CertificateNumber, Member.CertificateTypeId) != null)
                {
                    code = ErrorCode.DataAlreadyExist;
                }
                else
                {
                    dbContext.FamilyMember.Add(Member);
                    int rows = dbContext.SaveChanges();
                    if (rows <= 0)
                        code = ErrorCode.DataAddError;
                    else
                        code = ErrorCode.Success;
                }
            }
            return code;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Member"></param>
        /// <returns></returns>
        public static int Remove(FamilyMember Member)
        {
            int rows = 0;
            using (FamilyRelationshipContext dbContext = new FamilyRelationshipContext())
            {
                FamilyMember rem = dbContext.FamilyMember.Where<FamilyMember>(c => c.FamilyId == Member.FamilyId).FirstOrDefault();
                dbContext.FamilyMember.Remove(rem);
                rows = dbContext.SaveChanges();
            }
            return rows;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Member"></param>
        /// <returns></returns>
        public static int Update(FamilyMember Member)
        {
            int rows = 0;
            using (FamilyRelationshipContext dbContext = new FamilyRelationshipContext())
            {
                FamilyMember up = dbContext.FamilyMember.Where<FamilyMember>(c => c.FamilyId == Member.FamilyId).FirstOrDefault();
                up.GenerationIndex = Member.GenerationIndex;
                up.Address = Member.Address;
                up.Age = Member.Age;
                up.Birthday = Member.Birthday;
                up.CertificateNumber = Member.CertificateNumber;
                up.CertificateTypeId = Member.CertificateTypeId;
                up.Country = Member.Country;
                up.Deathday = Member.Deathday;
                up.Description = Member.Description;
                up.Education = Member.Education;
                up.LastName = Member.LastName;
                up.Father_FamilyMemberId = Member.Father_FamilyMemberId;
                up.Father = Member.Father;
                up.File = Member.File;
                up.GivenName = Member.GivenName;
                up.Hometown = Member.Hometown;
                up.Job = Member.Job;
                up.MiddleName = Member.MiddleName;
                up.Mother_FamilyMemberId = Member.Mother_FamilyMemberId;
                up.Mother = Member.Mother;
                up.Nationality = Member.Nationality;
                up.OtherName1 = Member.OtherName1;
                up.OtherName2 = Member.OtherName2;
                up.PhotoImage = Member.PhotoImage;
                up.Ranking = Member.Ranking;
                up.CertificateType = Member.CertificateType;
                up.Sex = Member.Sex;
                up.Spouse_FamilyMemberId = Member.Spouse_FamilyMemberId;
                up.Spouse = Member.Spouse;
                up.FamilyId = Member.FamilyId;
                up.Family = Member.Family;
                rows = dbContext.SaveChanges();
            }
            return rows;
        }
    }
}
