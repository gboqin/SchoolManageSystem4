using AutoMapper;
using SchoolManageSystem.Basics.Extensions;
using SchoolManageSystem.Basics.IDCode.Snowflake;
using SchoolManageSystem.Basics.ResponseResults;
using SchoolManageSystem.Common;
using SchoolManageSystem.Common.Cache.Menory;
using SchoolManageSystem.Common.Cache.Redis;
using SchoolManageSystem.Common.JWT;
using SchoolManageSystem.Dto.CusEntity;
using SchoolManageSystem.Dto.RequestParam;
using SchoolManageSystem.Dto.SystemBo;
using SchoolManageSystem.IRepositorys;
using SchoolManageSystem.IServices;
using SchoolManageSystem.Repositorys.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManageSystem.Services
{
    /// <summary>
    /// 用户相关业务层
    /// </summary>
    public class UserService : BaseService, IUserService
    {
        public IUserRepository _userRepository { get; set; }
        public ISystemRepository _systemRepository { get; set; }
        public RedisClient redisClient { get; set; }
        public MemoryCache memoryCache { get; set; }
        public IdWorker IdWorker { get; set; }

        public UserService(IUserRepository userRepository, ISystemRepository systemRepository,
            IUnitOfWork<SchoolDbContext> unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _userRepository = userRepository;
            _systemRepository = systemRepository;
        }
        /// <summary>
        /// 用户登录 业务层
        /// </summary>
        /// <param name="loginParam"></param>
        /// <returns></returns>
        public async Task<ResponseResult<UserDto>> LoginAsync(LoginParam loginParam)
        {
            var result = new ResponseResult<UserDto>();
            /*var verificationResult = await _systemRepository.VerificationLoginAsync(loginParam);
            if (verificationResult != "验证通过")
            {
                result.Fail(ResponseCode.LoginFail, verificationResult, null);
                return result;
            }*/

            var responseResult = await _userRepository.LoginAsync(loginParam);
            if (responseResult.Success)
            {
                var userCache = responseResult.Data;
                CreateTokenCache(userCache);
                //var userDto = userCache.MapTo<UserDto>();
                var userDto = _mapper.Map<UserDto>(userCache);
                result.Succeed(userDto);
            }
            result.Code = responseResult.Code;
            result.Message = responseResult.Message;
            return result;
        }

        /// <summary>
        /// 重新生成缓存和token
        /// </summary>
        /// <param name="userCache"></param>
        private static void CreateTokenCache(UserCacheBo userCache)
        {
            // 将某些信息存到token的payload中，此处是放的内存缓存信息  可替换成其它的
            // aes加密的原因是避免直接被base64反编码看见
            //var encryptMsg = userCache.ToJson().EncryptAES();
            userCache.SessionId = Guid.NewGuid().ToString("N");
            var dic = new Dictionary<string, string>()
            {
                { "uid", userCache.Id.ToString() },
                { "sessionId", userCache.SessionId } //一个账号仅允许在一个地方登录
            };
            var token = TokenManager.GenerateToken(dic.ToJson(), 3 * 24);
            GlobalSystemWeb.HttpContext.Response.Headers["token"] = token;
            GlobalSystemWeb.HttpContext.Response.Headers["uid"] = userCache.Id.ToString();
            GlobalSystemWeb.HttpContext.Response.Headers["sessionId"] = userCache.SessionId.ToString();
            userCache.UserToken = token;
            GlobalSystemWeb.HttpContext.Response.Headers["Access-Control-Expose-Headers"] = "token,uid"; //多个以逗号分隔 eg:token,sid

            UserManager.CurrentUser = userCache;
        }
    }
}
