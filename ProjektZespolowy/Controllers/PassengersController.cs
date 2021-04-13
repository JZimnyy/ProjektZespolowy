using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdentitySample.Models;
using ProjektZespolowy.Models.Passengers;

namespace ProjektZespolowy.Controllers
{
    [Authorize]
    public class PassengersController : BaseController
    {

        #region Index()
        // GET: Passengers
        public ActionResult Index()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).First();

            List<PassengerViewModel> passengerViewModels = new List<PassengerViewModel>();

            var passengers = db.Passengers.Where(p => p.User.Id == user.Id).ToList();

            foreach(var passenger in passengers)
            {
                PassengerViewModel psg = new PassengerViewModel();
                psg.FirstName = passenger.FirstName;
                psg.LastName = passenger.LastName;
                psg.DocumentSerial = passenger.DocumentSerial;
                psg.PESEL = passenger.PESEL;
                psg.publicId = passenger.PublicId;

                passengerViewModels.Add(psg);
            }

            return View(passengerViewModels);
        }
        #endregion

        #region Details()
        // GET: Passengers/Details/5
        public ActionResult Details(Guid? id)
        {
            try 
            { 
                if (id == Guid.Empty)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Passenger passenger = db.Passengers.Where(p => p.PublicId == id).First();
                if (passenger == null)
                {
                    return HttpNotFound();
                }

                PassengerViewModel passengerViewModel = new PassengerViewModel
                {
                    FirstName = passenger.FirstName,
                    LastName = passenger.LastName,
                    publicId = passenger.PublicId,
                    PESEL = passenger.PESEL,
                    DocumentSerial = passenger.DocumentSerial
                };

                return View(passengerViewModel);
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        #endregion

        #region Create()
        // GET: Passengers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Passengers/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FirstName,LastName,PESEL,DocumentSerial")] PassengerFormModel passenger)
        {
            Passenger newPassenger = new Passenger { FirstName = passenger.FirstName, LastName = passenger.LastName, DocumentSerial = passenger.DocumentSerial, PESEL = passenger.PESEL };

            newPassenger.User = db.Users.Where(p => p.UserName == User.Identity.Name).First();
            newPassenger.UserId = 0; //tymczasowe rozwiazanie, trzeba usunac z modelu UserID
            newPassenger.PublicId = Guid.NewGuid();
                        
            
            if (ModelState.IsValid)
            {
                db.Passengers.Add(newPassenger);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(passenger);
        }
        #endregion

        #region Edit()
        // GET: Passengers/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == Guid.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Passenger passenger = db.Passengers.Where(p => p.PublicId == id).First();
            if (passenger == null)
            {
                return HttpNotFound();
            }

            PassengerFormModel passengerFormModel = new PassengerFormModel
            {
                FirstName = passenger.FirstName,
                LastName = passenger.LastName,
                PESEL = passenger.PESEL,
                DocumentSerial = passenger.DocumentSerial,
            };

            return View(passengerFormModel);
        }

        // POST: Passengers/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid? id, [Bind(Include = "FirstName,LastName,PESEL,DocumentSerial")] PassengerFormModel passenger)
        {
            try { 
                if (ModelState.IsValid)
                {
                    if (id == Guid.Empty)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Passenger passengerToEdit = db.Passengers.Include(p=>p.User).Where(p => p.PublicId == id).First();
                    if (passengerToEdit == null)
                    {
                        return HttpNotFound();
                    }

                    passengerToEdit.FirstName = passenger.FirstName;
                    passengerToEdit.LastName = passenger.LastName;
                    passengerToEdit.PESEL = passenger.PESEL;
                    passengerToEdit.DocumentSerial = passenger.DocumentSerial;
               

                    db.Entry(passengerToEdit).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(passenger);
            } 
            catch
            {
                return RedirectToAction("Index");
            }
        }
        #endregion

        #region Delete()
        // GET: Passengers/Delete/5
        public ActionResult Delete(Guid? id)
        {
            try { 
                if (id == Guid.Empty)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Passenger passenger = db.Passengers.Include(p => p.User).Where(p => p.PublicId == id).First();
                if (passenger == null)
                {
                    return HttpNotFound();
                }

                PassengerFormModel toDelete = new PassengerFormModel
                {
                    FirstName = passenger.FirstName,
                    LastName = passenger.LastName,
                    PESEL = passenger.PESEL,
                    DocumentSerial = passenger.DocumentSerial
                };

                return View(toDelete);
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        // POST: Passengers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Passenger passenger = db.Passengers.Include(p => p.User).Where(p => p.PublicId == id).First();
            db.Passengers.Remove(passenger);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion 
    }
}
