using System;

namespace FourthWallCafe.MVC.Models;

public class TimeOfDay
{
    public int TimeOfDayID { get; set; }
    public string TimeOfDayName { get; set; }  // This holds the name
    public ICollection<ItemPrice> ItemPrices { get; set; }
}

