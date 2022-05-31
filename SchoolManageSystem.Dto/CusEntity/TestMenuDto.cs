using SchoolManageSystem.Basics.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SchoolManageSystem.Dto.CusEntity
{
    public class TestMenuDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long? ParentId { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public int? OrderIndex { get; set; }
        public DateTime? CreatedTime { get; set; }
    }
}
