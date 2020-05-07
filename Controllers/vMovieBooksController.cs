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
    public class vMovieBooksController : ApiController
    {
        private Entities7 db = new Entities7();

        // GET: api/vMovieBooks
        public IQueryable<vMovieBook> GetvMovieBooks()
        {
            return db.vMovieBooks;
        }

        // GET: api/vMovieBooks/5
        [ResponseType(typeof(vMovieBook))]
        public IHttpActionResult GetvMovieBook(string id)
        {
            vMovieBook vMovieBook = db.vMovieBooks.Find(id);
            if (vMovieBook == null)
            {
                return NotFound();
            }

            return Ok(vMovieBook);
        }

        // PUT: api/vMovieBooks/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutvMovieBook(string id, vMovieBook vMovieBook)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vMovieBook.MovieName)
            {
                return BadRequest();
            }

            db.Entry(vMovieBook).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vMovieBookExists(id))
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

        // POST: api/vMovieBooks
        [ResponseType(typeof(vMovieBook))]
        public IHttpActionResult PostvMovieBook(vMovieBook vMovieBook)
        {
            Movy movy = db.Movies.Single(t => t.MovieName == vMovieBook.MovieName);

            User user = db.Users.Single(t => t.UserName == vMovieBook.UserName);

            Theater theater = db.Theaters.SingleOrDefault(t => t.ShowTime == vMovieBook.ShowTime && t.Price == vMovieBook.Price);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var movieBooking = new MovieBooking()
            {
                MovieId = movy.MovieId,
                UserId = user.UserId,
                TheaterId = theater.TheaterId
            };

            db.MovieBookings.Add(movieBooking);
            db.SaveChanges();
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //db.vMovieBooks.Add(vMovieBook);

            //try
            //{
            //    db.SaveChanges();
            //}
            //catch (DbUpdateException)
            //{
            //    if (vMovieBookExists(vMovieBook.MovieName))
            //    {
            //        return Conflict();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            return CreatedAtRoute("DefaultApi", new { id = vMovieBook.MovieName }, vMovieBook);
        }

        // DELETE: api/vMovieBooks/5
        [ResponseType(typeof(vMovieBook))]
        public IHttpActionResult DeletevMovieBook(string id)
        {
            vMovieBook vMovieBook = db.vMovieBooks.Find(id);
            if (vMovieBook == null)
            {
                return NotFound();
            }

            db.vMovieBooks.Remove(vMovieBook);
            db.SaveChanges();

            return Ok(vMovieBook);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool vMovieBookExists(string id)
        {
            return db.vMovieBooks.Count(e => e.MovieName == id) > 0;
        }
    }
}