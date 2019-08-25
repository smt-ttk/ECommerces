using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using ECommerce.DTO;
using ECommerce.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Controllers
{
    public class AjaxController : Controller
    {
        private static readonly AjaxMethod AjaxMethod = new AjaxMethod();
        
        [Route("/api")]
        public JsonResult Handle()
        {
            DTO.AjaxResponseDto ajaxResponse = new DTO.AjaxResponseDto();

            string json = HttpContext.Request.Form["JSON"].ToString();
            DTO.AjaxRequestDto ajaxRequest = Newtonsoft.Json.JsonConvert.DeserializeObject<DTO.AjaxRequestDto>(json);

            if (ajaxRequest.Method == "SaveProduct")
            {
                AjaxMethod.SaveProduct(ajaxRequest.Json);
            }
            else if (ajaxRequest.Method == "ProductsByCategoryId")
            {
                ajaxResponse.Dynamic = AjaxMethod.ProductsByCategoryId(ajaxRequest.Json);
            }
            else if (ajaxRequest.Method == "RemoveProduct")
            {
                ajaxResponse.Dynamic = AjaxMethod.RemoveProduct(ajaxRequest.Json);
            }

            else if (ajaxRequest.Method == "SaveContact")
            {
                AjaxMethod.SaveContact(ajaxRequest.Json);
            }
            else if (ajaxRequest.Method == "UpdateProduct")
            {
                ajaxResponse.Dynamic = AjaxMethod.UpdateProduct(ajaxRequest.Json);
            }

            return new JsonResult(ajaxResponse);
        }
    }

    public class AjaxMethod
    {
        private static readonly Adapter.ProductAdapter productAdapter = new Adapter.ProductAdapter();
        public bool RemoveProduct(string json)
        {
            bool result = true;

            DTO.ProductRemoveDto productRemove = Newtonsoft.Json.JsonConvert.DeserializeObject<DTO.ProductRemoveDto>(json);

            productAdapter.Delete<Data.Models.Product>(productRemove.ProductId);

            return result;
        }
        public bool UpdateProduct (string json)
        {
            bool result = false;

            DTO.ProductUpdate productUpdate = Newtonsoft.Json.JsonConvert.DeserializeObject<DTO.ProductUpdate>(json);
            using (ECommerceContext eCommerceContext= new ECommerceContext())
            {
                Data.Models.Product product = eCommerceContext.Products.SingleOrDefault(a => a.Id == productUpdate.ProductId);
                if (product != null)
                {
                    eCommerceContext.Products.Update(product);
                    eCommerceContext.SaveChanges();
                    result = true;
                }
            }

            return result;
        }
        public void SaveProduct(string json)
        {
            DTO.ProductSaveDto productSave = Newtonsoft.Json.JsonConvert.DeserializeObject<DTO.ProductSaveDto>(json);

            Data.Models.Product product = new Data.Models.Product()

               {
                    Name = productSave.ProductName,
                    Description = productSave.ProductDescription,
                    StateId = (int)Enums.State.Active,
                    CategoryId = productSave.CategoryId,
                    CreateDate = DateTime.UtcNow

                 };
            product = productAdapter.Insert<Data.Models.Product>(product);
            //using (ECommerceContext eCommerceContext = new ECommerceContext())
            //{
            //    eCommerceContext.Products.Add(new Models.Product()
            //    {
            //        Name = productSave.ProductName,
            //        Description = productSave.ProductDescription,
            //        StateId = (int)Enums.State.Active,
            //        CategoryId = productSave.CategoryId,
            //        CreateDate = DateTime.UtcNow,

            //    });

            //    eCommerceContext.SaveChanges();
            //}
        }
        public void SaveContact(string json)
        {
            DTO.ContactSaveDto contactSave = Newtonsoft.Json.JsonConvert.DeserializeObject<DTO.ContactSaveDto>(json);

            Data.Models.Contact contact = new Contact()
            {
                name = contactSave.name,
                surname = contactSave.surname,
                message = contactSave.message,

            };

            contact = productAdapter.Insert<Data.Models.Contact>(contact);

            //using (ECommerceContext eCommerceContext = new ECommerceContext())
            //{
            //    eCommerceContext.Contacts.Add(new Models.Contact()
            //    {
            //        name= contactSave.name,
            //        surname = contactSave.surname,
            //        message =contactSave.message,

            //    });

            //    eCommerceContext.SaveChanges();


            //}
        }

        public List<Data.Models.Product> ProductsByCategoryId(string json)
        {
            List<Data.Models.Product> result = new List<Data.Models.Product>();
            DTO.ProductsByCategoryId productsByCategoryId = Newtonsoft.Json.JsonConvert.DeserializeObject<DTO.ProductsByCategoryId>(json);

            
            IQueryable<Product> products = productAdapter.Get<Data.Models.Product>();

            result = products.Include(a => a.Category).ToList().Where(a => a.CategoryId == productsByCategoryId.CategoryId).ToList();

            using (ECommerceContext eCommerceContext = new ECommerceContext())
            {
                result = eCommerceContext.Products.Where(a => a.CategoryId == productsByCategoryId.CategoryId).ToList();
            }

            return result;
        }
    }
}