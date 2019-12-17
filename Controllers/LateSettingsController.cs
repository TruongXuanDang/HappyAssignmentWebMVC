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
    public class LateSettingsController : Controller
    {
        private HappyMVCAssignmentContext db = new HappyMVCAssignmentContext();

        // GET: LateSettings
        public ActionResult Index()
        {
            return View(db.LateSettings.ToList());
        }

        // GET: LateSettings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LateSetting lateSetting = db.LateSettings.Find(id);
            if (lateSetting == null)
            {
                return HttpNotFound();
            }
            return View(lateSetting);
        }

        // GET: LateSettings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LateSettings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,MoneyPerLate,PushPerLate,SecondRate,ThirdRate")] LateSetting lateSetting)
        {
            if (ModelState.IsValid)
            {
                db.LateSettings.Add(lateSetting);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(lateSetting);
        }

        // GET: LateSettings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LateSetting lateSetting = db.LateSettings.Find(id);
            if (lateSetting == null)
            {
                return HttpNotFound();
            }
            return View(lateSetting);
        }

        // POST: LateSettings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MoneyPerLate,PushPerLate,SecondRate,ThirdRate")] LateSetting lateSetting)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lateSetting).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lateSetting);
        }

        // GET: LateSettings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LateSetting lateSetting = db.LateSettings.Find(id);
            if (lateSetting == null)
            {
                return HttpNotFound();
            }
            return View(lateSetting);
        }

        // POST: LateSettings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LateSetting lateSetting = db.LateSettings.Find(id);
            db.LateSettings.Remove(lateSetting);
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
