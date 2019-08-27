using SPblog.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Net;
using System.Data;
using System.Data.Entity;
using System.IO;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SPblog.Helpers;


namespace SPblog.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index() // adding nullable int to page (paging)
        {
           

            var myPosts = db.Posts.Where(b => b.Published).OrderByDescending(b => b.Created).ToList();
            return View(myPosts);
        }

        //[HttpPost]
        //public IQueryable<BlogPost> IndexSearch(string searchStr)
        //{
        //    IQueryable<BlogPost> result = null;
        //    if (searchStr != null)
        //    {
        //        result = db.Posts.AsQueryable();
        //        result = result.Where(p => p.Title.Contains(searchStr) ||
        //                              p.Body.Contains(searchStr) ||
        //                              p.Comments.Any(c => c.Body.Contains(searchStr) ||
        //                              c.Author.FirstName.Contains(searchStr) ||
        //                              c.Author.LastName.Contains(searchStr) ||
        //                              c.Author.DisplayName.Contains(searchStr) ||
        //                              c.Author.Email.Contains(searchStr)));
        //    }
        //    else
        //    {
        //        result = db.Posts.AsQueryable();
        //    }

        //    return result.OrderByDescending(p => p.Created);
        //}


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
           
            ViewBag.Message = "Your Contact Page";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(EmailModel email)
        {

            
            var from = $"{email.FromEmail}<{WebConfigurationManager.AppSettings["emailfrom"]}>"; // string interpolation
            var emailMessage = new MailMessage(from, WebConfigurationManager.AppSettings["emailto"])
            {
                Subject = email.Subject,
                Body = email.Body,
                IsBodyHtml = true
            };

            var svc = new PersonalEmail();
            await svc.SendAsync(emailMessage);


            return RedirectToAction("ThankYou");
        }

        public ActionResult ThankYou()
        {
            
            return View();
        }

    }
}


    
