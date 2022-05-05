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
	/// 账号
	/// </summary>
	[Table("tb_SysUser")]
	[Description("账号")]
	public partial class SysUser : BaseEntity<long>
	{
		/// <summary>
		/// 账户
		/// </summary>
		[Description("账号")]
		[StringLength(32)]
		[Column("Account")]
		public string Account { get; set; }
		/// <summary>
		/// email
		/// </summary>
		[Description("email")]
		[StringLength(64)]
		[Column("Email")]
		public string Email { get; set; }
		/// <summary>
		/// 姓名
		/// </summary>
		[Description("姓名")]
		[StringLength(64)]
		[Column("Name")]
		public string Name { get; set; }

		/// <summary>
		/// 密码
		/// </summary>
		[Description("密码")]
		[StringLength(128)]
		[Column("Password")]
		public string Password { get; set; }

		/// <summary>
		/// 手机号
		/// </summary>
		[Description("手机号")]
		[StringLength(16)]
		[Column("Phone")]
		public string Phone { get; set; }
		/// <summary>
		/// 性别
		/// </summary>
		[Description("性别")]
		[Column("Sex")]
		public EnumSex Sex { get; set; }
		/// <summary>
		/// 激活状态
		/// </summary>
		[Description("激活状态")]
		public EnumState State { get; set; } = EnumState.Enabled;
		public long TenantId { get; set; }
		public virtual SysTenant TenantInfo { get; set; }
		public ICollection<SysUserRoleRelation> SysUserRoleRelations { get; set; }
	}
}
