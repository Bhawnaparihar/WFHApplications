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
    public class MovieBookingsController : ApiController
    {
        private Entities7 db = new Entities7();

        // GET: api/MovieBookings
        public IQueryable<MovieBooking> GetMovieBookings()
        {
            return db.MovieBookings;
        }

        // GET: api/MovieBookings/5
        [ResponseType(typeof(MovieBooking))]
        public IHttpActionResult GetMovieBooking(int id)
        {
            MovieBooking movieBooking = db.MovieBookings.Find(id);
            if (movieBooking == null)
            {
                return NotFound();
            }

            return Ok(movieBooking);
        }

        // PUT: api/MovieBookings/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMovieBooking(int id, MovieBooking movieBooking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != movieBooking.MovieBookingId)
            {
                return BadRequest();
            }

            db.Entry(movieBooking).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieBookingExists(id))
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

        // POST: api/MovieBookings
        [ResponseType(typeof(MovieBooking))]
        public IHttpActionResult PostMovieBooking(MovieBooking movieBooking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.MovieBookings.Add(movieBooking);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = movieBooking.MovieBookingId }, movieBooking);
        }

        // DELETE: api/MovieBookings/5
        [ResponseType(typeof(MovieBooking))]
        public IHttpActionResult DeleteMovieBooking(int id)
        {
            MovieBooking movieBooking = db.MovieBookings.Find(id);
            if (movieBooking == null)
            {
                return NotFound();
            }

            db.MovieBookings.Remove(movieBooking);
            db.SaveChanges();

            return Ok(movieBooking);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MovieBookingExists(int id)
        {
            return db.MovieBookings.Count(e => e.MovieBookingId == id) > 0;
        }
    }
}