using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KLTN.Common.Datatables;
using KLTN.DataModels.Models.Contact;
using KLTN.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace KLTN.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class ContactController : BaseController
    {
        private readonly IContactService _contactService;
        private readonly IMapper _mapper;

        public ContactController(IContactService contactService, IMapper mapper)
        {
            _contactService = contactService;
            _mapper = mapper;
        }
        
        // GET
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult LoadContact([FromBody] DTParameters dtParameters)
        {
            var tupleData = _contactService.LoadContact(dtParameters);

            return Json(new
            {
                draw = dtParameters.Draw,
                recordsTotal = tupleData.Item3,
                recordsFiltered = tupleData.Item2,
                data = tupleData.Item1
                    .Skip(dtParameters.Start)
                    .Take(dtParameters.Length)
            });
        }

        /// <summary>
        /// Action Get Detail return view Detail Contact with an ContactViewModel
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Detail(int? id)
        {
            try
            {
                if (id == null) return BadRequest();
                var contact = _contactService.GetContactById(id.Value);
                return View(contact.Item1);
            }
            catch(Exception e)
            {
                Log.Error("Have an error when get detail contact", e);
                return BadRequest();
            }
        }

        /// <summary>
        /// Action Post Detail handling FeedbackCustomer
        /// </summary>
        /// <param name="contactViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Detail(ContactViewModel contactViewModel)
        {
            if (!ModelState.IsValid) return View(contactViewModel);
            try
            {
                var contact = await _contactService.ReplyContact(contactViewModel);
                return contact ? RedirectToAction("Index","Contact") : (IActionResult)BadRequest();
            }
            catch (Exception e)
            {
                Log.Error("Have an error when reply contact", e);
                return BadRequest();
            }
        }
    }
}