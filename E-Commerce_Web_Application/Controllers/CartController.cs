using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using E_Commerce_Web_Application.Models;
using System.Threading;
using System.Net.Mail;
using System.Net;
using System.IO;

namespace E_Commerce_Web_Application.Controllers
{
    public class CartController : Controller
    {
        private Entities db = new Entities();
        //
        // GET: /Cart/
        public ActionResult MyCart(int? id)
        {
           
            if (Session["Authentication"] == null)
            {
                return RedirectToAction("LoginModal", "Account");

            }
            else if(id!=null)
            {
                string uname = Session["UserName"].ToString();
                int pid = (int)id;
                var proexistintable = (from x in db.CustomerShoppingCarts
                                       where x.ItemId == id && x.CustomerUserName == uname
                                       select x).OrderByDescending(z => z.DateTime).ToList();
                if (proexistintable.Count != 0)
                {
                    proexistintable = (from x in db.CustomerShoppingCarts
                                       where x.CustomerUserName == uname
                                       select x).OrderByDescending(z => z.DateTime).ToList();

                    return View("CartListView", proexistintable);
                }
                else
                {
                    var ob = new CustomerShoppingCart();
                    List<ItemTable> itemInfo = (from x in db.ItemTables
                                                where x.ItemId == (int)id
                                                select x).ToList();
                    foreach (var n in itemInfo)
                    {
                        ob.ItemName = n.ItemName;
                        ob.ItemImageName = n.ItemImageName;
                        ob.ItemPrice = n.ItemPrice;
                        ob.NoOfItem = 1;
                    }

                    ob.ItemId = pid;
                    ob.CustomerUserName = Session["UserName"].ToString();
                    ob.DateTime = DateTime.Now;
                    db.CustomerShoppingCarts.Add(ob);
                    db.SaveChanges();
                }
                proexistintable = (from x in db.CustomerShoppingCarts
                                   where x.CustomerUserName == uname
                                   select x).OrderByDescending(z => z.DateTime).ToList();
                return View("CartListView", proexistintable);
            }
            else
            {
                string uname = Session["UserName"].ToString();
                    var proexistintable = (from x in db.CustomerShoppingCarts
                                           where x.CustomerUserName == uname
                                           select x).OrderByDescending(z => z.DateTime).ToList();

                    return View("CartListView",proexistintable);
            }
        }
       
        public PartialViewResult DecreaseOperation(int id)
        {
            string uname = Session["UserName"].ToString();
            var model = (from x in db.CustomerShoppingCarts
                         where x.ItemId == id && x.CustomerUserName == uname
                         select x).Single();
            if (model.NoOfItem >= 2)
            {
                model.NoOfItem--;
                db.SaveChanges();
            }
            var proexistintable = (from x in db.CustomerShoppingCarts
                                   where x.CustomerUserName == uname
                                   select x).OrderByDescending(z => z.DateTime).ToList();
            return PartialView("_CartListViewPartialView", proexistintable);
        }

