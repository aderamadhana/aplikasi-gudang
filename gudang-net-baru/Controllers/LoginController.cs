using gudang_net_baru.Models;
using gudang_net_baru.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Threading.Tasks;

namespace gudang_net_baru.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ApplicationDbContext context;
        private readonly RoleManager<IdentityRole> roleManager;

        public LoginController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
            this.roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(LoginDto loginDto)
        {
            if (!ModelState.IsValid) { 
                return View(loginDto);
            }

            var result = await signInManager.PasswordSignInAsync(userName: loginDto.Email,          // <-- note: userName, not "user"
                password: loginDto.Password,
                isPersistent: loginDto.RememberMe,
                lockoutOnFailure: false
             );
            if (result.Succeeded) {
                var user = await userManager.FindByEmailAsync(loginDto.Email);
                var roles = await userManager.GetRolesAsync(user);
                var roleIds = await roleManager.Roles
                    .Where(r => roles.Contains(r.Name))
                    .Select(r => r.Id)                 // string by default
                    .ToListAsync();


                var menu = await context.Menu
                    .Where(m => m.ParentId == null && m.Status == true && m.RoleId == roleIds.First())
                    .Select(m => new {
                        m.IdMenu,
                        m.MenuName,
                        m.MenuType,
                        m.ParentId,
                        m.MenuIcon,
                        m.Urutan,
                        ControllerName = m.ControllerName ?? "",
                        ControllerFunction = m.ControllerFunction ?? "",
                        Status = m.Status ?? false,
                        Children = context.Menu
                            .Where(c => c.ParentId == m.IdMenu && c.Status == true && c.MenuType == "menu" && c.RoleId == roleIds.First())
                            .Select(c => new {
                                c.IdMenu,
                                c.MenuName,
                                c.MenuType,
                                c.ParentId,
                                c.MenuIcon,
                                ControllerName = c.ControllerName ?? "",
                                ControllerFunction = c.ControllerFunction ?? "",
                                c.Urutan,
                                Status = c.Status ?? false
                            })
                            .OrderBy(c => c.Urutan)
                            .ToList()
                    })
                    .OrderBy(m => m.Urutan).ToListAsync();

                if (roles.Any())
                {
                    HttpContext.Session.SetString("UserRole", roles.First());
                    HttpContext.Session.SetString("UserId", user.Id);
                    HttpContext.Session.SetString("Menu", JsonSerializer.Serialize(menu));
                }

                return RedirectToAction("Index", "Home");
            }else
            {
                ViewBag.ErrorMessage = "Invalid login attempt";
            }
            
            return View(loginDto);
        }

        public async Task<IActionResult> Logout()
        {
            if (signInManager.IsSignedIn(User))
            {
                await signInManager.SignOutAsync();
            }

            HttpContext.Session.Clear();

            foreach (var cookie in HttpContext.Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }

            return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public async Task<IActionResult> SetRoleSession(string role)
        {
            if (!string.IsNullOrEmpty(role))
            {
                var roleIds = await roleManager.Roles
                    .Where(r => r.Name.Contains(role))
                    .Select(r => r.Id)                 // string by default
                    .ToListAsync();

                var menu = await context.Menu
                    .Where(m => m.ParentId == null && m.Status == true && m.RoleId == roleIds.First())
                    .Select(m => new {
                        m.IdMenu,
                        m.MenuName,
                        m.MenuType,
                        m.MenuIcon,
                        m.ParentId,
                        m.Urutan,
                        ControllerName = m.ControllerName ?? "",
                        ControllerFunction = m.ControllerFunction ?? "",
                        Status = m.Status ?? false,
                        Children = context.Menu
                            .Where(c => c.ParentId == m.IdMenu && c.Status == true && c.MenuType == "menu" && c.RoleId == roleIds.First())
                            .Select(c => new {
                                c.IdMenu,
                                c.MenuName,
                                c.MenuType,
                                c.ParentId,
                                c.MenuIcon,
                                ControllerName = c.ControllerName ?? "",
                                ControllerFunction = c.ControllerFunction ?? "",
                                c.Urutan,
                                Status = c.Status ?? false
                            })
                            .OrderBy(c => c.Urutan)
                            .ToList()
                    })
                    .OrderBy(m => m.Urutan).ToListAsync();

                HttpContext.Session.SetString("UserRole", role);
                HttpContext.Session.SetString("Menu", JsonSerializer.Serialize(menu));
            }

            return Ok();
        }
    }
}
