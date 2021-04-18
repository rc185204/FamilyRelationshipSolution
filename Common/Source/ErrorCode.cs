using System;

namespace FRS.Common
{
    /// <summary>
    /// 错误码
    /// </summary>
    public enum ErrorCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 0,

        /// <summary>
        /// 未知错误
        /// </summary>
        Unknown_Error,

        #region FileSystem
        /// <summary>
        /// 文件系统错误
        /// </summary>
        FileSystemError,

        /// <summary>
        /// 不支持的媒体类型
        /// </summary>
        Unsupported_MediaType,
        #endregion

        #region Data base error

        /// <summary>
        /// 数据库错误
        /// </summary>
        DataBase_Error,

        /// <summary>
        /// 查询数据不存在
        /// </summary>
        NoData,

        /// <summary>
        /// 数据已经存在，不能新增
        /// </summary>
        DataAlreadyExist,

        /// <summary>
        /// 添加数据错误
        /// </summary>
        DataAddError,

        /// <summary>
        /// 数据修改错误
        /// </summary>
        DataModifyError,

        /// <summary>
        /// 数据删除错误
        /// </summary>
        DataDetectError,


        #endregion

        #region Authentication
        /// <summary>
        /// 原始密码错误
        /// </summary>
        Original_Password_Error,

        /// <summary>
        /// 登录用户名或密码错误
        /// </summary>
        Login_Error,

        /// <summary>
        /// 验证token无效
        /// </summary>
        AccessToken_NoUseful,
        #endregion

        #region BLUser
        /// <summary>
        /// 无此用户
        /// </summary>
        No_User,


        #endregion
    }

}
