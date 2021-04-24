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
            var tickets = db.Tickets.Include(t => t.Flight).Include(t => t.Passenger).Where(t=>t.Passenger.User == User.Identity);

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
        public ActionResult Details(string id)
        {
            if (id == string.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.FirstOrDefault(p => p.PublicId.Equals(id));
            if (ticket == null || ticket.Passenger.User != User.Identity)
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
        public ActionResult Create()
        {
            ViewBag.FlightId = new SelectList(db.Flights, "FlightId", "FlightId");
            ViewBag.PassengerId = new SelectList(db.Passengers, "PassengerId", "FirstName");
            return View();
        }

        // POST: Tickets/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PassengerId,FlightId")] TicketFormModel request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if(!db.Passengers.Any(p=>p.PassengerId == request.PassengerId) || !db.Flights.Any(p=>p.FlightId == request.FlightId))
                    {
                        ViewBag.FlightId = new SelectList(db.Flights, "FlightId", "FlightId", request.FlightId);
                        ViewBag.PassengerId = new SelectList(db.Passengers, "PassengerId", "FirstName", request.PassengerId);
                        return View(request);
                    }

                    //DO SPRAWDZENIA, MOZE GENEROWAĆ BŁĘDY
                    Ticket ticket = new Ticket()
                    {
                        PublicId = Guid.NewGuid(),
                        FlightId = request.FlightId,
                        PassengerId = request.PassengerId
                    };

                    Flight flight = ticket.Flight;
                    flight.NumberOfFreeSeats--;

                    db.Tickets.Add(ticket);
                    db.Entry(flight).State = EntityState.Modified;
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
        public ActionResult Edit(string id)
        {
            if (id == string.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.FirstOrDefault(p => p.PublicId.Equals(id));
            if (ticket == null)
            {
                return HttpNotFound();
            }
            ViewBag.FlightId = new SelectList(db.Flights, "FlightId", "FlightId", ticket.FlightId);
            ViewBag.PassengerId = new SelectList(db.Passengers, "PassengerId", "FirstName", ticket.PassengerId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TicketId,PublicId,PassengerId,FlightId")] Ticket ticket)
        {

            if (ModelState.IsValid)
            {
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FlightId = new SelectList(db.Flights, "FlightId", "FlightId", ticket.FlightId);
            ViewBag.PassengerId = new SelectList(db.Passengers, "PassengerId", "FirstName", ticket.PassengerId);
            return View(ticket);
        }
        #endregion

        // GET: Tickets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
