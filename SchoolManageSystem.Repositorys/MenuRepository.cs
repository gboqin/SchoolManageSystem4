using Microsoft.EntityFrameworkCore;
using SchoolManageSystem.Basics.Enums;
using SchoolManageSystem.Entities;
using SchoolManageSystem.IRepositorys;
using SchoolManageSystem.Repositorys.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManageSystem.Repositorys
{
    public class MenuRepository : Repository<SysMenu>, IMenuRepository
    {
        public MenuRepository(SchoolDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<SysMenu>> GetMenusByRoleIds(long[] roleIds, bool enabledOnly)
        {
            IQueryable<SysMenu> menus = null;
            if (enabledOnly)
            {
                menus = from r in _dbContext.Set<SysRoleMenuRelation>()
                        join m in _dbContext.Set<SysMenu>() on r.MenuId equals m.Id
                        where roleIds.Contains(r.RoleId) && m.State == EnumState.Enabled
                        select m;

            }
            else
            {
                menus = from r in _dbContext.Set<SysRoleMenuRelation>()
                        join m in _dbContext.Set<SysMenu>() on r.MenuId equals m.Id
                        where roleIds.Contains(r.RoleId)
                        select m;
            }

            return await menus.ToListAsync();
        }

        public async Task<List<long>> GetMenuIdsByRoleIds(long[] roleIds, bool enabledOnly) 
        {
            IQueryable<long> menuIds = null;
            if (enabledOnly)
            {
                menuIds = from r in _dbContext.Set<SysRoleMenuRelation>()
                        join m in _dbContext.Set<SysMenu>() on r.MenuId equals m.Id
                        where roleIds.Contains(r.RoleId) && m.State == EnumState.Enabled && m.Deleted==0
                        select m.Id;

            }
            else
            {
                menuIds = from r in _dbContext.Set<SysRoleMenuRelation>()
                        join m in _dbContext.Set<SysMenu>() on r.MenuId equals m.Id
                        where roleIds.Contains(r.RoleId) && m.Deleted == 0
                          select m.Id;
            }

            return await menuIds.ToListAsync();
        }

        public async Task<List<SysMenu>> GetTopParentMenus()
        {
            IQueryable<SysMenu> menus = null;
            menus = from m in _dbContext.Set<SysMenu>() 
                    where m.Type == EnumMenuType.Menu && m.ParentId == -1
                    orderby m.OrderIndex
                    select m;
            var a = menus.ToList();
            return await menus.ToListAsync();
        }

        public async Task<List<SysMenu>> GetMenusByParent(long? parentId)
        {
            IQueryable<SysMenu> menus = null;
            menus = from m in _dbSet 
                    where m.Type == EnumMenuType.Menu &&  m.ParentId == parentId
                    orderby m.OrderIndex
                    select m;
            return await menus.ToListAsync();
        }

        public async Task<List<SysMenu>> GetNavTreeMenus()
        {
            IQueryable<SysMenu> menus = null;
            menus = from m in _dbSet
                    where m.Type == EnumMenuType.Menu
                    orderby m.OrderIndex
                    select m;
            return await menus.ToListAsync();
        }
    }
}
