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
    /// 菜单/按钮资源表
    /// </summary>
    [Table("tb_SysMenu")]
    [Description("菜单/按钮资源表")]
    public partial class SysMenu:BaseEntity<long>
    {
        /// <summary>
        /// 显示名称
        /// </summary>
        [Required]
        [StringLength(100)]
        [Description("名称")]
        [Column("Name")]
        public string Name { get; set; }
        /// <summary>
        /// 是否是菜单0:菜单,1:按钮
        /// </summary>
        [Description("是否是菜单0:菜单,1:按钮")]
        [Column("Type")]
        public EnumMenuType Type { get; set; }
        /// <summary>
        /// 父Id
        /// </summary>
 		[Description("父菜单Id")]
        [Column("ParentsId")]
        public long? ParentId { get; set; }
        /// <summary>
        /// 权限特性
        /// </summary>
        [Required]
        [Description("权限特性")]
        [StringLength(32)]
        [Column("PermissionCode")]
        public string PermissionCode { get; set; }
        [Required]
        [Description("级别")]
        [Column("Level")]
        public int Level { get; set; }
        /// <summary>
        /// 顺序
        /// </summary>
        [Description("顺序")]
        [Column("OrderIndex")]
        public int OrderIndex { get; set; }
        /// <summary>
        /// 鼠标悬停提示信息
        /// </summary>
        [Description("鼠标悬停提示信息")]
        [StringLength(64)]
        [Column("Tips")]
        public string Tips { get; set; }
        /// <summary>
        /// 链接
        /// </summary>
        [Description("链接")]
        [StringLength(64)]
        [Column("Url")]
        public string Url { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        [Description("图标")]
        [StringLength(64)]
        [Column("Icon")]
        public string Icon { get; set; }
        /// <summary>
        /// 是否隐藏
        /// </summary>
        [Description("是否隐藏")]
        [Column("Hidden")]
        public bool? Hidden { get; set; }
        /// <summary>
        /// 状态1:启用,0:禁用
        /// </summary>
        [Description("状态1:启用,0:禁用")]
        [Column("Status")]
        [DefaultValue(1)]
        public EnumState State { get; set; }
        public ICollection<SysRoleMenuRelation> SysRoleMenuRelations { get; set; }
    }
}
