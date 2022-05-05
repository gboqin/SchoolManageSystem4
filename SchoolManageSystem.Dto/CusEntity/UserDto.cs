using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolManageSystem.Dto.CusEntity
{
    public class UserDto
    {
        public long Id { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public List<string> RoleList { get; set; }
    }
}
