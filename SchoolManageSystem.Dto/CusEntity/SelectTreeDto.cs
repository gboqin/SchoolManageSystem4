using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolManageSystem.Dto.CusEntity
{
    public class SelectTreeDto
    {
        public long id { get; set; } 
        public string name { get; set; }
        public long parentId { get; set; }
        //public string url { get; set; }
        public string icon { get; set; }
        public int orderIndex { get; set; }
        public List<SelectTreeDto> children { get; set; }
    }
}
