﻿using AutoMapper;
using SchoolManageSystem.IRepositorys;
using SchoolManageSystem.IServices;
using SchoolManageSystem.Repositorys.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolManageSystem.Services
{
    public class BaseService : IBaseService
    {
        public readonly IUnitOfWork<SchoolDbContext> _unitOfWork;
        public readonly IMapper _mapper;

        public BaseService(IUnitOfWork<SchoolDbContext> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            //service层使用UserResponse model = _mapper.Map<UserResponse>(userinfo);
            //Api层 var userinfo = await _userService.GetUserDetails(id);
            _mapper = mapper;
        }

        /*
            public interface IRoleService : IBaseService
            {
                Task<ExecuteResult<Role>> Create(RoleViewModel viewModel);
                Task<ExecuteResult> Update(RoleViewModel viewModel);
                Task<ExecuteResult> Delete(RoleViewModel viewModel);
            } 
        

            public class RoleService : BaseService, IRoleService
            {
                public RoleService(IUnitOfWork<AuthorityDbContext> unitOfWork, IMapper mapper, IdWorker idWorker) : base(unitOfWork, mapper, idWorker)
                {
                }

                public async Task<ExecuteResult<Role>> Create(RoleViewModel viewModel)
                {
                    ExecuteResult<Role> result = new ExecuteResult<Role>();
                    //检查字段
                    if (viewModel.CheckField(ExecuteType.Create, _unitOfWork) is ExecuteResult checkResult && !checkResult.IsSucceed)
                    {
                        return result.SetFailMessage(checkResult.Message);
                    }
                    using (var tran = _unitOfWork.BeginTransaction())//开启一个事务
                    {
                        Role newRow = _mapper.Map<Role>(viewModel);
                        newRow.Id = _idWorker.NextId();//获取一个雪花Id
                        newRow.Creator = 1219490056771866624;//由于暂时还没有做登录，所以拿不到登录者信息，先随便写一个后面再完善
                        newRow.CreateTime = DateTime.Now;
                        _unitOfWork.GetRepository<Role>().Insert(newRow);
                        await _unitOfWork.SaveChangesAsync();
                        await tran.CommitAsync();//提交事务

                        result.SetData(newRow);//添加成功，把新的实体返回回去
                    }
                    return result;
                }

                public async Task<ExecuteResult> Delete(RoleViewModel viewModel)
                {
                    ExecuteResult result = new ExecuteResult();
                    //检查字段
                    if (viewModel.CheckField(ExecuteType.Delete, _unitOfWork) is ExecuteResult checkResult && !checkResult.IsSucceed)
                    {
                        return checkResult;
                    }
                    _unitOfWork.GetRepository<Role>().Delete(viewModel.Id);
                    await _unitOfWork.SaveChangesAsync();//提交
                    return result;
                }

                public async Task<ExecuteResult> Update(RoleViewModel viewModel)
                {
                    ExecuteResult result = new ExecuteResult();
                    //检查字段
                    if (viewModel.CheckField(ExecuteType.Update, _unitOfWork) is ExecuteResult checkResult && !checkResult.IsSucceed)
                    {
                        return checkResult;
                    }
                    //var date= _unitOfWork.GetRepository<Role>().GetPagedListAsync(r=>r.Id==0&&r.Name.Contains("R"), orderBy: source=> source.OrderBy(r=>r.Id).ThenByDescending(r=>r.Name));
                    //从数据库中取出该记录
                    var row = await _unitOfWork.GetRepository<Role>().FindAsync(viewModel.Id);//在viewModel.CheckField中已经获取了一次用于检查，所以此处不会重复再从数据库取一次，有缓存
                    //修改对应的值
                    row.Name = viewModel.Name;
                    row.DisplayName = viewModel.DisplayName;
                    row.Remark = viewModel.Remark;
                    row.Modifier = 1219490056771866624;//由于暂时还没有做登录，所以拿不到登录者信息，先随便写一个后面再完善
                    row.ModifyTime = DateTime.Now;
                    _unitOfWork.GetRepository<Role>().Update(row);
                    await _unitOfWork.SaveChangesAsync();//提交

                    return result;
                }
            }
        */
    }
}
