using Microsoft.AspNetCore.Mvc;
using Library.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;


namespace Library.Controllers
{
  public class BooksController : Controller
  {
    private readonly LibraryContext _db;

    public BooksController(LibraryContext db)
    {
      _db = db;
    }

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
    public ActionResult Create(string title, int author)
    {
      Book thisBook = new Book();
      thisBook.Title = title;
      _db.Books.Add(thisBook);
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
                          .Include(book => book.AuthorBookJoinEntities)
                          .ThenInclude(join => join.Author)
                          .FirstOrDefault(book => book.BookId == id);
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
  }
}