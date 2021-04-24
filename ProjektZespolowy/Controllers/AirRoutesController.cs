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
using ProjektZespolowy.Models.AirRoutes;

namespace ProjektZespolowy.Controllers
{
    public class AirRoutesController : BaseController
    {
        #region Index()
        // GET: AirRoutes
        public ActionResult Index()
        {
            var airRoutes = db.AirRoutes.Include(a => a.AirLine).Where(p=>p.IsActive == true).ToList();

            List<AirRouteViewModel> model = new List<AirRouteViewModel>();

            foreach(var airRoute in airRoutes)
            {
                AirRouteViewModel route = new AirRouteViewModel()
                {
                    AirLine = airRoute.AirLine,
                    PublicId = airRoute.PublicId,
                    StartAirport = db.Airports.FirstOrDefault(p => p.Code.Equals(airRoute.StartAirportCode)),
                    FinishAirport = db.Airports.FirstOrDefault(p => p.Code.Equals(airRoute.FinishAirportCode))
                };
                model.Add(route);
            }

            return View(model.ToList());
        }
        #endregion

        #region Details()
        // GET: AirRoutes/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == Guid.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AirRoute airRoute = db.AirRoutes.FirstOrDefault(p => p.PublicId.Equals(id));
            if (airRoute == null)
            {
                return HttpNotFound();
            }

            AirRouteViewModel model = new AirRouteViewModel()
            {
                PublicId = airRoute.PublicId,
                AirLine = airRoute.AirLine,
                StartAirport = db.Airports.FirstOrDefault(p => p.Code.Equals(airRoute.StartAirportCode)),
                FinishAirport = db.Airports.FirstOrDefault(p => p.Code.Equals(airRoute.FinishAirportCode))
            };

            return View(model);
        }
        #endregion

        #region Create()
        // GET: AirRoutes/Create
        public ActionResult Create()
        {
            ViewBag.AirLineId = new SelectList(db.AirLines, "AirLineId", "Name");
            ViewBag.StartAirPortsId = new SelectList(db.Airports,"Code","Name");
            ViewBag.FinishAirPortsId = new SelectList(db.Airports,"Code","Name");
            return View();
        }

        // POST: AirRoutes/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AirLineId,StartAirportCode,FinishAirportCode")] AirRouteFormModel request)
        {
            if (ModelState.IsValid)
            {
                if(request.StartAirportCode.Equals(request.FinishAirportCode))
                {
                    return View(request);
                }


                if(!db.AirLines.Where(p=>p.AirLineId == request.AirLineId).Any() || !db.Airports.Where(p=>p.Code.Equals(request.StartAirportCode)).Any() || !db.Airports.Where(p => p.Code.Equals(request.FinishAirportCode)).Any())
                {
                    return View(request);
                }

                AirRoute airRoute = new AirRoute()
                {
                    PublicId = Guid.NewGuid(),
                    AirLineId = request.AirLineId,
                    AirLine = db.AirLines.Find(request.AirLineId),
                    StartAirportCode = request.StartAirportCode,
                    FinishAirportCode = request.FinishAirportCode
                };

                db.AirRoutes.Add(airRoute);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AirLineId = new SelectList(db.AirLines, "AirLineId", "Name", request.AirLineId);
            ViewBag.StartAirPortsId = new SelectList(db.Airports, "Code", "Name",request.StartAirportCode);
            ViewBag.FinishAirPortsId = new SelectList(db.Airports, "Code", "Name",request.FinishAirportCode);
            return View(request);
        }
        #endregion

        #region Edit()
        // GET: AirRoutes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == Guid.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AirRoute airRoute = db.AirRoutes.FirstOrDefault(p => p.PublicId.Equals(id));
            if (airRoute == null)
            {
                return HttpNotFound();
            }

            AirRouteFormModel model = new AirRouteFormModel()
            {
                AirLineId = airRoute.AirLineId,
                StartAirportCode = airRoute.StartAirportCode,
                FinishAirportCode = airRoute.FinishAirportCode
            };

            ViewBag.AirLineId = new SelectList(db.AirLines, "AirLineId", "Name", model.AirLineId);
            ViewBag.StartAirPortsId = new SelectList(db.Airports, "Code", "Name",model.StartAirportCode);
            ViewBag.FinishAirPortsId = new SelectList(db.Airports, "Code", "Name",model.FinishAirportCode);
            return View(model);
        }

        // POST: AirRoutes/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid? id,[Bind(Include = "AirLineId,StartAirportCode,FinishAirportCode")] AirRouteFormModel request)
        {
            try
            {
                if(id == Guid.Empty)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (ModelState.IsValid)
                {
                    if (request.StartAirportCode.Equals(request.FinishAirportCode))
                    {
                        return View(request);
                    }


                    if (!db.AirLines.Where(p => p.AirLineId == request.AirLineId).Any() || !db.Airports.Where(p => p.Code.Equals(request.StartAirportCode)).Any() || !db.Airports.Where(p => p.Code.Equals(request.FinishAirportCode)).Any())
                    {
                        return View(request);
                    }


                    AirRoute airRoute = db.AirRoutes.FirstOrDefault(p => p.PublicId.Equals(id));
                    if(airRoute==null)
                    {
                        return HttpNotFound();
                    }

                    airRoute.AirLineId = request.AirLineId;
                    airRoute.StartAirportCode = request.StartAirportCode;
                    airRoute.FinishAirportCode = request.FinishAirportCode;

                    db.Entry(airRoute).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.AirLineId = new SelectList(db.AirLines, "AirLineId", "Name", request.AirLineId);
                ViewBag.StartAirPortsId = new SelectList(db.Airports, "Code", "Name", request.StartAirportCode);
                ViewBag.FinishAirPortsId = new SelectList(db.Airports, "Code", "Name", request.FinishAirportCode);
                return View(request);
            }
            catch
            {
                return View(request);
            }
        }
        #endregion

        #region Delete()
        // GET: AirRoutes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == Guid.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AirRoute airRoute = db.AirRoutes.FirstOrDefault(p => p.PublicId.Equals(id));
            if (airRoute == null)
            {
                return HttpNotFound();
            }

            AirRouteViewModel model = new AirRouteViewModel()
            {
                AirLine = airRoute.AirLine,
                StartAirport = db.Airports.FirstOrDefault(p => p.Code.Equals(airRoute.StartAirportCode)),
                FinishAirport = db.Airports.FirstOrDefault(p => p.Code.Equals(airRoute.FinishAirportCode))
            };

            return View(model);
        }

        // POST: AirRoutes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            if(id == Guid.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AirRoute airRoute = db.AirRoutes.FirstOrDefault(p => p.PublicId.Equals(id));
            airRoute.IsActive = false;

            db.Entry(airRoute).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion

    }
}
