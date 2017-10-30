using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RegistrationAndLogin.Models;
using System.Net.Mail;
using System.Net;

namespace RegistrationAndLogin.Controllers
{
    public class UserController : Controller
    {
        //Registration GET
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        //Registration POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Exclude= "IsEmailVerified, ActivationCode")] User user)
        {
            bool Status = false;
            string message = "";

            //Model validation
            if (ModelState.IsValid)
            {
                #region //check if email exists 
                var exists = checkEmailExists(user.EmailId);
                if (exists)
                {
                    ModelState.AddModelError("EmailExists", "Email alreayd exists");
                    return View(user);
                }
                #endregion

                #region //generate activation code
                user.ActivationCode = Guid.NewGuid();
                #endregion

                #region //password hashing
                user.Password = Crypto.Hash(user.Password);
                user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword);
                #endregion
                user.IsEmailVerified = false;

                #region //save new user to database
                using(RegAndLoginDBEntities db = new RegAndLoginDBEntities())
                {
                    db.Users.Add(user);
                    db.SaveChanges();

                    //send email to user
                    SendVerificationLinkEmail(user.EmailId, user.ActivationCode.ToString());
                    message = "Registration Email sent. Please log into your email to verrify your account creation.";
                    Status = true;
                }
                #endregion 


            }
            else
            {
                message = "Invalid Request";

            }

            ViewBag.Message = message;
            ViewBag.Status = Status;

            return View(user);
        }

        //Verify Email

        //Verify Email LINK

        //Login GET

        //Login POST


        //Logout
        [NonAction]
        public bool checkEmailExists(string emailId)
        {
            using (RegAndLoginDBEntities db = new RegAndLoginDBEntities())
            {
                var v = db.Users.Where(a => a.EmailId == emailId).FirstOrDefault();
                return v != null;
            }
        }

        [NonAction]
        public void SendVerificationLinkEmail(string emailId, string activationCode)
        {
            var verifyUrl = "/User/VerifyAccount/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("cscherbert89@gmail.com", "Collin Scherbert");
            var toEmail = new MailAddress(emailId);

            var fromEmailPassword = "Whitn@ll08"; //replace with actual password
            string subject = "Your Account is successfully created.";

            string body = "<br></br>We are excited to tell you that your account has been created." +
                " Please click on the link below to verify your account."
                + "<br></br> <a href='" + link +"'>" + link + "</a>";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false, 
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }
    }


}