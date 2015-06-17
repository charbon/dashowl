using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DashOwl.DAL;
using DashOwl.Models;
using System.IO;

namespace DashOwl.Controllers
{
    public class IncidentController : Controller
    {
        private DashOwlContext db = new DashOwlContext();

        // GET: Incidents
        public ActionResult Index()
        {
            return View(db.Incidents.ToList());
        }

        // GET: Incidents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Incident incident = db.Incidents.Find(id);
            if (incident == null)
            {
                return HttpNotFound();
            }
            return View(incident);
        }

        // GET: Incidents/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Incidents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CreationDate,Description,imageupload,url")] Incident incident, HttpPostedFileBase imageupload, string url)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newImage = new MediaAsset();

                    if (imageupload != null)
                    {
                        string relativePath = "/MediaStorage/" + Path.GetFileName(imageupload.FileName);
                        string absolutePath = Server.MapPath(relativePath);
                        imageupload.SaveAs(absolutePath);
                        newImage.ServerURL = relativePath;
                        newImage.ExternalURL = imageupload.FileName;
                    }
                    
                    newImage.ExternalURL = url;
                    newImage.CreationDate = DateTime.Now;

                    db.MediaAssets.Add(newImage);

                    incident.CreationDate = DateTime.Now;

                    db.Incidents.Add(incident);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable ToString save changes. Try again later.");
            }
            return View(incident);
        }

        // GET: Incidents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Incident incident = db.Incidents.Find(id);
            if (incident == null)
            {
                return HttpNotFound();
            }
            return View(incident);
        }

        // POST: Incidents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CreationDate,Description,ExternalURL,ServerURL")] Incident incident)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Incidents.Attach(incident);
                    db.Entry(incident).Property(x => x.Description).IsModified = true;                   
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Please try later.");
            }
            return View(incident);
        }

        // GET: Incidents/Delete/5
        public ActionResult Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed.";
            }
            Incident incident = db.Incidents.Find(id);
            if (incident == null)
            {
                return HttpNotFound();
            }
            return View(incident);
        }

        // POST: Incidents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Incident incident = db.Incidents.Find(id);
            db.Incidents.Remove(incident);
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

        #region Helper Methods
        private bool isUrl(string url)
        {
            return url.ToLower().StartsWith("http");
        }
        #endregion
    }
}