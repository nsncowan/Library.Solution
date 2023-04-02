using Microsoft.AspNetCore.Mvc;
using Library.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Library.Controllers
{
  [Authorize]
    public class PatronsController : Controller
  {
    private readonly LibraryContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public PatronsController(UserManager<ApplicationUser> userManager, LibraryContext db)
    {
      _userManager = userManager;
      _db = db;
    }

     public ActionResult Index()
    {
      List<Patron> model = _db.Patrons
                            .Include(patron => patron.CheckoutJoinEntities)
                            .ToList();
      return View(model);
    }

    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Create(Patron patron)
    {
      _db.Patrons.Add(patron);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      Patron thisPatron = _db.Patrons
                              .Include(patron => patron.CheckoutJoinEntities)
                              .ThenInclude(checkout => checkout.Copy)
                              .ThenInclude(copy=>copy.Book)
                              .FirstOrDefault(patron => patron.PatronId == id);
      return View(thisPatron);
    }
  }
}