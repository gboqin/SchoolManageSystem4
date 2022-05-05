using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SchoolManageSystem.Entities.Core
{
    //struct 类型是一种值类型，通常用来封装小型相关变量组，
    public abstract class Entity<TKey>: IEntity where TKey:struct 
    {
        [Key]
        public TKey Id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        public virtual DateTime? CreatedTime { get; set; } = DateTime.Now;
    }
}
