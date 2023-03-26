using Microsoft.AspNetCore.Mvc;
using Library.Models;
using System.Collections.Generic;
using System.Linq;

namespace Library.Controllers
{
  public class HomeController : Controller
  {
    private readonly LibraryContext _db;

    public HomeController(LibraryContext db)
    {
      _db = db;
    }

    [HttpGet("/")]
    public ActionResult Index()
    {
      return View();
    }
  }
}