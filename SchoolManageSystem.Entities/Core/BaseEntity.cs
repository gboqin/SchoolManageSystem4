using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SchoolManageSystem.Entities.Core
{
    public abstract class BaseEntity<TKey>:Entity<TKey> where TKey:struct
    {
        /// <summary>
        /// 修改时间
        /// </summary>
        public virtual DateTime ModifiedTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 操作人名称
        /// </summary>
        public virtual string OperatorName { get; set; }
        /// <summary>
        /// 软删除
        /// </summary>
        [DefaultValue(0)]
        public virtual int Deleted { get; set; }
    }
}
