using SchoolManageSystem.Basics.ResponseResults;
using SchoolManageSystem.Dto.CusEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManageSystem.IServices
{
    public interface IRoleService: IBaseService
    {
        Task<ResponseResult<List<RoleDto>>> GetAllRoles();
        Task<ResponseResult<List<RoleDto>>> GetAllRolesByName(string roleName);
        //引用MenuService
        //Task<ResponseResult<List<MenuDto>>> GetMenus();
        Task<ResponseResult<List<long>>> GetMenuIdsByRoleId(long roleId);
        Task<ResponseResult<RoleDto>> AddRole(RoleDto role);
        Task<ResponseResult<RoleDto>> EditRole(RoleDto role);
        Task<ResponseResult<RoleDto>> DeleteRole(RoleDto role);
        Task<int> AssignPermissions(long roleId, List<long> firstMenuIds, List<long> setMenuIds);
    }
}
