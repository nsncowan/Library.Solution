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

    public async Task<ActionResult> Details()
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      Console.WriteLine("LOOK HERE");
      Console.WriteLine(userId);
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      Patron thisPatron = _db.Patrons
                            .Include(patron => patron.CheckoutJoinEntities)
                            .ThenInclude(checkout => checkout.Copy)
                            .ThenInclude(copy=>copy.Book)
                            .FirstOrDefault(patron => patron.User.Id == currentUser.Id);
      return View(thisPatron);


      // List<Book> userBooks = _db.Copies
      //                         .Where(copy => copy.User.Id == currentUser.Id)
      //                         .Include(copy => copy.Checkout)
      //                         .ToList();
      // return View(userBooks);
    }

  //    public ActionResult Details(int id)
  //   {
  //     Patron thisPatron = _db.Patrons
  //                             .Include(patron => patron.CheckoutJoinEntities)
  //                             .ThenInclude(checkout => checkout.Copy)
  //                             .ThenInclude(copy=>copy.Book)
  //                             .FirstOrDefault(patron => patron.PatronId == id);
  //     return View(thisPatron);
  // }
  }
  }