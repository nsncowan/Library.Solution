using System.Collections.Generic;

namespace Library.Models
{
  public class Author
  {
    public int AuthorId { get; set; }
    public string Name { get; set; }
    public List<AuthorBook> AuthorBookJoinEntities { get;}
  }
}