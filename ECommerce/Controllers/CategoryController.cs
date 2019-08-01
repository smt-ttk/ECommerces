using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers
{
    public class CategoryController : Controller
    {
        [Route("/kategori/{id}")] //kullanıcının görmesi gereken yol...
        public IActionResult Index(int id)
        {
            Category category = new Category();
            using (ECommerceContext eCommerceContext = new ECommerceContext())
            {
                category = eCommerceContext.Categories.SingleOrDefault(a =>a.Id == id);// select * from Categories where Id == 3
            }

            ViewData["Title"] = category.Name;
            return View(category);
        }
    }
}