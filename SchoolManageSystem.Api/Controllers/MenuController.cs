using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchoolManageSystem.Common.Controllers;
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
    }
}
