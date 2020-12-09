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
using PagedList;
using PagedList.Mvc;

namespace JobBoard.UI.MVC.Controllers
{

    public class OpenPositionsController : Controller
    {
        private JobBoardEntities db = new JobBoardEntities();

        // GET: OpenPositions
        //Job Board
        [Authorize]        
        public ActionResult Index(int? locationId, string searchString, string currentFilter, int page = 1)
        {
            int pageSize = 10;

            var openPositions = db.OpenPositions.Where(op => op.IsActive != false).Include(op => op.Location).Include(op => op.Position).ToList();

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;




            if (!String.IsNullOrEmpty(searchString))
            {
                //Search by Keyword and Location
                if (locationId != null)
                {
                    var openPositionsByBoth = db.OpenPositions.Where(op => op.LocationId == locationId && op.IsActive != false && op.Position.Title.ToLower().Contains(searchString.ToLower())).ToList();

                    ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "City");
                    ViewBag.CurrentFilter = searchString;

                    return View(openPositionsByBoth.ToPagedList(page, pageSize));
                }

                //Search by just title
                openPositions = db.OpenPositions.Where(op => op.IsActive != false && op.Position.Title.ToLower().Contains(searchString.ToLower())).ToList();
            }
            else if (locationId != null)
            {
                //Search by just Location
                var openPositionsByLocation = db.OpenPositions.Where(op => op.LocationId == locationId && op.IsActive != false).ToList();

                ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "City");
                ViewBag.CurrentFilter = searchString;

                return View(openPositionsByLocation.ToPagedList(page, pageSize));
            }

            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "City");
            return View(openPositions.ToPagedList(page, pageSize));
        }


        public ActionResult YourPositions(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var yourPositions = db.OpenPositions.Where(op => op.Location.ManagerId == id && op.IsActive != false);
            return View(yourPositions.ToList());
        }

        // GET: OpenPositions/Details/5
        [Authorize]
        public ActionResult Details(int? id, string userId)
        {
            if (id == null || userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OpenPosition openPosition = db.OpenPositions.Find(id);
            UserDetail userDetail = db.UserDetails.Find(userId);
            if (openPosition == null || userDetail == null)
            {
                return HttpNotFound();
            }

            //Determine if user has already applied to this position
            //Search database for applications with matching OpenPositionId and UserId. Return True if any records are found, False is not
            var userApplied = db.Applications.Where(a => a.UserId == userId && a.OpenPositionId == id).Any();           
            if (userApplied)
            {
                ViewBag.HasApplied = true;
            }
            else
            {
                ViewBag.HasApplied = false;
            }

            return View(openPosition);
        }

        public ActionResult CreateApp(int? opId)
        {
            if (opId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            OpenPosition openPosition = db.OpenPositions.Find(opId);

            if (openPosition == null)
            {
                return HttpNotFound();
            }

            #region Get Current User
            string userID = User.Identity.GetUserId();

            UserDetail currentUser = db.UserDetails.Where(ud => ud.UserId == userID).FirstOrDefault();
            #endregion

            Application application = new Application();

            application.OpenPositionId = openPosition.OpenPositionId;

            application.UserId = currentUser.UserId;

            application.ApplicationDate = DateTime.Now;

            //Default to Pending
            application.ApplicationStatusId = 1;

            application.ResumeFilename = currentUser.ResumeFileName;

            try
            {
                db.Applications.Add(application);
                db.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }

            return RedirectToAction("Details", new { id = opId, userId = currentUser.UserId });
        }


        // GET: OpenPositions/Create
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult Create()
        {
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "City");
            ViewBag.PositionId = new SelectList(db.Positions, "PositionId", "Title");
            return View();
        }

        // POST: OpenPositions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, Manager")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OpenPositionId,LocationId,PositionId,IsRemote,EmploymentType,Duration,IsActive")] OpenPosition openPosition)
        {
            if (ModelState.IsValid)
            {
                openPosition.IsActive = true;
                db.OpenPositions.Add(openPosition);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "City", openPosition.LocationId);
            ViewBag.PositionId = new SelectList(db.Positions, "PositionId", "Title", openPosition.PositionId);
            return View(openPosition);
        }

        // GET: OpenPositions/Edit/5
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult Edit(int? id)
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
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "City", openPosition.LocationId);
            ViewBag.PositionId = new SelectList(db.Positions, "PositionId", "Title", openPosition.PositionId);
            return View(openPosition);
        }

        // POST: OpenPositions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin, Manager")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OpenPositionId,LocationId,PositionId,IsRemote,EmploymentType,Duration,IsActive")] OpenPosition openPosition)
        {
            if (ModelState.IsValid)
            {
                db.Entry(openPosition).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "City", openPosition.LocationId);
            ViewBag.PositionId = new SelectList(db.Positions, "PositionId", "Title", openPosition.PositionId);
            return View(openPosition);
        }

        //// GET: OpenPositions/Delete/5
        //[Authorize(Roles = "Admin, Manager")]
        //public ActionResult Delete(int? id)
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
        //    return View(openPosition);
        //}

        // POST: OpenPositions/Delete/5        
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult Delete(int id)
        {
            // Soft delete to satisfy FK_Applications_OpenPositions constraint

            OpenPosition openPosition = db.OpenPositions.Find(id);

            openPosition.IsActive = false;

            db.Entry(openPosition).State = EntityState.Modified;

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
