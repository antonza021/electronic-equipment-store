using electronic_equipment_store.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace electronic_equipment_store.Controllers
{
    public class HomeController : Controller
    {
        private readonly List<Product> _products; // ��������������, ��� � ��� ���� ������ ���������

        // ����������� ��� ������������� ������ ��������� (������ �������� �� �������� �������� ������)
        public HomeController()
        {
            _products = new List<Product>
            {
                new Product { Id = 1, Name = "�������", Description = "������ ������� ��� ������ � �����������", Price = 999.99m, ImageUrl = "/images/laptop.jpg" },
                new Product { Id = 2, Name = "��������", Description = "����������� �������� � ������� ����������� ������", Price = 599.99m, ImageUrl = "/images/smartphone.jpg" },
                // �������� ��� �������� ��� �������������
            };
        }

        // �������� ��� ����������� ������ ���������
        public IActionResult Index()
        {
            return View(_products); // �������� ������ ��������� � �������������
        }

        // �������� ��� ����������� ������� ����������� ��������
        public IActionResult Details(int id)
        {
            // ������� ������� � ��������� ���������������
            var product = _products.Find(p => p.Id == id);
            if (product == null)
            {
                return NotFound(); // ���������� ������ 404, ���� ������� �� ������
            }

            return View(product); // �������� ��������� ������� � �������������
        }
    }
}
