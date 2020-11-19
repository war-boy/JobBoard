using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobBoard.DATA.EF;

namespace JobBoard.UI.MVC.Controllers
{
    public class PerformanceReviewsController : Controller
    {
        private JobBoardEntities db = new JobBoardEntities();

        // GET: PerformanceReviews
        [Authorize]
        public ActionResult Index()
        {
            var performanceReviews = db.PerformanceReviews.Include(p => p.Location);
            return View(performanceReviews.ToList());
        }

        // GET: PerformanceReviews/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PerformanceReview performanceReview = db.PerformanceReviews.Find(id);
            if (performanceReview == null)
            {
                return HttpNotFound();
            }
            return View(performanceReview);
        }

        // GET: PerformanceReviews/Create
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult Create()
        {
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "StoreNumber");
            return View();
        }

        // POST: PerformanceReviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, Manager")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PerformanceReviewId,LocationId,ReviewDate,PerformanceRating,ReviewDetails")] PerformanceReview performanceReview)
        {
            if (ModelState.IsValid)
            {
                db.PerformanceReviews.Add(performanceReview);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "StoreNumber", performanceReview.LocationId);
            return View(performanceReview);
        }

        // GET: PerformanceReviews/Edit/5
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PerformanceReview performanceReview = db.PerformanceReviews.Find(id);
            if (performanceReview == null)
            {
                return HttpNotFound();
            }
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "StoreNumber", performanceReview.LocationId);
            return View(performanceReview);
        }

        // POST: PerformanceReviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, Manager")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PerformanceReviewId,LocationId,ReviewDate,PerformanceRating,ReviewDetails")] PerformanceReview performanceReview)
        {
            if (ModelState.IsValid)
            {
                db.Entry(performanceReview).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "StoreNumber", performanceReview.LocationId);
            return View(performanceReview);
        }

        // GET: PerformanceReviews/Delete/5
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PerformanceReview performanceReview = db.PerformanceReviews.Find(id);
            if (performanceReview == null)
            {
                return HttpNotFound();
            }
            return View(performanceReview);
        }

        // POST: PerformanceReviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin, Manager")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PerformanceReview performanceReview = db.PerformanceReviews.Find(id);
            db.PerformanceReviews.Remove(performanceReview);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
