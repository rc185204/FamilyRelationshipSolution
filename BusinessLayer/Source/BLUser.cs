using FRS.Common;
using FRS.DatabaseContext;
using FRS.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace FRS.BusinessLayer
{
    /// <summary>
    /// 
    /// </summary>
    public class BLUser
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        public static User Valid(string userName, string passWord)
        {
            User User = null;
            using (FamilyRelationshipContext dbContext = FamilyRelationshipContext.GetFamilyRelationshipContext())
            {
                ///该写法有sql注入风险
                //User = dbContext.Users.Where<User>(user => (user.UserName == UserName && user.Password == PassWord)).First() as User;
                dbContext.Configuration.LazyLoadingEnabled = false;
                string sql = "select * from [User] where UserName=@UserName and Password=@Password";
                var args = new DbParameter[] {
                                    new SqlParameter( "UserName",  userName), // 写法1
                                    new SqlParameter { ParameterName  = "Password", Value = passWord},// 写法2
                                };
                User = dbContext.Database.SqlQuery<User>(sql, args).FirstOrDefault();
                if (User != null)
                {
                    User.Role = dbContext.Role.Find(User.RoleId);
                    if (User.FamilyId != null)
                        User.Family = dbContext.Family.Find(User.FamilyId);
                }
            }
            return User;
        }

        /// <summary>
        /// 根据用户ID获取用户
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static User Get(string userName)
        {
            User User = null;
            using (FamilyRelationshipContext dbContext = FamilyRelationshipContext.GetFamilyRelationshipContext())
            {
                User = dbContext.User.Where(c => c.UserName == userName).FirstOrDefault();
                if (User != null)
                {
                    User.Role = dbContext.Role.Find(User.RoleId);
                    if (User.FamilyId != null)
                        User.Family = dbContext.Family.Find(User.FamilyId);
                }
            }
            return User;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public static User Get(User member)
        {
            User User = null;
            using (FamilyRelationshipContext dbContext = FamilyRelationshipContext.GetFamilyRelationshipContext())
            {
                User = dbContext.User.Find(member.UserId);
                if (User != null)
                {
                    User.Role = dbContext.Role.Find(User.RoleId);
                    if (User.FamilyId != null)
                        User.Family = dbContext.Family.Find(User.FamilyId);
                }
            }
            return User;
        }

        /// <summary>
        /// 根据角色查对应的用户列表
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public static List<User> GetAll(Role role)
        {
            List<User> List = null;
            using (FamilyRelationshipContext dbContext = FamilyRelationshipContext.GetFamilyRelationshipContext())
            {
                List = dbContext.User.Where<User>(c => c.RoleId == role.RoleId).ToList();
            }
            return List;
        }


        /// <summary>
        /// 模糊查询
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public static List<User> InquiryUsers(User member)
        {
            //string sql = "select * from [User] WHERE 1=1 ";
            //if (Member.UserName != "")
            //    sql = sql + "and UserName LIKE '%" + Member.UserName + "%' ";
            //if (Member.RoleId != 0)
            //    sql = sql + "and  RoleId = " + Member.RoleId + " ";
            //if (Member.FamilyId != null)
            //    sql = sql + "and  FamilyId = " + Member.FamilyId.Value + " ";
            //if (Member.UserTrueName != "")
            //    sql = sql + "and UserTrueName LIKE '%" + Member.UserTrueName + "%'";

            List<User> list = null;
            using (FamilyRelationshipContext dbContext = FamilyRelationshipContext.GetFamilyRelationshipContext())
            {
                //Expression<Func<User, bool>> exprelamada = null;

                ParameterExpression m_Parameter = Expression.Parameter(typeof(User), "x");
                List<Expression> m_lstExpression = new List<Expression>();
                if (!string.IsNullOrEmpty(member.UserName))
                {
                    var propertyExp = Expression.Property(m_Parameter, "UserName"); // 申明属性
                    var containsMethod = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });// 判断条件 %UserTrueName%
                    var constExp = Expression.Constant(member.UserName);// 表达式
                    var ContainsExp = Expression.Call(propertyExp, containsMethod, constExp);
                    m_lstExpression.Add(ContainsExp);
                }

                if (member.RoleId != 0)
                {
                    MemberExpression leftExp = Expression.Property(m_Parameter, "RoleId");
                    var RightExp = Expression.Constant(member.RoleId, typeof(int));
                    var EqualExp = Expression.Equal(leftExp, RightExp);
                    m_lstExpression.Add(EqualExp);
                }

                //if (!string.IsNullOrEmpty(member.FamilyId))
                //{
                //    bool issame = member.FamilyId.Equals(1);
                //    MemberExpression leftExp = Expression.Property(m_Parameter, "FamilyId");
                //    int FamilyId = member.FamilyId.Value;
                //    var RightExp = Expression.Constant(FamilyId, typeof(int?));
                //    var EqualExp = Expression.Equal(leftExp, RightExp);
                //    m_lstExpression.Add(EqualExp);
                //}

                if (!string.IsNullOrEmpty(member.UserTrueName))
                {
                    var propertyExp = Expression.Property(m_Parameter, "UserTrueName"); // 申明属性
                    var containsMethod = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });// 判断条件 %UserTrueName%
                    var constExp = Expression.Constant(member.UserTrueName);// 表达式
                    var ContainsExp = Expression.Call(propertyExp, containsMethod, constExp);
                    m_lstExpression.Add(ContainsExp);
                }

                Expression whereExpr = null;
                foreach (var expr in m_lstExpression)
                {
                    if (whereExpr == null)
                        whereExpr = expr;
                    else
                        whereExpr = Expression.And(whereExpr, expr);
                }
                Expression<Func<User, bool>> exprelamada = null;
                if (whereExpr != null)
                {
                    exprelamada = Expression.Lambda<Func<User, bool>>(whereExpr, m_Parameter);
                }
                var sql = dbContext.User.Where<User>(exprelamada);// 可以看到 sql 语句
                list = sql.ToList();
                //list = dbContext.Database.SqlQuery<User>(sql).ToList();
                if (list != null)
                {
                    foreach (User e in list)
                    {
                        e.Role = dbContext.Role.Find(e.RoleId);
                        if (e.FamilyId != null)
                            e.Family = dbContext.Family.Find(e.FamilyId);
                    }                    
                }
            }
            return list;
        }

        /// <summary>
        /// 判断用户是否关联了家族
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public static bool HaveFamily(User member)
        {
            bool have = member.FamilyId != null ? true : false;
            return have;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public static ErrorCode Add(User member, User adminUser)
        {
            ErrorCode code = ErrorCode.Unknown_Error;
            if (adminUser == null || member == null)
            {
                code = ErrorCode.DataNotExist;
                return code;
            }

            using (FamilyRelationshipContext dbContext = FamilyRelationshipContext.GetFamilyRelationshipContext())
            {
                User admin = dbContext.User.Find(adminUser.UserId);// 判断管理员的合法性
                if (admin == null)
                    code = ErrorCode.No_User;
                else if (adminUser.RoleId > member.RoleId)
                    code = ErrorCode.NoAccess;
                else
                {
                    int count = dbContext.User.Where(c => c.UserName == member.UserName.Trim()).Count();
                    if (count > 0)
                        code = ErrorCode.DataAlreadyExist;
                    else
                    {
                        dbContext.User.Add(member);
                        int rows = dbContext.SaveChanges();
                        if (rows <= 0)
                            code = ErrorCode.DataAddError;
                        else
                            code = ErrorCode.Success;
                    }
                }
            }
            return code;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public static ErrorCode Remove(User member, User adminUser)
        {
            ErrorCode code = ErrorCode.Unknown_Error;
            if (adminUser == null || member == null)
            {
                code = ErrorCode.DataNotExist;
                return code;
            }
            using (FamilyRelationshipContext dbContext = FamilyRelationshipContext.GetFamilyRelationshipContext())
            {
                User admin = dbContext.User.Find(adminUser.UserId);// 判断管理员的合法性
                if (admin == null)
                    code = ErrorCode.No_User;
                else if (adminUser.RoleId > member.RoleId)
                    code = ErrorCode.NoAccess;
                else
                {
                    User rem = dbContext.User.Find(member.UserId);
                    if (rem != null)
                    {
                        dbContext.User.Remove(rem);
                        int rows = dbContext.SaveChanges();
                        if (rows > 0)
                            code = ErrorCode.Success;
                        else
                            code = ErrorCode.DataDetectError;
                    }
                    else
                        code = ErrorCode.Success;
                }
            }
            return code;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public static ErrorCode Modify(User member, User adminUser)
        {            
            ErrorCode code = ErrorCode.Unknown_Error;
            if (adminUser == null || member == null)
            {
                code = ErrorCode.DataNotExist;
                return code;
            }
            using (FamilyRelationshipContext dbContext = FamilyRelationshipContext.GetFamilyRelationshipContext())
            {
                User admin = dbContext.User.Find(adminUser.UserId);// 判断管理员的合法性
                if (admin == null)
                    code = ErrorCode.No_User;
                else if (adminUser.RoleId > member.RoleId)
                    code = ErrorCode.NoAccess;
                else
                {
                    User up = dbContext.User.Find(member.UserId);
                    if (up != null)
                    {
                        up.Password = member.Password;
                        up.RoleId = member.RoleId;
                        //up.Role = Member.Role;
                        up.UserTrueName = member.UserTrueName;
                        up.FamilyId = member.FamilyId;
                        //up.Family = Member.Family;
                        int rows = dbContext.SaveChanges();
                        if (rows > 0)
                            code = ErrorCode.Success;
                        else
                            code = ErrorCode.DataModifyError;
                    }
                    else
                        code = ErrorCode.DataNotExist;
                }
            }
            return code;
        }

        /// <summary>
        /// 修改用户的基本信息
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public static ErrorCode ModifyBaseInfo(User member)
        {
            ErrorCode code = ErrorCode.Unknown_Error;
            if (member == null)
            {
                code = ErrorCode.DataNotExist;
                return code;
            }
            using (FamilyRelationshipContext dbContext = FamilyRelationshipContext.GetFamilyRelationshipContext())
            {                
                User up = dbContext.User.Find(member.UserId);
                if (up != null)
                {
                    up.Password = member.Password;
                    up.UserTrueName = member.UserTrueName;
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
