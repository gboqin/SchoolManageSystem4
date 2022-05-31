using Microsoft.EntityFrameworkCore;
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
    public class RoleMenuRepository: Repository<SysRoleMenuRelation>,IRoleMenuRepository
    {
        public RoleMenuRepository(SchoolDbContext db) : base(db) { }

        public async Task AddRoleMenuRelation(long roleId, List<long> menuIds)
        {
            var rms = new List<SysRoleMenuRelation>();
            if (menuIds.Count > 0)
            {
                foreach (var item in menuIds)
                {
                    rms.Add(new SysRoleMenuRelation { RoleId = roleId, MenuId= item });
                }
            }
            await base.InsertAsync(rms);
        }

        public async Task<List<long>> GetMenuIdsByRoleId(long Id)
        {
            var ids = from rm in _dbContext.Set<SysRoleMenuRelation>()
                      where rm.Deleted == 0 && rm.RoleId == Id
                      select rm.MenuId;
            //_dbContext.Set<SysRoleMenuRelation>().Where(rm => rm.Deleted == 0 && rm.RoleId == Id).Select(rm => rm.MenuId).ToListAsync();
            var ss = await ids.ToListAsync();

            return ss;
        }

        public async Task SetRoleMenuRelation(long roleId, List<long> firstMIds, List<long> setMIds)
        {
            //f有，s无的差集
            var fss = firstMIds.Except(setMIds).ToList();//取得差集
            if (fss.Count > 0) 
            { 
                foreach (var item in fss)
                {
                    var rm = await _dbContext.Set<SysRoleMenuRelation>().Where(rm => rm.Deleted == 0 && rm.RoleId == roleId && rm.MenuId == item).FirstOrDefaultAsync();
                    base.Delete(rm);
                }
            }
            //s有，f无的差集
            var sfs = setMIds.Except(firstMIds).ToList();
            if (sfs.Count>0)
            {
                foreach (var item in sfs)
                {
                    var rm = new SysRoleMenuRelation { RoleId = roleId, MenuId = item };
                    await base.InsertAsync(rm);
                }
            }
        }
    }

}
