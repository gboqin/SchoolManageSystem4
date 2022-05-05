using SchoolManageSystem.Basics.Enums;
using SchoolManageSystem.Entities.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SchoolManageSystem.Entities
{
    [Table("tb_Depart")]
    public partial class Depart : BaseEntityNoDeleted<int>
    {
        /// <summary>
        /// 部门名称
        /// </summary>
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string DepartName { get; set; }

        /// <summary>
        /// 部门类别
        /// </summary>
        public EnumDeptType DeptType { get; set; }

        /// <summary>
        /// 年组编号
        /// </summary>
        public int? GradeId { get; set; } = null;

        /// <summary>
        /// 年组信息
        /// </summary>
        [ForeignKey("GradeId")]
        public virtual Depart GradeDepart { get; set; }
    }
}
