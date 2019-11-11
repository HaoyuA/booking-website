using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FIT5032_Assign.Models;

namespace FIT5032_Assign.Controllers
{
    public class RoomStatesController : Controller
    {
        private HotelModel db = new HotelModel();

        // GET: RoomStates
        public ActionResult Index()
        {
            var roomStates = db.RoomStates.Include(r => r.Room);
            return View(roomStates.ToList());
        }

        // GET: RoomStates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomStates roomStates = db.RoomStates.Find(id);
            if (roomStates == null)
            {
                return HttpNotFound();
            }
            return View(roomStates);
        }

        // GET: RoomStates/Create
        public ActionResult Create()
        {
            ViewBag.RoomId = new SelectList(db.Room, "Id", "RoomType");
            return View();
        }

        // POST: RoomStates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,RoomId,Date,PriceChange,AvaibleRoom")] RoomStates roomStates)
        {
            if (ModelState.IsValid)
            {
                db.RoomStates.Add(roomStates);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RoomId = new SelectList(db.Room, "Id", "RoomType", roomStates.RoomId);
            return View(roomStates);
        }

        // GET: RoomStates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomStates roomStates = db.RoomStates.Find(id);
            if (roomStates == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoomId = new SelectList(db.Room, "Id", "RoomType", roomStates.RoomId);
            return View(roomStates);
        }

        // POST: RoomStates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,RoomId,Date,PriceChange,AvaibleRoom")] RoomStates roomStates)
        {
            if (ModelState.IsValid)
            {
                db.Entry(roomStates).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RoomId = new SelectList(db.Room, "Id", "RoomType", roomStates.RoomId);
            return View(roomStates);
        }

        // GET: RoomStates/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomStates roomStates = db.RoomStates.Find(id);
            if (roomStates == null)
            {
                return HttpNotFound();
            }
            return View(roomStates);
        }

        // POST: RoomStates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RoomStates roomStates = db.RoomStates.Find(id);
            db.RoomStates.Remove(roomStates);
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
