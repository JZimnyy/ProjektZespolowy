using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdentitySample.Models;
using ProjektZespolowy.Models;
using ProjektZespolowy.Models.Flights;

namespace ProjektZespolowy.Controllers
{
    public class FlightsController : BaseController
    {

        #region Index()
        // GET: Flights
        public ActionResult Index()
        {
            var flights = db.Flights.Include(f => f.AirRoute);

            List<FlightViewModel> list = new List<FlightViewModel>();

            foreach(var flight in flights)
            {
                FlightViewModel model = new FlightViewModel()
                {
                    PublicId = flight.PublicId,
                    AirRouteId = flight.AirRouteId,
                    AirRoute = flight.AirRoute,
                    NumberOfFreeSeats = flight.NumberOfFreeSeats,
                    ArrivalDate = flight.ArrivalDate,
                    DepartureDate = flight.DepartureDate,
                    Price = flight.Price
                };

                list.Add(model);
            }

            return View(list.ToList());
        }
        #endregion

        #region Details()
        // GET: Flights/Details/5
        public ActionResult Details(string id)
        {
            if (id == string.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Flight flight = db.Flights.FirstOrDefault(p => p.PublicId.Equals(id));

            if (flight == null)
            {
                return HttpNotFound();
            }

            FlightViewModel model = new FlightViewModel()
            {
                PublicId = flight.PublicId,
                AirRouteId = flight.AirRouteId,
                AirRoute = flight.AirRoute,
                NumberOfFreeSeats = flight.NumberOfFreeSeats,
                ArrivalDate = flight.ArrivalDate,
                DepartureDate = flight.DepartureDate,
                Price = flight.Price
            };

            return View(model);
        }
        #endregion

        #region Create()
        // GET: Flights/Create
        public ActionResult Create()
        {
            ViewBag.AirRouteId = new SelectList(db.AirRoutes, "AirRouteId", "StartAirportCode");
            return View();
        }

        // POST: Flights/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AirRouteId,NumberOfFreeSeats,DepartureDate,ArrivalDate,Price")] FlightFormModel request)
        {
            try
            {
            if (ModelState.IsValid)
            {
                if(!db.AirRoutes.Any(p=>p.AirRouteId == request.AirRouteId))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                Flight flight = new Flight()
                {
                    PublicId = Guid.NewGuid(),
                    AirRouteId = request.AirRouteId,
                    AirRoute = db.AirRoutes.Find(request.AirRouteId),
                    NumberOfFreeSeats = request.NumberOfFreeSeats,
                    DepartureDate = request.DepartureDate,
                    ArrivalDate = request.ArrivalDate,
                    Price = request.Price
                };

                db.Flights.Add(flight);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AirRouteId = new SelectList(db.AirRoutes, "AirRouteId", "StartAirportCode", request.AirRouteId);
            return View(request);
            }
            catch
            {
                return View(request);
            }
        }
        #endregion

        #region Edit()
        // GET: Flights/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == string.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Flight flight = db.Flights.FirstOrDefault(p => p.PublicId.Equals(id));

            if (flight == null)
            {
                return HttpNotFound();
            }

            FlightFormModel model = new FlightFormModel()
            {
                AirRouteId = flight.AirRouteId,
                NumberOfFreeSeats = flight.NumberOfFreeSeats,
                DepartureDate = flight.DepartureDate,
                ArrivalDate = flight.ArrivalDate,
                Price = flight.Price
            };

            ViewBag.AirRouteId = new SelectList(db.AirRoutes, "AirRouteId", "StartAirportCode", model.AirRouteId);
            return View(model);
        }

        // POST: Flights/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id,[Bind(Include = "AirRouteId,NumberOfFreeSeats,DepartureDate,ArrivalDate,Price")] FlightFormModel request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if(db.AirRoutes.Any(p=>p.AirRouteId == request.AirRouteId))
                    {
                        return View(request);
                    }

                    Flight flight = db.Flights.FirstOrDefault(p => p.PublicId.Equals(id));

                    if(flight == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }

                    flight.AirRouteId = request.AirRouteId;
                    flight.AirRoute = db.AirRoutes.Find(request.AirRouteId);
                    flight.ArrivalDate = request.ArrivalDate;
                    flight.DepartureDate = request.DepartureDate;
                    flight.NumberOfFreeSeats = request.NumberOfFreeSeats;
                    flight.Price = request.Price;

                    db.Entry(flight).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.AirRouteId = new SelectList(db.AirRoutes, "AirRouteId", "StartAirportCode", request.AirRouteId);
                return View(request);
            }
            catch
            {
                ViewBag.AirRouteId = new SelectList(db.AirRoutes, "AirRouteId", "StartAirportCode", request.AirRouteId);
                return View(request);
            }
        }
        #endregion

        #region Delete()
        // GET: Flights/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == string.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Flight flight = db.Flights.Include(p => p.AirRoute).FirstOrDefault(p => p.PublicId.Equals(id));
            if (flight == null)
            {
                return HttpNotFound();
            }

            FlightViewModel model = new FlightViewModel()
            {
                PublicId = flight.PublicId,
                AirRouteId = flight.AirRouteId,
                AirRoute = flight.AirRoute,
                ArrivalDate = flight.ArrivalDate,
                DepartureDate = flight.DepartureDate,
                NumberOfFreeSeats = flight.NumberOfFreeSeats,
                Price = flight.Price
            };

            return View(model);
        }

        // POST: Flights/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (id == string.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Flight flight = db.Flights.Include(p => p.AirRoute).FirstOrDefault(p => p.PublicId.Equals(id));
            db.Flights.Remove(flight);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion

    }
}
