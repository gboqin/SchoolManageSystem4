using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolManageSystem.Basics.Extensions
{
    /// <summary>
    /// 序列化静态扩展
    /// </summary>
    public static class SerializerExtension
    {
        /// <summary>
        /// 转换成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str">序列字符串</param>
        /// <param name="isIgnoreNull">是否忽略空对象</param>
        /// <param name="isIgnoreEx">是否忽略序列异常</param>
        /// <returns></returns>
        public static T ToObject<T>(this string str, bool isIgnoreNull = true, bool isIgnoreEx = false)
        {
            var setting = new JsonSerializerSettings
            {
                NullValueHandling = isIgnoreNull ? NullValueHandling.Ignore : NullValueHandling.Include
            };
            try
            {
                if (string.IsNullOrEmpty(str))
                {
                    return default(T);
                }
                else if ("\"\"" == str)
                {
                    return default(T);
                }
                else
                {
                    return JsonConvert.DeserializeObject<T>(str, setting);
                }
            }
            catch (Exception)
            {
                if (!isIgnoreEx)
                    throw;
                return default(T);
            }
        }

        /// <summary>
        /// 转换成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static T ToObject<T>(this string str, JsonSerializerSettings settings)
        {
            try
            {
                if (string.IsNullOrEmpty(str))
                {
                    return default(T);
                }
                else if ("\"\"" == str)
                {
                    return default(T);
                }
                else
                {
                    return JsonConvert.DeserializeObject<T>(str, settings);
                }
            }
            catch (Exception)
            {

                return default(T);
            }
        }
    }
}
