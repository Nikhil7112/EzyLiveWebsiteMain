using EzyLiveWebsiteMain.Models.owner;
using EzyLiveWebsiteMain.Models.Review;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EzyLiveWebsiteMain.Models.Contact;
using System.Text.RegularExpressions;
using EzyLiveWebsiteMain.Models.Login;
using System.Data.Entity;
using System.Web.Helpers;

namespace EzyLiveWebsiteMain.Controllers
{
    public class HomeController : Controller
    {
        EzyLiveMainEntities db = new EzyLiveMainEntities();
        EzyLiveMainEntities1 db1 = new EzyLiveMainEntities1();
        EzyLiveMainEntities2 db2 = new EzyLiveMainEntities2();
        EzyLiveMainEntities3 db3 = new EzyLiveMainEntities3();

        string EmailPattern = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";
        // GET: Home
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Owner s)
        {
            string fileName = Path.GetFileNameWithoutExtension(s.ImageFile.FileName);
            string extension = Path.GetExtension(s.ImageFile.FileName);
            fileName += extension;
            s.image = "~/images/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/images/"), fileName);
            s.ImageFile.SaveAs(fileName);
            db.Owners.Add(s);
            int a = db.SaveChanges();
            if (a > 0)
            {
                ViewBag.Message = "<script> alert('Record Inserted') </script>";
                ModelState.Clear();
            }
            else
            {
                ViewBag.Message = "<script> alert('Record did not Inserted') </script>";
            }
            return View();
        }
        public ActionResult Index()
        {
            var data = db.Owners.ToList();

            return View(data);

        }
     
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Contact(Contact c)
        {
            
         
                if (ModelState.IsValid == true)
                {
                    db1.Contacts.Add(c);
                    int a = db1.SaveChanges();


                    if (a > 0)
                    {
                        TempData["Submitted"] = "<script> alert('Thank you for Contacting us') </script>";
                        return RedirectToAction("Create", "Home");
                    }
                    else
                    {
                        ViewBag.Submitted = "<script> alert('Failed') </script>";
                    }
               }
            
            
            return View();
        }
        public ActionResult Review()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Review(Review r)
        {

            if (ModelState.IsValid == true)
            {
                db2.Reviews.Add(r);
                int a = db2.SaveChanges();


                if (a > 0)
                {
                    TempData["Sent"] = "<script> alert('Thank you for Contacting us') </script>";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.sent = "<script> alert('Failed') </script>";
                }
                
            }
            return View();
        }
        public ActionResult ViewReview()
        {  
                var data = db2.Reviews.ToList();
            return View(data);
        }
        
        public ActionResult Deletereview(int Id)
        {
            if (Id > 0)
            {
                var OwnerEmail = db2.Reviews.FirstOrDefault(model => model.Id == Id);
                if (OwnerEmail != null)
                {
                    db2.Entry(OwnerEmail).State = EntityState.Deleted;
                    int rowsAffected = db2.SaveChanges();
                    if (rowsAffected > 0)
                    {
                        TempData["Delete"] = "Data Deleted";
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["Delete"] = "Data not Deleted";
                        return RedirectToAction("Index", "Home");
                    }
                }
               

            }

            return View();
        }
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(Login l)
        {
            if(ModelState.IsValid == true)
            {
                db3.Logins.Add(l);
                int a= db3.SaveChanges();
                if(a > 0)
                {
                    ViewBag.Registered = "Registered Successfully";
                    ModelState.Clear();
                }
                else
                {
                    ViewBag.Registered = "Failed";
                }
              
            }
            return View();
        }
        public ActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignIn(Login n)
        {
            var user = db3.Logins.Where(model => model.Username == n.Username && model.Password == n.Password).FirstOrDefault();
            if (user != null)
            {
               
               
                TempData["LoginDone"] = "<script> alert('Login Successful') </script>";
                return RedirectToAction("Create", "Home");
            }
            else
            {
                ViewBag.Error = "<script> alert('Username Or Password is Incorrect !!') </script>";
                return View();
            }
            
        }
    
        public ActionResult Logout()
        {
        return RedirectToAction("SignIn", "Home");   
        }
        
    }
}