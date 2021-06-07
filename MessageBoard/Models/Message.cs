using Microsoft.AspNetCore.Identity;

using System;

namespace MessageBoard.Models
{
  public class Message
  {
    public Message()
    {
      PostedWhen = DateTime.Now;
    }
    public int MessageId { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public string ImageUrl { get; set; }
    public DateTime PostedWhen { get; set; }
    public string UserId { get; set; }
    public virtual IdentityUser User { get; set; }
  }
}