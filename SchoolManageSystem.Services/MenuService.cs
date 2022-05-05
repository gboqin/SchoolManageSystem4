using AutoMapper;
using SchoolManageSystem.Basics.ResponseResults;
using SchoolManageSystem.Basics.Enums;
using SchoolManageSystem.Dto.CusEntity;
using SchoolManageSystem.Entities;
using SchoolManageSystem.IRepositorys;
using SchoolManageSystem.IServices;
using SchoolManageSystem.Repositorys.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManageSystem.Services
{
    public class MenuService : BaseService, IMenuService
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IRoleMenuRepository _rmRepository;

        public MenuService(IMenuRepository menuRepository, IUnitOfWork<SchoolDbContext> unitOfWork, IMapper mapper) :
            base(unitOfWork, mapper)
        {
            _menuRepository = menuRepository;
        }

        public async Task<List<MenuDto>> GetMenusByRoleIds(long[] roleIds)
        {

            var menus = await _menuRepository.GetMenusByRoleIds(roleIds, true);
            //不存在join
            //var menus = from r in _unitOfWork.DbContext.Set<SysRoleMenuRelation>()
            //join m in _unitOfWork.DbContext.Set<SysMenu>() on r.MenuId equals m.Id
            //where roleIds.Contains(r.RoleId) && m.State == Basic.Enums.EnumState.Enabled
            //select m;

            return _mapper.Map<List<MenuDto>>(menus);
        }

        public async Task<ResponseResult<List<MenuDto>>> GetTopParentMenus()
        {
            var result = new ResponseResult<List<MenuDto>> ();
            var menus = await _menuRepository.GetTopParentMenus();
            return result.Succeed(_mapper.Map<List<MenuDto>>(menus));
        }

        public async Task<ResponseResult<List<MenuDto>>> GetMenusByParent(long? parentId)
        {
            var result = new ResponseResult<List<MenuDto>>();
            var menus = await _menuRepository.GetMenusByParent(parentId);
            return result.Succeed(_mapper.Map<List<MenuDto>>(menus));
        }

        /// <summary>
        /// 获取菜单树结构
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResult<List<MenuDto>>> GetNavTreeMenus()
        {
            var result = new ResponseResult<List<MenuDto>>();
            var allMenuList=  _mapper.Map<List<MenuDto>>(await _menuRepository.GetNavTreeMenus());
            List<MenuDto> rootNodeList = new List<MenuDto>();
            foreach (var parentNodeList in allMenuList.Where(t => t.ParentId == null))
            {
                MenuDto menuNode = new MenuDto();
                menuNode.Id = parentNodeList.Id;
                menuNode.Name = parentNodeList.Name;
                menuNode.Type = parentNodeList.Type;
                menuNode.ParentId = parentNodeList.ParentId;
                menuNode.PermissionCode = parentNodeList.PermissionCode;
                menuNode.Level = parentNodeList.Level;
                menuNode.OrderIndex = parentNodeList.OrderIndex;
                menuNode.Tips = parentNodeList.Tips;
                menuNode.Url = parentNodeList.Url;
                menuNode.Icon = parentNodeList.Icon;
                menuNode.ChildMenus = CreateChildTree(allMenuList, menuNode);
                rootNodeList.Add(menuNode);
            }
            return result.Succeed(rootNodeList);
        }

        /// <summary>
        /// 递归生成子树
        /// </summary>
        /// <param name="AllMenuList"></param>
        /// <param name="vmMenu"></param>
        /// <returns></returns>
        private List<MenuDto> CreateChildTree(List<MenuDto> AllMenuList, MenuDto vmMenu)
        {
            long parentMenuID = vmMenu.Id;//根节点ID
            List<MenuDto> nodeList = new List<MenuDto>();
            var children = AllMenuList.Where(t => t.ParentId == parentMenuID);
            foreach (var chl in children)
            {
                MenuDto node = new MenuDto();
                node.Id = chl.Id;
                node.Name = chl.Name;
                node.Type = chl.Type;
                node.ParentId = chl.ParentId;
                node.PermissionCode = chl.PermissionCode;
                node.Level = chl.Level;
                node.OrderIndex = chl.OrderIndex;
                node.Tips = chl.Tips;
                node.Url = chl.Url;
                node.Icon = chl.Icon;
                node.ChildMenus = CreateChildTree(AllMenuList, node);
                nodeList.Add(node);
            }
            return nodeList;
        }

        public async Task<ResponseResult<List<MenuDto>>> GetAllMenus() 
        {
            var result = new ResponseResult<List<MenuDto>>();
            var all = await _menuRepository.GetAllAsync();
            var allMenuList = _mapper.Map<List<MenuDto>>(all.Where(m => m.Deleted == 0 && m.State == EnumState.Enabled));
            List<MenuDto> rootNodeList = new List<MenuDto>();
            foreach (var parentNodeList in allMenuList.Where(t => t.ParentId == null))
            {
                MenuDto menuNode = new MenuDto();
                menuNode.Id = parentNodeList.Id;
                menuNode.Name = parentNodeList.Name;
                menuNode.Type = parentNodeList.Type;
                menuNode.ParentId = parentNodeList.ParentId;
                menuNode.PermissionCode = parentNodeList.PermissionCode;
                menuNode.Level = parentNodeList.Level;
                menuNode.OrderIndex = parentNodeList.OrderIndex;
                menuNode.Tips = parentNodeList.Tips;
                menuNode.Url = parentNodeList.Url;
                menuNode.Icon = parentNodeList.Icon;
                menuNode.ChildMenus = CreateChildTree(allMenuList, menuNode);
                rootNodeList.Add(menuNode);
            }
            return result.Succeed(rootNodeList);
        }
        public async Task<ResponseResult<List<long>>> GetMenuIdsByRoleIds(params long[] Ids) {
            var result = new ResponseResult<List<long>>();
            var reIds = await _menuRepository.GetMenuIdsByRoleIds(Ids, true);

            return result.Succeed(reIds);
        }
    }
}
