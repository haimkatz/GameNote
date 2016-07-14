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
    public class UserTeamNotesController : ApiController
    {
        private GameNoteContext db = new GameNoteContext();

        // GET: api/UserTeamNotes
        public IQueryable<UserTeamNote> GetUserTeamNotes()
        {
            return db.UserTeamNotes;
        }

        // GET: api/UserTeamNotes/5
        [ResponseType(typeof(UserTeamNote))]
        public async Task<IHttpActionResult> GetUserTeamNote(string id)
        {
            UserTeamNote userTeamNote = await db.UserTeamNotes.FindAsync(id);
            if (userTeamNote == null)
            {
                return NotFound();
            }

            return Ok(userTeamNote);
        }

        [ResponseType(typeof (UserTeamNote))]
        public async Task<IHttpActionResult> GetUTNbyUserTeam(string userid, string teamid)
        {
            UserTeamNote userTeamNote =
                await db.UserTeamNotes.Where(u => u.UserID == userid & u.teamid == teamid).FirstOrDefaultAsync();
            if (userTeamNote == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(userTeamNote);
            }
        }

        public async Task<IHttpActionResult> PutUserTeamNote(string id, UserTeamNote userTeamNote)
        { 
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userTeamNote.Id)
            {
                return BadRequest();
            }

            UserTeamNote utn = await db.UserTeamNotes.FindAsync(id);
            utn.TeamNote = userTeamNote.TeamNote;
            utn.MakePublic = userTeamNote.MakePublic; 
            utn.UpdatedAt = DateTimeOffset.Now;
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserTeamNoteExists(id))
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

        // POST: api/UserTeamNotes
        [ResponseType(typeof(UserTeamNote))]
        public async Task<IHttpActionResult> PostUserTeamNote(UserTeamNote userTeamNote)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            userTeamNote.UpdatedAt = DateTimeOffset.Now;
                userTeamNote.Deleted = false;
            db.UserTeamNotes.Add(userTeamNote);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserTeamNoteExists(userTeamNote.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = userTeamNote.Id }, userTeamNote);
        }

        // DELETE: api/UserTeamNotes/5
        [ResponseType(typeof(UserTeamNote))]
        public async Task<IHttpActionResult> DeleteUserTeamNote(string id)
        {
            UserTeamNote userTeamNote = await db.UserTeamNotes.FindAsync(id);
            if (userTeamNote == null)
            {
                return NotFound();
            }

            db.UserTeamNotes.Remove(userTeamNote);
            await db.SaveChangesAsync();

            return Ok(userTeamNote);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserTeamNoteExists(string id)
        {
            return db.UserTeamNotes.Count(e => e.Id == id) > 0;
        }
    }
}