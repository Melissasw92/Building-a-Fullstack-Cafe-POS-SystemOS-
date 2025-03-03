using System;

namespace FourthWallCafe.MVC.Models;

    public class ItemPrice
    {
        public int ItemPriceID { get; set; }
        public decimal Price { get; set; }
        public int TimeOfDayID { get; set; }  // Foreign key for TimeOfDay
        public int ItemID { get; set; }       // Foreign key for Item

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public TimeOfDay TimeOfDay { get; set; }   // Navigation Property
        public Item Item { get; set; }             // Navigation Property for Item
    }



