using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SPblog.Models;

namespace SPblog.Controllers
{
    [Authorize(Roles = "Admin")]
    [RequireHttps]
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        //private UserManager<ApplicationUser> userManager;
        //public void CommentsController()
        //{
        //    userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        //}

        // GET: Comments
        [AllowAnonymous]
        public ActionResult Index()
        {
            var comments = db.Comments.Include(c => c.Author).Include(c => c.BlogPost);
            return View(comments.ToList());
        }

        // GET: Comments/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comments/Create
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Create()
        {
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.BlogPostId = new SelectList(db.Posts, "Id", "Title");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Create([Bind(Include = "BlogPostId, CommentBody")] Comment comment, string commentBody)
        {
            if (ModelState.IsValid)
            {
                comment.Body = commentBody;
                comment.Created = DateTimeOffset.Now;
                //comment.AuthorId = userManager.FindByName(User.Identity.Name).Id;
                comment.AuthorId = User.Identity.GetUserId();
                db.Comments.Add(comment);
                db.SaveChanges();
               
            }
            var slug = db.Posts.Find(comment.BlogPostId).Slug;
            return RedirectToAction("Details", "BlogPosts",  new { slug = slug });
        }

        // GET: Comments/Edit/5
        [AllowAnonymous]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName", comment.AuthorId);
            ViewBag.BlogPostId = new SelectList(db.Posts, "Id", "Title", comment.BlogPostId);

            //var postId = db.Comments.Find(id).BlogPostId;
            //ViewBag.Slug = db.Posts.Find(postId).Slug;
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Edit([Bind(Include = "Id,BlogPostId,AuthorId,Created,UpdateReason, Body")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                if ((User.IsInRole("Admin") || User.IsInRole("Moderator")) && comment.UpdateReason == null)
                {
                    ModelState.AddModelError("UpdateReason", "An Update reason is required!");
                    return View(comment);
                }
                comment.Updated = DateTimeOffset.Now;
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();

                var slug = db.Posts.Find(comment.BlogPostId).Slug;
                return RedirectToAction("Details", "BlogPosts", new { slug = slug });
            }
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName", comment.AuthorId);
            ViewBag.BlogPostId = new SelectList(db.Posts, "Id", "Title", comment.BlogPostId);
            return View(comment);
        }

        // GET: Comments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
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
