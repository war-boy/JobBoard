using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobBoard.DATA.EF;
using Microsoft.AspNet.Identity;

namespace JobBoard.UI.MVC.Controllers
{
    public class ApplicationsController : Controller
    {
        private JobBoardEntities db = new JobBoardEntities();

        // GET: Applications
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var applications = db.Applications.Include(a => a.ApplicationStatu).Include(a => a.OpenPosition).Include(a => a.UserDetail);
            return View(applications.ToList());

        }

        //Get list of all of applications created by the current user
        public ActionResult YourApplications(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (Request.IsAuthenticated && User.IsInRole("Admin"))
            {
                var applications = db.Applications.Include(a => a.ApplicationStatu).Include(a => a.OpenPosition).Include(a => a.UserDetail);
                return View(applications.ToList());
            }

            var yourApplications = db.Applications.Where(a => a.UserId == id);
            return View(yourApplications.ToList());

        }

        //Get list of all applications sent for the given OpenPosition
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult PositionApplications(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            OpenPosition openPosition = db.OpenPositions.Find(id);
            if (openPosition == null)
            {
                return HttpNotFound();
            }

            ViewBag.OpenPosition = openPosition;

            var positionApplications = db.Applications.Where(a => a.OpenPositionId == id && a.ApplicationStatusId != 2);
            return View(positionApplications.ToList());
        }

        // GET: Applications/Details/5
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Application application = db.Applications.Find(id);
            if (application == null)
            {
                return HttpNotFound();
            }
            return View(application);
        }

        // GET: Applications/Create
        //public ActionResult Create(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    OpenPosition openPosition = db.OpenPositions.Find(id);
        //    if (openPosition == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    ViewBag.OpId = openPosition.OpenPositionId;

        //    ViewBag.ApplicationStatusId = new SelectList(db.ApplicationStatus, "ApplicationStatusId", "StatusName");
        //    ViewBag.OpenPositionId = new SelectList(db.OpenPositions, "OpenPositionId", "OpenPositionId");
        //    ViewBag.UserId = new SelectList(db.UserDetails, "UserId", "FirstName");
        //    return View();
        //}

        // POST: Applications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult CreateApp([Bind(Include = "ApplicationId,UserId,OpenPositionId,ApplicationDate,ManagerNotes,ApplicationStatusId,ResumeFilename")] Application application, int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    OpenPosition openPosition = db.OpenPositions.Find(id);

        //    if (openPosition == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    #region Get Current User
        //    string userID = User.Identity.GetUserId();

        //    UserDetail currentUser = db.UserDetails.Where(ud => ud.UserId == userID).FirstOrDefault();
        //    #endregion

        //    application.OpenPositionId = openPosition.OpenPositionId;

        //    application.UserId = currentUser.UserId;

        //    application.ApplicationDate = DateTime.Now;

        //    application.ApplicationStatusId = 1;

        //    application.ResumeFilename = currentUser.ResumeFileName;

        //    if (ModelState.IsValid)
        //    {
        //        db.Applications.Add(application);
        //        db.SaveChanges();
        //        return RedirectToAction("ApplicationConfirm", application);
        //    }

        //    //ViewBag.ApplicationStatusId = new SelectList(db.ApplicationStatus, "ApplicationStatusId", "StatusName", application.ApplicationStatusId);
        //    //ViewBag.OpenPositionId = new SelectList(db.OpenPositions, "OpenPositionId", "OpenPositionId", application.OpenPositionId);
        //    //ViewBag.UserId = new SelectList(db.UserDetails, "UserId", "FirstName", application.UserId);

        //    ViewBag.ApplicationFailed = "We were unable to submit your application. Please contact IT User Support";
        //    return View();
        //}

        //// GET: Applications/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Application application = db.Applications.Find(id);
        //    if (application == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.ApplicationStatusId = new SelectList(db.ApplicationStatus, "ApplicationStatusId", "StatusName", application.ApplicationStatusId);
        //    ViewBag.OpenPositionId = new SelectList(db.OpenPositions, "OpenPositionId", "OpenPositionId", application.OpenPositionId);
        //    ViewBag.UserId = new SelectList(db.UserDetails, "UserId", "FirstName", application.UserId);
        //    return View(application);
        //}

        //// POST: Applications/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ApplicationId,UserId,OpenPositionId,ApplicationDate,ManagerNotes,ApplicationStatusId,ResumeFilename")] Application application)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(application).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.ApplicationStatusId = new SelectList(db.ApplicationStatus, "ApplicationStatusId", "StatusName", application.ApplicationStatusId);
        //    ViewBag.OpenPositionId = new SelectList(db.OpenPositions, "OpenPositionId", "OpenPositionId", application.OpenPositionId);
        //    ViewBag.UserId = new SelectList(db.UserDetails, "UserId", "FirstName", application.UserId);
        //    return View(application);
        //}

        // GET: Applications/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Application application = db.Applications.Find(id);
            if (application == null)
            {
                return HttpNotFound();
            }
            return View(application);
        }

        // POST: Applications/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Application application = db.Applications.Find(id);
            db.Applications.Remove(application);
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
