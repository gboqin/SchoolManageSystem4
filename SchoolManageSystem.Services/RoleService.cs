using AutoMapper;
using SchoolManageSystem.Basics.ResponseResults;
using SchoolManageSystem.Dto.CusEntity;
using SchoolManageSystem.Entities;
using SchoolManageSystem.IRepositorys;
using SchoolManageSystem.IServices;
using SchoolManageSystem.Repositorys.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManageSystem.Services
{
    public class RoleService : BaseService, IRoleService
    {
        public IRoleRepository _roleRepository { get; set; }
        public IRoleMenuRepository _roleMenuRepository { get; set; }
        public RoleService(IRoleRepository roleRepository, IRoleMenuRepository roleMenuRepository,
            IUnitOfWork<SchoolDbContext> unitOfWork, IMapper mapper) : base(unitOfWork, mapper) {
            _roleRepository = roleRepository;
            _roleMenuRepository = roleMenuRepository;
        }

        public async Task<ResponseResult<List<RoleDto>>> GetAllRoles()
        {
            var result = new ResponseResult<List<RoleDto>>();
            var list = await _roleRepository.GetAllAsync(r => r.Deleted == 0);
            return result.Succeed(_mapper.Map<List<RoleDto>>(list));
        }

        public async Task<ResponseResult<List<RoleDto>>> GetAllRolesByName(string roleName)
        {
            var result = new ResponseResult<List<RoleDto>>();
            IList<SysRole> list = new List<SysRole>();
            if (string.IsNullOrWhiteSpace(roleName))
            {
                list = await _roleRepository.GetAllAsync(r => r.Deleted == 0);
            }
            else
            {
                list = await _roleRepository.GetAllAsync(r => r.Deleted == 0 && r.Name.Contains(roleName));
            }

            return result.Succeed(_mapper.Map<List<RoleDto>>(list));
        }

        public async Task<ResponseResult<List<long>>> GetMenuIdsByRoleId(long roleId)
        {
            var result = new ResponseResult<List<long>>();
            var ids = await _roleMenuRepository.GetMenuIdsByRoleId(roleId);
            return result.Succeed(ids);
        }

        public async Task<ResponseResult<RoleDto>> AddRole(RoleDto role)
        {
            var result = new ResponseResult<RoleDto>();
            var model = await _roleRepository.InsertAsync(_mapper.Map<SysRole>(role));
            await _unitOfWork.SaveChangesAsync();
            return result.Succeed(_mapper.Map<RoleDto>(model));
        }

        public async Task<ResponseResult<RoleDto>> EditRole(RoleDto role)
        {
            var result = new ResponseResult<RoleDto>();
            _roleRepository.Update(_mapper.Map<SysRole>(role));
            await _unitOfWork.SaveChangesAsync();
            return result.Succeed(role);
        }

        public async Task<ResponseResult<RoleDto>> DeleteRole(RoleDto role)
        {
            var result = new ResponseResult<RoleDto>();
            using (var tran = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var rms = _roleMenuRepository.GetAll(rm => rm.RoleId == role.Id).ToList();
                    if (rms.Count() > 0)
                        _roleMenuRepository.Delete(rms);
                    _roleRepository.Delete(_mapper.Map<SysRole>(role));
                    await _unitOfWork.SaveChangesAsync();
                    await tran.CommitAsync();
                    return result.Succeed(role);
                }
                catch (Exception e)
                {
                    tran.Rollback();
                    return result.Fail(Basics.Enums.ResponseCode.DbEx, e.Message, role);
                }
            }

        }

        public async Task<int> AssignPermissions(long roleId, List<long> firstMenuIds, List<long> setMenuIds)
        {
            await _roleMenuRepository.SetRoleMenuRelation(roleId, firstMenuIds, setMenuIds);
            var result = await _unitOfWork.SaveChangesAsync();
            return result;
        }
    }
}
