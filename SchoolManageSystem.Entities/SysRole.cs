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
    /// 角色表
    /// </summary>
    [Table("tb_SysRole")]
    [Description("角色")]
    public partial class SysRole : BaseEntity<long>
    {
        /// <summary>
        /// 租户ID
        /// </summary>
        public long TenantId { get; set; }
        [Description("角色名称")]
        [StringLength(100)]
        [Column("Name")]
        public string Name { get; set; }
        [Description("角色描述")]
        [StringLength(200)]
        [Column("RoleDesc")]
        public string RoleDesc { get; set; }
        [Description("排序号")]
        [Column("Num")]
        public int? Num { get; set; }
        public virtual ICollection<SysRoleMenuRelation> SysRoleMenuRelations { get; set; }
        public virtual ICollection<SysUserRoleRelation> SysUserRoleRelations { get; set; }
    }
}
