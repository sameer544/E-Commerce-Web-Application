using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Commerce_Web_Application.Models
{
    public class ViewModels
    {
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[a-zA-Z]\w+|[0-9][0-9_]*[a-zA-Z]+\w*$", ErrorMessage = "User Name Can't Have Spaces")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$", ErrorMessage = "Minimum eight characters, at least one letter and one number")]
        public string Password { get; set; }
        public string Result { get; set; }
        public int ItemId { get; set; }
        [Required]
        [Display(Name="Enter Product Name")]
        public string ItemName { get; set; }
        [Display(Name = "Brand Name")]
        public string ItemBrandName { get; set; }
        [Display(Name = "Brand Id")]
        public Nullable<int> ItemBrandId { get; set; }
        [Display(Name = "Product Type")]
        public string ProductTypeName { get; set; }
        [Display(Name = "Product Type Id")]
        public Nullable<int> ProductTypeId { get; set; }

        public Nullable<int> ItemImageId { get; set; }
        [Display(Name = "Set Price")]
        public Nullable<decimal> ItemPrice { get; set; }
        [Display(Name = "Discount")]
        public Nullable<decimal> ItemDiscountPriceInPercentage { get; set; }
        [Display(Name = "Upload Image")]
        public string ItemImageName { get; set; }
        public Nullable<System.DateTime> DateTime { get; set; }
        public List<ItemTable> ItemTable { get; set; }
        public Dictionary<string, List<ItemDetailsTable>> SpecType { get; set; }
 
        public List<ItemTable> ProductInfo { get; set; }
        public string ImageName { get; set; }
        public int CountItemInCart { get; set; }
        public List<UserRegistration> CustomerInfo { get; set; }
        public int count(string uname)
        {
            Entities ob = new Entities();
            return ((from x in ob.CustomerShoppingCarts
                     where x.CustomerUserName == uname
                     select x).Count());
        }
        public int NoofItem { get; set; }
        public List<WishlistTable> Wishlist { get; set; }
        public List<SearchTable> TagsTables { get; set; }
        public IEnumerable<PaymentSystemTable> PaymentSystem { get; set; }
        public string PaymentTypeName { get; set; }
        public int PaymentTypeId { get; set; }
        public IEnumerable<CustomerShoppingCart> CustomerShoppingCartTables { get; set; }
        public IEnumerable<CustomerAddress> CutmAddress { get; set; }
    }
}