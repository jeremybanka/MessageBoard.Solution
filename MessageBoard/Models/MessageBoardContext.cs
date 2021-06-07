using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MessageBoard.Models
{
  public class MessageBoardContext : IdentityDbContext<IdentityUser>
  {
    public MessageBoardContext(DbContextOptions<MessageBoardContext> options)
      : base(options)
    {
    }

    public DbSet<Message> Messages { get; set; }
  }
}