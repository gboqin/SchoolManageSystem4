using SchoolManageSystem.Basics.ResponseResults;
using SchoolManageSystem.Dto.CusEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManageSystem.IServices
{
    public interface IMenuService : IBaseService
    {
        
        Task<List<MenuDto>> GetMenusByRoleIds(params long[] Ids);
        Task<ResponseResult<List<MenuDto>>> GetTopParentMenus();
        Task<ResponseResult<List<MenuDto>>> GetMenusByParent(long? parentId);

        Task<ResponseResult<List<MenuDto>>> GetNavTreeMenus();
        Task<ResponseResult<List<MenuDto>>> GetAllMenus();
        Task<ResponseResult<List<Dictionary<string,object>>>> GetTreeMenus();
        Task<ResponseResult<List<SelectTreeDto>>> GetTreeSelectMenus();
        Task<ResponseResult<List<long>>> GetMenuIdsByRoleIds(params long[] Ids);
        Task<ResponseResult<List<MenuDto>>> GetAllMenuList();
        Task<ResponseResult<List<TestMenuDto>>> GetAllTestMenuList(); 
        Task<ResponseResult<TestMenuDto>> AddMenu(TestMenuDto menu);
        Task<ResponseResult<TestMenuDto>> EdtMenu(TestMenuDto menu);
        Task<ResponseResult<int>> DelMenu(TestMenuDto menu);
    }
}
