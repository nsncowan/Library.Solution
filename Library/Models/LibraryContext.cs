using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Library.Models
{
  public class LibraryContext : IdentityDbContext<ApplicationUser>
  {
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Patron> Patrons { get; set; }
    public DbSet<Copy> Copies { get; set; }
    public DbSet<Checkout> Checkouts { get; set; }
    public DbSet<AuthorBook> AuthorBooks { get; set; }

    public LibraryContext(DbContextOptions options) : base(options) { }
  }
}