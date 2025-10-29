using gudang_net_baru.Models;
using gudang_net_baru.Models.Konfigurasi.Menu;
using gudang_net_baru.Models.Master.Lokasi;
using gudang_net_baru.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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

        [HttpPost]
        public IActionResult getMenu(bool status)
        {

            // DataTables params
            var draw = Request.Form["draw"].FirstOrDefault();
            int start = int.Parse(Request.Form["start"].FirstOrDefault() ?? "0");
            int length = int.Parse(Request.Form["length"].FirstOrDefault() ?? "10");
            string search = Request.Form["search[value]"].FirstOrDefault() ?? "";

            // permanent constraints for this endpoint
            var baseQuery = context.Menu
                .Where(r => r.Status == status);

            // total BEFORE user search, AFTER permanent constraints
            var recordsTotal = baseQuery.Count();

            // apply search
            var filteredQuery = baseQuery;
            if (!string.IsNullOrWhiteSpace(search))
                filteredQuery = filteredQuery.Where(r =>
                    r.MenuName.Contains(search) || r.MenuType.Contains(search));

            var recordsFiltered = filteredQuery.Count();

            // page
            var data = filteredQuery.OrderBy(r => r.Urutan)
                        .Skip(start)
                        .Take(length)
                        .Select(r => new {
                            r.IdMenu,
                            r.MenuName,
                            r.MenuType,
                            r.Urutan,
                            r.Status
                        })
                        .ToList();

            return Json(new
            {
                draw,
                recordsTotal,
                recordsFiltered,
                data
            });
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

            var list_parent = await context.Menu
                .Where(m => m.MenuType == "menu" && m.ParentId == null && m.Status == true)
                .Select(m => new {
                    m.IdMenu,
                    m.MenuName,
                    m.MenuType,
                    m.ParentId,
                    m.Status
                })
                .ToListAsync();

            ViewBag.ListController = list_controller;
            ViewBag.ListRole = list_role;
            ViewBag.ListParent = list_parent;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(MenuDto menuDto)
        {
            if (!ModelState.IsValid)
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

                var list_parent = await context.Menu
                    .Where(m => m.MenuType == "menu" && m.ParentId == null && m.Status == true)
                    .Select(m => new {
                        m.IdMenu,
                        m.MenuName,
                        m.MenuType,
                        m.ParentId,
                        Status = m.Status ?? false
                    })
                    .ToListAsync();

                ViewBag.ListController = list_controller;
                ViewBag.ListRole = list_role;
                ViewBag.ListParent = list_parent;
                return View(menuDto);
            }

            var menu = new MenuEntity()
            {
                IdMenu = Guid.NewGuid().ToString("N"),
                MenuName = menuDto.MenuName,
                MenuType = menuDto.MenuType,
                RoleId = menuDto.RoleId,
                Urutan = menuDto.Urutan,
                MenuIcon = menuDto.MenuIcon,
                ControllerName = menuDto.ControllerName,
                ControllerFunction = menuDto.ControllerFunction,
                ParentId = menuDto.ParentId,
                Status = true,
                CreatedAt = DateTime.Now,
                CreatedBy = HttpContext.Session.GetString("UserId")
            };

            context.Menu.Add(menu);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Ordering()
        {
            var parentsWithChildren = await context.Menu
                .Where(m => m.MenuType == "menu" && m.ParentId == null && m.Status == true)
                .Select(m => new {
                    m.IdMenu,
                    m.MenuName,
                    m.MenuType,
                    m.ParentId,
                    m.Urutan,
                    Status = m.Status ?? false,
                    Children = context.Menu
                        .Where(c => c.ParentId == m.IdMenu && c.Status == true && c.MenuType == "menu")
                        .Select(c => new {
                            c.IdMenu,
                            c.MenuName,
                            c.MenuType,
                            c.ParentId,
                            c.Urutan,
                            Status = c.Status ?? false
                        })
                        .OrderBy(c => c.Urutan)
                        .ToList()
                })
                .OrderBy(m => m.Urutan)
                .ToListAsync();

            ViewBag.Menu = parentsWithChildren;
            
            return View();
        }

        public async Task<IActionResult> EditAsync(string id)
        {
            var menu = await context.Menu.AsNoTracking()
                .Where(m => m.IdMenu == id)
                .Select(m => new { m.IdMenu, m.MenuIcon, m.RoleId, m.Urutan, m.MenuName, m.MenuType,
                    ControllerName = m.ControllerName ?? "",
                    ControllerFunction = m.ControllerFunction ?? "",
                    ParentId = m.ParentId ?? "",
                })
                .FirstOrDefaultAsync();

            if (menu == null)
            {
                return RedirectToAction("Index");
            }
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

            var list_parent = await context.Menu
                .Where(m => m.MenuType == "menu" && m.ParentId == null && m.Status == true)
                .Select(m => new {
                    m.IdMenu,
                    m.MenuName,
                    m.MenuType,
                    m.ParentId,
                    m.Status
                })
                .ToListAsync();

            var menu_dto = new MenuDto()
            {
                MenuType = menu?.MenuType,
                RoleId = menu?.RoleId,
                MenuName = menu?.MenuName,
                MenuIcon = menu?.MenuIcon,
                ControllerName = menu?.ControllerName,
                ControllerFunction = menu?.ControllerFunction,
                ParentId = menu?.ParentId,
                Urutan = menu?.Urutan ?? 0
            };

            ViewData["Id"] = menu?.IdMenu;

            ViewBag.ListController = list_controller;
            ViewBag.ListRole = list_role;
            ViewBag.ListParent = list_parent;
            return View(menu_dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, MenuDto menuDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new { success = false, errors });
            }

            var menu = await context.Menu.FirstOrDefaultAsync(m => m.IdMenu == id);

            if (menu == null)
            {
                return RedirectToAction("Index");
            }


            menu.MenuName = menuDto.MenuName;
            menu.MenuType = menuDto.MenuType;
            menu.RoleId = menuDto.RoleId;
            menu.Urutan = menuDto.Urutan;
            menu.MenuIcon = menuDto.MenuIcon;
            menu.ControllerName = menuDto.ControllerName;
            menu.ControllerFunction = menuDto.ControllerFunction;
            menu.ParentId = menuDto.ParentId;
            menu.Status = true;
            menu.CreatedAt = DateTime.Now;
            menu.CreatedBy = HttpContext.Session.GetString("UserId");

            context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
