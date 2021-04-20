using FRS.Common;
using FRS.DatabaseContext;
using FRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRS.BusinessLayer
{
    public class BLCertificateType
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<CertificateType> GetAll()
        {
            List<CertificateType> list = null;
            using (FamilyRelationshipContext dbContext = new FamilyRelationshipContext())
            {
                list = dbContext.CertificateType.ToList();               
            }
            return list;
        }

        public static CertificateType Get(int CertificateTypeId)
        {
            CertificateType CertificateType = null;
            using (FamilyRelationshipContext dbContext = new FamilyRelationshipContext())
            {
                CertificateType = dbContext.CertificateType.Where<CertificateType>(c=>c.CertificateTypeId == CertificateTypeId).FirstOrDefault();
            }
            return CertificateType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Member"></param>
        /// <returns></returns>
        public static ErrorCode Add(CertificateType Member)
        {
            ErrorCode code = ErrorCode.Unknown_Error;
            using (FamilyRelationshipContext dbContext = new FamilyRelationshipContext())
            {
                if (dbContext.CertificateType.Find(Member.CertificateTypeName) != null)
                {
                    code = ErrorCode.DataAlreadyExist;
                }
                else
                {
                    dbContext.CertificateType.Add(Member);
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
        public static int Remove(CertificateType Member)
        {
            int rows = 0;
            using (FamilyRelationshipContext dbContext = new FamilyRelationshipContext())
            {
                CertificateType rem = dbContext.CertificateType.Where<CertificateType>(c => c.CertificateTypeId == Member.CertificateTypeId).FirstOrDefault();
                dbContext.CertificateType.Remove(rem);
                rows = dbContext.SaveChanges();
            }
            return rows;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CertificateTypeId"></param>
        /// <returns></returns>
        public static int Remove(int CertificateTypeId)
        {
            int rows = 0;
            using (FamilyRelationshipContext dbContext = new FamilyRelationshipContext())
            {
                CertificateType rem = dbContext.CertificateType.Where<CertificateType>(c => c.CertificateTypeId == CertificateTypeId).FirstOrDefault();
                dbContext.CertificateType.Remove(rem);
                rows = dbContext.SaveChanges();
            }
            return rows;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Member"></param>
        /// <returns></returns>
        public static int Update(CertificateType Member)
        {
            int rows = 0;
            using (FamilyRelationshipContext dbContext = new FamilyRelationshipContext())
            {
                CertificateType up = dbContext.CertificateType.Where<CertificateType>(c => c.CertificateTypeId == Member.CertificateTypeId).FirstOrDefault();
                up.CertificateTypeName = Member.CertificateTypeName;
                up.Description = Member.Description;
                rows = dbContext.SaveChanges();
            }
            return rows;
        }
    }
}
