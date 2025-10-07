using gudang_net_baru.Models;
using gudang_net_baru.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;

namespace gudang_net_baru.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ApplicationDbContext context;
        public RoleController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager) { 
            this.roleManager = roleManager;
            this.context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult getRole()
        {

            // DataTables params
            var draw = Request.Form["draw"].FirstOrDefault();
            int start = int.Parse(Request.Form["start"].FirstOrDefault() ?? "0");
            int length = int.Parse(Request.Form["length"].FirstOrDefault() ?? "10");
            string search = Request.Form["search[value]"].FirstOrDefault() ?? "";

            // permanent constraints for this endpoint
            var baseQuery = roleManager.Roles;

            // total BEFORE user search, AFTER permanent constraints
            var recordsTotal = baseQuery.Count();

            // apply search
            var filteredQuery = baseQuery;
            if (!string.IsNullOrWhiteSpace(search))
                filteredQuery = filteredQuery.Where(r =>
                    r.Name.Contains(search) || r.NormalizedName.Contains(search));

            var recordsFiltered = filteredQuery.Count();

            // page
            var data = filteredQuery.OrderBy(r => r.Id)
                        .Skip(start)
                        .Take(length)
                        .Select(r => new {
                            r.Id,
                            r.Name,
                            r.NormalizedName
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

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(RoleDto roleDto)
        {
            if (!ModelState.IsValid)
            {
                return View(roleDto);
            }

            var role = new IdentityRole()
            {
                Name = roleDto.Name,
                NormalizedName = roleDto.Name,
            };

            context.Roles.Add(role);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest();

            var role = await roleManager.FindByIdAsync(id); // <- WAJIB pakai await
            if (role == null)
                return RedirectToAction("Index", "Role");

            var vm = new RoleDto
            {
                Name = role.Name,
                NormalizedName = role.NormalizedName
            };

            ViewData["Id"] = role.Id;
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> EditAsync(string id, RoleDto roleDto)
        {
            var role = await roleManager.FindByIdAsync(id); // <- WAJIB pakai await
            if (role == null)
                return RedirectToAction("Index", "Role");

            if (!ModelState.IsValid)
            {
                ViewData["Id"] = role.Id;
                return View(roleDto);
            }

            role.Name = roleDto.Name;
            role.NormalizedName = roleDto.Name;

            context.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
