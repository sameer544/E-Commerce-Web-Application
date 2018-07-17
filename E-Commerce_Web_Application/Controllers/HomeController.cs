using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_Commerce_Web_Application.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {

            Session["NavbarHide"] = null;
            return View();
        }
        public ActionResult LogOff()
        {
            Session["Authentication"] = null;
            Session["UserName"] = null;
            return RedirectToAction("Index","Home");
        }
	}
}