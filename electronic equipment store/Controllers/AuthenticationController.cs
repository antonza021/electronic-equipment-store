using electronic_equipment_store.Models.DBModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using electronic_equipment_store.App_Data;

namespace electronic_equipment_store.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly AppDbContext _context;

        public AuthenticationController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(Users model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.customer_name == model.customer_name);

                if (user != null)
                {
                    var encpass = EncryptPassword(model.password!);
                    if (encpass == user.password)
                    {
                        var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.customer_name!),
                    new Claim(ClaimTypes.NameIdentifier, user.customer_id.ToString())
                };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var authProperties = new AuthenticationProperties
                        {
                        };

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                        return RedirectToAction("Index", "Home");
                    }
                }
                ViewBag.ErrorMessage = "Неверный логин или пароль";
            }

            return View(model);
        }


        public async Task<IActionResult> Exit()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        private string EncryptPassword(string password)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha512.ComputeHash(inputBytes);
                string hash = BitConverter.ToString(hashBytes);

                return hash;
            }
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registration(Users model)
        {

            if (ModelState.IsValid)
            {
                var existingAccount = _context.Users.FirstOrDefault(a => a.customer_name == model.customer_name);
                if (existingAccount != null)
                {
                    ViewBag.ErrorMessage = "Пользователь с таким логином уже существует.";
                    return View(model);
                }
                var account = new Users
                {
                    email = model.email,
                    role = "user",
                    address = "unknown",
                    customer_name = model.customer_name,
                    password = EncryptPassword(model.password!)
                };

                _context.Users.Add(account);
                _context.SaveChanges();

                return RedirectToAction("Login");
            }
            return View("Registration", model);
        }
    }
}
