using System;

namespace FourthWallCafe.MVC.Models;

public class UpdateServerRequest
{
    public int ServerID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime? TermDate { get; set; }
}
