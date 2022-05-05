using SchoolManageSystem.Basics.Enums;
using SchoolManageSystem.Entities.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SchoolManageSystem.Entities
{
    /// <summary>
    /// 租户表
    /// </summary>
    [Table("tb_SysTenant")]
    [Description("租户表")]
    public class SysTenant : BaseEntity<long>
    {
        /// <summary>
        /// 租户名称
        /// </summary>
        [Required, MaxLength(50)]
        public string TenantName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(32)]
        public string Remark { get; set; }
        /// <summary>
        /// 状态:0-未激活;1-启用;2-禁用;3-注销
        /// </summary>
        public EnumState State { get; set; } = EnumState.Enabled;
        public virtual ICollection<SysUser> SysUsers { get; set; }
    }
}
