using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using MovieApp1.Models;

namespace MovieApp1.Controllers
{
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class vMovieActorsController : ApiController
    {
        private Entities7 db = new Entities7();

        // GET: api/vMovieActors
        public IQueryable<vMovieActor> GetvMovieActors()
        {
            return db.vMovieActors;
        }

        // GET: api/vMovieActors/5
        [ResponseType(typeof(vMovieActor))]
        public IHttpActionResult GetvMovieActor(string id)
        {
            vMovieActor vMovieActor = db.vMovieActors.Find(id);
            if (vMovieActor == null)
            {
                return NotFound();
            }

            return Ok(vMovieActor);
        }

        // PUT: api/vMovieActors/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutvMovieActor(string id, vMovieActor vMovieActor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vMovieActor.MovieName)
            {
                return BadRequest();
            }

            db.Entry(vMovieActor).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vMovieActorExists(id))
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

        // POST: api/vMovieActors
        [ResponseType(typeof(vMovieActor))]
        public IHttpActionResult PostvMovieActor(vMovieActor vMovieActor)
        {
            Actor actor = db.Actors.Single(t => t.ActorName == vMovieActor.ActorName);

            Movy movy = db.Movies.Single(t=>t.MovieName == vMovieActor.MovieName );

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var movieActor = new MovieActor()
            {
                ActorId = actor.ActorId,
                MovieId = movy.MovieId
            };

            db.MovieActors.Add(movieActor);
            db.SaveChanges();
            /* try
             {
                 db.SaveChanges();
             }
             catch (DbUpdateException)
             {
                 if (vMovieActorExists(vMovieActor.MovieName))
                 {
                     return Conflict();
                 }
                 else
                 {
                     throw;
                 }
             }
             */

            return CreatedAtRoute("DefaultApi", new { id = vMovieActor.MovieName }, vMovieActor);
        }

        // DELETE: api/vMovieActors/5
        [ResponseType(typeof(vMovieActor))]
        public IHttpActionResult DeletevMovieActor(string id)
        {
            vMovieActor vMovieActor = db.vMovieActors.Find(id);
            if (vMovieActor == null)
            {
                return NotFound();
            }

            db.vMovieActors.Remove(vMovieActor);
            db.SaveChanges();

            return Ok(vMovieActor);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool vMovieActorExists(string id)
        {
            return db.vMovieActors.Count(e => e.MovieName == id) > 0;
        }
    }
}