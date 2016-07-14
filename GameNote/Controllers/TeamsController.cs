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
    public class TeamsController : ApiController
    {
        private GameNoteContext db = new GameNoteContext();

        // GET: api/Teams
        public IQueryable<Team> GetTeams()
        {
            return db.Teams;
        }

        // GET: api/Teams/5
        [ResponseType(typeof(Team))]
        public async Task<IHttpActionResult> GetTeam(string id)
        {
            Team team = await db.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }

            //var retteam = new Team()
            //{
            //    abbreviation = team.abbreviation,
            //    city = team.city,
            //    conference = team.conference,
            //    division = team.division,
            //    first_name = team.first_name,
            //    full_name = team.full_name,
            //    gamenotes = team.gamenotes,
            //    last_name = team.last_name,
            //    site_name = team.site_name,
            //    team_id = team.team_id
            //};
            return Ok(team);
        }
        public async Task<IHttpActionResult> GetTeambyteamidasync(string teamid)
        {
            Team team = await db.Teams.Where(t => t.team_id == teamid).FirstOrDefaultAsync();
            if (team == null)
            {
                return NotFound();
            }
            //var retteam = new Team()
            //{
            //    abbreviation = team.abbreviation,
            //    city = team.city,
            //    conference = team.conference,
            //    division = team.division,
            //    first_name = team.first_name,
            //    full_name = team.full_name,
            //    gamenotes = team.gamenotes,
            //    last_name = team.last_name,
            //    site_name = team.site_name,
            //    team_id = team.team_id
            //};
            return Ok(team);
        }

        // PUT: api/Teams/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTeam(string id, Team team)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != team.Id)
            {
                return BadRequest();
            }

            Team orgteam = await db.Teams.FindAsync(id);

            orgteam.gamenotes = team.gamenotes;
            orgteam.UpdatedAt = DateTime.Now;
            orgteam.Deleted = false;
            
          //  db.Entry(team).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (  DbUpdateConcurrencyException ex)
            {
                if (!TeamExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw ex;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Teams
        [ResponseType(typeof(Team))]
        public async Task<IHttpActionResult> PostTeam(Team team)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            team.Id = Guid.NewGuid().ToString();
            team.CreatedAt = DateTime.Now;
            team.UpdatedAt = DateTime.Now;
            team.Deleted = false;


            db.Teams.Add(team);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TeamExists(team.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            var retteam = new Team()
            {
                abbreviation = team.abbreviation,
                city = team.city,
                conference = team.conference,
                division = team.division,
                first_name = team.first_name,
                full_name = team.full_name,
                gamenotes = team.gamenotes,
                last_name = team.last_name,
                site_name = team.site_name,
                team_id = team.team_id
            };
            return Ok(retteam);
        }

        // DELETE: api/Teams/5
        [ResponseType(typeof(Team))]
        public async Task<IHttpActionResult> DeleteTeam(string id)
        {
            Team team = await db.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }
            team.Deleted = true;
            db.Teams.Remove(team);
            await db.SaveChangesAsync();

            return Ok(team);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TeamExists(string id)
        {
            return db.Teams.Count(e => e.Id == id) > 0;
        }

    }
}