        public PartialViewResult IncreasesOperation(int id)
        {
          
            string uname = Session["UserName"].ToString();
            var model = (from x in db.CustomerShoppingCarts
                         where x.ItemId == id && x.CustomerUserName == uname
                         select x).Single();
            if (model.NoOfItem >= 1)
            {
                model.NoOfItem++;
                db.SaveChanges();
            }
            var proexistintable = (from x in db.CustomerShoppingCarts
                                   where x.CustomerUserName == uname
                                   select x).OrderByDescending(z => z.DateTime).ToList();
            return PartialView("_CartListViewPartialView", proexistintable);
        }
        public ActionResult Remove(int? id)
        {

            string uname = Session["UserName"].ToString();
            try
            {
                var itemremove = (from x in db.CustomerShoppingCarts
                                  where x.ItemId == id && x.CustomerUserName == uname
                                  select x).Single();
                db.CustomerShoppingCarts.Remove(itemremove);
                db.SaveChanges();
            }
            catch (Exception) { }
            var proexistintable = (from x in db.CustomerShoppingCarts
                                   where x.CustomerUserName == uname
                                   select x).OrderByDescending(z => z.DateTime).ToList();
            //Response.Write("<script>alert('Removed')</script>");
            id = 0;

            return RedirectToAction("MyCart", proexistintable);
            //return View("MyCart",proexistintable);
        }
        public ActionResult Next()
        {
            Session["NavbarHide"] = true;
            string uname = Session["UserName"].ToString();
            Session["TotalAmount"] = (from x in db.CustomerShoppingCarts
                                      where x.CustomerUserName == uname
                                      select x.ItemPrice * x.NoOfItem).Sum().ToString();
            var proexistintable = (from x in db.CustomerShoppingCarts
                                   where x.CustomerUserName == uname
                                   select x).OrderByDescending(z => z.DateTime).ToList();
            return View("CartListView", proexistintable);
        }
        public ActionResult OrderSummry()
        {
            string uname = Session["UserName"].ToString();
            var ob=new OrderPlacedModel();
             ob.CartInfo= (from x in db.CustomerShoppingCarts
                                   where x.CustomerUserName == uname
                                   select x).OrderByDescending(z => z.DateTime).ToList();
             ob.PaymentSystem = db.PaymentSystemTables.ToList();
             ob.CustAdress = db.CustomerAddresses.Where(x => x.UserName == uname).ToList();
            
            return View(ob);
        }
        public ActionResult Order(OrderPlacedModel ob)
        {
            
           
            string Uname = Session["UserName"].ToString();
            var cartinfo = db.CustomerShoppingCarts.Where(x => x.CustomerUserName == Uname).ToList(); 
            var addressId=db.CustomerAddresses.Where(x=>x.id==ob.CustAddressId).SingleOrDefault();
           
            var TypeofPayment=db.PaymentSystemTables.Where(x=>x.PaymentMethodsId==ob.PaymentTypeId).Select(x=>x.PaymentMethods).SingleOrDefault();
            foreach(var n in cartinfo)
            {
                OrderAddress orad = new OrderAddress();
                OrderTable ortable = new OrderTable();
                Thread.Sleep(100);
                orad.OrderId=ortable.OrderId = DateTime.Now.Ticks;
                ortable.Price = n.ItemPrice;
                ortable.NoOfItem = n.NoOfItem;
                ortable.UserName = Uname;
                orad.Address = addressId.Address;
                orad.City = addressId.City;
                orad.State=addressId.State;
                var ad = (decimal)addressId.PinCode;
                orad.PinCode=ad;
                orad.UserName=Uname;
                ortable.OrderAddress = "" + addressId.AddressType + "," + addressId.Address + "," + addressId.City + "," + addressId.State + "," + addressId.PinCode;
                ortable.TypeOfPaymentMethod = TypeofPayment;
                ortable.ItemId = n.ItemId;
                db.OrderTables.Add(ortable);
                db.OrderAddresses.Add(orad);
                var EmailAddress = db.UserRegistrations.Where(x => x.UserName == Uname).Select(x => x.EmailId).SingleOrDefault();
                string Mailbody = this.createEmailBody(n.ItemName, n.ItemPrice.ToString(),orad.OrderId.ToString(),n.NoOfItem.ToString());
                
                db.SaveChanges();
                try
                {
                    this.MailSend(EmailAddress, Mailbody);
                }
                catch(Exception)
                {
                   
                }
            }
            var deletedata = db.CustomerShoppingCarts.Where(x => x.CustomerUserName == Uname).ToList();
            foreach(var n in deletedata )
            {
                
                db.CustomerShoppingCarts.Remove(n);
                db.SaveChanges();
            }
            return RedirectToAction("YourOrder", "Account");
        }
        private string createEmailBody(string ProductName, string Price,string OrdarId,string NoofItem)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/Views/NotificationOrder/HtmlPage1.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{NoofItem}", NoofItem);
            body = body.Replace("{OrderId}", OrdarId);
            body = body.Replace("{ProductName}", ProductName);
            body = body.Replace("{Price}", Price);
            return body;

        }  
        private void MailSend(string EmailAddress, string body)
        {
          
            MailMessage msg = new MailMessage();
            MailAddress fm = new MailAddress("Your Gmail Id", "EStore");
            msg.From = fm;
            MailAddress to = new MailAddress("" + EmailAddress);
            msg.To.Add(to);
            msg.Subject = "Order Confirmation";
            msg.IsBodyHtml = true;
            msg.Body = body;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            NetworkCredential nkc = new NetworkCredential("Your Gmail Id", "Password");
            smtp.Credentials = nkc;
            smtp.EnableSsl = true;
            smtp.Send(msg);
        }
    }
}