using SchoolManageSystem.Basics.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolManageSystem.Basics.ResponseResults
{
    public class ResponseLayuiResult: ResponseLayuiResult<object>
    {

    }

    public class ResponseLayuiResult<T>
    {
        public ResponseLayuiResult<T> Fail(ResponseCode code, string msg, T data)
        {
            this.code = code;
            this.msg = msg;
            this.data = data;
            return this;
        }
        public ResponseLayuiResult<T> Succeed(T data, ResponseCode code = ResponseCode.Success, string msg = "successful")
        {
            this.code = 0;
            this.msg = "";
            this.data = data;
            return this;
        }
        public T data { get; set; }
        public ResponseCode code { get; set; }
        public int count { get; set; }
        public string msg { get; set; }
        public bool success => code == 0;//自定义成功状态码为200
    }
}
