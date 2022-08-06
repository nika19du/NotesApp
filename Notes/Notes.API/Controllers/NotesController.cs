using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Notes.API.Data;
using Notes.API.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.API.Controllers
{
    [ApiController] //its api controller without views
    [Route("api/[controller]")]
    public class NotesController : Controller
    {
        public NotesController(NotesDbContext notesDbContext)
        {
            this.notesDbContext = notesDbContext;
        }
        private readonly NotesDbContext notesDbContext;

        [HttpGet]
        public async Task<IActionResult> GetAllNotes()
        {
            //Get the notes from database
            return Ok(await notesDbContext.Notes.ToListAsync());
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [ActionName("GetNoteById")]
        public async Task<IActionResult> GetNoteById([FromRoute] Guid id)
        {
            //await notesDbContext.Notes.FirstOrDefault(x => x.Id == id);
            //or
            var note = await notesDbContext.Notes.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }
            return Ok(note);
        }

        [HttpPost]
        public async Task<IActionResult> AddNote(Note note)
        {
            note.Id = Guid.NewGuid();
            await notesDbContext.Notes.AddAsync(note);
            await notesDbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetNoteById),new { id=note.Id},note);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateNote([FromRoute] Guid id,[FromBody] Note updatedNote)
        {
            var existingNote = await notesDbContext.Notes.FindAsync(id);

            if (existingNote == null)
            {
                return NotFound();
            }
            existingNote.Title = updatedNote.Title;
            existingNote.Description = updatedNote.Description;
            existingNote.isVisible = updatedNote.isVisible;

            await notesDbContext.SaveChangesAsync();
            return Ok(existingNote);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteNoe([FromRoute] Guid id)
        {
            var existingNote = await notesDbContext.Notes.FindAsync(id);

            if (existingNote == null)
            {
                return NotFound();
            }

            notesDbContext.Notes.Remove(existingNote);
            await notesDbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
