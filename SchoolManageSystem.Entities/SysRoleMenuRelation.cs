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
    /// 角色菜单关系表
    /// </summary>
    [Table("tb_SysRoleMenu")]
    [Description("角色菜单关系表")]

    public class SysRoleMenuRelation : BaseEntityNoDeleted<long>
    {
        public long RoleId { get; set; }
        public virtual SysRole SysRole { get; set; }
        public long MenuId { get; set; }
        public virtual SysMenu SysMenu { get; set; }
        public long TenantId { get; set; }
        public EnumState State { get; set; } = EnumState.Enabled;
    }
}
