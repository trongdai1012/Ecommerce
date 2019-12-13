using System;
using System.Linq;
using System.Threading.Tasks;
using KLTN.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using X.PagedList;

namespace KLTN.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        private readonly IBrandService _brandService;

        private readonly IFeedbackService _feedbackService;

        public ProductController(IProductService productService, IFeedbackService feedbackService, IBrandService brandService)
        {
            _productService = productService;

            _feedbackService = feedbackService;

            _brandService = brandService;
        }

        public IActionResult LaptopDetail(int id)
        {
            var laptop = _productService.GetLaptopById(id).Item1;
            return View(laptop);
        }

        public IActionResult MobileDetail(int id)
        {
            var mobile = _productService.GetMobileById(id).Item1;
            return View(mobile);
        }

        [HttpPost]
        public JsonResult Rating(int productId, byte rate, string comment)
        {
            if (rate < 1 || rate > 5) return Json(new { status = 3 });
            try
            {
                var myRating = _feedbackService.Rating(productId, comment, rate);
                return Json(
                    new
                    {
                        status = myRating
                    });
            }
            catch (Exception e)
            {
                Log.Error("Have an error when Rating product in controller", e);
                return Json(
                    new
                    {
                        status = false
                    });
            }
        }

        [HttpPost]
        public JsonResult LikeProduct(int productId)
        {
            var feedback = _feedbackService.LikeProduct(productId);
            return Json(
                new
                {
                    Feedback = feedback
                });
        }

        [HttpPost]
        public IActionResult FeedbackProduct(int id)
        {
            try
            {
                var listFeedback = _feedbackService.GetFeedbackByProducId(id);
                return PartialView("_FeedbackProduct", listFeedback);
            }
            catch (Exception e)
            {
                Log.Error("Have an error at FeedbackProduct in ProductController", e);
                return BadRequest();
            }
        }

        public async Task<IActionResult> LapTop(string searchKey, int pageIndex = 1, int pageSize = 12)
        {
            ViewBag.PageIndex = pageIndex;
            return View();
        }

        public async Task<IActionResult> ListLapTop(string searchKey, int brandId, int orderBy, int pageIndex, int pageSize = 12)
        {
            ViewBag.ListBrand = await _brandService.GetBrandHasLaptop();

            var listLap = await _productService.GetAllLaptop(searchKey, brandId);

            if (orderBy == 1)
            {
                return PartialView("_ListLapTop", listLap.OrderBy(x => x.InitialPrice).ToPagedList(pageIndex, pageSize));
            }if(orderBy == 2)
            {
                return PartialView("_ListLapTop", listLap.OrderByDescending(x => x.InitialPrice).ToPagedList(pageIndex, pageSize));
            }

            return PartialView("_ListLapTop", listLap.OrderBy(x => x.InitialPrice).ToPagedList(pageIndex, pageSize));
        }
    
        public async Task<IActionResult> Product(string searchKey)
        {
            ViewBag.SearchKey = searchKey;
            return View();
        }

        public async Task<IActionResult> ListAll(string searchString, int orderBy, int pageIndex = 1, int pageSize = 12)
        {
            ViewBag.ListBrand = await _brandService.GetAllBrand();

            var listLap = await _productService.GetAll(searchString);

            if (orderBy == 1)
            {
                return PartialView("_ListAllProduct", listLap.OrderBy(x => x.InitialPrice).ToPagedList(pageIndex, pageSize));
            }
            if (orderBy == 2)
            {
                return PartialView("_ListAllProduct", listLap.OrderByDescending(x => x.InitialPrice).ToPagedList(pageIndex, pageSize));
            }

            return PartialView("_ListAllProduct", listLap.OrderBy(x => x.InitialPrice).ToPagedList(pageIndex, pageSize));
        }

        public async Task<IActionResult> Mobile(string searchKey, int pageIndex = 1, int pageSize = 12)
        {
            ViewBag.PageIndex = pageIndex;

            return View();
        }

        public async Task<IActionResult> ListMobile(string searchKey, int brandId, int orderBy, int pageIndex, int pageSize = 12)
        {
            ViewBag.ListBrand = await _brandService.GetBrandHasMobile();

            var listLap = await _productService.GetAllMobile(searchKey, brandId);

            if (orderBy == 1)
            {
                return PartialView("_ListMobile", listLap.OrderBy(x => x.InitialPrice).ToPagedList(pageIndex, pageSize));
            }
            if (orderBy == 2)
            {
                return PartialView("_ListMobile", listLap.OrderByDescending(x => x.InitialPrice).ToPagedList(pageIndex, pageSize));
            }

            return PartialView("_ListMobile", listLap.OrderBy(x => x.InitialPrice).ToPagedList(pageIndex, pageSize));
        }
    }
}