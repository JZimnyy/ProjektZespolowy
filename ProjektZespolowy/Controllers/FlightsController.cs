using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
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
        public ActionResult Details(Guid id)
        {
            if (id == Guid.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Flight flight = db.Flights.FirstOrDefault(p => p.PublicId==id);

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
            var routes = db.AirRoutes.Select(x => new SelectListItem
            {
                Value = x.AirRouteId.ToString(),
                Text = x.AirLine.Name + " " + x.StartAirportCode + "-" + x.FinishAirportCode
            });

            ViewBag.AirRouteId = new SelectList(routes, "Value", "Text");
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
                var routes = db.AirRoutes.Select(x => new SelectListItem
                {
                    Value = x.AirRouteId.ToString(),
                    Text = x.AirLine.Name + " " + x.StartAirportCode + "-" + x.FinishAirportCode
                });

                ViewBag.AirRouteId = new SelectList(routes, "Value", "Text");

                if (ModelState.IsValid)
                {
                    if(!db.AirRoutes.Any(p=>p.AirRouteId == request.AirRouteId))
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }

                    DateTime start = DateTime.Parse(request.DepartureDate, new CultureInfo("pl-PL"), DateTimeStyles.NoCurrentDateDefault);
                    DateTime finish = DateTime.Parse(request.ArrivalDate, new CultureInfo("pl-PL"), DateTimeStyles.NoCurrentDateDefault);


                    Flight flight = new Flight()
                    {
                        PublicId = Guid.NewGuid(),
                        AirRouteId = request.AirRouteId,
                        AirRoute = db.AirRoutes.Find(request.AirRouteId),
                        NumberOfFreeSeats = request.NumberOfFreeSeats,
                        DepartureDate = start,
                        ArrivalDate = finish,
                        Price = request.Price
                    };

                    if(flight.DepartureDate>=flight.ArrivalDate)
                    {
                        ViewBag.Error = "Niezgodne daty";
                        return View(request);
                    }

                    db.Flights.Add(flight);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            return View(request);
            }
            catch
            {
                var routes = db.AirRoutes.Select(x => new SelectListItem
                {
                    Value = x.AirRouteId.ToString(),
                    Text = x.AirLine.Name + " " + x.StartAirportCode + "-" + x.FinishAirportCode
                });

                ViewBag.AirRouteId = new SelectList(routes, "Value", "Text");

                return View(request);
            }
        }
        #endregion

        #region Edit()
        // GET: Flights/Edit/5
        public ActionResult Edit(Guid id)
        {
            if (id == Guid.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Flight flight = db.Flights.FirstOrDefault(p => p.PublicId == id);

            if (flight == null)
            {
                return HttpNotFound();
            }

            FlightFormModel model = new FlightFormModel()
            {
                AirRouteId = flight.AirRouteId,
                NumberOfFreeSeats = flight.NumberOfFreeSeats,
                DepartureDate = flight.DepartureDate.ToString("yyyy-MM-dd hh:mm"),
                ArrivalDate = flight.ArrivalDate.ToString("yyyy-MM-dd hh:mm"),
                Price = flight.Price
            };

            var routes = db.AirRoutes.Select(x => new SelectListItem
            {
                Value = x.AirRouteId.ToString(),
                Text = x.AirLine.Name + " " + x.StartAirportCode + "-" + x.FinishAirportCode
            });

            ViewBag.AirRouteId = new SelectList(routes, "Value", "Text",model.AirRouteId);
            return View(model);
        }

        // POST: Flights/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id,[Bind(Include = "AirRouteId,NumberOfFreeSeats,DepartureDate,ArrivalDate,Price")] FlightFormModel request)
        {
            try
            {
                var routes = db.AirRoutes.Select(x => new SelectListItem
                {
                    Value = x.AirRouteId.ToString(),
                    Text = x.AirLine.Name + " " + x.StartAirportCode + "-" + x.FinishAirportCode
                });

                ViewBag.AirRouteId = new SelectList(routes, "Value", "Text",request.AirRouteId);

                if (ModelState.IsValid)
                {
                    if(!db.AirRoutes.Any(p=>p.AirRouteId == request.AirRouteId))
                    {
                        return View(request);
                    }

                    Flight flight = db.Flights.FirstOrDefault(p => p.PublicId ==id);

                    if(flight == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }

                    DateTime start = DateTime.Parse(request.DepartureDate, new CultureInfo("pl-PL"), DateTimeStyles.NoCurrentDateDefault);
                    DateTime finish = DateTime.Parse(request.ArrivalDate, new CultureInfo("pl-PL"), DateTimeStyles.NoCurrentDateDefault);

                    flight.AirRouteId = request.AirRouteId;
                    flight.AirRoute = db.AirRoutes.Find(request.AirRouteId);
                    flight.ArrivalDate = finish;
                    flight.DepartureDate = start;
                    flight.NumberOfFreeSeats = request.NumberOfFreeSeats;
                    flight.Price = request.Price;

                    if(flight.DepartureDate>=flight.ArrivalDate)
                    {
                        ViewBag.Error = "Niezgodne daty";
                        return View(request);
                    }

                    db.Entry(flight).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(request);
            }
            catch(Exception e)
            {
                var routes = db.AirRoutes.Select(x => new SelectListItem
                {
                    Value = x.AirRouteId.ToString(),
                    Text = x.AirLine.Name + " " + x.StartAirportCode + "-" + x.FinishAirportCode
                });
                ViewBag.Error = "Błąd w formularzu!";
                ViewBag.AirRouteId = new SelectList(routes, "Value", "Text",request.AirRouteId);
                return View(request);
            }
        }
        #endregion

        #region Delete()
        // GET: Flights/Delete/5
        public ActionResult Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Flight flight = db.Flights.Include(p => p.AirRoute).FirstOrDefault(p => p.PublicId ==id);
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
        public ActionResult DeleteConfirmed(Guid id)
        {
            if (id == Guid.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Flight flight = db.Flights.Include(p => p.AirRoute).FirstOrDefault(p => p.PublicId == id);
            db.Flights.Remove(flight);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion

    }
}
