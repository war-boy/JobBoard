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

    [Authorize]
    public class UserDetailsController : Controller
    {
        private JobBoardEntities db = new JobBoardEntities();

        // GET: UserDetails
        public ActionResult Index()
        {
            var userDetails = db.UserDetails.Include(u => u.PerformanceView).Include(u => u.AspNetUser);
            return View(userDetails.ToList());
        }

        // GET: UserDetails/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserDetail userDetail = db.UserDetails.Find(id);
            if (userDetail == null)
            {
                return HttpNotFound();
            }



            return View(userDetail);
        }

        public ActionResult AddResume(string id, HttpPostedFileBase resume)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            UserDetail userDetail = db.UserDetails.Find(id);
            if (userDetail == null)
            {
                return HttpNotFound();
            }

            #region FileUpload
            string resumeName;

            if (resume != null)
            {
                resumeName = resume.FileName;

                string ext = resumeName.Substring(resumeName.LastIndexOf('.'));

                string[] validExts = { ".pdf", ".doc", ".docx" };

                if (validExts.Contains(ext.ToLower()) && (resume.ContentLength <= 4194304))//4mb
                {
                    resumeName = Guid.NewGuid() + ext.ToLower();

                    string savePath = Server.MapPath("~/Content/resumes");

                }
                else
                {
                    ViewBag.Message = "Your resume was not uploaded correctely, please try a different file type (only .pdf, .doc and .docx are accepted";
                }

                userDetail.ResumeFileName = resumeName;
                #endregion

                db.Entry(userDetail).State = EntityState.Modified;
                db.SaveChanges();
                //return View("Details", new { ID = id, userDetail });
                return View("Index");
            }
            //return View(userDetail);
            return View("Index");
        }

        // GET: UserDetails/Create
        public ActionResult Create()
        {
            ViewBag.PerformanceReviewId = new SelectList(db.PerformanceReviews, "PerformanceReviewId", "PerformanceRating");
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: UserDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,FirstName,LastName,ResumeFileName,IsOpenToRelocation,Title,EmploymentType,VisaStatus,DateOfHire,Notes,UserImage,PerformanceReviewId")] UserDetail userDetail)
        {
            if (ModelState.IsValid)
            {

                db.UserDetails.Add(userDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PerformanceReviewId = new SelectList(db.PerformanceReviews, "PerformanceReviewId", "PerformanceRating", userDetail.PerformanceReviewId);
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", userDetail.UserId);
            return View(userDetail);
        }


        // GET: UserDetails/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserDetail userDetail = db.UserDetails.Find(id);
            if (userDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.PerformanceReviewId = new SelectList(db.PerformanceReviews, "PerformanceReviewId", "PerformanceRating", userDetail.PerformanceReviewId);
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", userDetail.UserId);
            return View(userDetail);
        }

        // POST: UserDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,FirstName,LastName,ResumeFileName,IsOpenToRelocation,Title,EmploymentType,VisaStatus,DateOfHire,Notes,UserImage,PerformanceReviewId")] UserDetail userDetail, HttpPostedFileBase resume)
        {
            if (resume != null && ModelState.IsValid)
            {
                string resumeName = resume.FileName;

                string ext = resumeName.Substring(resumeName.LastIndexOf('.'));

                string[] validExts = { ".pdf", ".doc", ".docx" };

                if (validExts.Contains(ext.ToLower()) && (resume.ContentLength <= 4194304))//4mb
                {
                    resumeName = Guid.NewGuid() + ext.ToLower();

                    string savePath = Server.MapPath("~/Content/resumes");

                    userDetail.ResumeFileName = resumeName;

                }

                //Manual Delete 
                if (userDetail.ResumeFileName != null)
                {
                    System.IO.File.Delete(Server.MapPath("~/Content/resumes/" + userDetail.ResumeFileName));
                }
                

                db.Entry(userDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PerformanceReviewId = new SelectList(db.PerformanceReviews, "PerformanceReviewId", "PerformanceRating", userDetail.PerformanceReviewId);
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", userDetail.UserId);
            return View(userDetail);
        }




        // GET: UserDetails/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserDetail userDetail = db.UserDetails.Find(id);
            if (userDetail == null)
            {
                return HttpNotFound();
            }
            return View(userDetail);
        }

        // POST: UserDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            UserDetail userDetail = db.UserDetails.Find(id);
            db.UserDetails.Remove(userDetail);
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

