using electronic_equipment_store.App_Data;
using electronic_equipment_store.Models;
using electronic_equipment_store.Models.DBModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace electronic_equipment_store.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task FindOrder()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userOrders = await _context.Orders
                                          .Where(order => order.customer_id == Convert.ToInt32(userId))
                                          .ToListAsync();
            ViewBag.UserOrders = userOrders.Count;
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
        public async Task<IActionResult> Products()
        {
            CheckRole();
            if (User.Identity!.IsAuthenticated)
            {
                var prod = await _context.Products.ToListAsync();
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userRole = await _context.Users
                  .Where(a => a.customer_id.ToString() == userId)
                  .Select(a => a.role)
                  .FirstOrDefaultAsync();
                if (userRole != null && userRole == "user")
                {
                    return View(prod);
                }
                else if (userRole != null && userRole == "admin" || userRole == "employee")
                {
                    var TypeList = _context.Categories.Select(c => new SelectListItem
                    {
                        Text = $"{c.category_name}",
                        Value = c.category_id.ToString()
                    }).ToList();
                    TypeList.Insert(0, new SelectListItem { Text = "Выберите комплектующее", Value = "" });
                    ViewBag.TypeList = TypeList;
                    var ManifList = _context.Manufacturers.Select(c => new SelectListItem
                    {
                        Text = $"{c.manufacturer_name}",
                        Value = c.manufacturer_id.ToString()
                    }).ToList();
                    ManifList.Insert(0, new SelectListItem { Text = "Выберите комплектующее", Value = "" });
                    ViewBag.ManifList = ManifList;
                    var prodlistt = _context.Products.Select(c => new SelectListItem
                    {
                        Text = $"{c.product_name}",
                        Value = c.product_id.ToString()
                    }).ToList();
                    prodlistt.Insert(0, new SelectListItem { Text = "Выберите комплектующее", Value = "" });
                    ViewBag.DeleteCompList = prodlistt;
                    return View("AddProducts", prod);
                }
            }
            return RedirectToAction("AunthError", "Error");
        }
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productID, double unitPrice)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _context.Users.FirstOrDefaultAsync(a => a.customer_id == Convert.ToInt32(userId));

            var Order = new Orders
            {
                customer_id = user.customer_id,
                order_date = DateOnly.FromDateTime(DateTime.UtcNow),
                total_amount = Convert.ToDecimal(unitPrice),
            };
            try
            {
                _context.Orders.Add(Order);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Произошла ошибка при добавлении пользователя: " + ex.Message);
            }
            var model = new Order_Items
            {
                product_id = productID,
                quantity = 1,
                unit_price = unitPrice,
                order_id = Order.order_id,
            };
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Order_Items.Add(model);
                    _context.SaveChanges();
                    ViewBag.UserId = userId;
                    await FindOrder(); 
                    return RedirectToAction("Order", "Main");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Произошла ошибка при добавлении пользователя: " + ex.Message);
                }
            }
            return RedirectToAction("AunthError", "Error");
        }
      
        [HttpPost]
        public ActionResult AddProducts(IFormCollection form)
        {
            CheckRole();
            var model = new Products
            {
                product_name = form["Title"],
                description = form["desc"],
                price = Convert.ToDecimal(form["price"]),
                category_id = Convert.ToInt32(form["type"]),
                manufacturer_id = Convert.ToInt32(form["manifacturers"]),
                stock_quantity = Convert.ToInt32(form["count"]),
            };

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Products.Add(model);
                    _context.SaveChanges();
                    return RedirectToAction("Products", "Home");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Произошла ошибка при добавлении материала: " + ex.Message);
                }
            }

            return View(model);
        }
        public async Task<IActionResult> DeleteProduct(int idprod)
        {
            var prod = await _context.Products.FindAsync(idprod);
            if (prod == null)
            {
                return View("AunthError");
            }

            try
            {
                _context.Products.Remove(prod);
                await _context.SaveChangesAsync();
                return RedirectToAction("Products", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Произошла ошибка при удалении пользователя: " + ex.Message);
                return RedirectToAction("AunthError", "Error");
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditProducts(Products prod)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingProd = await _context.Products.FindAsync(prod.product_id);

                    if (existingProd != null)
                    {
                        existingProd.product_name = prod.product_name;
                        existingProd.price = prod.price;
                        existingProd.description = prod.description;
                        existingProd.category_id = prod.category_id;
                        existingProd.manufacturer_id = prod.manufacturer_id;
                        existingProd.stock_quantity = prod.stock_quantity;

                        _context.Entry(existingProd).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Products", "Home");
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
            return View(prod);
        }
        public async Task<IActionResult> Promotions()
        {
            CheckRole();
            if (User.Identity!.IsAuthenticated)
            {
                var prom = await _context.Promotions.ToListAsync();
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userRole = await _context.Users
                  .Where(a => a.customer_id.ToString() == userId)
                  .Select(a => a.role)
                  .FirstOrDefaultAsync();
                var promlist = _context.Promotions.Select(c => new SelectListItem
                {
                    Text = $"{c.promotion_name}",
                    Value = c.promotion_id.ToString()
                }).ToList();
                promlist.Insert(0, new SelectListItem { Text = "Выберите праздник", Value = "" });
                ViewBag.DeletePromList = promlist;
                if (userRole != null && userRole == "user")
                {
                    return View(prom);
                }
                else if (userRole != null && userRole == "admin" || userRole == "employee")
                {
                    return View("AddPromotions", prom);
                }
            }
            return RedirectToAction("AunthError", "Error");
        }
        [HttpPost]
        public ActionResult AddPromotions(IFormCollection form)
        {
            CheckRole();
            var model = new Promotions
            {
                promotion_name = form["Title"],
                start_date = Convert.ToDateTime(form["dateStart"]),
                end_date = Convert.ToDateTime(form["dateEnd"]),
                discount_percentage = Convert.ToDecimal(form["pecent"]),
            };

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Promotions.Add(model);
                    _context.SaveChanges();
                    return RedirectToAction("Promotions", "Home");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Произошла ошибка при добавлении материала: " + ex.Message);
                }
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> DeletePromotions(int idprom)
        {
            var prom = await _context.Promotions.FindAsync(idprom);
            if (prom == null)
            {
                return View("AunthError");
            }

            try
            {
                _context.Promotions.Remove(prom);
                await _context.SaveChangesAsync();
                return RedirectToAction("Promotions", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Произошла ошибка при удалении пользователя: " + ex.Message);
                return RedirectToAction("AunthError", "Error");
            }
        }
        public async Task<IActionResult> Manufacturers()
        {
            CheckRole();
            var manuf = await _context.Manufacturers.ToListAsync();
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = await _context.Users
              .Where(a => a.customer_id.ToString() == userId)
              .Select(a => a.role)
              .FirstOrDefaultAsync();
            var manuflist = _context.Manufacturers.Select(c => new SelectListItem
            {
                Text = $"{c.manufacturer_name}",
                Value = c.manufacturer_id.ToString()
            }).ToList();
            manuflist.Insert(0, new SelectListItem { Text = "Выберите произовдителя", Value = "" });
            ViewBag.DeleteManufList = manuflist;
            if (userRole != null && userRole == "user")
            {
                return View(manuf);
            }
            else if (userRole != null && userRole == "admin" || userRole == "employee")
            {
                return View("AddManufacturers", manuf);
            }
            return RedirectToAction("AunthError", "Error");
        }
        [HttpPost]
        public ActionResult AddManufacturers(IFormCollection form)
        {
            CheckRole();
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var model = new Manufactures
            {
                manufacturer_name = form["Title"],
                description = form["desc"],
            };

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Manufacturers.Add(model);
                    _context.SaveChanges();
                    return RedirectToAction("Manufacturers", "Home");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Произошла ошибка при добавлении оценки: " + ex.Message);
                }
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteManufacturers(int idmanuf)
        {
            var manuf = await _context.Manufacturers.FindAsync(idmanuf);
            if (manuf == null)
            {
                return View("AunthError");
            }

            try
            {
                _context.Manufacturers.Remove(manuf);
                await _context.SaveChangesAsync();
                return RedirectToAction("Manufacturers", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Произошла ошибка при удалении пользователя: " + ex.Message);
                return RedirectToAction("AunthError", "Error");
            }
        }
        public async Task<IActionResult> Payments()
        {
            CheckRole();
            var pay = await _context.Payments.ToListAsync();
            return View(pay);
        }

    }
}

