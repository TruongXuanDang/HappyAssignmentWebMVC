using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HappyMVCAssignment.Models;

namespace HappyMVCAssignment.Controllers
{
    public class LateEventsController : Controller
    {
        private HappyMVCAssignmentContext db = new HappyMVCAssignmentContext();

        // GET: LateEvents
        public ActionResult Index()
        {
            var lateEvents = db.LateEvents.Include(l => l.Student);
            return View(lateEvents.ToList());
        }

        // GET: LateEvents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LateEvent lateEvent = db.LateEvents.Find(id);
            if (lateEvent == null)
            {
                return HttpNotFound();
            }
            return View(lateEvent);
        }

        // GET: LateEvents/Create
        public ActionResult Create()
        {
            ViewBag.StudentId = new SelectList(db.Students, "Id", "Name");
            return View();
        }

        // POST: LateEvents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,LateDate,LateType,LateMoney,PushCount,StudentId")] LateEvent lateEvent)
        {
            if (ModelState.IsValid)
            {
                db.LateEvents.Add(lateEvent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StudentId = new SelectList(db.Students, "Id", "Name", lateEvent.StudentId);
            return View(lateEvent);
        }

        // GET: LateEvents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LateEvent lateEvent = db.LateEvents.Find(id);
            if (lateEvent == null)
            {
                return HttpNotFound();
            }
            ViewBag.StudentId = new SelectList(db.Students, "Id", "Name", lateEvent.StudentId);
            return View(lateEvent);
        }

        // POST: LateEvents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,LateDate,LateType,LateMoney,PushCount,StudentId")] LateEvent lateEvent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lateEvent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StudentId = new SelectList(db.Students, "Id", "Name", lateEvent.StudentId);
            return View(lateEvent);
        }

        // GET: LateEvents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LateEvent lateEvent = db.LateEvents.Find(id);
            if (lateEvent == null)
            {
                return HttpNotFound();
            }
            return View(lateEvent);
        }

        // POST: LateEvents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LateEvent lateEvent = db.LateEvents.Find(id);
            db.LateEvents.Remove(lateEvent);
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
