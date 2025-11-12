using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialMithila.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult ContactDataList()
        {
            return View();
        }
    }
}