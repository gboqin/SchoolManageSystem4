using SchoolManageSystem.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManageSystem.IRepositorys
{
    public interface IRoleMenuRepository :IRepository<SysRoleMenuRelation>
    {
        Task<List<long>> GetMenuIdsByRoleId(long Id);
        Task AddRoleMenuRelation(long roleId, List<long> menuIds);
        Task SetRoleMenuRelation(long roleId, List<long> firstMIds, List<long> setMIds);
    }
}
