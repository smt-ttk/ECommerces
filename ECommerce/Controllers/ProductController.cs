using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ECommerce.Controllers
{
    public class ProductController : Controller
    {
        [Route("/urun/{id}")]
        public IActionResult Index(int id)
        {
            Data.Models.Product product;
            using (ECommerceContext eCommerceContext = new ECommerceContext())
            {
                product = eCommerceContext.Products.SingleOrDefault(a => a.Id == id);
            }

            return View(product);
        }
    }
}