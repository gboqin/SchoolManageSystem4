using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SchoolManageSystem.Entities.Core
{
    public abstract class BaseEntityNoDeleted<TKey>:BaseEntity<TKey> where TKey:struct
    {
        /// <summary>
        /// NotMapped特性可以应用到EF实体类的属性中，Code-First默认的约定，是为所有带有get,和set属性选择器的属性创建数据列。
        /// NotManpped特性打破了这个约定，你可以使用NotMapped特性到某个属性上面，然后Code-First就不会为这个属性在数据表中创建列了。
        /// </summary>
        [NotMapped]
        public override int Deleted { get; set; }
    }
}
