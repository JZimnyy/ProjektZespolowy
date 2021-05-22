using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjektZespolowy.Models;
using ProjektZespolowy.Models.AirPort;

namespace ProjektZespolowy.Controllers
{
    public class AirportsController : BaseController
    {
       
        #region Index()
        // GET: Airports
        public ActionResult Index()
        {
            
            return View();
        }
        #endregion

        #region AirportsList(search,code)
        public ActionResult AirportsList(string search,string code)
        {
            List<Airport> airports = db.Airports.Where(p => p.IsActive == true).ToList();

            if(search!=String.Empty && search!=null)
            {
                if(code!=null && code!=String.Empty)
                {
                    airports = db.Airports.Where(p => p.IsActive == true && p.Name.Contains(search) && p.Code.Contains(code)).ToList();
                }else
                {
                airports = db.Airports.Where(p => p.IsActive == true && p.Name.Contains(search)).ToList();
                }
            }else if(code!=null && code!=String.Empty)
            {
                airports = db.Airports.Where(p => p.IsActive == true && p.Code.Contains(code)).ToList();
            }

            List<AirPortViewModel> airportsViewModel = new List<AirPortViewModel>();

            foreach (Airport airport in airports)
            {
                AirPortViewModel ap = new AirPortViewModel();
                ap.Name = airport.Name;
                ap.Code = airport.Code;
                ap.publicId = airport.PublicId;
                ap.StartedRoutes = db.AirRoutes.Where(c => c.StartAirportCode == airport.Code).ToList();
                ap.FinishRoutes = db.AirRoutes.Where(c => c.FinishAirportCode == airport.Code).ToList();

                airportsViewModel.Add(ap);
            }

            return PartialView(airportsViewModel);
        }
        #endregion

        #region Details()
        // GET: Airports/Details/5
        public ActionResult Details(string code)
        {
            if (code == string.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Airport airport = db.Airports.FirstOrDefault(p=>p.Code.Equals(code));
            if (airport == null)
            {
                return HttpNotFound();
            }

            AirPortViewModel airPortViewModel = new AirPortViewModel
            {
                publicId = airport.PublicId,
                Name = airport.Name,
                Code = airport.Code,
                StartedRoutes = db.AirRoutes.Where(c => c.StartAirportCode == airport.Code).ToList(),
                FinishRoutes = db.AirRoutes.Where(c => c.FinishAirportCode == airport.Code).ToList()
            };

            return View(airPortViewModel);
        }
        #endregion

        #region Create()
        // GET: Airports/Create
        public ActionResult Create()
        {
            return View();
        }

 
        // POST: Airports/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AirportId,Name,Code")] AirPortFormModel airport)
        {

            try
            {

                if (ModelState.IsValid)
                {
                    Airport newAirport = new Airport { Name = airport.Name, Code = airport.Code, PublicId = Guid.NewGuid(), IsActive = true};
                    db.Airports.Add(newAirport);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(airport);
            }
            catch
            {
                return View(airport);
            }

        }
        #endregion

        #region Edit()
        // GET: Airports/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == Guid.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Airport airport = db.Airports.Where(a => a.PublicId == id).First();

            if (airport == null)
            {
                return HttpNotFound();
            }

            AirPortFormModel toEdit = new AirPortFormModel { Name = airport.Name, Code = airport.Code };
            return View(toEdit);
        }

        // POST: Airports/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid? id,[Bind(Include = "AirportId,PublicId,Name,Code")] AirPortFormModel airport)
        {
            
            if (ModelState.IsValid)
            {
                if (id == Guid.Empty)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                Airport airportToEdit = db.Airports.Where(a => a.PublicId == id).First();

                if (airportToEdit == null)
                {
                    return HttpNotFound();
                }

                airportToEdit.Name = airport.Name;
                airportToEdit.Code = airport.Code;

                db.Entry(airportToEdit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(airport);
        }

        #endregion

        #region Delete()
        // GET: Airports/Delete/5
        public ActionResult Delete(Guid? id)
        {
            try{ 
                if (id == Guid.Empty)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Airport airport = db.Airports.Where(a => a.PublicId == id).First();
                if (airport == null)
                {
                    return HttpNotFound();
                }

                AirPortFormModel toDelete = new AirPortFormModel
                {
                    Name = airport.Name,
                    Code = airport.Code
                };

                return View(toDelete);
            }
            catch{
                return RedirectToAction("Index");
            }
        }

        // POST: Airports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Airport airport = db.Airports.Where(a => a.PublicId == id).First();
            airport.IsActive = false;

            db.Entry(airport).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion 
        
    }
}
