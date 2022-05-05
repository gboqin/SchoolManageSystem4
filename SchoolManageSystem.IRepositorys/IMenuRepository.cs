using SchoolManageSystem.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManageSystem.IRepositorys
{
    public interface IMenuRepository : IRepository<SysMenu>
    {
        Task<List<SysMenu>> GetMenusByRoleIds(long[] roleIds, bool enabledOnly);

        Task<List<long>> GetMenuIdsByRoleIds(long[] roleIds, bool enabledOnly);
        Task<List<SysMenu>> GetTopParentMenus();
        Task<List<SysMenu>> GetMenusByParent(long? parentId);
        Task<List<SysMenu>> GetNavTreeMenus();
    }
}
