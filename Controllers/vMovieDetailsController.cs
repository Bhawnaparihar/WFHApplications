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
    public class vMovieDetailsController : ApiController
    {
        private Entities7 db = new Entities7();

        // GET: api/vMovieDetails
        public IQueryable<vMovieDetail> GetvMovieDetails()
        {
            return db.vMovieDetails;
        }

        // GET: api/vMovieDetails/5
        [ResponseType(typeof(vMovieDetail))]
        public IHttpActionResult GetvMovieDetail(string id)
        {
            vMovieDetail vMovieDetail = db.vMovieDetails.Find(id);
            if (vMovieDetail == null)
            {
                return NotFound();
            }

            return Ok(vMovieDetail);
        }

        // PUT: api/vMovieDetails/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutvMovieDetail(string id, vMovieDetail vMovieDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vMovieDetail.MovieName)
            {
                return BadRequest();
            }

            db.Entry(vMovieDetail).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vMovieDetailExists(id))
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

        // POST: api/vMovieDetails
        [ResponseType(typeof(vMovieDetail))]
        public IHttpActionResult PostvMovieDetail(vMovieDetail vMovieDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.vMovieDetails.Add(vMovieDetail);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (vMovieDetailExists(vMovieDetail.MovieName))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = vMovieDetail.MovieName }, vMovieDetail);
        }

        // DELETE: api/vMovieDetails/5
        [ResponseType(typeof(vMovieDetail))]
        public IHttpActionResult DeletevMovieDetail(string id)
        {
            vMovieDetail vMovieDetail = db.vMovieDetails.Find(id);
            if (vMovieDetail == null)
            {
                return NotFound();
            }

            db.vMovieDetails.Remove(vMovieDetail);
            db.SaveChanges();

            return Ok(vMovieDetail);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool vMovieDetailExists(string id)
        {
            return db.vMovieDetails.Count(e => e.MovieName == id) > 0;
        }
    }
}