using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using E_Commerce_Web_Application.Models;

namespace E_Commerce_Web_Application.Controllers
{
    public class DetailsController : Controller
    {
        //
        // GET: /Details/
        private Entities db = new Entities();

        public ActionResult Product(int id)
        {
            ViewModels ob = new ViewModels();

            var gt = (from x in db.ItemDetailsTables
                      where x.ItemId == id
                      orderby x.Id
                      group x by x.SpecType into gp
                      select gp).OrderBy(z=>z.Max(x=>x.Id)).ToDictionary(z => z.Key, p => p.ToList());
            ob.SpecType = gt;
            //var image = (from x in db.ItemImagesTables
            //             where x.ItemId == id
            //             select x).OrderBy(z=>z.Id).ToList();
            //ob.SpecImages = image;
            ob.ProductInfo = (from x in db.ItemTables
                                   where x.ItemId == id
                                   select x).ToList();
            var sd = (from x in db.ItemTables
                      where x.ItemId == id
                      select x.ItemImageName).Single();
            ob.ImageName = sd;
            return View("Details", ob);
        }

    }
}
