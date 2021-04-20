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
    public class BLFamily
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Member"></param>
        /// <returns></returns>
        public static Family Get(Family Member)
        {            
            return Get(Member.FamilyId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FamilyId"></param>
        /// <returns></returns>
        public static Family Get(int FamilyId)
        {
            Family Family = null;
            using (FamilyRelationshipContext dbContext = FamilyRelationshipContext.GetFamilyRelationshipContext())
            {
                Family = dbContext.Family.Find(FamilyId);
                //if (Family != null)
                //{
                //    Family.FamilyMembers = dbContext.FamilyMember.Where<FamilyMember>(c => c.FamilyId == Family.FamilyId).ToList();
                //    foreach (FamilyMember e in Family.FamilyMembers)
                //    {
                //        e.CertificateType = dbContext.CertificateType.Where<CertificateType>(c => c.CertificateTypeId == e.CertificateTypeId).FirstOrDefault();
                //        e.Family = dbContext.Family.Where<Family>(c => c.FamilyId == e.FamilyId).FirstOrDefault();
                //        e.Father = dbContext.FamilyMember.Where<FamilyMember>(c => c.FamilyMemberId == e.Father_FamilyMemberId).FirstOrDefault();
                //        e.Mother = dbContext.FamilyMember.Where<FamilyMember>(c => c.FamilyMemberId == e.Mother_FamilyMemberId).FirstOrDefault();
                //        e.Spouse = dbContext.FamilyMember.Where<FamilyMember>(c => c.FamilyMemberId == e.Spouse_FamilyMemberId).FirstOrDefault();
                //    }
                //}
            }
            return Family;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Member"></param>
        /// <returns></returns>
        public static ErrorCode Add(Family Member)
        {
            ErrorCode code = ErrorCode.Unknown_Error;
            using (FamilyRelationshipContext dbContext = FamilyRelationshipContext.GetFamilyRelationshipContext())
            {
                if (dbContext.Family.Find(Member.FamilyName) != null)
                {
                    code = ErrorCode.DataAlreadyExist;
                }
                else
                {
                    dbContext.Family.Add(Member);
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
        public static int Remove(Family Member)
        {
            int rows = 0;
            using (FamilyRelationshipContext dbContext = FamilyRelationshipContext.GetFamilyRelationshipContext())
            {
                Family rem = dbContext.Family.Find(Member.FamilyId);
                dbContext.Family.Remove(rem);
                rows = dbContext.SaveChanges();
            }
            return rows;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FamilyId"></param>
        /// <returns></returns>
        public static int Remove(int FamilyId)
        {
            int rows = 0;
            using (FamilyRelationshipContext dbContext = FamilyRelationshipContext.GetFamilyRelationshipContext())
            {
                Family rem = dbContext.Family.Find(FamilyId);
                dbContext.Family.Remove(rem);
                rows = dbContext.SaveChanges();
            }
            return rows;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Member"></param>
        /// <returns></returns>
        public static int Update(Family Member)
        {
            int rows = 0;
            using (FamilyRelationshipContext dbContext = FamilyRelationshipContext.GetFamilyRelationshipContext())
            {
                Family up = dbContext.Family.Find(Member.FamilyId);// Find 它可以帮助我们通过主键来查找对应的实体。并且如果相应的实体已经被DbContext缓存，EF会在缓存中直接返回对应的实体，而不会执行数据库访问。
                up.FamilyName = Member.FamilyName;
                up.FamilyOrigin = Member.FamilyOrigin; 
                up.FamilyHistory = Member.FamilyHistory;
                up.GenerationInfo = Member.GenerationInfo;
                up.Other1 = Member.Other1;
                up.Other2 = Member.Other2;
                rows = dbContext.SaveChanges();
            }
            return rows;
        }
    }
}
