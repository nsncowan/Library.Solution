using System.Collections.Generic;

namespace Library.Models
{
  public class Patron
  {
    public int PatronId { get; set; }
    public string Name { get; set; }
    public List<Checkout> CheckoutJoinEntities { get; }
    public ApplicationUser User { get; set; }
  }
}