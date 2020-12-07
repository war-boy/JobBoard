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
            var userDetails = db.UserDetails.Include(u => u.AspNetUser);
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

            ViewBag.Message = TempData["message"];
            return View(userDetail);
        }

        public ActionResult AddResume(string userViewId, HttpPostedFileBase resume)
        {
            if (userViewId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            UserDetail userDetail = db.UserDetails.Find(userViewId);
            if (userDetail == null)
            {
                return HttpNotFound();
            }

            #region Resume File Upload
            string resumeName;

            if (resume != null)
            {
                resumeName = resume.FileName;

                string ext = resumeName.Substring(resumeName.LastIndexOf('.'));

                string[] validExts = { ".pdf" };

                if (validExts.Contains(ext.ToLower()) && (resume.ContentLength <= 4194304))//4mb
                {
                    resumeName = Guid.NewGuid() + ext.ToLower();

                    //string savePath = Server.MapPath("~/Content/resumes/");

                    resume.SaveAs(Server.MapPath("~/Content/resumes/" + resumeName));

                }
                else
                {
                    TempData["message"] = "Your resume was not uploaded correctly, please try a different file type (only .pdf files are accepted)";
                    return RedirectToAction("Details", new { id = userViewId });
                }

                userDetail.ResumeFileName = resumeName;
                #endregion

                db.Entry(userDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = userViewId });
            }

            TempData["message"] = "There was an error uploading your resume. Please try again later or contact IT User Support.";
            return RedirectToAction("Details", new { id = userViewId });
        }

        public ActionResult DeleteResume(string userViewId)
        {
            if (userViewId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            UserDetail userDetail = db.UserDetails.Find(userViewId);
            if (userDetail == null)
            {
                return HttpNotFound();
            }

            userDetail.ResumeFileName = null;

            //db.UserDetails.Remove(userDetail.ResumeFileName);

            db.Entry(userDetail).State = EntityState.Modified;

            // Manual Delete
            if (userDetail.ResumeFileName != null)
            {
                System.IO.File.Delete(Server.MapPath("~/Content/resumes/" + userDetail.ResumeFileName));
            }

            db.SaveChanges();
            TempData["message"] = "Your Resume was successfully deleted.";
            return RedirectToAction("Details", new { id = userViewId });
        }

        


        // GET: UserDetails/Create
        public ActionResult Create()
        {
            
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: UserDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,FirstName,LastName,ResumeFileName,IsOpenToRelocation,Title,EmploymentType,VisaStatus,DateOfHire,Notes,UserImage,IsOpenToNewOpps")] UserDetail userDetail)
        {
            if (ModelState.IsValid)
            {

                db.UserDetails.Add(userDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

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
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", userDetail.UserId);
            return View(userDetail);
        }

        // POST: UserDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,FirstName,LastName,ResumeFileName,IsOpenToRelocation,Title,EmploymentType,VisaStatus,DateOfHire,Notes,UserImage,IsOpenToNewOpps")] UserDetail userDetail, HttpPostedFileBase resume, HttpPostedFileBase userImg, string userViewId)
        {
          
            if (ModelState.IsValid)
            {
                if (resume != null)
                {
                    #region Resume

                    string resumeName = resume.FileName;

                    string ext = resumeName.Substring(resumeName.LastIndexOf('.'));

                    string[] validResumeExts = { ".pdf" };

                    if (validResumeExts.Contains(ext.ToLower()) && (resume.ContentLength <= 4194304))//4mb
                    {
                        //Manual Delete 
                        if (userDetail.ResumeFileName != null)
                        {
                            System.IO.File.Delete(Server.MapPath("~/Content/resumes/" + userDetail.ResumeFileName));
                        }

                        resumeName = Guid.NewGuid() + ext.ToLower();

                        resume.SaveAs(Server.MapPath("~/Content/resumes/" + resumeName));

                        userDetail.ResumeFileName = resumeName;

                    }
                    else
                    {
                        ViewBag.InvalidResumeFile = "Your resume was not uploaded correctely, please try a different file type (only .pdf files are accepted)";
                        return View(userDetail);
                    }

                    #endregion
                }

                if (userImg != null)
                {
                    #region Image

                    string userImgName = userImg.FileName;

                    string ext = userImgName.Substring(userImgName.LastIndexOf('.'));

                    string[] validImgExts = { ".jpeg", ".jpg", ".png" };

                    if (validImgExts.Contains(ext.ToLower()) && (userImg.ContentLength <= 4194304))
                    {
                        //Manual Delete 
                        if (userDetail.UserImage != null)
                        {
                            System.IO.File.Delete(Server.MapPath("~/Content/employeeImages/" + userDetail.UserImage));
                        }

                        userImgName = Guid.NewGuid() + ext.ToLower();

                        userImg.SaveAs(Server.MapPath("~/Content/employeeImages/" + userImgName));

                        userDetail.UserImage = userImgName;

                    }
                    else
                    {
                        ViewBag.InvalidImageFile = "Your Image was not uploaded correctely, please try a different file type (only .jpg, .jpeg and .png files are accepted)";
                        return View(userDetail);
                    }

                    #endregion
                }

                db.Entry(userDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = userViewId });
            }


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

            // Manual Delete Resume
            if (userDetail.ResumeFileName != null)
            {
                System.IO.File.Delete(Server.MapPath("~/Content/resumes/" + userDetail.ResumeFileName));
            }

            // Manual Delete User Image
            if (userDetail.ResumeFileName != null)
            {
                System.IO.File.Delete(Server.MapPath("~/Content/resumes/" + userDetail.ResumeFileName));
            }

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

