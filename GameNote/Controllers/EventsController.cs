using System;
using System.CodeDom;
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
    public class EventsController : ApiController
    {
        private GameNoteContext db = new GameNoteContext();

        // GET: api/Events
        public IQueryable<Event> GetEvents()
        {
            return db.Events;
        }


        // GET: api/Events/5
        [ResponseType(typeof(Event))]
        public async Task<IHttpActionResult> GetEvent(string id)
        {
            Event @event = await db.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            return Ok(@event);
        }

        public async Task<IHttpActionResult> GetbyEventidEvent (string eventid)

        { 
            Event myevent = await db.Events.Where(e => e.event_id == eventid).FirstOrDefaultAsync();
          
            if (myevent == null)
            {
            return NotFound();
        }

            return Ok(myevent);
        }
        // PUT: api/Events/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutEvent( Event @event)
        {
            try
            {
                @event.sys_atid = db.Teams.Where(t=>t.team_id ==@event.away_teamid).FirstOrDefault().Id;
               

           }
            catch (Exception)
            {
                
                throw;
            }
            if (@event.sys_atid ==null)
            {
                return BadRequest(ModelState);
            }


            string id = @event.Id;
            db.Entry(@event).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
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

        // POST: api/Events
        [ResponseType(typeof(Event))]
        public async Task<IHttpActionResult> PostEvent(Event @event)
        {
            try
            {
                @event.sys_atid = geid(@event.away_teamid);
                @event.sys_htid = geid(@event.home_teamid);
                @event.Id = Guid.NewGuid().ToString();
                @event.CreatedAt = DateTime.Now;
                @event.UpdatedAt = DateTime.Now;
                @event.Deleted = false;
            }
            catch (Exception)
            {

                throw;
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Events.Add(@event);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EventExists(@event.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = @event.Id }, @event);
        }

        // DELETE: api/Events/5
        [ResponseType(typeof(Event))]
        public async Task<IHttpActionResult> DeleteEvent(string id)
        {
            Event @event = await db.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            db.Events.Remove(@event);
            await db.SaveChangesAsync();

            return Ok(@event);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EventExists(string id)
        {
            return db.Events.Count(e => e.Id == id) > 0;
        }
       
        private string geid(String team_id)
        {
            return db.Teams.Where(t => t.team_id == team_id).FirstOrDefault().Id;
        }
    }
}