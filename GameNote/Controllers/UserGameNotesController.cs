using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using GameNote.DataObjects;
using GameNote.Models;

namespace GameNote.Controllers
{
    public class UserGameNotesController : ApiController
    {
        private GameNoteContext db = new GameNoteContext();

        // GET: api/UserGameNotes
        public IQueryable<UserGameNote> GetUserGameNotes()
        {
            return db.UserGameNotes;
        }

        // GET: api/UserGameNotes/5
        [ResponseType(typeof(UserGameNote))]
        public async Task<IHttpActionResult> GetUserGameNote(string id)
        {
            UserGameNote userGameNote = await db.UserGameNotes.FindAsync(id);
            if (userGameNote == null)
            {
                return NotFound();
            }

            return Ok(userGameNote);
        }

        // PUT: api/UserGameNotes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUserGameNote(string id, UserGameNote userGameNote)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UserGameNote myplayer = await db.UserGameNotes.FindAsync(userGameNote.Id);
            myplayer.UserID = userGameNote.UserID;
            myplayer.GameNote = userGameNote.GameNote;
            myplayer.gameid = userGameNote.gameid;
          myplayer.UpdatedAt = DateTime.Now;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserGameNoteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/UserGameNotes
        [ResponseType(typeof(UserGameNote))]
        public async Task<IHttpActionResult> PostUserGameNote(UserGameNote userGameNote)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            userGameNote.Id = Guid.NewGuid().ToString();
            userGameNote.CreatedAt = DateTime.Now;
            userGameNote.UpdatedAt = DateTime.Now;
            userGameNote.Deleted = false;
          db.UserGameNotes.Add(userGameNote);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserGameNoteExists(userGameNote.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = userGameNote.Id }, userGameNote);
        }

        // DELETE: api/UserGameNotes/5
        [ResponseType(typeof(UserGameNote))]
        public async Task<IHttpActionResult> DeleteUserGameNote(string id)
        {
            UserGameNote userGameNote = await db.UserGameNotes.FindAsync(id);
            if (userGameNote == null)
            {
                return NotFound();
            }

            db.UserGameNotes.Remove(userGameNote);
            await db.SaveChangesAsync();

            return Ok(userGameNote);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserGameNoteExists(string id)
        {
            return db.UserGameNotes.Count(e => e.Id == id) > 0;
        }
    }
}