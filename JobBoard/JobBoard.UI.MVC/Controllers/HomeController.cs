using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using JobBoard.DATA.EF;
using JobBoard.UI.MVC.Models;
using System.Net.Mail;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace JobBoard.UI.MVC.Controllers
{
    public class HomeController : Controller
    {
        private JobBoardEntities db = new JobBoardEntities();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        //[Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        [HttpGet]
        //[Authorize]
        public ActionResult Contact(string senderId, string receiverId)
        {
            #region Check Ids
            if (senderId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            UserDetail senderUserDetail = db.UserDetails.Find(senderId);
            if (senderUserDetail == null)
            {
                return HttpNotFound();
            }

            if (receiverId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            UserDetail receiverUserDetail = db.UserDetails.Find(receiverId);
            if (receiverUserDetail == null)
            {
                return HttpNotFound();
            }
            #endregion

            var sender = db.UserDetails.Where(ud => ud.UserId == senderId).Single();
            var receiver = db.UserDetails.Where(ud => ud.UserId == receiverId).Single();

            ViewBag.Sender = sender;
            ViewBag.Receiver = receiver;

            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(ContactViewModel cvm)
        {
            if (!ModelState.IsValid)
            {
                return View(cvm);
            }

            MailMessage mm = new MailMessage(
                $"admin@ryaneutsler.com",

                cvm.ReceiverEmail,

                //$"ryan.eutsler@outlook.com {recevierEmail}",

                cvm.Subject,
               
                cvm.Message
                );


            mm.IsBodyHtml = true;

            mm.ReplyToList.Add(cvm.SenderEmail);

            SmtpClient client = new SmtpClient("mail.ryaneutsler.com");

            client.Credentials = new NetworkCredential("admin@ryaneutsler.com", "Yearoftheknife219!!");

            client.Port = 8889;

            try
            {
                client.Send(mm);
            }
            catch (Exception ex)
            {
                ViewBag.CustomerMessage = $"We're sorry your request could not be completed at this time. Please try again later.<br/>Error Message:<br/>{ex.StackTrace}";
                return View(cvm);
            }

            return View("MessageConfirmation", cvm);
        }
    }
}
