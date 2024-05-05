using electronic_equipment_store.App_Data;
using electronic_equipment_store.Models.DBModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace electronic_equipment_store.Controllers
{
    public class MainController : Controller
    {
        private readonly AppDbContext _context;
        public MainController(AppDbContext context)
        {
            _context = context;
        }
        public void CheckRole()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _context.Users.FirstOrDefault(u => u.customer_id == Convert.ToInt32(userId));

            string? userRole = "";
            if (user != null)
            {
                userRole = user.role;
            }

            ViewBag.UserRole = userRole;
        }
        public IActionResult Index()
        {
            CheckRole();
            return View();
        }
        public async Task<IActionResult> Reviews()
        {
            CheckRole();
            var rew = await _context.Reviews.ToListAsync();
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = await _context.Users
                .Where(a => a.customer_id.ToString() == userId)
                .Select(a => a.role)
                .FirstOrDefaultAsync();
            if (userRole != null && userRole == "admin" || userRole == "owner")
            {
                return View(rew);
            }
            else if (userRole != null && userRole == "user")
            {
                var prodlist = _context.Products.Select(c => new SelectListItem
                {
                    Text = $"{c.product_name} ({c.stock_quantity} шт.)",
                    Value = c.product_id.ToString()
                }).ToList();
                prodlist.Insert(0, new SelectListItem { Text = "Выберите комплектующее", Value = "" });
                ViewBag.ComponentList = prodlist;
                return View("AddReviews", rew);
            }
            return RedirectToAction("AunthError", "Error");

        }
        [HttpPost]
        public async Task<IActionResult> AddReviews(IFormCollection form)
        {
            CheckRole();
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _context.Users.FirstOrDefaultAsync(a => a.customer_id == Convert.ToInt32(userId));
            var model = new Reviews
            {
                customer_id = user.customer_id,
                product_id = Convert.ToInt32(form["type"]),
                review_text = form["comment"],
                rating = Convert.ToInt32(form["rating"]),
            };

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Reviews.Add(model);
                    _context.SaveChanges();
                    return RedirectToAction("Reviews", "Main");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Произошла ошибка при добавлении пользователя: " + ex.Message);
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Users()
        {
            CheckRole();
            if (User.Identity!.IsAuthenticated)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userRole = await _context.Users
                  .Where(a => a.customer_id.ToString() == userId)
                  .Select(a => a.role)
                  .FirstOrDefaultAsync();


                if (userRole != null && userRole == "admin" || userRole == "owner")
                {
                    var users = await _context.Users.ToListAsync();

                    return View(users);

                }
            }
            return RedirectToAction("NotEnoughRights", "Error");
        }
        private string EncryptPassword(string? password)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(password!);
                byte[] hashBytes = sha512.ComputeHash(inputBytes);
                string hash = BitConverter.ToString(hashBytes);

                return hash;
            }
        }
        [HttpPost]
        public ActionResult AddUser(IFormCollection form)
        {
            var model = new Users
            {
                customer_name = form["login"],
                password = EncryptPassword(form["password"]),
                role = form["role"],
                address = "unknown",
                email = form["email"],
            };

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Users.Add(model);
                    _context.SaveChanges();
                    return RedirectToAction("Users", "Main");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Произошла ошибка при добавлении пользователя: " + ex.Message);
                }
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            try
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Users", "Main");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Произошла ошибка при удалении пользователя: " + ex.Message);
                return RedirectToAction("AunthError", "Error");
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditUser(Users user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingUser = await _context.Users.FindAsync(user.customer_id);

                    if (existingUser != null)
                    {
                        existingUser.customer_name = user.customer_name;
                        existingUser.password = EncryptPassword(user.password);
                        existingUser.email = user.email;
                        existingUser.role = user.role;

                        _context.Entry(existingUser).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Users", "Main");
                    }
                    else
                    {
                        return NotFound(); 
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Произошла ошибка при редактировании пользователя: " + ex.Message);
                    return RedirectToAction("AunthError", "Error");
                }
            }
            return View(user);
        }

        public async Task FindOrder()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userOrders = await _context.Orders
                                          .Where(order => order.customer_id == Convert.ToInt32(userId))
                                          .ToListAsync();
            ViewBag.UserOrders = userOrders.Count;
        }
        public async Task<IActionResult> Order()
        {
            CheckRole();
            if (User.Identity!.IsAuthenticated)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var userOrders = await _context.Orders
                                               .Where(order => order.customer_id == Convert.ToInt32(userId))
                                               .ToListAsync();

                if (userOrders != null && userOrders.Count > 0)
                {
                    return View(userOrders);
                }
                else
                {
                    return View("EmptyCart");
                }
            }
            return RedirectToAction("NotEnoughRights", "Error");
        }


    }
}

