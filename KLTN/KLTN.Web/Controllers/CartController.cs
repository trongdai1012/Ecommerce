using System;
using System.Collections.Generic;
using System.Linq;
using KLTN.Common.Helpers;
using KLTN.DataModels.Models.Orders;
using KLTN.DataModels.Models.Products;
using KLTN.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KLTN.Web.Controllers
{
    [Route("cart")]
    public class CartController : Controller
    {
        private readonly IProductService _productService;

        private readonly IOrderService _orderService;

        private readonly IUserService _userService;

        private readonly HttpContext _httpContext;

        public CartController(IProductService productService, IOrderService orderService, IUserService userService, IHttpContextAccessor contextAccessor)
        {
            _productService = productService;
            _orderService = orderService;
            _userService = userService;
            _httpContext = contextAccessor.HttpContext;
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
                var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
                var index = IsExist(id);
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
                    cart.Add(quantity == null
                        ? new CartItem
                        {
                            Product = _productService.GetProductById(Convert.ToInt32(id)).Item1,
                            Quantity = 1
                        }
                        : new CartItem
                        {
                            Product = _productService.GetProductById(Convert.ToInt32(id)).Item1,
                            Quantity = quantity.Value
                        });
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            return RedirectToAction("Index");
        }

        [Route("remove/{id}")]
        public IActionResult Remove(string id)
        {
            var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            var index = IsExist(id);
            cart.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("Index");
        }
        
        [Authorize]
        [HttpGet]
        [Route("Payment")]
        public IActionResult Payment()
        {
            var user = _userService.GetUserById(Convert.ToInt32(_httpContext.User.FindFirst(x => x.Type == "Id").Value));
            var order = new OrderViewModel
            {
                RecipientFirstName = user.FirstName,
                RecipientLastName = user.LastName,
                RecipientEmail = user.Email,
                RecipientAddress = user.Address,
                RecipientProvinceName = user.ProvinceName,
                RecipientDistrictName = user.DistrictName,
                RecipientPrecinctName = user.PrecinctName,
                RecipientDistrictCode = user.DistrictId,
                RecipientPrecinctCode = user.PrecinctId,
                RecipientProvinceCode = user.ProvinceId,
                RecipientPhone = user.Phone
            };
            return View(order);
        }

        [HttpPost]
        [Authorize]
        [Route("Payment")]
        public IActionResult Payment(OrderViewModel orderView)
        {
            var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            if (cart == null) return BadRequest();
            var listOrdDt = new List<OrderDetailViewModel>();
            decimal subTotal = 0;
            foreach (var item in cart)
            {
                listOrdDt.Add(new OrderDetailViewModel
                {
                    ProductId = item.Product.Id,
                    Quantity = item.Quantity,
                    Image = item.Product.Image,
                    Price = item.Product.CurrentPrice,
                    TotalPrice = item.Product.CurrentPrice * item.Quantity
                });
                subTotal += item.Product.CurrentPrice * item.Quantity;
            }

            orderView.TotalPrice = subTotal;
            _orderService.Create(orderView,listOrdDt);

            return RedirectToAction("Index","Home");
        }

        [Route("update")]
        [HttpPut]
        public JsonResult Update(string id, int? quantity)
        {
            var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            var index = IsExist(id);
            var cartModel = cart.ElementAt(index);
            if (quantity == null || quantity < 1)
            {
                cartModel.Quantity = 1;
            }
            else
            {
                cartModel.Quantity = quantity.Value;
            }

            var totalPrice = cartModel.Product.CurrentPrice * cartModel.Quantity;

            decimal subTotalPrice = 0;
            foreach (var item in cart)
            {
                subTotalPrice += item.Quantity * item.Product.CurrentPrice;
            }

            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return Json(
                new
                {
                    quantity = cartModel.Quantity,
                    total = totalPrice,
                    subTotal = subTotalPrice
                });
        }

        private int IsExist(string id)
        {
            var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            for (var i = 0; i < cart.Count; i++)
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