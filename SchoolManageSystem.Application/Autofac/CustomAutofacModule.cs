using Autofac;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Module = Autofac.Module;

namespace SchoolManageSystem.Application.Autofac
{
    /// <summary>
    /// Autofac基层接口，用来注入
    /// </summary>
    public class CustomAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // InstancePerLifetimeScope 同一个 Lifetime 生成的对象是同一个实例
            // SingleInstance 单例模式，每次调用，都会使用同一个实例化的对象；每次都用同一个对象
            // InstancePerDependency 默认模式，每次调用，都会重新实例化对象；每次请求都创建一个新的对象

            //告诉autofac框架注册数据仓储层所在程序集中的所有类的对象实例
            Assembly repAss = Assembly.Load("SchoolManageSystem.Repositorys");
            //创建repAss中的所有类的instance以此类的实现接口存储
            builder.RegisterTypes(repAss.GetTypes()).AsImplementedInterfaces().InstancePerLifetimeScope();

            //告诉autofac框架注册业务逻辑层所在程序集中的所有类的对象实例
            Assembly serAss = Assembly.Load("SchoolManageSystem.Services");
            //创建serAss中的所有类的instance以此类的实现接口存储
            builder.RegisterTypes(serAss.GetTypes()).AsImplementedInterfaces().InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
