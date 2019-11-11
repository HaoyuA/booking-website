using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FIT5032_Assign.Models;
using Microsoft.AspNet.Identity;

namespace FIT5032_Assign.Controllers
{
    public class BookingsController : Controller
    {
        private HotelModel db = new HotelModel();

        // GET: Bookings
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var bookings = db.Booking.Where(b => b.ASPUserId == userId).OrderBy(b => b.CheckinDate);
            return View(bookings.ToList());
        }

        // GET: Bookings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Booking.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // GET: Bookings/Create
        public ActionResult Create()
        {
            ViewBag.RoomId = new SelectList(db.Room, "Id", "RoomType");
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CheckinDate,CheckoutDate,ASPUserId,Rating,Comment,RoomId,Price,RoomNumber")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Booking.Add(booking);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RoomId = new SelectList(db.Room, "Id", "RoomType", booking.RoomId);
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Booking.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoomId = new SelectList(db.Room, "Id", "RoomType", booking.RoomId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CheckinDate,CheckoutDate,ASPUserId,Rating,Comment,RoomId,Price,RoomNumber")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RoomId = new SelectList(db.Room, "Id", "RoomType", booking.RoomId);
            return View(booking);
        }

        [HttpPost]
        
        public ActionResult AddBooking(int id, int number, string from, string to, double roomPrice)
        {
            if (number <= 0)
            {
                return Content("<script>alert('You must book at least one room');history.go(-1);</script>");
            }
            var userId = User.Identity.GetUserId();
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var dateFrom = DateTime.ParseExact(from, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var dateTo = DateTime.ParseExact(to, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var msg = "sass";
            if(dateFrom == dateTo)
            {
                Response.Write("<script>alert('" + msg + "')</script>");
                return View();
            }
            TimeSpan span = dateTo - dateFrom;
            var days = span.TotalDays;
            var date = dateFrom;
            while (date < dateTo)
            {
                db.RoomStates.Where(r => r.Date == dateFrom && r.RoomId == id).FirstOrDefault().AvaibleRoom -= number;
                db.SaveChanges();
                date = date.AddDays(1);
            }
            Booking booking = new Booking
            {

                RoomId = id,
                CheckinDate = dateFrom,
                CheckoutDate = dateTo,
                ASPUserId = userId,
                Price = roomPrice * days,
                RoomNumber = number
            };
            db.Booking.Add(booking);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

            // GET: Bookings/Delete/5
            public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Booking.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        public ActionResult Rating(int id)
        {
            Booking booking = db.Booking.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            ViewBag.HotelName = booking.Room.Hotel.HotelName;
            ViewBag.Id = id;
            return View();
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Rating(int id, int? rating, string comment)
        {
            db.Booking.Find(id).Rating = rating;
            db.Booking.Find(id).Comment = comment;
            db.SaveChanges();
            int hotelId = db.Booking.Find(id).Room.HotelId;
            var totalRating = Convert.ToDecimal(db.Hotel.Find(hotelId).HotelRating) * db.Hotel.Find(hotelId).HotelRatingCount;
            var newRating = (totalRating + rating) / (db.Hotel.Find(hotelId).HotelRatingCount + 1);
            db.Hotel.Find(hotelId).HotelRatingCount += 1;
            db.Hotel.Find(hotelId).HotelRating = newRating.ToString();
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult EditRating(int id)
        {
            Booking booking = db.Booking.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            ViewBag.HotelName = booking.Room.Hotel.HotelName;
            ViewBag.Id = id;
            ViewBag.comment = booking.Comment;
            return View();
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult EditRating(int id, int? rating, string comment)
        {
            var previousRating = db.Booking.Find(id).Rating;
            db.Booking.Find(id).Rating = rating;
            db.Booking.Find(id).Comment = comment;
            db.SaveChanges();
            int hotelId = db.Booking.Find(id).Room.HotelId;
            var totalRating = Convert.ToDecimal(db.Hotel.Find(hotelId).HotelRating) * db.Hotel.Find(hotelId).HotelRatingCount;
            var newRating = (totalRating + rating - previousRating) / (db.Hotel.Find(hotelId).HotelRatingCount);
            
            db.Hotel.Find(hotelId).HotelRating = newRating.ToString();
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Booking booking = db.Booking.Find(id);
            db.Booking.Remove(booking);
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
