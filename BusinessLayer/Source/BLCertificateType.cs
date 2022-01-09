using FRS.Common;
using FRS.DatabaseContext;
using FRS.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
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
            using (FamilyRelationshipContext dbContext = FamilyRelationshipContext.GetFamilyRelationshipContext())
            {
                list = dbContext.CertificateType.ToList();               
            }
            return list;
        }

        public static CertificateType Get(int CertificateTypeId)
        {
            CertificateType CertificateType = null;
            using (FamilyRelationshipContext dbContext = FamilyRelationshipContext.GetFamilyRelationshipContext())
            {
                CertificateType = dbContext.CertificateType.Find(CertificateTypeId);
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
            using (FamilyRelationshipContext dbContext = FamilyRelationshipContext.GetFamilyRelationshipContext())
            {
                int count = dbContext.CertificateType.Where(c=>c.CertificateTypeName == Member.CertificateTypeName.Trim()).Count();
                if (count > 0)
                //if (dbContext.CertificateType.Find(Member.CertificateTypeName) != null)
                {
                    code = ErrorCode.DataAlreadyExist;
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
        public static ErrorCode Remove(CertificateType Member)
        {
            ErrorCode code = ErrorCode.Unknown_Error;
            using (FamilyRelationshipContext dbContext = FamilyRelationshipContext.GetFamilyRelationshipContext())
            {
                CertificateType rem = dbContext.CertificateType.Find(Member.CertificateTypeId);
                if (rem != null)
                {
                    dbContext.CertificateType.Remove(rem);
                    int rows = dbContext.SaveChanges();
                    if (rows > 0)
                        code = ErrorCode.Success;
                    else
                        code = ErrorCode.DataDetectError;
                }
                else
                    code = ErrorCode.Success;                
            }
            return code;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Member"></param>
        /// <returns></returns>
        public static ErrorCode Modify(CertificateType Member)
        {
            ErrorCode code = ErrorCode.Unknown_Error;
            using (FamilyRelationshipContext dbContext = FamilyRelationshipContext.GetFamilyRelationshipContext())
            {
                CertificateType up = dbContext.CertificateType.Find(Member.CertificateTypeId);
                if (up != null)
                {
                    up.CertificateTypeName = Member.CertificateTypeName;
                    up.Description = Member.Description;
                    int rows = dbContext.SaveChanges();

                    if (rows > 0)
                        code = ErrorCode.Success;
                    else
                        code = ErrorCode.DataModifyError;
                }
                else
                    code = ErrorCode.DataNotExist;
            }
            return code;
        }
    }
}
