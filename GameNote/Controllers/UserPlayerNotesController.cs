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
    public class UserPlayerNotesController : ApiController
    {
        private GameNoteContext db = new GameNoteContext();

        // GET: api/UserPlayerNotes
        public IQueryable<UserPlayerNote> GetUserPlayerNotes()
        {
            return db.UserPlayerNotes;
        }

        // GET: api/UserPlayerNotes/5
        [ResponseType(typeof(UserPlayerNote))]
        public async Task<IHttpActionResult> GetUserPlayerNote(string id)
        {
            UserPlayerNote userPlayerNote = await db.UserPlayerNotes.FindAsync(id);
            if (userPlayerNote == null)
            {
                return NotFound();
            }

            return Ok(userPlayerNote);
        }

        // PUT: api/UserPlayerNotes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUserPlayerNote(string id, UserPlayerNote userPlayerNote)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != userPlayerNote.Id)
            {
                return BadRequest();
            }
            UserPlayerNote myplayer = await db.UserPlayerNotes.FindAsync(userPlayerNote.Id);
            myplayer.Id = userPlayerNote.Id;
            myplayer.UserID = userPlayerNote.UserID;
            myplayer.playernote = userPlayerNote.playernote;
         myplayer.UpdatedAt = DateTime.Now;
         
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserPlayerNoteExists(id))
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

        // POST: api/UserPlayerNotes
        [ResponseType(typeof(UserPlayerNote))]
        public async Task<IHttpActionResult> PostUserPlayerNote(UserPlayerNote userPlayerNote)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            userPlayerNote.Id = Guid.NewGuid().ToString();
         userPlayerNote.CreatedAt = DateTime.Now;
            userPlayerNote.UpdatedAt = DateTime.Now;
            userPlayerNote.Deleted = false;
           db.UserPlayerNotes.Add(userPlayerNote);

            try
            {
                db.UserPlayerNotes.Add(userPlayerNote);

                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserPlayerNoteExists(userPlayerNote.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = userPlayerNote.Id }, userPlayerNote);
        }

        // DELETE: api/UserPlayerNotes/5
        [ResponseType(typeof(UserPlayerNote))]
        public async Task<IHttpActionResult> DeleteUserPlayerNote(string id)
        {
            UserPlayerNote userPlayerNote = await db.UserPlayerNotes.FindAsync(id);
            if (userPlayerNote == null)
            {
                return NotFound();
            }

            db.UserPlayerNotes.Remove(userPlayerNote);
            await db.SaveChangesAsync();

            return Ok(userPlayerNote);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserPlayerNoteExists(string id)
        {
            return db.UserPlayerNotes.Count(e => e.Id == id) > 0;
        }
    }
}