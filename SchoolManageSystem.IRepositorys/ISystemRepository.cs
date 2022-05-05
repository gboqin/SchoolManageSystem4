using SchoolManageSystem.Basics.ResponseResults;
using SchoolManageSystem.Dto.RequestParam;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManageSystem.IRepositorys
{
    public interface ISystemRepository
    {
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        Task<CaptchaResult> GetCaptchaResultAsync();
        /// <summary>
        /// 验证登录时的验证码
        /// </summary>
        /// <param name="loginParam"></param>
        /// <returns></returns>
        Task<string> VerificationLoginAsync(LoginParam loginParam);
    }
}
