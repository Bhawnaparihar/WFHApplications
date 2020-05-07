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
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "get,put,post,delete")]
    public class MovieActorsController : ApiController
    {
        private Entities7 db = new Entities7();
        int actorId;
        int movieId;
        int movieactorId;

        // GET: api/MovieActors
        public IQueryable<MovieActor> GetMovieActors()
        {
            return db.MovieActors;
        }

        // GET: api/MovieActors/5
        [ResponseType(typeof(MovieActor))]
        public IHttpActionResult GetMovieActor(int id)
        {
            MovieActor movieActor = db.MovieActors.Find(id);
            if (movieActor == null)
            {
                return NotFound();
            }

            return Ok(movieActor);
        }

        // PUT: api/MovieActors/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMovieActor(int id, MovieActor movieActor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != movieActor.MovieActorId)
            {
                return BadRequest();
            }

            db.Entry(movieActor).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieActorExists(id))
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

        // POST: api/MovieActors
        [ResponseType(typeof(MovieActor))]
        public IHttpActionResult PostMovieActor(MovieActor movieActor)
        {
            try 
            { 
            var movieactors = new MovieActor();
            var movie = new Movy();
            var actor = new Actor();

            var mn = db.Movies.Find(movieActor.Movy);
            
            if (mn != null)
            {
                    mn.MovieId = movie.MovieId;
                    movieactors.MovieId = mn.MovieId;
                    var act = new Actor();
               // return ("Enter actor name: " + act.ActorName);
                //act.ActorName = Console.ReadLine();

                var an = db.Actors.Find(movieActor.Actor);
               

                if (an != null)
                {
                        an.ActorId = actor.ActorId;
                        var a = db.MovieActors.SingleOrDefault(t => t.ActorId == actorId);
                    movieactorId = a.MovieActorId;


                    if (a != null)
                    {
                        movieActor.MovieActorId = movieactorId;
                        db.MovieActors.Add(movieActor);
                        db.SaveChanges();
                        
                    }
                    else
                    {
                        Console.WriteLine("There is no such actor!");
                    }
                }
                else
                {
                    Console.WriteLine("There is no such movie");
                    Console.WriteLine("Enter valid name");
                }
            }
            }
            catch (Exception)
            {
                
                throw;
            }
            return CreatedAtRoute("DefaultApi", new { id = movieActor.MovieActorId }, movieActor);

        }

        //if (!ModelState.IsValid)
        //{
        //    return BadRequest(ModelState);
        //}

        //db.MovieActors.Add(movieActor);
        //db.SaveChanges();

        //return CreatedAtRoute("DefaultApi", new { id = movieActor.MovieActorId }, movieActor);


        // DELETE: api/MovieActors/5
        [ResponseType(typeof(MovieActor))]
        public IHttpActionResult DeleteMovieActor(int id)
        {
            MovieActor movieActor = db.MovieActors.Find(id);
            if (movieActor == null)
            {
                return NotFound();
            }

            db.MovieActors.Remove(movieActor);
            db.SaveChanges();

            return Ok(movieActor);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MovieActorExists(int id)
        {
            return db.MovieActors.Count(e => e.MovieActorId == id) > 0;
        }
    }
}