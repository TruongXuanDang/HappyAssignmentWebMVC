using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HappyMVCAssignment.Models;
using LinqKit;
using Microsoft.Ajax.Utilities;
using PagedList;

namespace HappyMVCAssignment.Controllers
{
    public class StudentsController : Controller
    {
        private HappyMVCAssignmentContext db = new HappyMVCAssignmentContext();

        // GET: Students
        public ActionResult Index(String keyword,int? page, int? limit)
        {
            if (page == null)
            {
                page = 1;
            }

            if (limit == null)
            {
                limit = 10;
            }
            var students = db.Students.Include(s => s.Classroom);
            var predicate = PredicateBuilder.New<Student>(true);
            if (!keyword.IsNullOrWhiteSpace())
            {
                predicate = predicate.Or(f => f.Name.Contains(keyword));
                predicate = predicate.Or(f => f.Code.Contains(keyword));
                predicate = predicate.Or(f => f.Phone.Contains(keyword));
                predicate = predicate.Or(f => f.Classroom.Name.Contains(keyword));
                predicate = predicate.Or(f => f.Email.Contains(keyword));
                ViewBag.Keyword = keyword;
            }
            var data = db.Students.AsExpandable().Where(predicate).OrderByDescending(s => s.Id).ToPagedList(page.Value, limit.Value);
            return View(data);
        }

        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            ViewBag.ClassroomId = new SelectList(db.Classrooms, "Id", "Name");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Code,Phone,Email,Status,ClassroomId")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClassroomId = new SelectList(db.Classrooms, "Id", "Name", student.ClassroomId);
            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClassroomId = new SelectList(db.Classrooms, "Id", "Name", student.ClassroomId);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Code,Phone,Email,Status,ClassroomId")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClassroomId = new SelectList(db.Classrooms, "Id", "Name", student.ClassroomId);
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
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
