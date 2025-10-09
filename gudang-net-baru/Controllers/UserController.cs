using gudang_net_baru.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Threading.Tasks;

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
        public async Task<IActionResult> EditAsync(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            var userRole = await userManager.GetRolesAsync(user);

            if(user == null)
            {
                return RedirectToAction("Index");
            }
            var roles = await roleManager.Roles
                .Select(r => new RoleDto { Id = r.Id, Name = r.Name })
                .ToListAsync();

            ViewBag.Roles = roles;
            var vm = new UserDto()
            {
                LastName = user.LastName,
                FirstName = user.FirstName,
                Email = user.Email,
                UserName = user.UserName,
                Selected = userRole.ToList()
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(string id, UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                var roles = await roleManager.Roles
                .Select(r => new RoleDto { Id = r.Id, Name = r.Name })
                .ToListAsync();

                ViewBag.Roles = roles;
                return View(userDto);
            }
            var user = await userManager.FindByIdAsync(id);
            if(user == null)
            {
                return RedirectToAction("Index");
            }

            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.Email = userDto.Email;
            user.UserName = userDto.UserName;

            // SelectedRoleIds bisa null kalau tidak ada yang dicentang
            var selectedRoleIds = userDto.SelectedRoleIds ?? new List<string>();

            // Ambil nama role dari ID yang dipilih
            var selectedRoleNames = await roleManager.Roles
                .Where(r => selectedRoleIds.Contains(r.Id))
                .Select(r => r.Name)
                .ToListAsync();

            // Role current (nama)
            var currentRoleNames = await userManager.GetRolesAsync(user);

            // Hitung diff
            var toAdd = selectedRoleNames.Except(currentRoleNames).ToArray();
            var toRemove = currentRoleNames.Except(selectedRoleNames).ToArray();

            // Apply changes
            if (toAdd.Length > 0)
            {
                var addRes = await userManager.AddToRolesAsync(user, toAdd);
                if (!addRes.Succeeded)
                {
                    foreach (var e in addRes.Errors) ModelState.AddModelError("", e.Description);
                    // re-render view bila perlu (pastikan isi ulang list roles di VM)
                    // return View(userDto);
                }
            }

            if (toRemove.Length > 0)
            {
                var remRes = await userManager.RemoveFromRolesAsync(user, toRemove);
                if (!remRes.Succeeded)
                {
                    foreach (var e in remRes.Errors) ModelState.AddModelError("", e.Description);
                    // return View(userDto);
                }
            }

            // Update user profile lainnya
            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var e in result.Errors) ModelState.AddModelError("", e.Description);
                // return View(userDto);
            }

            // Refresh auth (sekali saja, di luar loop)
            await signInManager.RefreshSignInAsync(user);

            // Selesai
            return RedirectToAction(nameof(Index));
        }
    }
}
