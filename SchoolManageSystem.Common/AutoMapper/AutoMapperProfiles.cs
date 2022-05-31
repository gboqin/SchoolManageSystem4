using AutoMapper;
using SchoolManageSystem.Dto.CusEntity;
using SchoolManageSystem.Dto.SystemBo;
using SchoolManageSystem.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolManageSystem.Common.AutoMapper
{
    public class AutoMapperProfiles : Profile
    {
        /// <summary>
        /// 配置构造函数，用来创建关系映射
        /// </summary>
        public AutoMapperProfiles()
        {
            #region user
            CreateMap<UserCacheBo, UserDto>();
            #endregion

            #region menu
            CreateMap<SysMenu, MenuDto>();
            CreateMap<MenuDto, SysMenu>();
            //CreateMap<SysMenu, TestMenuDto>().ForMember(m=> m.Id,o=>o.MapFrom(s=>s.Id.ToString()))
            //    .ForMember(m => m.ParentId, o => o.MapFrom(s => s.ParentId.ToString()));
            CreateMap<SysMenu, TestMenuDto>();
            CreateMap<TestMenuDto, SysMenu>();
            CreateMap<SysMenu, SelectTreeDto>();
            CreateMap<SelectTreeDto, SysMenu>();
            #endregion

            #region role
            CreateMap<SysRole, RoleDto>();
            CreateMap<RoleDto, SysRole > ();
            #endregion
            /*CreateMap<Post, PostResource>()
            .ForMember(dest => dest.LastModified, opt => opt.MapFrom(src => src.UpdatedTime.HasValue ? src.CreatedTime : src.UpdatedTime));
            CreateMap<PostResource, Post>();*/
            //CreateMap<Car, CarDto>();
            //CreateMap<Glass, GlassDto>();
            //CreateMap<SysTenant, TenantDto>();
        }
    }
}
