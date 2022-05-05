using SchoolManageSystem.Entities;
using SchoolManageSystem.IRepositorys;
using SchoolManageSystem.Repositorys.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolManageSystem.Repositorys
{
    public class RoleRepository:Repository<SysRole>,IRoleRepository
    {
        public RoleRepository(SchoolDbContext dbContext) : base(dbContext) { }
    }
}
