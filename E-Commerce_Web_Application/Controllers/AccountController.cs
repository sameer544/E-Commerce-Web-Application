using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using E_Commerce_Web_Application.Models;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_Web_Application.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/
        private Entities db = new Entities();
        public ActionResult LoginModal()
        {
           
            
                Session["LoginPage"] = "true";
                return Redirect(Request.UrlReferrer.ToString());
            
        }
        [HttpPost]

        public ActionResult LoginModal(ViewModels ob)
        {

            string Password = CalculateMD5Hash(ob.Password);
            var Unam = db.UserRegistrations.Where(x => x.UserName == ob.UserName || x.EmailId == ob.UserName).SingleOrDefault();

            if (Unam != null && (ob.UserName == Unam.UserName || ob.UserName.ToUpper() == Unam.EmailId.ToUpper()))
            {
                var login = db.UserRegistrations.Where(x => x.UserName == Unam.UserName && x.Password == Password).SingleOrDefault();

                if (login == null && Unam.AccountStatus != false)
                {
                    return View("_Error2");
                }
                else
                {
                    if (Unam.AccountStatus == false)
                    {
                        return View("Status");
                    }
                    else
                    {
                        if (Session["NavbarHide"] != null)
                        {
                            Session["Authentication"] = "true";
                            Session["UserName"] = Unam.UserName;

                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            Session["Authentication"] = "true";
                            Session["UserName"] = Unam.UserName;
                            //return RedirectToAction("Index", "Home");
                            return Redirect(Request.UrlReferrer.ToString());
                        }
                    }
                }

            }
            else
                return View("Error", ob);
        }
         [HttpPost]
       
        public string CalculateMD5Hash(string input)
        {

            // step 1, calculate MD5 hash from input

            MD5 md5 = System.Security.Cryptography.MD5.Create();

            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);

            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {

                sb.Append(hash[i].ToString("X2"));

            }

            return sb.ToString();

        }
        public ActionResult Registration()
        {
            Session["NavbarHide"] = "true";
            Session["RegistrationPage"] = "true";
            return View();
        }
        [HttpPost]
        public ActionResult Registration(UserRegistration ob)
        {
            Session["RegistrationPage"] = null;
            try
            {
                var token = db.UserRegistrations.Max(x => x.VerificationToken);
                ob.VerificationToken = RandomNumber(token);
            }
            catch (Exception)
            {
                ob.VerificationToken = RandomNumber(0);
            }
            try
            {
                ob.Password = CalculateMD5Hash(ob.Password);
                db.UserRegistrations.Add(ob);
                db.SaveChanges();
                MailSend(ob.EmailId, ob.VerificationToken);
            }
            catch (Exception)
            {
                return View("_Error1");
            }
            return View("Status");
        }
        public decimal RandomNumber(decimal Token)
        {
            if (Token == 0)
            {
                Token = 100000000 + 1;
            }
            else
            {
                Token++;
            }
            return Token;
        }
        public void MailSend(string EmailAddress, decimal Token)
        {
            string tokens = Token.ToString();
            tokens = CalculateMD5Hash(tokens);
            MailMessage msg = new MailMessage();
            MailAddress fm = new MailAddress("Gmail Id", "EStore");
            msg.From = fm;
            MailAddress to = new MailAddress("" + EmailAddress);
            msg.To.Add(to);
            msg.Subject = "Email Varification";
            msg.Body = @"Click Here:http://localhost:16182//Account/Verify/" + tokens;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            NetworkCredential nkc = new NetworkCredential("Gamil Id", "Password");
            smtp.Credentials = nkc;
            smtp.EnableSsl = true;
            smtp.Send(msg);
        }
        public ActionResult YourAccount(string SaveAndCancel)
        {
            if (Session["UserName"] != null)
            {
                ViewUserInfoModel ob = new ViewUserInfoModel();
                string uname = Session["UserName"].ToString();
                var urinfo = (from x in db.UserRegistrations
                              where x.UserName == uname
                              select x).SingleOrDefault();
                ob.FirstName = urinfo.FirstName;
                ob.LastName = urinfo.LastName;
                ob.EmailId = urinfo.EmailId;
                if (urinfo.Gender == "M")
                    ob.Gender = "Male";
                else
                    ob.Gender = "Female";
                return View(ob);
            }
            else
                return RedirectToAction("Login");
        }
        [HttpPost]
        public ActionResult YourAccount(UserRegistration ob)
        {

            return View();
        }

        [HttpPost]
        public PartialViewResult EditName(string SaveAndCancel, ViewUserInfoModel ob)
        {
            if (SaveAndCancel == "Edit")
            {

                string uname = Session["UserName"].ToString();
                var urinfo = (from x in db.UserRegistrations
                              where x.UserName == uname
                              select x).SingleOrDefault();
                ob.FirstName = urinfo.FirstName;
                ob.LastName = urinfo.LastName;
                ob.Type = "Edit";
                return PartialView("_EditName", ob);
            }
            else if (SaveAndCancel == "Cancel")
            {
                string uname = Session["UserName"].ToString();
                var urinfo = (from x in db.UserRegistrations
                              where x.UserName == uname
                              select x).SingleOrDefault();
                ob.FirstName = urinfo.FirstName;
                ob.LastName = urinfo.LastName;
                ob.Type = "Cancel";
                return PartialView("_EditName", ob);
            }
            else if (SaveAndCancel == "Save")
            {
                string uname = Session["UserName"].ToString();
                var urinfo = (from x in db.UserRegistrations
                              where x.UserName == uname
                              select x).SingleOrDefault();
                urinfo.FirstName = ob.FirstName;
                urinfo.LastName = ob.LastName;
                db.SaveChanges();
                ob.Type = "Save";
                return PartialView("_EditName", ob);
            }
            else
                return PartialView("_EditName", ob);
        }
        [HttpPost]
        public PartialViewResult EditGender(string SaveAndChange, ViewUserInfoModel ob)
        {
            string uname = Session["UserName"].ToString();
            if (SaveAndChange == "Cancel")
            {
                ob.Type = "Cancel";
                ob.Gender = db.UserRegistrations.Where(x => x.UserName == uname).Select(x => x.Gender).Single();
                return PartialView("_PartialPageGenderEdit", ob);
            }
            else
                if (SaveAndChange == "Edit")
                {
                    ob.Type = "Edit";
                    var row = db.UserRegistrations.Where(x => x.UserName == uname).Single();
                    ob.Gender = row.Gender;
                    return PartialView("_PartialPageGenderEdit", ob);
                }
                else
                {
                    ob.Type = "Save";
                    var row = db.UserRegistrations.Where(x => x.UserName == uname).Single();
                    row.Gender = ob.Gender;
                    db.SaveChanges();
                    ob.Gender = db.UserRegistrations.Where(x => x.UserName == uname).Select(x => x.Gender).Single();
                    return PartialView("_PartialPageGenderEdit", ob);
                }
        }
        [HttpPost]
        public PartialViewResult EditEmail(String SaveAndCancelEmail, ViewUserInfoModel ob)
        {
            string uname = Session["UserName"].ToString();
            if (SaveAndCancelEmail == "Save")
            {
                ob.Type = "Save";
                var row = db.UserRegistrations.Where(x => x.UserName == uname).Single();
                row.EmailId = ob.EmailId;
                db.SaveChanges();
                return PartialView("_PartialPageEmailEdit", ob);
            }
            else if (SaveAndCancelEmail == "Edit")
            {
                ob.Type = "Edit";
                ob.EmailId = db.UserRegistrations.Where(x => x.UserName == uname).Select(x => x.EmailId).Single();
                return PartialView("_PartialPageEmailEdit", ob);
            }
            else
            {
                ob.Type = "Cancel";
                ob.EmailId = db.UserRegistrations.Where(x => x.UserName == uname).Select(x => x.EmailId).Single();
                return PartialView("_PartialPageEmailEdit", ob);
            }
        }

        public ActionResult ChangePassword()
        {
            if (Session["Authentication"] == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordTableViewModel ob)
        {
            if (Session["Authentication"] != null)
            {
                string uname = Session["UserName"].ToString();
                string Yourpassword = CalculateMD5Hash(ob.YourPassword);
                var verifyuserpassword = db.UserRegistrations.Where(x => x.UserName == uname && x.Password == Yourpassword).SingleOrDefault();
                if (verifyuserpassword == null)
                {
                    Response.Write("<script>alert('Your Password Not Matched')</script>");
                    return View();
                }
                else
                {
                    verifyuserpassword.Password = CalculateMD5Hash(ob.NewPassword);
                    db.SaveChanges();
                    Response.Write("<script>alert('Your Password Change')</script>");
                    Session["Authentication"] = null;
                    Session["UserName"] = null;
                    return RedirectToAction("Index","Home");
                }
            }
            else
                return View();

        }
        public ActionResult AddressManage()
        {
            if (Session["UserName"] != null)
            {
                string unam = Session["UserName"].ToString();
                ViewModelCustomerAddressTable ca = new ViewModelCustomerAddressTable();
                ca.CustomerAddresses = db.CustomerAddresses.Where(x => x.UserName == unam).OrderByDescending(x => x.id).ToList();
                return View(ca);
            }
            else
            {
                return RedirectToAction("Index","Home");
            }
        }
        [HttpPost]
        public ActionResult AddAddressInTable()
        {
            if (Session["Authentication"] != null)
            {
                string unam = Session["UserName"].ToString();
                ViewModelCustomerAddressTable ca = new ViewModelCustomerAddressTable();
                ca.CustomerAddresses = db.CustomerAddresses.Where(x => x.UserName == unam).OrderByDescending(x => x.id).ToList();
                ca.Type = "";
                return PartialView("_PartialPageAddressAdd", ca);
            }
            else
                return RedirectToAction("Index","Home");
        }
        [HttpPost]
        public ActionResult AddAddressInTable1(string saveandchange, ViewModelCustomerAddressTable ob)
        {
            string unam = Session["UserName"].ToString();
            if (saveandchange == "Cancel")
            {
                Session["AddressId"] = null;
                ob.CustomerAddresses = db.CustomerAddresses.Where(x => x.UserName == unam).OrderByDescending(x => x.id).ToList();
                ob.Type = "Cancel";
                return PartialView("_PartialPageAddressAdd", ob);
            }
            if (saveandchange == "Save" && Session["AddressId"] != null)
            {
                if (ob.State != null && ob.City != null && ob.ModileNumber != null && ob.Name != null && ob.PinCode != null && ob.Address != null && ob.AddressType != null)
                {
                    int id = int.Parse(Session["AddressId"].ToString());


                    var EditInfo = db.CustomerAddresses.Where(x => x.id == id).Single();
                    EditInfo.Address = ob.Address;
                    EditInfo.City = ob.City;
                    EditInfo.PinCode = ob.PinCode;
                    EditInfo.Name = ob.Name;
                    EditInfo.State = ob.State;
                    EditInfo.AddressType = ob.AddressType;
                    EditInfo.ModileNumber = ob.ModileNumber;
                    EditInfo.UserName = unam;
                    db.SaveChanges();
                    ob.CustomerAddresses = db.CustomerAddresses.Where(x => x.UserName == unam).OrderByDescending(x => x.id).ToList();
                    ob.Type = "Save";
                    Session["AddressId"] = null;
                    return PartialView("_PartialPageAddressAdd", ob);
                }
                else
                {
                    ob.Type = "Fill";
                    ob.CustomerAddresses = db.CustomerAddresses.Where(x => x.UserName == unam).OrderByDescending(x => x.id).ToList();
                    return PartialView("_PartialPageAddressAdd", ob);
                }
            }
            if (saveandchange == "Save")
            {
                if (ob.State != null && ob.City != null && ob.ModileNumber != null && ob.Name != null && ob.PinCode != null && ob.Address != null && ob.AddressType != null)
                {

                    CustomerAddress saveinfointable = new CustomerAddress();
                    saveinfointable.Address = ob.Address;
                    saveinfointable.City = ob.City;
                    saveinfointable.PinCode = ob.PinCode;
                    saveinfointable.Name = ob.Name;
                    saveinfointable.State = ob.State;
                    saveinfointable.AddressType = ob.AddressType;
                    saveinfointable.ModileNumber = ob.ModileNumber;
                    saveinfointable.UserName = unam;
                    db.CustomerAddresses.Add(saveinfointable);
                    db.SaveChanges();
                    ob.CustomerAddresses = db.CustomerAddresses.Where(x => x.UserName == unam).OrderByDescending(x => x.id).ToList();
                    ob.Type = "Save";
                    return PartialView("_PartialPageAddressAdd", ob);
                }
                else
                {
                    ob.Type = "Fill";
                    ob.CustomerAddresses = db.CustomerAddresses.Where(x => x.UserName == unam).OrderByDescending(x => x.id).ToList();
                    return PartialView("_PartialPageAddressAdd", ob);
                }
            }

            return PartialView("_PartialPageAddressAdd");
        }
        public ActionResult RemoveAddress(int Id)
        {
            db.CustomerAddresses.Remove(db.CustomerAddresses.Where(x => x.id == Id).Single());
            db.SaveChanges();
            ViewModelCustomerAddressTable ob = new ViewModelCustomerAddressTable();
            string uname = Session["UserName"].ToString();
            ob.CustomerAddresses = db.CustomerAddresses.Where(x => x.UserName == uname).ToList();
            return PartialView("_PartialPageRemoveAddress", ob);
        }
        public ActionResult EditAddress(int Id, ViewModelCustomerAddressTable ob)
        {
            string uname = Session["UserName"].ToString();
            ob.CustomerAddresses = db.CustomerAddresses.Where(x => x.UserName == uname && x.id != Id).ToList();
            var Editinfo = db.CustomerAddresses.Where(x => x.UserName == uname && x.id == Id).Single();
            ob.City = Editinfo.City;
            ob.Address = Editinfo.Address;
            ob.AddressType = Editinfo.AddressType;
            ob.Name = Editinfo.Name;
            ob.PinCode = Editinfo.PinCode;
            ob.State = Editinfo.State;
            ob.ModileNumber = Editinfo.ModileNumber;
            ob.Type = "";
            Session["AddressId"] = Id;
            return PartialView("_PartialPageAddressAdd", ob);
        }
        public ActionResult Verify(string Id)
        {
            string username = FindUserName(Id);
            if (Id != null && !(from x in db.UserRegistrations where x.UserName == username select x.AccountStatus).Single())
            {

                var ob = (from x in db.UserRegistrations
                          where x.UserName == username
                          select x).SingleOrDefault();
                ob.AccountStatus = true;
                db.SaveChanges();

                if ((from x in db.UserRegistrations where x.UserName == username select x.AccountStatus).Single())
                {
                    return PartialView("_UserEmailVerificationPage");
                }
                else
                    return RedirectToAction("Registration");
            }
            else
            {
                return RedirectToAction("Index","Home");
            }
        }
        public string FindUserName(string TokenHashvalue)
        {
            var listusername = (from x in db.UserRegistrations
                                select x).OrderByDescending(z => z.VerificationToken).ToList();
            string UserName = "";
            foreach (var n in listusername)
            {
                string tk = n.VerificationToken.ToString();
                if (CalculateMD5Hash(tk) == TokenHashvalue)
                {
                    UserName = n.UserName;
                    break;
                }
            }
            return UserName;
        }
        
        public PartialViewResult WishlistFunction(int Id)
        {
            WishlistTable ob = new WishlistTable();
            if (Session["Authentication"] == null)
            {
                return PartialView("partial1");

            }
            else
            {

                ob.UserName = Session["UserName"].ToString();
                int? itid = (from x in db.WishlistTables
                             where x.ItemId == Id && x.UserName == ob.UserName
                             select x.ItemId).SingleOrDefault();
                if (itid == null)
                {

                    ob.ItemId = Id;
                    ob.UserName = Session["UserName"].ToString();
                    db.WishlistTables.Add(ob);
                    db.SaveChanges();
                    //Response.Write("<script>alert('Save In Wishlist')</script>");
                    return PartialView("_ColorChange");
                }
                else
                {

                    ob.UserName = Session["UserName"].ToString();
                    var revdata = (from x in db.WishlistTables
                                   where x.ItemId == Id && x.UserName == ob.UserName
                                   select x).Single();
                    db.WishlistTables.Remove(revdata);
                    db.SaveChanges();
                    //Response.Write("<script>alert('Remove')</script>");
                    return PartialView("Partial1");
                }

            }
        }
        public ActionResult Wishlist()
        {
            if (Session["Authentication"] != null)
            {
                string uname = Session["UserName"].ToString();
                var wishlist = db.WishlistTables.Where(x => x.UserName == uname).ToList();
                ViewModels ob = new ViewModels();
                ob.ItemTable = new List<ItemTable>();
                foreach (var n in wishlist)
                {
                    var info = db.ItemTables.Where(x => x.ItemId == n.ItemId).Single();
                    if (info != null)
                        ob.ItemTable.Add(new ItemTable() { ItemName = info.ItemName, ItemPrice = info.ItemPrice, ItemImageName = info.ItemImageName, ItemId = info.ItemId });
                }
                return View(ob);
            }
            else
            {
                return RedirectToAction("Login");
            }

        }
        public PartialViewResult RemoveFromWishlist(int Id)
        {
            string uname = Session["UserName"].ToString();
            if (Id != 0)
            {
                var deleteinfo = db.WishlistTables.Where(x => x.ItemId == Id && x.UserName == uname).Single();
                db.WishlistTables.Remove(deleteinfo);
                db.SaveChanges();
            }

            var wishlist = db.WishlistTables.Where(x => x.UserName == uname).ToList();
            ViewModels ob = new ViewModels();
            ob.ItemTable = new List<ItemTable>();
            foreach (var n in wishlist)
            {
                var info = db.ItemTables.Where(x => x.ItemId == n.ItemId).Single();
                if (info != null)
                    ob.ItemTable.Add(new ItemTable() { ItemName = info.ItemName, ItemPrice = info.ItemPrice, ItemImageName = info.ItemImageName, ItemId = info.ItemId });
            }
            return PartialView("_PartialWishlistPage", ob);
        }
        public ActionResult YourOrder()
        {
            String Uname = Session["UserName"].ToString();
            var Order = db.OrderTables.Where(x => x.UserName == Uname).ToList();
            return View(Order);
        }
        public ActionResult Cancel(decimal Id)
        {
            var DeleteAddress = db.OrderAddresses.Where(x => x.OrderId == Id).SingleOrDefault();
            var CancelOrder = db.OrderTables.Where(x => x.OrderId == Id).SingleOrDefault();
            db.OrderAddresses.Remove(DeleteAddress);
            db.OrderTables.Remove(CancelOrder);
            db.SaveChanges();
            String Uname = Session["UserName"].ToString();
            var Order = db.OrderTables.Where(x => x.UserName == Uname).ToList();
            return View("YourOrder", Order);
        }
    }
}