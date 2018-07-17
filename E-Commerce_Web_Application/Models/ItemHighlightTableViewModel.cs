using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Commerce_Web_Application.Models
{
    public class ItemHighlightTableViewModel
    {
        
        public int Id { get; set; }
        public int HighlightId { get; set; }
        public string HighlightValue { get; set; }
        public Nullable<int> ItemId { get; set; }
    }
}