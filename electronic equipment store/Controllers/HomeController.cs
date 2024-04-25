using electronic_equipment_store.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace electronic_equipment_store.Controllers
{
    public class HomeController : Controller
    {
        private readonly List<Product> _products; // Предполагается, что у вас есть список продуктов

        // Конструктор для инициализации списка продуктов (можете заменить на реальный источник данных)
        public HomeController()
        {
            _products = new List<Product>
            {
                new Product { Id = 1, Name = "Ноутбук", Description = "Мощный ноутбук для работы и развлечений", Price = 999.99m, ImageUrl = "/images/laptop.jpg" },
                new Product { Id = 2, Name = "Смартфон", Description = "Современный смартфон с высоким разрешением камеры", Price = 599.99m, ImageUrl = "/images/smartphone.jpg" },
                // Добавьте еще продукты при необходимости
            };
        }

        // Действие для отображения списка продуктов
        public IActionResult Index()
        {
            return View(_products); // Передаем список продуктов в представление
        }

        // Действие для отображения деталей конкретного продукта
        public IActionResult Details(int id)
        {
            // Находим продукт с указанным идентификатором
            var product = _products.Find(p => p.Id == id);
            if (product == null)
            {
                return NotFound(); // Возвращаем ошибку 404, если продукт не найден
            }

            return View(product); // Передаем найденный продукт в представление
        }
    }
}
