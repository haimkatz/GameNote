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
    public class PlayersController : ApiController
    {
        private GameNoteContext db = new GameNoteContext();

        // GET: api/Players
        public IQueryable<Player> GetPlayers()
        {
            return db.Players;
        }

        // GET: api/Players/5
        [ResponseType(typeof(Player))]
        public async Task<IHttpActionResult> GetPlayer(string id)
        {
            Player player = await db.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }

            return Ok(player);
        }
        public async Task<IHttpActionResult> GetbynamePlayer(string displayname, string teamid)
        {
            Player player = await db.Players.Where(p=>p.display_name==displayname && p.team_id==teamid).FirstOrDefaultAsync();
            if (player == null)
            {
                return NotFound();
            }

            return Ok(player);
        }

        // PUT: api/Players/5
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPlayer(string id, Player player)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != player.Id)
            {
                return BadRequest();
            }
            Player myplayer = await db.Players.FindAsync(player.Id);
            myplayer.birthdate = player.birthdate;
            myplayer.age = player.age;
            myplayer.birthplace = player.birthplace;
            myplayer.mobileteam_id = player.mobileteam_id;
            myplayer.uniform_number = player.uniform_number;
            myplayer.playernote = player.playernote;
            myplayer.team_id = player.team_id;
            myplayer.UpdatedAt = DateTime.Now;
          
            //db.Entry(player).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerExists(id))
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

        // POST: api/Players
        [ResponseType(typeof(Player))]
        public async Task<IHttpActionResult> PostPlayer(Player player)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            player.Id = Guid.NewGuid().ToString();
            player.CreatedAt = DateTime.Now;
            player.UpdatedAt = DateTime.Now;
            player.Deleted = false;
            player.mobileteam_id = getmobileid(player.team_id);
            db.Players.Add(player);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                if (PlayerExists(player.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw e;
                }
            }

            return Ok(player);
        }

        // DELETE: api/Players/5
        [ResponseType(typeof(Player))]
        public async Task<IHttpActionResult> DeletePlayer(string id)
        {
            Player player = await db.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }
            player.Deleted = true;
            db.Players.Remove(player);
            await db.SaveChangesAsync();

            return Ok(player);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PlayerExists(string id)
        {
            return db.Players.Count(e => e.Id == id) > 0;
        }
        private string getmobileid(String team_id)
        {
            return db.Teams.Where(t => t.team_id == team_id).FirstOrDefault().Id;
        }
    }
}