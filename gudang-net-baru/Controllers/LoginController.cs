using gudang_net_baru.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace gudang_net_baru.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public LoginController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
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
                if (roles.Any())
                {
                    HttpContext.Session.SetString("UserRole", roles.First());
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

            return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public IActionResult SetRoleSession(string role)
        {
            if (!string.IsNullOrEmpty(role))
            {
                HttpContext.Session.SetString("UserRole", role);
            }

            return Ok();
        }
    }
}
