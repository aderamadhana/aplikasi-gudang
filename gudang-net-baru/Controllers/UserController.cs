using gudang_net_baru.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace gudang_net_baru.Controllers
{

    [Authorize(Roles = "ADMIN")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult getUser(bool status)
        {

            // DataTables params
            var draw = Request.Form["draw"].FirstOrDefault();
            int start = int.Parse(Request.Form["start"].FirstOrDefault() ?? "0");
            int length = int.Parse(Request.Form["length"].FirstOrDefault() ?? "10");
            string search = Request.Form["search[value]"].FirstOrDefault() ?? "";

            // permanent constraints for this endpoint
            var baseQuery = userManager.Users
                .Where(r => r.Status == status);

            // total BEFORE user search, AFTER permanent constraints
            var recordsTotal = baseQuery.Count();

            // apply search
            var filteredQuery = baseQuery;
            if (!string.IsNullOrWhiteSpace(search))
                filteredQuery = filteredQuery.Where(r =>
                    r.UserName.Contains(search) || r.Email.Contains(search));

            var recordsFiltered = filteredQuery.Count();

            // page
            var data = filteredQuery.OrderBy(r => r.Id)
                        .Skip(start)
                        .Take(length)
                        .Select(r => new {
                            r.Id,
                            r.UserName,
                            r.Email,
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

        public async Task<IActionResult> Create()
        {
            var roles = await roleManager.Roles
                .Select(r => new RoleDto { Id = r.Id, Name = r.Name })
                .ToListAsync();

            ViewBag.Roles = roles;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                var roles = await roleManager.Roles
                .Select(r => new RoleDto { Id = r.Id, Name = r.Name })
                .ToListAsync();

                ViewBag.Roles = roles;
                return View(userDto);
            }
            var user = new ApplicationUser()
            {
                LastName = userDto.LastName,
                FirstName = userDto.FirstName,
                UserName = userDto.UserName,
                Email = userDto.Email, 
                Status = true,
                CreatedBy = 1,
            };

            var result = await userManager.CreateAsync(user, userDto.Password);
            if (result.Succeeded)
            {

                foreach (var roleId in userDto.SelectedRoleIds)
                {
                    var role = await roleManager.FindByIdAsync(roleId);
                    if (role != null)
                    {
                        await userManager.AddToRoleAsync(user, role.Name);
                        await signInManager.SignInAsync(user, false);
                    }
                }
                return RedirectToAction("Index");
            }
            else
            {
                return View(userDto);
            }
        }

        public IActionResult Edit()
        {
            return View();
        }
    }
}
