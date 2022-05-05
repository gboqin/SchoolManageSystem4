using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SchoolManageSystem.Basics.Enums
{
    public enum EnumSex
    {
        [Description("未知")]
        [Display(Name = "未知")]
        UnKnown = -1,
        [Description("男")]
        Men = 1,
        [Description("女")]
        Women = 0
    }
}
