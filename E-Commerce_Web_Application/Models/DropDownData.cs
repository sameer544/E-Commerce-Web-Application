using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Commerce_Web_Application.Models
{
    public class DropDownData
    {
        private Entities db = new Entities();
        public List<NavBarItem> MobileName()
        {
            return (db.NavBarItems.SqlQuery("select * from NavbarItems where ProductTypeName='Mobile' and ProductTypeId=11").ToList());
        }
        public List<NavBarItem> MobileAccessories()
        {
            return (db.NavBarItems.SqlQuery("Select * from NavbarItems where ProductTypeName='Mobile Accessories' and producttypeId=12").ToList());

        }
        public List<NavBarItem> Laptop()
        {
            return (db.NavBarItems.SqlQuery("Select * from navbarItems where ProductTypeName='Laptop' and ProducttypeId=13").ToList());
        }
        public List<NavBarItem> ComputerAccessories()
        {
            return (db.NavBarItems.SqlQuery("select * from navbarItems where producttypename='computer accessories' and producttypeid=14").ToList());
        }
        public List<NavBarItem> TelevisionBysize()
        {
            return (db.NavBarItems.SqlQuery("select * from navbaritems where producttypename='Televisionsbysize' and producttypeId=15").ToList());
        }
        public List<NavBarItem> Televisionsbybrand()
        {
            return (db.NavBarItems.SqlQuery("select * from navbaritems where producttypename='televisionsbybrand' and producttypeid=13").ToList());
        }
        public List<NavBarItem> Washingmachine()
        {
            return (db.NavBarItems.SqlQuery("select * from navbaritems where producttypename='washing machine' and producttypeid=14").ToList());
        }
        public List<NavBarItem> refrigerators()
        {
            return (db.NavBarItems.SqlQuery("select * from navbaritems where producttypename='Refrigerators' and producttypeid=15").ToList());
        }
        public List<NavBarItem> AirConditioners()
        {
            return (db.NavBarItems.SqlQuery("select * from navbaritems where producttypename='Air Conditioners' and producttypeid=18").ToList());
        }
        public List<NavBarItem> Kitchenappliances()
        {
            return (db.NavBarItems.SqlQuery("select * from navbaritems where producttypename='kitchen Appliances' and producttypeid=15").ToList());
        }
        public List<NavBarItem> Footwear()
        {
            return (db.NavBarItems.Where(x => x.ProductTypeId == 20).ToList());
        }
        public List<NavBarItem> TopWaer()
        {
            return (db.NavBarItems.Where(x => x.ProductTypeId == 16).ToList());
        }
        public List<NavBarItem> Bottomwear()
        {
            return (db.NavBarItems.Where(x => x.ProductTypeId == 17).ToList());
        }
        public IEnumerable<NavBarItem> WomenEthnicWear()
        {
            return (db.NavBarItems.Where(x => x.ProductTypeId == 18).ToList());
        }
        public IEnumerable<NavBarItem> WomenFootWear()
        {
            return (db.NavBarItems.Where(x=>x.ProductTypeId==24).ToList());
        }
        public IEnumerable<NavBarItem> kidsBabyBoyClothing()
        {
            return (db.NavBarItems.Where(x=>x.ProductTypeId==25).ToList());
        }
        public IEnumerable<NavBarItem> KidsBabyGirlClothing()
        {
            return (db.NavBarItems.Where(x=>x.ProductTypeId==26).ToList());
        }
        public IEnumerable<NavBarItem> KidsToys()
        {
            return (db.NavBarItems.Where(x=>x.ProductTypeId==27).ToList());
        }
        public IEnumerable<NavBarItem> Homefurnitur()
        {
            return (db.NavBarItems.Where(x=>x.ProductTypeId==28).ToList());
        }
        public IEnumerable<NavBarItem> HomeDecor()
        {
            return (db.NavBarItems.Where(x=>x.ProductTypeId==29).ToList());
        }
        public IEnumerable<NavBarItem> Books()
        {
            return (db.NavBarItems.Where(x=>x.ProductTypeId==30).ToList());
        }
    }
}