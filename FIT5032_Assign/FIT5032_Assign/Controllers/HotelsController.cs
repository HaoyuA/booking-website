using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Globalization;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FIT5032_Assign.Models;
using Microsoft.AspNet.Identity;

namespace FIT5032_Assign.Controllers
{
    public class HotelsController : Controller
    {
        private HotelModel db = new HotelModel();

        // GET: Hotels
        public ActionResult Index(string searchString)
        {
            var Hotels = from m in db.Hotel

                         select m;
            
           
            if (!String.IsNullOrEmpty(searchString))

            {

                Hotels = Hotels.Where(s => s.HotelCity.Contains(searchString) || s.HotelName.Contains(searchString));
                 
            }
            return View(Hotels);
        }

        // GET: Hotels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hotel hotel = db.Hotel.Find(id);
            if (hotel == null)
            {
                return HttpNotFound();
            }
            return View(hotel);
        }

        // GET: Hotels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Hotels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Latitude,Longitutde,HotelAddress,HotelDescription,HotelRating,HotelName,HotelCity")] Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                db.Hotel.Add(hotel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hotel);
        }

        // GET: Hotels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hotel hotel = db.Hotel.Find(id);
            if (hotel == null)
            {
                return HttpNotFound();
            }
            return View(hotel);
        }

        // POST: Hotels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Latitude,Longitutde,HotelAddress,HotelDescription,HotelRating,HotelName,HotelCity")] Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hotel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hotel);
        }

        // GET: Hotels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hotel hotel = db.Hotel.Find(id);
            if (hotel == null)
            {
                return HttpNotFound();
            }
            return View(hotel);
        }

        // POST: Hotels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Hotel hotel = db.Hotel.Find(id);
            db.Hotel.Remove(hotel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Booking(int? id, string from, string to)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dateFrom = DateTime.Now.Date;
            var dateTo = DateTime.Now.Date.AddDays(1);
            if (from != null && to != null)
            {
                dateFrom = DateTime.ParseExact(from, "d/M/yyyy", CultureInfo.InvariantCulture);
                dateTo = DateTime.ParseExact(to, "d/M/yyyy", CultureInfo.InvariantCulture);
            }
            if (dateFrom >= dateTo)
            {
                return Content("<script>alert('Date to must be the date after date from');history.go(-1);</script>");
            }
            Hotel hotel = db.Hotel.Find(id);
            var roomTypes = db.Room.Where(r => r.HotelId == id);
            var bookings = db.Booking.Where(b => b.Room.HotelId == id);
            int starFiveCount = bookings.Where(b => b.Rating == 5).Count();
            int starFourCount = bookings.Where(b => b.Rating == 4).Count();
            int starThreeCount = bookings.Where(b => b.Rating == 3).Count();
            int starTwoCount = bookings.Where(b => b.Rating == 2).Count();
            int starOneCount = bookings.Where(b => b.Rating == 1).Count();
            int starZeroCount = bookings.Where(b => b.Rating == 0).Count();
            List<string> roomTypeNames = new List<string>();
            List<string> roomDescriptions = new List<string>();
            List<int> totalRoom = new List<int>();
            List<double> roomPrices = new List<double>();
            List<int> roomIds = new List<int>();
            foreach(Room room in roomTypes)
            {
                
                int avaliableRoom = int.MaxValue;
                var roomId = room.Id;
                var roomStates = db.RoomStates.Where(r => r.RoomId == roomId && r.Date >= dateFrom && r.Date <= dateTo);
                foreach(RoomStates roomState in roomStates)
                {

                    if(roomState.AvaibleRoom < avaliableRoom)
                    {
                        var price = room.RoomPrice + roomState.PriceChange;
                        avaliableRoom = roomState.AvaibleRoom;
                       
                    }
                }
                if(roomStates.Count() != 0 && avaliableRoom > 0)
                {
                    roomTypeNames.Add(room.RoomType);
                    roomDescriptions.Add(room.RoomDescription);
                    roomIds.Add(room.Id);
                    totalRoom.Add(avaliableRoom);
                    roomPrices.Add(room.RoomPrice);
                }
            }
            TimeSpan span = dateTo - dateFrom;
            ViewBag.roomTypeNames = roomTypeNames;
            ViewBag.descriptions = roomDescriptions;
            ViewBag.roomIds = roomIds;
            ViewBag.totalRoom = totalRoom;
            ViewBag.roomPrices = roomPrices;
            ViewBag.dateFrom = dateFrom.ToString("dd/MM/yyyy"); ;
            ViewBag.dateTo = dateTo.ToString("dd/MM/yyyy"); ;
            ViewBag.days = span.TotalDays;
            ViewBag.starFiveCount = starFiveCount;
            ViewBag.starFourCount = starFourCount;
            ViewBag.starThreeCount = starThreeCount;
            ViewBag.starTwoCount = starTwoCount;
            ViewBag.starOneCount = starOneCount;
            ViewBag.starZeroCount = starZeroCount;
            ViewBag.countAll = starFiveCount + starFourCount + starThreeCount + starTwoCount + starOneCount + starZeroCount;


            if (hotel == null)
            {
                return HttpNotFound();
            }
            return View(hotel);

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
