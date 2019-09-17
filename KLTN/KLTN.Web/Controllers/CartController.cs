using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KLTN.Common.Helpers;
using KLTN.DataModels.Models.Products;
using KLTN.Services;
using Microsoft.AspNetCore.Mvc;

namespace KLTN.Web.Controllers
{
    [Route("cart")]
    public class CartController : Controller
    {
        private readonly IProductService _productService;

        public CartController(IProductService productService)
        {
            _productService = productService;
        }

        [Route("index")]
        public IActionResult Index()
        {
            var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            ViewBag.cart = cart;
            ViewBag.total = cart.Sum(item => item.Product.CurrentPrice * item.Quantity);
            return View();
        }

        [Route("buy/{id}")]
        public IActionResult Buy(string id, int? quantity)
        {
            if (SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart") == null)
            {
                var cart = new List<CartItem>();
                if (quantity == null)
                {
                    cart.Add(new CartItem { Product = _productService.GetProductById(Convert.ToInt32(id)).Item1, Quantity = 1 });
                }
                else
                {
                    cart.Add(new CartItem { Product = _productService.GetProductById(Convert.ToInt32(id)).Item1, Quantity = quantity.Value });
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                List<CartItem> cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
                int index = IsExist(id);
                if (index != -1)
                {
                    if (quantity == null)
                    {
                        cart[index].Quantity += 1;
                    }
                    else
                    {
                        cart[index].Quantity += quantity.Value;
                    }
                }
                else
                {
                    if (quantity == null)
                    {
                        cart.Add(new CartItem { Product = _productService.GetProductById(Convert.ToInt32(id)).Item1, Quantity = 1 });
                    }
                    else
                    {
                        cart.Add(new CartItem { Product = _productService.GetProductById(Convert.ToInt32(id)).Item1, Quantity = quantity.Value });
                    }

                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            return RedirectToAction("Index");
        }

        [Route("remove/{id}")]
        public IActionResult Remove(string id)
        {
            List<CartItem> cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            int index = IsExist(id);
            cart.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("Index");
        }

        [Route("update")]
        public IActionResult Update([FromQuery]string id, [FromQuery]int? quantity)
        {
            List<CartItem> cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            int index = IsExist(id);
            var cartModel = cart.ElementAt(index);
            if (quantity == null || quantity < 1)
            {
                cartModel.Quantity = 1;
            }else
            {
                cartModel.Quantity = quantity.Value;
            }

            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("Index");
        }

        private int IsExist(string id)
        {
            List<CartItem> cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product.Id.ToString().Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}