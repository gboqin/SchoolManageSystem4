using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchoolManageSystem.Common.Controllers;
using SchoolManageSystem.Dto.CusEntity;
using SchoolManageSystem.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolManageSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : BaseUserController
    {
        private readonly ILogger<MenuController> _logger;
        private IMenuService _menuService { get; set; }

        public MenuController(ILogger<MenuController> logger, IMenuService menuService)
        {
            _logger = logger;
            _menuService = menuService;
        }

        [HttpGet, Route("TopParentMenus")]
        public async Task<ActionResult> GetTopParentMenus()
        {
            var result = await _menuService.GetTopParentMenus();
            return MyJson(result);
        }

        [HttpPost, Route("MenusByParent")]
        public async Task<ActionResult> GetMenusByParent(string parentId)
        {
            var result = await _menuService.GetMenusByParent(string.IsNullOrEmpty(parentId) ? 0 : long.Parse(parentId));
            return MyJson(result);
        }

        [HttpGet, Route("NavTreeMenus")]
        public async Task<ActionResult> GetNavTreeMenus()
        {
            var result = await _menuService.GetNavTreeMenus();
            return MyJson(result);
        }

        [HttpGet, Route("AllMenus")]
        public async Task<IActionResult> GetAllMenus()
        {
            var result = await _menuService.GetAllMenuList();
            return MyJson(result);
        }

        [HttpGet, Route("AllTestMenus")]
        public async Task<IActionResult> GetAllTestMenus()
        {
            var result = await _menuService.GetAllTestMenuList();
            return MyJson(result);
        }

        [HttpGet, Route("TreeSelectMenus")]
        public async Task<IActionResult> GetTreeSelectMenus()
        {
            var result = await _menuService.GetTreeSelectMenus();
            return MyJson(result);
        }

        [HttpPost, Route("AddMenu")]
        public async Task<IActionResult> AddMenu(TestMenuDto menu)
        {
            var result = await _menuService.AddMenu(menu);
            return MyJson(result);
        }

        [HttpPost, Route("EdtMenu")]
        public async Task<IActionResult> EdtRole(TestMenuDto menu)
        {
            var result = await _menuService.EdtMenu(menu);
            return MyJson(result);
        }

        [HttpPost, Route("DelMenu")]
        public async Task<IActionResult> DelMenu(TestMenuDto menu)
        {
            var result = await _menuService.DelMenu(menu);
            return MyJson(result);
        }
    }
}
