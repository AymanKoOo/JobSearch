using JobOffers.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models;
using System.Data.Entity;
using System.Net.Mail;
using System.Net;

namespace WebApplication3.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }
        /// Search
        public ActionResult Search()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Search(string searchName)
        {
            var result = db.Jobs.Where(a => a.JobTitle.Contains(searchName) || a.JobContent.Contains(searchName)
             || a.Category.CategoryName.Contains(searchName)||a.Category.CategoryDescription.Contains(searchName)).ToList();
            return View(result);
        }

        /// <summary>
        /// ///
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult GetJobByPublisher()//GET DATA OF JOB AND PUBLISER DATA
        {
            var UserID = User.Identity.GetUserId();

            var Jobs = from app in db.ApplyForJobs
                       join job in db.Jobs
                       on app.JobId equals job.Id
                       where job.user.Id == UserID
                       select app;

          /*  var grouped = from j in Jobs
                          group j by j.job.JobTitle
                          into gr
                          select new JobModel
                          {
                              JobTitle = gr.Key,
                              Items = gr
                          };
         */
            return View(Jobs);
        }

        public ActionResult Details(int JobId)
        {
            var job = db.Jobs.Find(JobId);
            if (job == null)
            {
                return HttpNotFound();
            }
            Session["JobId"] = JobId;
            return View(job);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// 
        /// //When Clicked in details apply then it opens aopply view 
        /// 

        // POST: Jobs/Delete/5
        
        public ActionResult DeleteApply(int applyID)
        {
          
            ApplyForJob ajob = db.ApplyForJobs.Find(applyID);
                db.ApplyForJobs.Remove(ajob);
                db.SaveChanges();
            var data = new { status = "ok", result = true };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [Authorize]//must login
        public ActionResult Apply()
        {
            return View();
        }

        //When It Enters aplly view
        [HttpPost]
        public ActionResult Apply(string Message)
        {
            //GetUserID returns id of logged user
            var UserId = User.Identity.GetUserId();
            var JobId = (int)Session["JobId"];
            //Check so cant apply for same job more than once
            var check = db.ApplyForJobs.Where(a => a.JobId == JobId && a.UserId == UserId).ToList();
            if (check.Count < 1)
            {
                var job = new ApplyForJob();

                job.UserId = UserId;
                job.JobId = JobId;
                job.Message = Message;
                job.ApplyDate = DateTime.Now;

                db.ApplyForJobs.Add(job);
                db.SaveChanges();
                ViewBag.Result = "Done";
            }
            else
            {
                ViewBag.Result = "You Have applied before";
            }

            return View();
        }


        //Returns Jobs BT THE USERS

        [Authorize]
        public ActionResult GetJobByUser()
        {

            var UserID = User.Identity.GetUserId();
            var Job = db.ApplyForJobs.Where(a => a.UserId == UserID).ToList();

            return View(Job);
        }
        //WAIT
        public ActionResult DetailsOfJon(int id)
        {
            var job = db.ApplyForJobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }
        //Edit
        public ActionResult Edit(String id)
        {
            var job = db.Roles.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }
        // POST: Roles/Edit/5
        [HttpPost]
        public ActionResult Edit(ApplyForJob job)
        {
            if (ModelState.IsValid)
            {
                db.Entry(job).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(job);
        }
        /// <summary>
        /// /////////
        /// </summary>
        /// <returns></returns>
         // GET: Roles/Delete/5
        public ActionResult Delete(String id)
        {
            var job = db.Roles.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View();
        }

        // POST: Roles/Delete/5
        [HttpPost]
        public ActionResult Delete(ApplyForJob job)
        {
            if (ModelState.IsValid)
            {
                var myjob = db.Jobs.Find(job.Id);
                db.Jobs.Remove(myjob);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(job);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
           

            return View();
        }

        [HttpPost]
        public ActionResult Contact(ContactModel contact)
        {
            var mail = new MailMessage();//sytemn.net.mail
            var loginInfo = new NetworkCredential("mohamednaser9851@gmail.com","mhnsaa1964");
            mail.From = new MailAddress(contact.Email);
            mail.To.Add(new MailAddress("mohamednaser9851@gmail.com"));
            mail.Subject = contact.Subject;
            mail.IsBodyHtml = true;
            string body = "اسم المرسل" + contact.Name + "<br>" + "بريد المرسل" + contact.Email + "<br>" +
                "نص الرسالة" + contact.Message;
            mail.Body = body;

            var smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = loginInfo;
            smtpClient.Send(mail);

            return RedirectToAction("Index");
        }
    }
}