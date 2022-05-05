using SchoolManageSystem.Basics.ResponseResults;
using SchoolManageSystem.Dto.RequestParam;
using SchoolManageSystem.Dto.SystemBo;
using SchoolManageSystem.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManageSystem.IRepositorys
{
    public interface IUserRepository : IRepository<SysUser>
    {
        Task<ResponseResult<UserCacheBo>> LoginAsync(LoginParam loginParam);
    }
}
