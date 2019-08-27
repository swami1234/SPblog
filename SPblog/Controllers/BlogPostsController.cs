using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SPblog.Helpers;
using SPblog.Models;


namespace SPblog.Controllers
{

    [Authorize(Roles = "Admin")]
    [RequireHttps]
    public class BlogPostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BlogPosts
        [AllowAnonymous]
        public ActionResult Index()
        {
           // return View(db.Posts.Where(b =>b.Published).ToList());
            return View(db.Posts.ToList());//this was before above code lien was added.
            
        }

        // GET: BlogPosts/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    BlogPost blogPost = db.Posts.Find(id);
        //    if (blogPost == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(blogPost);
        //}
        [AllowAnonymous]
        public ActionResult Details(string Slug)
        {
            if (String.IsNullOrWhiteSpace(Slug))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
             BlogPost blogPost = db.Posts.FirstOrDefault(p => p.Slug == Slug);
            
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            
            return View(blogPost);
        }

        //  var publishedposts = db.Posts.Where(b => b.Published).OrderByDescending(b => b.Created).ToList();
           // return View(publishedposts);

        // GET: BlogPosts/Create
        public ActionResult Create()
        {
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: BlogPosts1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Created,Body,MediaURL,Published")] BlogPost blogPost, HttpPostedFileBase image)

        {
            if (ModelState.IsValid)
            {
                if (ImageUploadValidator.IsWebFriendlyImage(image))
                {
                    var fileName = Path.GetFileName(image.FileName);
                    image.SaveAs(Path.Combine(Server.MapPath("~/Uploads/"), fileName));
                    blogPost.MediaURL = "/Uploads/" + fileName;
                }

                var Slug = StringUtilities.URLFriendly(blogPost.Title);
               
                


                if (String.IsNullOrWhiteSpace(Slug))
                {
                    ModelState.AddModelError("Title", "Invalid title");
                    return View(blogPost);
                }
                if (db.Posts.Any(p => p.Slug == Slug))
                {
                    ModelState.AddModelError("Title", "The title must be unique");
                    return View(blogPost);
                }
                   

                blogPost.Slug = Slug;
            

                blogPost.Created = DateTimeOffset.Now;
                db.Posts.Add(blogPost);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(blogPost);
        }




        // GET: BlogPosts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.Posts.Find(id);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        // POST: BlogPosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Slug,Created,Body,MediaURL,Published")] BlogPost blogPost, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (ImageUploadValidator.IsWebFriendlyImage(image))
                {
                    var fileName = Path.GetFileName(image.FileName);
                    image.SaveAs(Path.Combine(Server.MapPath("~/Uploads/"), fileName));
                    blogPost.MediaURL = "/Uploads/" + fileName;
                }
                var slug = StringUtilities.URLFriendly(blogPost.Title);

                if (blogPost.Slug != slug)
                {
                    if (string.IsNullOrWhiteSpace(slug))
                    {
                        ModelState.AddModelError("Title", "Invalid Title");
                        return View(blogPost);
                    }
                    if (db.Posts.Any(p => p.Slug == slug))
                    {
                        ModelState.AddModelError("Title", "The Title must be unique");
                        return View(blogPost);
                    }
                    blogPost.Slug = slug;
                }
                blogPost.Updated = DateTimeOffset.Now;
                db.Entry(blogPost).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            
           
               
            
            return View(blogPost);
        }

        // GET: BlogPosts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.Posts.Find(id);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        // POST: BlogPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BlogPost blogPost = db.Posts.Find(id);
            db.Posts.Remove(blogPost);
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
