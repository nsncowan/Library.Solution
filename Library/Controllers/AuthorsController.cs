using Microsoft.AspNetCore.Mvc;
using Library.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System;


namespace Library.Controllers
{
  public class AuthorsController : Controller
  {
    private readonly LibraryContext _db;
  
    public AuthorsController(LibraryContext db)
    {
      _db = db;
    }
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
        _db.AuthorBooks.Add(new AuthorBook() {BookId = bookId, AuthorId = authorId});
        _db.SaveChanges();
      }
    }
    public ActionResult GetAuthorName(int authorId)
    {
      // List<Author> authors = _db.Authors.ToList().Where(author => author.AuthorId == authorId).ToList();
      Author thisAuthor = _db.Authors.FirstOrDefault(author => author.AuthorId == authorId);
      var json = JsonSerializer.Serialize(thisAuthor);
      Console.WriteLine(authorId);
      Console.WriteLine(thisAuthor.Name);
      return Json(json);
    }
  }
}