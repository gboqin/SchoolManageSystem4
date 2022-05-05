using SchoolManageSystem.Basics.ResponseResults;
using SchoolManageSystem.Dto.CusEntity;
using SchoolManageSystem.Dto.RequestParam;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManageSystem.IServices
{
    public interface IUserService : IBaseService
    {
        Task<ResponseResult<UserDto>> LoginAsync(LoginParam loginParam);
    }
}
