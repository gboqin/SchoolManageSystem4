using SchoolManageSystem.Basics.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolManageSystem.Basics.ResponseResults
{
    /// <inheritdoc />
    /// <summary>
    /// 响应返回体
    /// </summary>
    public class ResponseResult : ResponseResult<object>
    {
    }
    /// <summary>
    /// 响应返回体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResponseResult<T> : BaseResponseResult
    {
        public T Data { get; set; }

        public ResponseResult<T> Fail(ResponseCode code, string msg, T data)
        {
            Code = code;
            Message = msg;
            Data = data;
            return this;
        }

        public ResponseResult<T> Succeed(T data, ResponseCode code = ResponseCode.Success, string msg = "successful")
        {
            Code = code;
            Message = msg;
            Data = data;
            return this;
        }
    }
    public class BaseResponseResult
    {
        public ResponseCode Code { get; set; }

        public string Message { get; set; }

        public bool Success => Code == ResponseCode.Success;//自定义成功状态码为200
    }
}
