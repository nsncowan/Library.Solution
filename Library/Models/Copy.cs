using System.Collections.Generic;

namespace Library.Models
{
  public class Copy
  {
    public int CopyId { get; set; }
    public int BookId { get; set; }
    public Book Book { get; set; }
    public Checkout Checkout { get; set; }
  }
}