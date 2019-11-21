using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KLTN.DataModels.Models.Contact;
using KLTN.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace KLTN.Web.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(ContactViewModel contactModel)
        {
            if (!ModelState.IsValid) return View(contactModel);

            try
            {
                var result = _contactService.SendContact(contactModel.Title, contactModel.Content);

                return RedirectToAction("SendContactSuccess","Notification");
                //if(result == )
            }catch(Exception e)
            {
                Log.Error("Have an error when send contact by Customer",e);
                return RedirectToAction("SendContactFail", "Notification");
            }
        }
    }
}