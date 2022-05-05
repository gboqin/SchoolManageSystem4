using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SchoolManageSystem.Basics.Enums
{
    /// <summary>
    /// 菜单类型
    /// </summary>
    public enum EnumMenuType
    {
        /// <summary>
        /// 菜单
        /// </summary>
        [Description("菜单")]
        Menu = 1,
        /// <summary>
        /// 按钮
        /// </summary>
        [Description("按钮")]
        Button = 2,
    }
}
