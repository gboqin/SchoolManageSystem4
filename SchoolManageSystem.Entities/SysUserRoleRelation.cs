using SchoolManageSystem.Basics.Enums;
using SchoolManageSystem.Entities.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SchoolManageSystem.Entities
{
    /// <summary>
    /// 用户角色关系表
    /// </summary>
    [Table("tb_SysUserRole")]
    [Description("用户角色关系表")]
    public class SysUserRoleRelation : BaseEntityNoDeleted<long>
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }
        public virtual SysUser SysUser { get; set; }
        /// <summary>
        /// 角色Id
        /// </summary>
        public long RoleId { get; set; }
        public virtual SysRole SysRole { get; set; }
        /// <summary>
        /// 租户ID
        /// </summary>
        public long TenantId { get; set; }
        /// <summary>
        /// 状态 
        /// </summary>
        public EnumState State { get; set; } = EnumState.Enabled;
    }
}
