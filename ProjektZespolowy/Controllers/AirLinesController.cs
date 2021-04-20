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
using ProjektZespolowy.Models.AirLines;

namespace ProjektZespolowy.Controllers
{
    public class AirLinesController : BaseController
    {

        #region Index()
        // GET: AirLines
        public ActionResult Index()
        {
            List<AirLine> airLines = db.AirLines.Include(p=>p.Routes).ToList();

            List<AirlineViewModel> airlineViewModels = new List<AirlineViewModel>();

            foreach(var airline in airLines)
            {
                AirlineViewModel model = new AirlineViewModel()
                {
                    Name = airline.Name,
                    Country = airline.Country,
                    LinkToPage = airline.LinkToPage,
                    Routes = airline.Routes,
                    PublicId = airline.PublicId
                };
                airlineViewModels.Add(model);
            }

            return View(airlineViewModels);
        }
        #endregion

        #region Details()
        // GET: AirLines/Details/5
        public ActionResult Details(Guid id)
        {
            if (id == Guid.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AirLine airLine = db.AirLines.FirstOrDefault(p => p.PublicId.Equals(id));
            if (airLine == null)
            {
                return HttpNotFound();
            }

            AirlineViewModel model = new AirlineViewModel()
            {
                Name = airLine.Name,
                Country = airLine.Country,
                LinkToPage = airLine.LinkToPage,
                PublicId = airLine.PublicId,
                Routes = airLine.Routes
            };

            return View(model);
        }
        #endregion

        #region Create()
        // GET: AirLines/Create
        public ActionResult Create()
        {
            return View();
        }


        // POST: AirLines/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Country,LinkToPage")] AirLineFormModel airLine)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    AirLine newAirline = new AirLine()
                    {
                        Name = airLine.Name,
                        Country = airLine.Country,
                        LinkToPage = airLine.LinkToPage,
                        PublicId = Guid.NewGuid()
                    };

                    db.AirLines.Add(newAirline);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(airLine);
            }
            catch
            {
                return View(airLine);
            }

        }
        #endregion

        #region Edit()
        // GET: AirLines/Edit/5
        public ActionResult Edit(Guid id)
        {
            if (id == Guid.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AirLine airLine = db.AirLines.FirstOrDefault(p => p.PublicId.Equals(id));
            if (airLine == null)
            {
                return HttpNotFound();
            }

            AirLineFormModel formModel = new AirLineFormModel()
            {
                Name = airLine.Name,
                Country = airLine.Country,
                LinkToPage = airLine.LinkToPage
            };

            return View(formModel);
        }

        // POST: AirLines/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id,[Bind(Include = "Name,Country,LinkToPage")] AirLineFormModel request)
        {
            try
            {
                if(id==Guid.Empty)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (ModelState.IsValid)
                {
                    AirLine airLine = db.AirLines.FirstOrDefault(p => p.PublicId.Equals(id));
                    if(airLine!=null)
                    {
                        airLine.Name = request.Name;
                        airLine.Country = request.Country;
                        airLine.LinkToPage = request.LinkToPage;

                        db.Entry(airLine).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                return View(request);
            }
            catch
            {
                return View(request);
            }
        }
        #endregion

        #region Delete()
        // GET: AirLines/Delete/5
        public ActionResult Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AirLine airLine = db.AirLines.FirstOrDefault(p => p.PublicId.Equals(id));
            if (airLine == null)
            {
                return HttpNotFound();
            }

            AirLineFormModel toDelete = new AirLineFormModel()
            {
                Name = airLine.Name,
                Country = airLine.Country,
                LinkToPage = airLine.LinkToPage
            };

            return View(toDelete);
        }

        // POST: AirLines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            AirLine airLine = db.AirLines.FirstOrDefault(p => p.PublicId.Equals(id));
            db.AirLines.Remove(airLine);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion

    }
}
