using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
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
            ViewBag.lateSettings = db.LateSettings.FirstOrDefault();
            ViewBag.StudentId = new SelectList(db.Students, "Id", "Name");
            return View();
        }

        // POST: LateEvents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( LateEvent lateEvent)
        {
            var currentEventDate = lateEvent.LateDate.Date;
            
            if (ModelState.IsValid && !db.LateEvents.Any(l => EntityFunctions.TruncateTime(l.LateDate) == currentEventDate && l.StudentId == lateEvent.StudentId))
            {
                db.LateEvents.Add(lateEvent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.errorMessage = "This student was attended";
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
            var currentEventDate = lateEvent.LateDate.Date;
            if (ModelState.IsValid && !db.LateEvents.Any(l => EntityFunctions.TruncateTime(l.LateDate) == currentEventDate && l.StudentId == lateEvent.StudentId))
            {
                db.Entry(lateEvent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.errorMessage = "This student was attended";
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

        public double CalculatePunishment (LateEvent currentEvent)
        {
            var lateSettings = db.LateSettings.FirstOrDefault();

            IQueryable<LateEvent> eventsForTheSameStudent = db.LateEvents.Where(l => l.StudentId == currentEvent.StudentId);
            DateTime theFirstPreviousDate = currentEvent.LateDate.AddDays(-1).Date;
            DateTime theSecondPreviousDate = currentEvent.LateDate.AddDays(-2).Date;
            DateTime theThirdPreviousDate = currentEvent.LateDate.AddDays(-3).Date;

            if (eventsForTheSameStudent.Any(e=> EntityFunctions.TruncateTime(e.LateDate) == theFirstPreviousDate)  
                && !eventsForTheSameStudent.Any(e => EntityFunctions.TruncateTime(e.LateDate) == theSecondPreviousDate))
            {
                currentEvent.LateMoney = lateSettings.MoneyPerLate*lateSettings.SecondRate;
                currentEvent.PushCount = lateSettings.PushPerLate*lateSettings.SecondRate;
            }
            else if(eventsForTheSameStudent.Any(e => EntityFunctions.TruncateTime(e.LateDate) == theFirstPreviousDate) 
                && eventsForTheSameStudent.Any(e => EntityFunctions.TruncateTime(e.LateDate) == theSecondPreviousDate)
                && !eventsForTheSameStudent.Any(e => EntityFunctions.TruncateTime(e.LateDate) == theThirdPreviousDate))
            {
                currentEvent.LateMoney = lateSettings.MoneyPerLate * lateSettings.ThirdRate;
                currentEvent.PushCount = lateSettings.PushPerLate * lateSettings.ThirdRate;
            }
            else
            {
                currentEvent.LateMoney = lateSettings.MoneyPerLate;
                currentEvent.PushCount = lateSettings.PushPerLate ;
            }

            return currentEvent.LateType == LateEvent.Type.Money ? currentEvent.LateMoney : currentEvent.PushCount;
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
