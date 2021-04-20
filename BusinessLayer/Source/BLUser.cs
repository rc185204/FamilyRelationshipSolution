using FRS.Common;
using FRS.DatabaseContext;
using FRS.Models;
using System;
using System.Data.Common;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;

namespace FRS.BusinessLayer
{
    public class BLUser
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="PassWord"></param>
        /// <returns></returns>
        public static User Valid(string UserName, string PassWord)
        {
            User User = null;
            using (FamilyRelationshipContext dbContext = FamilyRelationshipContext.GetFamilyRelationshipContext())
            {
                ///该写法有sql注入风险
                //User = dbContext.Users.Where<User>(user => (user.UserName == UserName && user.Password == PassWord)).First() as User;
                dbContext.Configuration.LazyLoadingEnabled = false;
                string sql = "select * from [User] where UserName=@UserName and Password=@Password";
                var args = new DbParameter[] {
                                    new SqlParameter( "UserName",  UserName), // 写法1
                                    new SqlParameter { ParameterName  = "Password", Value = PassWord},// 写法2
                                };
                User = dbContext.Database.SqlQuery<User>(sql, args).FirstOrDefault();
                if (User != null)
                {
                    User.Role = dbContext.Role.Find(User.RoleId);
                    if (User.FamilyId != null)
                        User.Family = dbContext.Family.Where<Family>(c=> c.FamilyId == User.FamilyId).FirstOrDefault();
                }                
            }
            return User;
        }

        /// <summary>
        /// 判断用户是否关联了家族
        /// </summary>
        /// <param name="Member"></param>
        /// <returns></returns>
        public static bool HaveFamily(User Member)
        {
            bool have = Member.FamilyId != null ? true : false;
            return have;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Member"></param>
        /// <returns></returns>
        public static ErrorCode Add(User Member)
        {
            ErrorCode code = ErrorCode.Unknown_Error;
            using (FamilyRelationshipContext dbContext = FamilyRelationshipContext.GetFamilyRelationshipContext())
            {
                if (dbContext.User.Find(Member.UserName) != null)
                {
                    code = ErrorCode.DataAlreadyExist;
                }
                else
                {
                    dbContext.User.Add(Member);
                    int rows = dbContext.SaveChanges();
                    if (rows <= 0)
                        code = ErrorCode.DataAddError;
                    else
                        code = ErrorCode.Success;
                }
            }
            return code;
        }

    }
}
