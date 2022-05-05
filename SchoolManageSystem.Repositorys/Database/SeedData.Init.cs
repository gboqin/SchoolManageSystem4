using SchoolManageSystem.Basics.Enums;
using SchoolManageSystem.Basics.Security;
using SchoolManageSystem.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolManageSystem.Repositorys.Database
{
    public static partial class SeedData
    {
        /// <summary>
        /// 初始租户信息
        /// </summary>
        public static List<SysTenant> InitTenants => new List<SysTenant>()
        {
            new SysTenant()
            {
                TenantName="系统管理员",
                Remark="系统管理员",
                State = EnumState.Enabled
            },
            new SysTenant()
            {
                TenantName="管理员",
                Remark="用户",
                State = EnumState.Enabled
            }
        };

        /// <summary>
        /// 初始用户
        /// </summary>
        public static List<SysUser> InitUsers => new List<SysUser>()
        {
            new SysUser()
            {
                Account="admin",
                Name = "admin",
                Password = "admin".Md5Encrypt(),
                //Password = Crypto.HashPassword("admin"),//默认密码同账号名
                Email = "levywang123@gmail.com",
                Sex = EnumSex.Men,
                State = EnumState.Enabled,
                TenantId = 1
            },
            new SysUser()
            {
                Account = "admin1",
                Name = "admin1",
                Password = "admin".Md5Encrypt(),
                //Password = Crypto.HashPassword("admin"),
                Email = "levy_wang@qq.com",
                Sex = EnumSex.Men,
                State = EnumState.Enabled,
                TenantId = 2
            }
        };
        /// <summary>
        /// 初始角色
        /// </summary>
        public static List<SysRole> InitRoles => new List<SysRole>()
        {
            new SysRole()
            {
                TenantId = 1,
                Name = "Admin",
                RoleDesc = "系统创建"
            },
            new SysRole()
            {
                TenantId = 2,
                Name = "System",
                RoleDesc = "系统创建"
            }
        };
        /// <summary>
        /// 初始用户角色关系
        /// </summary>
        public static List<SysUserRoleRelation> InitUserRoleRelations => new List<SysUserRoleRelation>()
        {
            new SysUserRoleRelation()
            {
                RoleId = 1,
                UserId = 1,
                TenantId = 1,
                State = EnumState.Enabled
            },
            new SysUserRoleRelation()
            {
                RoleId = 2,
                UserId = 2,
                TenantId = 2,
                State = EnumState.Enabled
            }
        };
        /// <summary>
        /// 初始按钮
        /// </summary>
        public static List<SysMenu> InitMenus => new List<SysMenu>()
        {
            new SysMenu()
            {
                PermissionCode="M1",
                Name = "系统管理",
                Level = 1,
                Url = string.Empty,
                Type = EnumMenuType.Menu,
                Icon = "el-icon-setting",
                OrderIndex = 1
            },
            new SysMenu()
            {
                PermissionCode = "M2",
                Name = "一级菜单",
                Level = 1,
                Url = "menu/singleMenu/index",
                Type = EnumMenuType.Menu,
                Icon = "el-icon-wind-power",
                OrderIndex = 2
            },
            new SysMenu()
            {
                PermissionCode = "M3",
                Name = "二级菜单",
                Level = 1,
                Url = string.Empty,
                Type = EnumMenuType.Menu,
                Icon = "el-icon-ice-cream-round",
                OrderIndex = 3,
                CreatedTime = DateTime.Now,
            },
            new SysMenu()
            {
                PermissionCode = "M4",
                Name = "三级多级菜单",
                Level = 1,
                Url = string.Empty,
                Type = EnumMenuType.Menu,
                Icon = "el-icon-ice-cream-round",
                OrderIndex = 4
            },
            new SysMenu()
            {
                PermissionCode = "M5",
                Name = "任务调度",
                Level = 1,
                Url = string.Empty,
                Type = EnumMenuType.Menu,
                Icon = "lion-icon-renwutiaodu",
                OrderIndex = 5
            },
            new SysMenu()
            {
                PermissionCode = "M101",
                Name = "角色管理",
                ParentId = 1,
                Level = 2,
                Url = "home/systemManage/roleManage",
                Type = EnumMenuType.Menu,
                Icon = "el-icon-user",
                OrderIndex = 1
            },
            new SysMenu()
            {
                PermissionCode = "B101",
                Name = "用户管理",
                ParentId = 1,
                Level = 2,
                Url = "home/systemManage/userManage",
                Type = EnumMenuType.Menu,
                Icon = "el-icon-user",
                OrderIndex = 2
            },
            new SysMenu()
            {
                PermissionCode = "M102",
                Name = "菜单管理",
                ParentId = 1,
                Level = 2,
                Url = "home/systemManage/menuManage",
                Type = EnumMenuType.Menu,
                Icon = "el-icon-menu",
                OrderIndex = 3
            },
            new SysMenu()
            {
                PermissionCode = "M101_R_ADD",
                Name = "角色-添加",
                ParentId = 6,
                Level = 1,
                Url = "r_add",
                Type = EnumMenuType.Button,
                Icon = string.Empty,
                OrderIndex = 1
            },
            new SysMenu()
            {
                PermissionCode = "B101_R_RELATION",
                Name = "角色-关联角色",
                ParentId = 6,
                Level = 1,
                Url = "r_relation",
                Type = EnumMenuType.Button,
                Icon = string.Empty,
                OrderIndex = 2
            },
            new SysMenu()
            {
                PermissionCode = "B101_R_PERMS_CONFIG",
                Name = "角色-配置权限",
                ParentId = 6,
                Level = 1,
                Url = "r_perms_config",
                Type = EnumMenuType.Button,
                Icon = string.Empty,
                OrderIndex = 3
            },
            new SysMenu()
            {
                PermissionCode = "B101_R_DELETE",
                Name = "角色-删除",
                ParentId = 6,
                Level = 1,
                Url = "r_delete",
                Type = EnumMenuType.Button,
                Icon = string.Empty,
                OrderIndex = 4
            },
            new SysMenu()
            {
                PermissionCode = "B101_R_EDIT",
                Name = "角色-编辑",
                ParentId = 6,
                Level = 1,
                Url = "r_edit",
                Type = EnumMenuType.Button,
                Icon = string.Empty,
                OrderIndex = 5
            },
            new SysMenu()
            {
                PermissionCode = "B101_U_ADD",
                Name = "用户-增加",
                ParentId = 7,
                Level = 1,
                Url = "u_add",
                Type = EnumMenuType.Button,
                Icon = string.Empty,
                OrderIndex = 1
            },

            new SysMenu()
            {
                PermissionCode = "B101_U_DELETE",
                Name = "用户-删除",
                ParentId = 7,
                Level = 1,
                Url = "u_delete",
                Type = EnumMenuType.Button,
                Icon = string.Empty,
                OrderIndex = 2
            },
            new SysMenu()
            {
                PermissionCode = "B101_U_EDIT",
                Name = "用户-编辑",
                ParentId = 7,
                Level = 1,
                Url = "u_edit",
                Type = EnumMenuType.Button,
                Icon = string.Empty,
                OrderIndex = 0,
                CreatedTime = DateTime.Now,
            },

            new SysMenu()
            {
                PermissionCode = "M301",
                Name = "二级1-1",
                ParentId = 3,
                Level = 2,
                Url = "menu/secondMenu/second1-1",
                Type = EnumMenuType.Menu,
                Icon = "el-icon-milk-tea",
                OrderIndex = 1
            },
            new SysMenu()
            {
                PermissionCode = "M302",
                Name = "二级1-2",
                ParentId = 3,
                Level = 2,
                Url = "menu/secondMenu/second1-2",
                Type = EnumMenuType.Menu,
                Icon = "el-icon-potato-strips",
                OrderIndex = 2
            },
            new SysMenu()
            {
                PermissionCode = "M303",
                Name = "二级1-3",
                ParentId = 3,
                Level = 2,
                Url = "menu/secondMenu/second1-3",
                Type = EnumMenuType.Menu,
                Icon = "el-icon-lollipop",
                OrderIndex = 3
            },
            new SysMenu()
            {
                PermissionCode = "M401",
                Name = "三级1-1",
                ParentId = 4,
                Level = 2,
                Url = "menu/threeMenu/three1-1",
                Type = EnumMenuType.Menu,
                Icon = "el-icon-milk-tea",
                OrderIndex = 1
            },
            new SysMenu()
            {
                PermissionCode = "M402",
                Name = "三级1-2",
                ParentId = 4,
                Level = 2,
                Url = string.Empty,
                Type = EnumMenuType.Menu,
                Icon = "el-icon-potato-strips",
                OrderIndex = 2
            },
            new SysMenu()
            {
                PermissionCode = "M40201",
                Name = "三级1-2-1",
                ParentId = 21,
                Level = 3,
                Url = "menu/threeMenu/nextMenu/three1-2-1",
                Type = EnumMenuType.Menu,
                Icon = "el-icon-milk-tea",
                OrderIndex = 1
            },
            new SysMenu()
            {
                PermissionCode = "M40202",
                Name = "三级1-2-2",
                ParentId = 21,
                Level = 3,
                Url = "menu/threeMenu/nextMenu/three1-2-2",
                Type = EnumMenuType.Menu,
                Icon = "el-icon-potato-strips",
                OrderIndex = 2
            },
            new SysMenu()
            {
                PermissionCode = "M501",
                Name = "任务管理",
                ParentId = 5,
                Level = 2,
                Url = "quartz/task",
                Type = EnumMenuType.Menu,
                Icon = "lion-icon-renwu",
                OrderIndex = 1
            },
            new SysMenu()
            {
                PermissionCode = "M502",
                Name = "任务日志",
                ParentId = 5,
                Level = 2,
                Url = "quartz/tasklog",
                Type = EnumMenuType.Menu,
                Icon = "lion-icon-rizhi",
                OrderIndex = 2
            }
        };
        /// <summary>
        /// 通用用户注册关系  注意修改
        /// </summary>
        public static List<SysRoleMenuRelation> InitNormalRoleMenuRelations = new List<SysRoleMenuRelation>();
        //非系统管理员不能管理菜单 
        /*public static List<string> InitNoPerms = new List<string>()
        {
            "M102","M5","M501","M502"
        };*/

        /// <summary>
        /// 初始化角色按钮关系
        /// </summary>
        public static List<SysRoleMenuRelation> InitRoleMenuRelations => new List<SysRoleMenuRelation>()
        {
             new SysRoleMenuRelation()
                    {
                        TenantId = 1,
                        MenuId = 1,
                        RoleId = 1,
                        CreatedTime = DateTime.Now,
                        State = EnumState.Enabled
                    },
             new SysRoleMenuRelation()
                    {
                        TenantId = 2,
                        MenuId = 2,
                        RoleId = 2,
                        CreatedTime = DateTime.Now,
                        State = EnumState.Enabled
                    }
        };
    }
}
