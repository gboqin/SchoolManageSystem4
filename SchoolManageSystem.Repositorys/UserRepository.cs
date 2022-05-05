using Microsoft.EntityFrameworkCore;
using SchoolManageSystem.Basics.Enums;
using SchoolManageSystem.Basics.ResponseResults;
using SchoolManageSystem.Basics.Security;
using SchoolManageSystem.Dto.CusEntity;
using SchoolManageSystem.Dto.RequestParam;
using SchoolManageSystem.Dto.SystemBo;
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
    public class UserRepository : Repository<SysUser>, IUserRepository
    {
        public UserRepository(SchoolDbContext db) : base(db) { }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginParam"></param>
        /// <returns></returns>
        public async Task<ResponseResult<UserCacheBo>> LoginAsync(LoginParam loginParam)
        {
            var response = new ResponseResult<UserCacheBo>();


            //var user = await FindAsync(c => c.Email == loginParam.Email && (int)c.State == (int)EnumState.Enabled);
            //await _dbContext.Set<SysUser>().FindAsync(c => c.Email == loginParam.Email);
            //var user.State = user.State ==EnumState.Enabled;
            var user = await _dbContext.Set<SysUser>().Where(c => c.Email == loginParam.Email && c.State == EnumState.Enabled).FirstOrDefaultAsync();
            if (user == null)
            {
                return response.Fail(ResponseCode.LoginFail, "账号不存在", null);
            }

            var pw = loginParam.Password.Md5Encrypt();
            //var pw = Crypto.HashPassword(loginParam.Password);
            if (user.Password != loginParam.Password.Md5Encrypt())
            {
                return response.Fail(ResponseCode.LoginFail, "账号或密码错误", null);
            }

            var dbData = _dbContext.Set<SysUser>().Where(u => u.Id == user.Id && (int)u.State == (int)EnumState.Enabled)
                           .Select(u => new UserCacheBo
                           {
                               Id = u.Id,
                               NickName = u.Account,
                               PassWord = u.Password,
                               Email = u.Email,
                               Sex = (int)u.Sex,
                               RoleCacheBos=_dbContext.Set< SysUserRoleRelation >()
                                .Where(ur=>ur.UserId==u.Id)
                                .Join(_dbContext.Set<SysRole>(),ur=>ur.RoleId,r=>r.Id,(ur,r)=>new { r.Id,r.Name,r.RoleDesc})
                                .Select(r=>new RoleDto() { Id=r.Id,Name=r.Name,RoleDesc=r.RoleDesc}).ToList(),
                               MenuCacheBos = _dbContext.Set<SysUserRoleRelation>()
                                 .Where(ur => ur.UserId == u.Id)
                                 .Join(_dbContext.Set<SysRoleMenuRelation>(), nr => nr.RoleId, rm => rm.RoleId, (nr, rm) => new { rm.MenuId })
                                 .Join(_dbContext.Set<SysMenu>(), nrm => nrm.MenuId, m => m.Id, (nrm, m) => new { m.Id, m.Name, m.Type, m.PermissionCode, m.Level, m.OrderIndex, m.Tips, m.Url, m.Icon, m.State })
                                 .Select(m => new MenuDto()
                                 {
                                     Id = m.Id,
                                     Name = m.Name,
                                     Type = m.Type,
                                     PermissionCode = m.PermissionCode,
                                     Level = m.Level,
                                     OrderIndex = m.OrderIndex,
                                     Tips = m.Tips,
                                     Url = m.Url,
                                     Icon = m.Icon
                                 }).ToList()

                           });
            //var dbData = from sysUser in _dbContext.Set<SysUser>()
            //             where sysUser.Id == user.Id && (int)sysUser.State == (int)EnumState.Enabled
            //             select new UserCacheBo
            //             {
            //                 Id = sysUser.Id,
            //                 NickName = sysUser.Account,
            //                 PassWord = sysUser.Password,
            //                 Email = sysUser.Email,
            //                 Sex = (int)sysUser.Sex,
            //                 MenuCacheBos = from userRoleRelation in _dbContext.Set<SysUserRoleRelation>()
            //                                //join sysRole in _dbContext.Set<SysRole>() on userRoleRelation.RoleId equals sysRole.Id
            //                                join roleMenuRelation in _dbContext.Set<SysRoleMenuRelation>() on userRoleRelation.RoleId equals roleMenuRelation.RoleId
            //                                join sysMenu in _dbContext.Set<SysMenu>() on roleMenuRelation.MenuId equals sysMenu.Id
            //                                where userRoleRelation.UserId == sysUser.Id && sysMenu.State==EnumState.Enabled
            //                                    && userRoleRelation.State == EnumState.Enabled
            //                                select new MenuDto()
            //                                {   Id = sysMenu.Id,
            //                                    Name = sysMenu.Name,
            //                                    Type = sysMenu.Type,
            //                                    PermissionCode =sysMenu.PermissionCode,
            //                                    Level = sysMenu.Level,
            //                                    OrderIndex = sysMenu.OrderIndex,
            //                                    Tips = sysMenu.Tips,
            //                                    Url = sysMenu.Url,
            //                                    Icon = sysMenu.Icon
            //                                }
            //             };
            // 使用include 无法使用条件判断
            //var dbData1 = CurrentDbContext.SysUsers.Where(c => c.UserId == user.UserId && c.Status == 1).Include(c => c.SysUserRoleRelations).ThenInclude(c => c.SysRole).FirstOrDefault();
            //var userCacheBo = dbData1.MapTo<UserCacheBo>();
            response.Succeed(await dbData.FirstOrDefaultAsync());
            return response;
        }
    }
}
