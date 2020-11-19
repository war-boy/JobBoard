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
    public class ApplicationStatusController : Controller
    {
        private JobBoardEntities db = new JobBoardEntities();

        // GET: ApplicationStatus
        [Authorize]
        public ActionResult Index()
        {
            return View(db.ApplicationStatus.ToList());
        }

        // GET: ApplicationStatus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationStatu applicationStatu = db.ApplicationStatus.Find(id);
            if (applicationStatu == null)
            {
                return HttpNotFound();
            }
            return View(applicationStatu);
        }

        // GET: ApplicationStatus/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: ApplicationStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "ApplicationStatusId,StatusName,StatusDescription")] ApplicationStatu applicationStatu)
        {
            if (ModelState.IsValid)
            {
                db.ApplicationStatus.Add(applicationStatu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(applicationStatu);
        }

        // GET: ApplicationStatus/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationStatu applicationStatu = db.ApplicationStatus.Find(id);
            if (applicationStatu == null)
            {
                return HttpNotFound();
            }
            return View(applicationStatu);
        }

        // POST: ApplicationStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ApplicationStatusId,StatusName,StatusDescription")] ApplicationStatu applicationStatu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(applicationStatu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(applicationStatu);
        }

        // GET: ApplicationStatus/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationStatu applicationStatu = db.ApplicationStatus.Find(id);
            if (applicationStatu == null)
            {
                return HttpNotFound();
            }
            return View(applicationStatu);
        }

        // POST: ApplicationStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ApplicationStatu applicationStatu = db.ApplicationStatus.Find(id);
            db.ApplicationStatus.Remove(applicationStatu);
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
