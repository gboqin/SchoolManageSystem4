using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchoolManageSystem.Common.Controllers;
using SchoolManageSystem.Dto.CusEntity;
using SchoolManageSystem.Dto.RequestParam;
using SchoolManageSystem.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolManageSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : BaseUserController
    {
        private readonly ILogger<RoleController> _logger;

        private IRoleService _roleService { get; set; }
        private IMenuService _menuService { get; set; }

        public RoleController(ILogger<RoleController> logger, IRoleService roleService, IMenuService menuService)
        {
            _logger = logger;
            _roleService = roleService;
            _menuService = menuService;
        }

        [HttpPost, Route("RolesByName")]
        public async Task<IActionResult> GetRolesByName(string roleName)
        {
            var result = await _roleService.GetAllRolesByName(roleName);
            return MyJson(result);
        }

        [HttpPost, Route("AddRole")]
        public async Task<IActionResult> AddRole(RoleDto role)
        {
            var result = await _roleService.AddRole(role);
            return MyJson(result);
        }

        [HttpPost, Route("EditRole")]
        public async Task<IActionResult> EditRole(RoleDto role)
        {
            var result = await _roleService.EditRole(role);
            return MyJson(result);
        }

        [HttpPost, Route("DelRole")]
        public async Task<IActionResult> DelRole(RoleDto role)
        {
            var result = await _roleService.DeleteRole(role);
            return MyJson(result);
        }

        [HttpGet, Route("TreeMenus")]
        public async Task<IActionResult> GetTreeMenus()
        {
            var result = await _menuService.GetTreeMenus();
            return MyJson(result);
        }

        [HttpPost,Route("GetMenuIdsByRIds")]
        public async Task<IActionResult> GetMenuIdsByRoleIds(params long[] Ids)
        {
            var result = await _menuService.GetMenuIdsByRoleIds(Ids);
            return MyJson(result);
        }

        [HttpPost, Route("FirstMenuIdsByRoleId")]
        public async Task<IActionResult> GetFirstMenuIdsByRoleId(long roleId)
        {
            var result = await _roleService.GetMenuIdsByRoleId(roleId);
            return MyJson(result);
        }

        [HttpPost, Route("AssignPermissions")]
        public async Task<int> AssignPermissions(AssingParam assingParam)
        {
            var result = await _roleService.AssignPermissions(assingParam.RId, assingParam.RawIds.ToList(), assingParam.SetIds.ToList());
            return result;
        }
    }
}
