using E_Commerce_Web_Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_Commerce_Web_Application.Controllers
{
    public class ProductController : Controller
    {
        private Entities db = new Entities();
       
        [HttpGet,ActionName("Search")]
        public ActionResult Filter(string Id,string Type,ViewModels ob)
        {
            int TypeId = int.Parse(Type);
            if (Session["Authentication"] != null)
            {
                string uname = Session["UserName"].ToString();
                var exits = db.WishlistTables.Where(x => x.UserName == uname).FirstOrDefault();
                if (exits != null)
                    ob.Wishlist = db.WishlistTables.Where(x => x.UserName == uname).ToList();
                else
                    ob.Wishlist = null;
            }
            ob.ItemTable = db.ItemTables.Where(x => x.ItemBrandName == Id && x.ProductTypeId==TypeId).ToList();
            ob.NoofItem = db.ItemTables.Where(x => x.ItemBrandName == Id && x.ProductTypeId == TypeId).Count();
            Session["ProductTypeId"] = TypeId;
            Session["Infoofbrand"] = Id;
            ob.Result = Id;
            return View("Product",ob);
        }
        [HttpPost,ActionName("Search")]
        public ActionResult Filter(string Search, string Id, string Type, ViewModels ob)
        {
            
            Session["ProductTypeId"] = "0";
            Session["Infoofbrand"] ="0";
            ob.ItemTable = new List<ItemTable>();
            if(Session["Authentication"]!=null)
            {
                string uname = Session["UserName"].ToString();
                var exits = db.WishlistTables.Where(x => x.UserName == uname).FirstOrDefault();
                if (exits != null)
                    ob.Wishlist = db.WishlistTables.Where(x => x.UserName == uname).ToList();
                else
                    ob.Wishlist = null;
            }
            var ItemList= db.SearchTables.Where(x => x.Tags == Search).Select(x => x.ItemId).ToList();
            int i=1;
            foreach(var n in ItemList)
            {
                ob.NoofItem = i;
                var row = db.ItemTables.Where(x => x.ItemId == n).Single();
                ob.ItemTable.Add(row);
                if(i==1)
                {
                    Session["InfoofBrand"] = db.SearchTables.Where(x=>x.ItemId==n).Select(x=>x.ItemBrandName).FirstOrDefault();
                }
                i++;
            }
            ob.Result = Search;
            return View("Product",ob);
        }

   
	}
}