﻿using System;
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
    public class OpenPositionsController : Controller
    {
        private JobBoardEntities db = new JobBoardEntities();

        // GET: OpenPositions
        public ActionResult Index()
        {
            var openPositions = db.OpenPositions.Include(o => o.Location).Include(o => o.Position);
            return View(openPositions.ToList());
        }

        // GET: OpenPositions/Details/5
        public ActionResult Details(int? id)
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
            return View(openPosition);
        }

        // GET: OpenPositions/Create
        public ActionResult Create()
        {
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "StoreNumber");
            ViewBag.PositionId = new SelectList(db.Positions, "PositionId", "Title");
            return View();
        }

        // POST: OpenPositions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OpenPositionId,LocationId,PositionId,IsRemote,EmploymentType,Duration")] OpenPosition openPosition)
        {
            if (ModelState.IsValid)
            {
                db.OpenPositions.Add(openPosition);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "StoreNumber", openPosition.LocationId);
            ViewBag.PositionId = new SelectList(db.Positions, "PositionId", "Title", openPosition.PositionId);
            return View(openPosition);
        }

        // GET: OpenPositions/Edit/5
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
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "StoreNumber", openPosition.LocationId);
            ViewBag.PositionId = new SelectList(db.Positions, "PositionId", "Title", openPosition.PositionId);
            return View(openPosition);
        }

        // POST: OpenPositions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OpenPositionId,LocationId,PositionId,IsRemote,EmploymentType,Duration")] OpenPosition openPosition)
        {
            if (ModelState.IsValid)
            {
                db.Entry(openPosition).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "StoreNumber", openPosition.LocationId);
            ViewBag.PositionId = new SelectList(db.Positions, "PositionId", "Title", openPosition.PositionId);
            return View(openPosition);
        }

        // GET: OpenPositions/Delete/5
        public ActionResult Delete(int? id)
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
            return View(openPosition);
        }

        // POST: OpenPositions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OpenPosition openPosition = db.OpenPositions.Find(id);
            db.OpenPositions.Remove(openPosition);
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
