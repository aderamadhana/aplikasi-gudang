using gudang_net_baru.Models;
using gudang_net_baru.Models.Konfigurasi.Menu;
using gudang_net_baru.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace gudang_net_baru.Controllers
{
    public class MenuController : Controller
    {
        private readonly IActionDescriptorCollectionProvider adp;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ApplicationDbContext context;
        public MenuController(IActionDescriptorCollectionProvider adp, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            this.adp = adp;
            this.roleManager = roleManager;
            this.context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CreateAsync()
        {
            var list_controller = adp.ActionDescriptors.Items
               .OfType<ControllerActionDescriptor>()
               .Select(d => d.ControllerTypeInfo.Name.Replace("Controller", ""))
               .Distinct()
               .OrderBy(n => n)
               .ToList();

            var list_role = await roleManager.Roles
                .Select(r => new RoleDto { Id = r.Id, Name = r.Name })
                .OrderBy(r => r.Name)
                .ToListAsync();

            var list_parent = context.Menu
                .Where(m => m.ParentId == null && m.Status == true && m.MenuType == "menu").ToList();

            ViewBag.ListController = list_controller;
            ViewBag.ListRole = list_role;
            ViewBag.ListParent = list_parent;

            return View();
        }
        [HttpPost]
        public IActionResult Create(MenuDto menuDto)
        {
            if (!ModelState.IsValid)
            {
                return View(menuDto);
            }

            return RedirectToAction("Index");
        }
    }
}
