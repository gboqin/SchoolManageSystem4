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
            foreach (var parentNodeList in allMenuList.Where(t => t.ParentId == -1))
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
            foreach (var parentNodeList in allMenuList.Where(t => t.ParentId == -1))
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

        public async Task<ResponseResult<List<Dictionary<string, object>>>> GetTreeMenus()
        {
            ResponseResult<List<Dictionary<string, object>>> result = new ResponseResult<List<Dictionary<string, object>>>();
            //创建一个字典集合
            List<Dictionary<string, object>> jsonoList = new List<Dictionary<string, object>>();
            //查询出表的所有数据
            var all = await _menuRepository.GetAllAsync();
            var test = all.Where(m => m.Deleted == 0 && m.State == EnumState.Enabled).ToList();
            var allMenuList = _mapper.Map<List<MenuDto>>(all.Where(m => m.Deleted == 0 && m.State == EnumState.Enabled));
            
            foreach (var dr in allMenuList.Where(t => t.ParentId == -1))//foreach循环遍历
            {
                //创建字典，将所需的字段和值添加进去
                Dictionary<string, object> json = new Dictionary<string, object>();
                json.Add("title", dr.Name);//标题
                json.Add("id", dr.Id);//上级id

                json.Add("children", GetChildren(dr.Id, _mapper.Map<List<MenuDto>>(allMenuList)));
                jsonoList.Add(json);//将字典添加进字典集合
            }

            return result.Succeed(jsonoList);//返回字典集合
        }

        public List<Dictionary<string,object>> GetChildren(long id, List<MenuDto> AllMenuList)
        {
            //创建一个字典集合
            List<Dictionary<string, object>> jsonoList = new List<Dictionary<string, object>>();
            //查询出表的所有数据
            var treeList = AllMenuList.Where(m => m.ParentId == id).ToList();

            foreach (var dr in treeList)//foreach循环遍历
            {
                //创建字典，将所需的字段和值添加进去
                Dictionary<string, object> json = new Dictionary<string, object>();
                json.Add("title", dr.Name);//标题
                json.Add("id", dr.Id);
                json.Add("children", GetChildren(dr.Id, treeList));
                jsonoList.Add(json);//将字典添加进字典集合
            }
            return jsonoList;//返回字典集合
        }

        public async Task<ResponseResult<List<SelectTreeDto>>> GetTreeSelectMenus()
        {
            ResponseResult<List<SelectTreeDto>> result = new ResponseResult<List<SelectTreeDto>>();
            //创建一个字典集合
            List<SelectTreeDto> jsonoList = new List<SelectTreeDto>();
            //查询出表的所有数据
            var all = await _menuRepository.GetAllAsync();
            var test = all.Where(m => m.Deleted == 0 && m.State == EnumState.Enabled).ToList();
            var allMenuList = _mapper.Map<List<SelectTreeDto>>(all.Where(m => m.Deleted == 0 && m.State == EnumState.Enabled));

            foreach (var dr in allMenuList.Where(t => t.parentId == -1))//foreach循环遍历
            {
                dr.children = GetTreeSelectChildren(dr.id, allMenuList); ;
                //创建字典，将所需的字段和值添加进去
                jsonoList.Add(dr);
            }

            return result.Succeed(jsonoList);//返回字典集合
        }

        public List<SelectTreeDto> GetTreeSelectChildren(long id, List<SelectTreeDto> AllMenuList)
        {
            //创建一个字典集合
            List<SelectTreeDto> childList = new List<SelectTreeDto>();
            //查询出表的所有数据
            var treeList = AllMenuList.Where(m => m.parentId == id).ToList();

            foreach (var dr in treeList)//foreach循环遍历
            {
                dr.children = GetTreeSelectChildren(dr.id, AllMenuList); ;
                //创建字典，将所需的字段和值添加进去
                childList.Add(dr);
            }
            return childList;//返回字典集合
        }

        public async Task<ResponseResult<List<TestMenuDto>>> GetAllTestMenuList()
        {
            ResponseResult<List<TestMenuDto>> result = new ResponseResult<List<TestMenuDto>>();
            List<long> ids = new List<long>();
            var all = await _menuRepository.GetAllAsync();
            var allMenuList = _mapper.Map<List<TestMenuDto>>(all.Where(m => m.Deleted == 0 && m.State == EnumState.Enabled));
            return result.Succeed(allMenuList);
        }

        public async Task<ResponseResult<List<MenuDto>>> GetAllMenuList()
        {
            ResponseResult<List<MenuDto>> result = new ResponseResult<List<MenuDto>>();
            List<long> ids = new List<long>();
            var all = await _menuRepository.GetAllAsync();
            var allMenuList = _mapper.Map<List<MenuDto>>(all.Where(m => m.Deleted == 0 && m.State == EnumState.Enabled));
            return result.Succeed(allMenuList);
        }
        public async Task<ResponseResult<TestMenuDto>> AddMenu(TestMenuDto menu)
        {
            var result = new ResponseResult<TestMenuDto>();
            SysMenu data = new SysMenu();
            data.Name = menu.Name;
            data.ParentId = menu.ParentId??-1;
            data.Url = menu.Url;
            data.Icon = menu.Icon;
            data.OrderIndex =  menu.OrderIndex??0;
            data.CreatedTime = DateTime.Now;
            data.PermissionCode = "Test111";
            data.Type = EnumMenuType.Menu;
            data.State = EnumState.Enabled;
            //var data = _mapper.Map<SysMenu>(menu);
            var model = await _menuRepository.InsertAsync(data);
            await _unitOfWork.SaveChangesAsync();
            return result.Succeed(_mapper.Map<TestMenuDto>(data));
        }
        public async Task<ResponseResult<TestMenuDto>> EdtMenu(TestMenuDto menu)
        {
            ResponseResult<TestMenuDto> result = new ResponseResult<TestMenuDto>();
            var data = _menuRepository.GetAll().Where(m=>m.Id==menu.Id).FirstOrDefault();
            data.Name = menu.Name;
            data.ParentId = menu.ParentId;
            data.Url = menu.Url;
            data.Icon = menu.Icon;
            data.OrderIndex = menu.OrderIndex ?? 0;
            _menuRepository.Update(data);
            await _unitOfWork.SaveChangesAsync();
            return result.Succeed(menu);
        }
        public async Task<ResponseResult<int>> DelMenu(TestMenuDto menu)
        {
            ResponseResult<int> result = new ResponseResult<int>();
            List<long> ids = new List<long>();
            var all = await _menuRepository.GetAllAsync();
            var allMenuList = _mapper.Map<List<TestMenuDto>>(all.Where(m => m.Deleted == 0 && m.State == EnumState.Enabled));
            ids.Add(menu.Id);
            GetIds(allMenuList, ids, menu.Id);
            var delMenus = all.Where(m => ids.Contains(m.Id)).ToList();

            using (var tran = _unitOfWork.BeginTransaction())
            {
                try
                {
                    _menuRepository.Delete(delMenus);
                    var len = await _unitOfWork.SaveChangesAsync();
                    await tran.CommitAsync();
                    return result.Succeed(len);
                }
                catch (Exception e)
                {
                    tran.Rollback();
                    return result.Fail(ResponseCode.DbEx, e.Message, 0);
                }
            }
        }
        public void GetIds(List<TestMenuDto> AllMenuList, List<long> ids, long oneId)
        {
            List<TestMenuDto> menus = AllMenuList.Where(m => m.ParentId == oneId).ToList();
            foreach(var item in menus)
            {
                ids.Add(item.Id);
                GetIds(AllMenuList, ids, item.Id);
            }
        }



    }
}
