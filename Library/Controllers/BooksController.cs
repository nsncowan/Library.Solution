using Microsoft.AspNetCore.Mvc;
using Library.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;



namespace Library.Controllers
{
  [Authorize]
  public class BooksController : Controller
  {
    private readonly LibraryContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public BooksController(UserManager<ApplicationUser> userManager, LibraryContext db)
    {
      _userManager = userManager;
      _db = db;
    }

    // public async Task<ActionResult> Index()
    // {
    //   string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    //   ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
    //   List<Book> model = _db.Books
    //                         .Where(entry => entry.User.Id == currentUser.Id)
    //                         .Include(book => book.AuthorBookJoinEntities)
    //                         .ToList();
    //   return View(model);
    // }

    public ActionResult Index()
    {
      List<Book> model = _db.Books
                            .Include(book => book.AuthorBookJoinEntities)
                            .ToList();
      return View(model);
    }

    public ActionResult Create()
    {
      ViewBag.AuthorList = _db.Authors.ToList();
      return View();
    }

    [HttpPost]
    public ActionResult Create(string title, int author, int copies)
    {
      Book thisBook = new Book();
      thisBook.Title = title;
      _db.Books.Add(thisBook);
      _db.SaveChanges();

      for(int i=1;i<=copies;i++){
        Copy thisCopy = new Copy();
        thisCopy.BookId = thisBook.BookId;
        _db.Copies.Add(thisCopy);
      }
      _db.SaveChanges();

      AddAuthorToBook(thisBook.BookId, author);
      return RedirectToAction("Index");
    }

    public void AddAuthorToBook(int bookId, int authorId)
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

    public ActionResult Details(int id)
    {
      Book thisBook = _db.Books
                          .Include(book => book.Copies)
                          .ThenInclude(copy => copy.Checkout)
                          .Include(book => book.AuthorBookJoinEntities)
                          .ThenInclude(join => join.Author)
                          .FirstOrDefault(book => book.BookId == id);

      //Count the number of Checkouts that are null 
      // *THIS ALSO WORKS : ViewBag.AvailableCopies = thisBook.Copies.Where(copy => copy.Checkout == null).ToList().Count;
      return View(thisBook);
    }

    public ActionResult Edit(int id)
    {
      Book thisBook = _db.Books
                          .Include(book => book.AuthorBookJoinEntities)
                          .ThenInclude(join => join.Author)
                          .FirstOrDefault(book => book.BookId == id);
      ViewBag.AuthorList = _db.Authors.ToList();
      return View(thisBook);
    }

    [HttpPost]
    public ActionResult Edit(Book book)
    {
      _db.Books.Update(book);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      Book thisBook = _db.Books.FirstOrDefault(book => book.BookId == id);
      return View(thisBook);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Book thisBook = _db.Books.FirstOrDefault(book => book.BookId == id);
      _db.Books.Remove(thisBook);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult DeleteJoin(int joinId)
    {
      AuthorBook joinEntry = _db.AuthorBooks.FirstOrDefault(entry => entry.AuthorBookId == joinId);
      _db.AuthorBooks.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult AddBookAuthors(int bookId, int author)
    {
      AddAuthorToBook(bookId, author);
      return RedirectToAction("Details", new { id = bookId });
    }

    public ActionResult Checkout(int id)
    {
      ViewBag.PatronList = new SelectList(_db.Patrons, "PatronId", "Name");
      Book thisBook = _db.Books.FirstOrDefault(book => book.BookId == id);
      return View(thisBook);
    }

    [HttpPost]
    public ActionResult Checkout(int bookId, int patronList)
    {
      // int bookId = book.BookId;
      Copy thisCopy = _db.Copies
                          .Include(copy => copy.Checkout)
                          .FirstOrDefault(copy => copy.BookId == bookId && copy.Checkout == null);
      int copyId = thisCopy.CopyId;
      // _db.Checkouts.Add(new Checkout() {CopyId = thisCopy.CopyId, PatronId = patronId, DueDate = new DateTime(2023, 4, 20) });
      // _db.SaveChanges();
      // return RedirectToAction("Index");

      #nullable enable
      Checkout? joinEntity = _db.Checkouts.FirstOrDefault(join => join.CopyId == copyId && join.PatronId == patronList);
      if (joinEntity == null && copyId != 0 && patronList != 0)
      #nullable disable
      {
        _db.Checkouts.Add(new Checkout() { CopyId = copyId, PatronId = patronList });
        _db.SaveChanges();
      }
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult ReturnBook(int checkoutId)
    {
      Checkout thisCheckout = _db.Checkouts.FirstOrDefault(checkout => checkout.CheckoutId == checkoutId);
      int thisPatronId = thisCheckout.PatronId;
      _db.Checkouts.Remove(thisCheckout);
      _db.SaveChanges();
      return RedirectToAction("Details", "Patrons", new {id = thisPatronId});
    }
  }
}