using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Library.Models
{
  public class Book
  {
    public int BookId { get; set; }
    public string Title { get; set; }
    public List<Copy> Copies { get; set; }
    public List<AuthorBook> AuthorBookJoinEntities { get; }
  }
}