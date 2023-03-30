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
  public class AuthorsController : Controller
  {
    private readonly LibraryContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthorsController(UserManager<ApplicationUser> userManager, LibraryContext db)
    {
      _userManager = userManager;
      _db = db;
    }
    // public async Task<ActionResult> Index()
    // {
    //   string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    //   ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
    //   List<Author> model = _db.Authors
    //                         .Where(entry => entry.User.Id == currentUser.Id)
    //                         .Include(author => author.AuthorBookJoinEntities)
    //                         .ToList();
    //   return View(model);
    // }

    public ActionResult Index()
    {
      List<Author> model = _db.Authors
                            .Include(author => author.AuthorBookJoinEntities)
                            .ToList();
      return View(model);
    }

    public ActionResult Details(int id)
    {
      Author thisAuthor = _db.Authors
                          .Include(author => author.AuthorBookJoinEntities)
                          .ThenInclude(join => join.Book)
                          .FirstOrDefault(author => author.AuthorId == id);
      return View(thisAuthor);
    }

    public ActionResult Create()
    {
      //ViewBag.BookList = _db.Books.ToList();
      return View();
    }

    [HttpPost]
    public ActionResult Create(Author author)
    {
      // Book thisBook = new Book ();
      // thisBook.BookId = id;
      // thisBook.Title = title;
      // _db.Books.Add(thisBook);
      // _db.SaveChanges();
      // AddBookToAuthor(id, authorId);
      _db.Authors.Add(author);
      _db.SaveChanges();

      return RedirectToAction("Index", "Home");
    }
    public void AddBookToAuthor(int bookId, int authorId)
    {
#nullable enable
      AuthorBook? joinEntity = _db.AuthorBooks.FirstOrDefault(join => join.BookId == bookId && join.AuthorId == authorId);
      if (joinEntity == null && authorId != 0 && bookId != 0)
#nullable disable
      {
        _db.AuthorBooks.Add(new AuthorBook() { BookId = bookId, AuthorId = authorId });
        _db.SaveChanges();
      }
    }
    public ActionResult GetAuthorName(int id)
    {
      // List<Author> authors = _db.Authors.ToList().Where(author => author.AuthorId == authorId).ToList();
      Author thisAuthor = _db.Authors.FirstOrDefault(author => author.AuthorId == id);
      var json = JsonSerializer.Serialize(thisAuthor);
      return Json(json);
    }
  }
}