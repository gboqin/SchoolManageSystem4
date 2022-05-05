using SchoolManageSystem.Entities;
using SchoolManageSystem.IRepositorys;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolManageSystem.Repositorys.Database
{
    /// <summary>
    /// 初始化种子数据
    /// </summary>
    public static partial class SeedData
    {

        /// <summary>
        /// 数据初始化
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <returns>返回是否创建了数据库（非迁移）</returns>
        public static bool Initialize(IUnitOfWork<SchoolDbContext> unitOfWork)
        {
            bool isCreateDb = false;
            //直接自动执行迁移,如果它创建了数据库，则返回true
            if (unitOfWork.DbContext.Database.EnsureCreated())
            {
                isCreateDb = true;
                //打印log-创建数据库及初始化期初数据

                #region 租户

                unitOfWork.GetRepository<SysTenant>().Insert(InitTenants);

                #endregion 租户

                #region 用户

                unitOfWork.GetRepository<SysUser>().Insert(InitUsers);

                #endregion

                #region 角色

                unitOfWork.GetRepository<SysRole>().Insert(InitRoles);

                #endregion

                #region 角色用户关系

                unitOfWork.GetRepository<SysUserRoleRelation>().Insert(InitUserRoleRelations);

                #endregion

                #region 菜单初始数据

                unitOfWork.GetRepository<SysMenu>().Insert(InitMenus);

                #endregion

                #region 菜单用户关系

                unitOfWork.GetRepository<SysRoleMenuRelation>().Insert(InitRoleMenuRelations);

                #endregion

                unitOfWork.SaveChanges();
            }
            return isCreateDb;
        }
    }
}
