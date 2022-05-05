using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SchoolManageSystem.Basics.Config
{
    /// <summary>
    /// 常量类
    /// </summary>
    public class SysConstants
    {
        /// <summary>
        /// 登录token验证结果返回
        /// </summary>
        public enum TokenValidType
        {
            [Description("success")]
            Success = 1,
            [Description("登录已失效，请重新登录")]
            LogonInvalid = 2,
            [Description("账号已在其它地方登录，若非本人操作，请尽快修改密码")]
            LoggedInOtherPlaces = 3,
        }
    }
}
