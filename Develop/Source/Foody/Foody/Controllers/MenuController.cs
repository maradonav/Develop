using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Foody.Models;
using PagedList;

namespace Foody.Controllers
{
    public class MenuController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: /Menu/
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "menuname_desc" : "";
            ViewBag.NameSortParm = sortOrder == "ControllerName" ? "controllername_desc" : "ControllerName";
            ViewBag.NameSortParm = sortOrder == "ActionName" ? "actionname_desc" : "ActionName";
            ViewBag.NameSortParm = sortOrder == "HierarchyLevel" ? "hierarchylevel_desc" : "HierarchyLevel";
            //ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var menus = from m in db.MenuList
                           select m;
            if (!string.IsNullOrEmpty(searchString))
            {
                menus = menus.Where(s => s.MenuId.Contains(searchString)||s.MenuName.Contains(searchString)
                                       || s.ControllerName.Contains(searchString) || s.ActionName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "menuname_desc":
                    menus = menus.OrderByDescending(s => s.MenuName);
                    break;
                case "ControllerName":
                    menus = menus.OrderBy(s => s.ControllerName);
                    break;
                case "controllername_desc":
                    menus = menus.OrderByDescending(s => s.ControllerName);
                    break;
                case "ActionName":
                    menus = menus.OrderBy(s => s.ActionName);
                    break;
                case "actionname_desc":
                    menus = menus.OrderByDescending(s => s.ActionName);
                    break;
                case "HierarchyLevel":
                    menus = menus.OrderBy(s => s.HierarchyLevel);
                    break;
                case "hierarchylevel_desc":
                    menus = menus.OrderByDescending(s => s.HierarchyLevel);
                    break;
                default:
                    menus = menus.OrderBy(s => s.MenuName);
                    break;
            }

            int pageSize = 4;
            int pageNumber = (page ?? 1);
            return View(menus.ToPagedList(pageNumber, pageSize));
        }

        // GET: /Menu/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.MenuList.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        // GET: /Menu/Create
        public ActionResult Create()
        {
            Menu model = new Menu();
            return View(model);
        }

        // POST: /Menu/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Menu menu)
        {
            if (ModelState.IsValid)
            {
                //menu.MenuId = (db.MenuList.Count() + 1).ToString(); 
                if (menu.ActionName == null)
                {
                    menu.ActionName = string.Empty;
                }

                if (menu.ControllerName == null)
                {
                    menu.ControllerName = string.Empty;
                }

                if(menu.Parameters==null)
                {
                    menu.Parameters = string.Empty;
                }

                if (menu.MenuType == null)
                {
                    menu.MenuType = string.Empty;
                }

                if (menu.ParentMenuId == null)
                {
                    menu.ParentMenuId = string.Empty;
                }
                menu.CreatedBy = User.Identity.Name;
                menu.LastUpdatedBy = User.Identity.Name;
                db.MenuList.Add(menu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(menu);
        }

        // GET: /Menu/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.MenuList.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        // POST: /Menu/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Menu menu)
        {
            if (ModelState.IsValid)
            {
                if (menu.ActionName == null)
                {
                    menu.ActionName = string.Empty;
                }

                if (menu.ControllerName == null)
                {
                    menu.ControllerName = string.Empty;
                }

                if (menu.Parameters == null)
                {
                    menu.Parameters = string.Empty;
                }

                if (menu.MenuType == null)
                {
                    menu.MenuType = string.Empty;
                }

                if (menu.ParentMenuId == null)
                {
                    menu.ParentMenuId = string.Empty;
                }
                menu.SetUpdatedInfo(User.Identity.Name);
                db.Entry(menu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(menu);
        }

        // GET: /Menu/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.MenuList.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        // POST: /Menu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Menu menu = db.MenuList.Find(id);
            db.MenuList.Remove(menu);
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
