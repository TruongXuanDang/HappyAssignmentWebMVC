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
        public ActionResult Index(int? page, int? limit, string start, string end)
        {
            if (page == null)
            {
                page = 1;
            }

            if (limit == null)
            {
                limit = 10;
            }
            var startTime = DateTime.Now;
            startTime = startTime.AddYears(-1);
            try
            {
                startTime = DateTime.Parse(start);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            var endTime = DateTime.Now;
            try
            {
                endTime = DateTime.Parse(end);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            ViewBag.limit = limit;
            var lateEvents = db.LateEvents.OrderByDescending(s => s.LateDate).Where(s => s.LateDate >= startTime && s.LateDate <= endTime);
            ViewBag.TotalPage = Math.Ceiling((double)lateEvents.Count() / limit.Value);
            ViewBag.CurrentPage = page;
            ViewBag.Limit = limit;
            ViewBag.Start = startTime.ToString("yyyy-MM-dd");
            ViewBag.End = endTime.ToString("yyyy-MM-dd");

            ViewBag.StudentList = db.Students.ToList();
            ViewBag.ClassList = db.Classrooms.ToList();

            var list = lateEvents.Skip((page.Value - 1) * limit.Value).Take(limit.Value).ToList();
            return View(list);
        }
        public ActionResult GetChartData(string start, string end, string studentId, string classId)
        {
            var startTime = DateTime.Now;
            startTime = startTime.AddYears(-1);
            int studentIdValue = studentId !="null"?int.Parse(studentId):0;
            int classIdValue = classId!="null"?int.Parse(classId):0;
            try
            {
                startTime = DateTime.Parse(start);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            startTime = new DateTime(startTime.Year, startTime.Month, startTime.Day, 0, 0, 0, 0);

            var endTime = DateTime.Now;
            try
            {
                endTime = DateTime.Parse(end);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            endTime = new DateTime(endTime.Year, endTime.Month, endTime.Day, 23, 59, 59, 0);

            var data = db.LateEvents.Where(s =>s.LateDate >= startTime && s.LateDate <= endTime )
                //.Where(s => s.StudentId == studentIdValue && s.StudentId != 0)
                //.Where(s => s.Student.ClassroomId == classIdValue && s.Student.ClassroomId != 0)
                .GroupBy(
                    s => new
                    {
                        Year = s.LateDate.Year,
                        Month = s.LateDate.Month,
                        Day = s.LateDate.Day
                    }
                ).Select(s => new
                {
                    Date = s.FirstOrDefault().LateDate,
                    Count = s.Count(),
                    PushCount = s.FirstOrDefault().PushCount,
                    LateMoney = s.FirstOrDefault().LateMoney
                }).OrderBy(s => s.Date).ToList();
            return new JsonResult()
            {
                Data = data.Select(s => new
                {
                    Date = s.Date.ToString("MM/dd/yyyy"),
                    Count = s.Count,
                    PushCount = s.PushCount,
                    LateMoney = s.LateMoney
                }),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
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
