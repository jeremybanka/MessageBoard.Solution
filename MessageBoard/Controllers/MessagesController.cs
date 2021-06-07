using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MessageBoard.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageBoard.Controllers
{
  [ApiVersion("0.0")]
  [Route("api/{v:apiVersion}/[controller]")]
  [ApiController]
  public class MessagesController : ControllerBase
  {
    private readonly MessageBoardContext _db;

    public MessagesController(MessageBoardContext db)
    {
      _db = db;
    }

    private bool MessageExists(int id) => _db.Messages.Any(a => a.MessageId == id);

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Message>>> Get()
      => await _db.Messages.ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Message>> GetMessage(int id)
    {
      var message = await _db.Messages.FindAsync(id);
      if (message == null) return NotFound();
      return message;
    }

    [HttpPost]
    public async Task<ActionResult<Message>> Post(Message m)
    {
      _db.Messages.Add(m);
      await _db.SaveChangesAsync();
      return CreatedAtAction(nameof(GetMessage), new { id = m.MessageId }, m);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Message m)
    {
      if (id != m.MessageId) return BadRequest();

      _db.Entry(m).State = EntityState.Modified;

      try
      {
        await _db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!MessageExists(id)) return NotFound();
        else throw;
      }
      return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMessage(int id)
    {
      var m = await _db.Messages.FindAsync(id);
      if (m == null) return NotFound();

      _db.Messages.Remove(m);
      await _db.SaveChangesAsync();

      return NoContent();
    }
  }
}
