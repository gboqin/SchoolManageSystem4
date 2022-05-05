using Microsoft.AspNetCore.Mvc;
using SchoolManageSystem.Basics.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolManageSystem.Basics.ResponseResults
{
    /// <summary>
    /// 自定义返回Json数据,返回给前端的Json结果数据类
    /// </summary>
    public class CustomJsonResult : ActionResult
    {
        public object Data { get; set; }

        public string DateTimeFormat { get; set; } = "yyyy-MM-dd HH:mm:ss";


        public override void ExecuteResult(ActionContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            var response = context.HttpContext.Response;
            response.ContentType = "application/json";
            if (Data == null) return;
            if (string.IsNullOrEmpty(DateTimeFormat))
            {
                DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            }
            string json;
#if DEBUG
            json = Data.ToJson(true, true, true, DateTimeFormat);//方便调式
#else
            json = Data.ToJson(true, false, true, DateTimeFormat);
#endif
            byte[] data = Encoding.UTF8.GetBytes(json);
            //response.Body.Write(data, 0, data.Length); 报错改为
            response.Body.WriteAsync(data, 0, data.Length);
        }
    }
}
