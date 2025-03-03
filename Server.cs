using System;

namespace FourthWallCafe.MVC.Models;

public class Server
{
    public int ServerId {get; set;}
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DoB { get; set; }

    public DateTime HireDate {get; set;}
    public DateTime? TermDate {get; set;}
    public ICollection<CafeOrder> Orders { get; set; }

}
