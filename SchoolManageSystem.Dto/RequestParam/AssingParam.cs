using SchoolManageSystem.Dto.CusEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolManageSystem.Dto.RequestParam
{
    public class AssingParam
    {
        public long RId { get; set; }
        public long[] RawIds { get; set; }
        public long[] SetIds { get; set; }
    }
}
