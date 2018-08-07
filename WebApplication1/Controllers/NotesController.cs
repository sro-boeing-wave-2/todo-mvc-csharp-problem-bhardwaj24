using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly NotesContext _context;

        public NotesController(NotesContext context)
        {
            _context = context;
        }

       //// GET: api/Notes
       //[HttpGet]
       // public IEnumerable<Note> GetNote()
       // {
       //     return _context.Note.Include(x => x.labellist).Include(y => y.check);
       // }

        // GET: api/Notes/5
        [HttpGet]
        public IActionResult GetNoteByPrimitive(
             [FromQuery(Name = "Id")] int Id,
             [FromQuery(Name = "Title")] string Title,
             [FromQuery(Name = "text")] string text,
             [FromQuery(Name = "Pinned")] bool Pinned)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            List<Note> temp = new List<Note>();
            temp = _context.Note.Include(x => x.check).Include(x => x.labellist)
                .Where(element => element.NoteTitle == ((Title == null) ? element.NoteTitle : Title)
                      && element.NoteContent == ((text == null) ? element.NoteContent : text)
                      && element.pinned == ((!Pinned) ? element.pinned : Pinned)
                      && element.Id == ((Id == 0) ? element.Id : Id)).ToList();


            if (temp == null)
            {
                return NotFound();
            }
            return Ok(temp);
        }
        //public async Task<IActionResult> GetNote([FromRoute] int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var note = await _context.Note.FindAsync(id);

        //    if (note == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(note);
        //}

        // PUT: api/Notes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNote([FromRoute] int id, [FromBody] Note note)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _context.Note.Include(x => x.labellist).Include(x => x.check).ForEachAsync(x =>
            {
                if (x.Id == note.Id)
                {
                    x.NoteTitle = note.NoteTitle;
                    x.NoteContent = note.NoteContent;
                    foreach (Labels y in note.labellist)
                    {
                        Labels a = x.labellist.Find(z => z.LabelId == y.LabelId);
                        if (a != null)
                        {
                            a.name = y.name;
                        }
                        else
                        {
                            Labels lab = new Labels() { name = y.name };
                            x.labellist.Add(lab);
                        }
                    }

                    foreach (checklist obj in note.check)
                    {
                        checklist c = x.check.Find(z => z.checklistId == obj.checklistId);
                        if (c != null)
                        {
                            c.listcontent = obj.listcontent;
                        }
                        else
                        {
                            checklist a = new checklist { listcontent = obj.listcontent};
                            x.check.Add(a);
                        }
                    }

                }
            });
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NoteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            //return CreatedAtAction(nameof(GetNoteByPrimitive), new
            //{
            //    note
            //});

            return Ok(note);
        }

        // POST: api/Notes
        [HttpPost]
        public async Task<IActionResult> PostNote([FromBody] Note note)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Note.Add(note);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNoteByPrimitive", new { id = note.Id }, note);
        }

        // DELETE: api/Notes/5
        [HttpDelete]
        public async Task<IActionResult> DeleteNote([FromQuery(Name = "Id")] int Id,
             [FromQuery(Name = "Title")] string Title,
             [FromQuery(Name = "text")] string text,
             [FromQuery(Name = "Pinned")] bool Pinned)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            List<Note> temp = new List<Note>();
            temp = _context.Note.Include(x => x.check).Include(x => x.labellist)
                .Where(element => element.NoteTitle == ((Title == null) ? element.NoteTitle : Title)
                      && element.NoteContent == ((text == null) ? element.NoteContent : text)
                      && element.pinned == ((!Pinned) ? element.pinned : Pinned)
                      && element.Id == ((Id == 0) ? element.Id : Id)).ToList();

            if (temp == null)
            {
                return NotFound();
            }

            //_context.Note.Remove(note);
            //await _context.SaveChangesAsync();

            //return Ok(note);
            temp.ForEach(NoteDel => _context.checklist.RemoveRange(NoteDel.check));
            temp.ForEach(NoteDel => _context.Labels.RemoveRange(NoteDel.labellist));
            _context.Note.RemoveRange(temp);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool NoteExists(int id)
        {
            return _context.Note.Any(e => e.Id == id);
        }
    }
}