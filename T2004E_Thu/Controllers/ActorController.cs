using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using T2004E_Thu.Context;
using T2004E_Thu.Models;
using System.Dynamic;
using System.IO;

namespace T2004E_Thu.Controllers
{
    public class ActorController : Controller
    {
        private DataContext db = new DataContext();

        // GET: Actor
        public ActionResult Index(string search,string sortOrder)
        {
            string sort = !String.IsNullOrEmpty(sortOrder) ? sortOrder : "asc";
            var actors = from p in db.Actors select p;
            if (!String.IsNullOrEmpty(search))
            {
                actors = actors.Where(p => p.NameA.Contains(search));
            }
            switch (sort)
            {
                case "asc": actors = actors.OrderBy(p => p.NameA); break;
                case "desc": actors = actors.OrderByDescending(p => p.NameA);break;
            }
            return View(actors);
        }

        // GET: Actor/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Actor actor = db.Actors.Find(id);
            if (actor == null)
            {
                return HttpNotFound();
            }
            return View(actor);
        }

        // GET: Actor/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Actor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,NameA,Year,description")] Actor actor,HttpPostedFileBase Image)
        {
            String actorImage = "default.png";
            if (Image != null)
            {
                string fileName = Path.GetFileName(Image.FileName);
                string path = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                Image.SaveAs(path);
                actorImage = "Uploads/" + fileName;
            }
            actor.Image = actorImage;
            if (ModelState.IsValid)
            {
                db.Actors.Add(actor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(actor);
        }

        // GET: Actor/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Actor actor = db.Actors.Find(id);
            if (actor == null)
            {
                return HttpNotFound();
            }
            return View(actor);
        }

        // POST: Actor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,NameA,Image,Year,description")] Actor actor,HttpPostedFileBase Image)
        {
           
            if (ModelState.IsValid)
            {
                db.Entry(actor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(actor);
        }

        // GET: Actor/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Actor actor = db.Actors.Find(id);
            if (actor == null)
            {
                return HttpNotFound();
            }
            return View(actor);
        }

        // POST: Actor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Actor actor = db.Actors.Find(id);
            db.Actors.Remove(actor);
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
    }
}
