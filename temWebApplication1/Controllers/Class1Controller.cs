using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using temWebApplication1.Models;

namespace temWebApplication1.Controllers
{
    public class Class1Controller : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Class1
        public ActionResult Index()
        {
            return View(db.Class1.ToList());
        }

        // GET: Class1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Class1 class1 = db.Class1.Find(id);
            if (class1 == null)
            {
                return HttpNotFound();
            }
            return View(class1);
        }

        // GET: Class1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Class1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Order,Login")] Class1 class1)
        {
            if (ModelState.IsValid)
            {
                db.Class1.Add(class1);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(class1);
        }

        // GET: Class1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Class1 class1 = db.Class1.Find(id);
            if (class1 == null)
            {
                return HttpNotFound();
            }
            return View(class1);
        }

        // POST: Class1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Order,Login")] Class1 class1)
        {
            if (ModelState.IsValid)
            {
                db.Entry(class1).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(class1);
        }

        // GET: Class1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Class1 class1 = db.Class1.Find(id);
            if (class1 == null)
            {
                return HttpNotFound();
            }
            return View(class1);
        }

        // POST: Class1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Class1 class1 = db.Class1.Find(id);
            db.Class1.Remove(class1);
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
