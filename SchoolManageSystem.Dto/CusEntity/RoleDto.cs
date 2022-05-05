using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolManageSystem.Dto.CusEntity
{
    public class RoleDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string RoleDesc { get; set; }
        /// <summary>
        /// 排序号
        /// </summary>
        public int? Num { get; set; }
        /// <summary>
        /// 最后操作时间
        /// </summary>
        public DateTime ModifiedTime { get; set; }
        /// <summary>
        /// 最后操作人
        /// </summary>
        public string OperatorName { get; set; }
    }
}
