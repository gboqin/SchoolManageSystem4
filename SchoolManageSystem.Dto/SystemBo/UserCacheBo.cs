using SchoolManageSystem.Dto.CusEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolManageSystem.Dto.SystemBo
{
    public class UserCacheBo
    {
        public long Id { get; set; }
        public string NickName { get; set; }
        public string PassWord { get; set; }
        public int Sex { get; set; }
        public string Email { get; set; }
        public string UserToken { get; set; }
        /// <summary>
        /// 为0的是管理员
        /// </summary>
        public long CreatedBy { get; set; }
        /// <summary>
        /// 会话ID
        /// </summary>
        public string SessionId { get; set; }
        public string LoginIp { get; set; }
        public DateTime LoginTime { get; set; }
        public IEnumerable<RoleDto> RoleCacheBos { get; set; }
        public List<string> RoleList => RoleCacheBos.Select(r => r.Name).ToList();
        public IEnumerable<MenuDto> MenuCacheBos { get; set; }

        /// <summary>
        /// 菜单代号
        /// </summary>
        public List<string> MenuCodeList => MenuCacheBos.Select(m => m.PermissionCode).ToList();
    }
}
