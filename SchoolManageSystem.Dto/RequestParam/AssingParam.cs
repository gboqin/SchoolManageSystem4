using SchoolManageSystem.Dto.CusEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolManageSystem.Dto.RequestParam
{
    public class AssingParam
    {
        public RoleDto role { get; set; }
        public List<long> firstMIds { get; set; }
        public List<long> setMIds { get; set; }
    }
}
