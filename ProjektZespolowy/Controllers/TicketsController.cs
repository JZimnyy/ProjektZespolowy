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
using ProjektZespolowy.Models.Tickets;

namespace ProjektZespolowy.Controllers
{
    public class TicketsController : BaseController
    {
        #region Index()
        // GET: Tickets
        public ActionResult Index()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).First();

            var tickets = db.Tickets.Include(t => t.Flight).Include(t => t.Passenger).Where(t=>t.Passenger.User.Id == user.Id);

            List<TicketViewModel> list = new List<TicketViewModel>();

            foreach(var ticket in tickets)
            {
                TicketViewModel model = new TicketViewModel()
                {
                    PublicId = ticket.PublicId,
                    PassengerId = ticket.PassengerId,
                    Passenger = ticket.Passenger,
                    FlightId = ticket.FlightId,
                    Flight = ticket.Flight
                };
                list.Add(model);
            }

            return View(list.ToList());
        }
        #endregion

        #region Details()
        // GET: Tickets/Details/5
        public ActionResult Details(Guid id)
        {
            if (id == Guid.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Include(p=>p.Flight).FirstOrDefault(p => p.PublicId ==id);

            var user = db.Users.Where(u => u.UserName == User.Identity.Name).First();

            if (ticket == null || ticket.Passenger.User.Id != user.Id)
            {
                return HttpNotFound();
            }

            TicketViewModel model = new TicketViewModel()
            {
                PublicId = ticket.PublicId,
                PassengerId = ticket.PassengerId,
                Passenger = ticket.Passenger,
                FlightId = ticket.FlightId,
                Flight = ticket.Flight
            };

            return View(model);
        }
        #endregion

        #region Create()
        // GET: Tickets/Create
        public ActionResult Create(Guid id)
        {
            if(id == Guid.Empty)
            {
                return RedirectToAction("Index","Flights");
            }

            var flight = db.Flights.Where(p => p.PublicId == id).First();

            if(flight == null)
            {
                return HttpNotFound();
            }

            ViewBag.DepartureDate = flight.DepartureDate.ToString("yyyy-MM-dd hh:mm");
            ViewBag.ArrivalDate = flight.ArrivalDate.ToString("yyyy-MM-dd hh:mm");
            ViewBag.StartAirport = flight.AirRoute.StartAirportCode;
            ViewBag.FinishAirport = flight.AirRoute.FinishAirportCode;
            ViewBag.AirLine = flight.AirRoute.AirLine.Name;
            ViewBag.Price = flight.Price;

            var flights = db.Flights.Where(p => p.PublicId == id).Select(x => new SelectListItem
            {
                Value = x.FlightId.ToString(),
                Text = x.AirRoute.StartAirportCode + "-" + x.AirRoute.FinishAirportCode
            });

            var user = db.Users.Where(u => u.UserName == User.Identity.Name).First();

            var passengers = db.Passengers.Where(p => p.User.Id == user.Id).Select(x => new SelectListItem
            {
                Value = x.PassengerId.ToString(),
                Text = x.FirstName + " " + x.LastName
            });

            ViewBag.FlightId = new SelectList(flights, "Value", "Text");
            ViewBag.PassengerId = new SelectList(passengers, "Value", "Text");
            return View();
        }

        // POST: Tickets/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Guid id,[Bind(Include = "PassengerId,FlightId")] TicketFormModel request)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return RedirectToAction("Index","Flights");
                }

                var selectedflight = db.Flights.Where(p => p.PublicId == id).First();

                if (selectedflight == null)
                {
                    return HttpNotFound();
                }

                ViewBag.DepartureDate = selectedflight.DepartureDate.ToString("yyyy-MM-dd hh:mm");
                ViewBag.ArrivalDate = selectedflight.ArrivalDate.ToString("yyyy-MM-dd hh:mm");
                ViewBag.StartAirport = selectedflight.AirRoute.StartAirportCode;
                ViewBag.FinishAirport = selectedflight.AirRoute.FinishAirportCode;
                ViewBag.AirLine = selectedflight.AirRoute.AirLine.Name;
                ViewBag.Price = selectedflight.Price;

                var flights = db.Flights.Where(p => p.PublicId == id).Select(x => new SelectListItem
                {
                    Value = x.FlightId.ToString(),
                    Text = x.AirRoute.StartAirportCode + "-" + x.AirRoute.FinishAirportCode
                });

                var user = db.Users.Where(u => u.UserName == User.Identity.Name).First();

                var passengers = db.Passengers.Where(p => p.User.Id == user.Id).Select(x => new SelectListItem
                {
                    Value = x.PassengerId.ToString(),
                    Text = x.FirstName + " " + x.LastName
                });

                ViewBag.FlightId = new SelectList(flights, "Value", "Text",request.FlightId);
                ViewBag.PassengerId = new SelectList(passengers, "Value", "Text",request.PassengerId);

                if (ModelState.IsValid)
                {
                    if(!db.Passengers.Any(p=>p.PassengerId == request.PassengerId) || !db.Flights.Any(p=>p.FlightId == request.FlightId))
                    {
                        return View(request);
                    }

                    if(db.Tickets.Where(p=>p.PassengerId == request.PassengerId && p.FlightId == request.FlightId).Any())
                    {
                        return RedirectToAction("Index");
                    }

                    //DO SPRAWDZENIA, MOZE GENEROWAĆ BŁĘDY
                    Ticket ticket = new Ticket()
                    {
                        PublicId = Guid.NewGuid(),
                        FlightId = request.FlightId,
                        PassengerId = request.PassengerId
                    };

                    ticket.Flight = selectedflight;
                    ticket.Passenger = db.Passengers.Find(request.PassengerId);
                    //flight.NumberOfFreeSeats--;

                    db.Tickets.Add(ticket);
                    //db.Entry(flight).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.FlightId = new SelectList(db.Flights, "FlightId", "FlightId", request.FlightId);
                ViewBag.PassengerId = new SelectList(db.Passengers, "PassengerId", "FirstName", request.PassengerId);
                return View(request);
            }
            catch
            {
                ViewBag.FlightId = new SelectList(db.Flights, "FlightId", "FlightId", request.FlightId);
                ViewBag.PassengerId = new SelectList(db.Passengers, "PassengerId", "FirstName", request.PassengerId);
                return View(request);
            }
        }
        #endregion

        #region Edit()
        // GET: Tickets/Edit/5
        //public ActionResult Edit(string id)
        //{
        //    if (id == string.Empty)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Ticket ticket = db.Tickets.FirstOrDefault(p => p.PublicId.Equals(id));
        //    if (ticket == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.FlightId = new SelectList(db.Flights, "FlightId", "FlightId", ticket.FlightId);
        //    ViewBag.PassengerId = new SelectList(db.Passengers, "PassengerId", "FirstName", ticket.PassengerId);
        //    return View(ticket);
        //}

        //// POST: Tickets/Edit/5
        //// Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        //// Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "TicketId,PublicId,PassengerId,FlightId")] Ticket ticket)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(ticket).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.FlightId = new SelectList(db.Flights, "FlightId", "FlightId", ticket.FlightId);
        //    ViewBag.PassengerId = new SelectList(db.Passengers, "PassengerId", "FirstName", ticket.PassengerId);
        //    return View(ticket);
        //}
        #endregion

        #region Delete
        // GET: Tickets/Delete/5
        public ActionResult Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Include(p=>p.Flight).Where(p => p.PublicId == id).First();
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).First();
            if (ticket == null || ticket.Passenger.User.Id != user.Id)
            {
                return HttpNotFound();
            }

            if(ticket.Flight.DepartureDate<=DateTime.Now)
            {
                return RedirectToAction("Index");
            }

            TicketViewModel viewModel = new TicketViewModel()
            {
                Flight = ticket.Flight,
                FlightId = ticket.FlightId,
                PublicId = ticket.PublicId,
                Passenger = ticket.Passenger,
                PassengerId = ticket.PassengerId
            };

            return View(viewModel);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Ticket ticket = db.Tickets.Where(p => p.PublicId == id).First();
            db.Tickets.Remove(ticket);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion
    }
}
