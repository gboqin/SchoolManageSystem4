using SchoolManageSystem.Basics.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolManageSystem.Basics.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// 自定义系统错误异常
    /// </summary>
    public class CustomSystemException : Exception
    {
        /// <summary>
        /// HTTP状态码
        /// </summary>
        public ResponseCode Code { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public object[] Args { get; set; }

        public CustomSystemException(string message, ResponseCode code, params object[] args) : base(message)
        {
            Code = code;
            Args = args;
        }
    }
}
