using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolManageSystem.Dto.SystemBo
{
    public class LoginUserBo
    {
        public string uid { get; set; }
        public List<string> RoleList { get; set; }
        public string token { get; set; }
        public string sessionId { get; set; }
    }
}
