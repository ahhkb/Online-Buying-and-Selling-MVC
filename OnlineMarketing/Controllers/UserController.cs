using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineMarketing.Models;
using PagedList;

namespace OnlineMarketing.Controllers
{
    public class UserController : Controller
    {
        marketingDBEntities db = new marketingDBEntities();
        // GET: User
        public ActionResult Index(int? page)
        {

            int pagesize = 6, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.categories.Where(x => x.cat_status == 1).OrderByDescending(x => x.cat_id).ToList();
            IPagedList<category> status = list.ToPagedList(pageindex, pagesize);


            return View(status);
        }



        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(user uvm)
        {
            user user = db.users.Where(x => x.u_email == uvm.u_email && x.u_password == uvm.u_password).SingleOrDefault();
            if (user != null)
            {
                // adfter login admin will go to category page
                Session["u_id"] = user.u_id.ToString();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.error = "Invalid Username or Password";
            }
            return View();
        }


        // this is user sigup  get
        [HttpGet]
        public ActionResult signUp()
        {
            return View();
        }


        // this is user sigup  Post
        [HttpPost]
        public ActionResult signUp(user uvm, HttpPostedFileBase imgfile)
        {
            string path = uploadimgfile(imgfile);
            if (path.Equals("-1"))
            {
                ViewBag.error = "Image could not be uploaded....";
            }
            else
            {
                user u = new user();
                u.u_username = uvm.u_username;
                u.u_email = uvm.u_email;
                u.u_password = uvm.u_password;
                u.u_image = path;
                u.u_contact = uvm.u_contact;
                db.users.Add(u);
                db.SaveChanges();
                return RedirectToAction("Login");
            }

            return View();
        }


        // thos is the sign out page
        public ActionResult signout()
        {
            Session.RemoveAll();
            Session.Abandon();
            return RedirectToAction("Index");
        }

        // this is  create Ad page after login user will redirect to this page for create post....
        [HttpGet]
        public ActionResult CreateAd()
        {
            List<category> li = db.categories.ToList();
            ViewBag.categoryList = new SelectList(li, "cat_id", "cat_name");

            return View();
        }

        // this is for create ad the ad create and save in to the db and redirected to the main index page  and the ad store in their selected category
        [HttpPost]
        public ActionResult CreateAd(product pvm, HttpPostedFileBase imgfile)
        {
            List<category> li = db.categories.ToList();
            ViewBag.categorylist = new SelectList(li, "cat_id", "cat_name");

            string path = uploadimgfile(imgfile);
            if (path.Equals("-1"))
            {
                ViewBag.error = "Image could not be uploaded....";
            }
            else
            {
                product p = new product();
                p.pro_name = pvm.pro_name;
                p.pro_price = pvm.pro_price;
                p.pro_image = path;
                p.pro_fk_category = pvm.pro_fk_category;
                p.pro_description = pvm.pro_description;
                p.pro_fk_users = Convert.ToInt32(Session["u_id"].ToString());

                db.products.Add(p);
                db.SaveChanges();
                Response.Redirect("Index");
            }

            return View();
        }


        // this the page zide of ads page
        public ActionResult Ads(int? id, int? page)
        {
            int pagesize = 6, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.products.Where(x => x.pro_fk_category == id).OrderByDescending(x => x.pro_id).ToList();
            IPagedList<product> status = list.ToPagedList(pageindex, pagesize);


            return View(status);

        }

        // this is for ther searching the ad 
        [HttpPost]
        public ActionResult Ads(int? id, int? page, string search , int ? price )
        {
            int pagesize = 6, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.products.Where(x => x.pro_name.Contains(search)).OrderByDescending(x => x.pro_id).ToList();
            IPagedList<product> status = list.ToPagedList(pageindex, pagesize);


            return View(status);

        }


        // dispaly the user and cat and product data in same page by using view Model

        public ActionResult viewAd(int? id)
        {
            AdViewmodel ad = new AdViewmodel();
            product p = db.products.Where(x => x.pro_id == id).SingleOrDefault();
            ad.pro_id = p.pro_id;
            ad.pro_name = p.pro_name;
            ad.pro_image = p.pro_image;
            ad.pro_price = p.pro_price;

            category cat = db.categories.Where(x => x.cat_id == p.pro_fk_category).SingleOrDefault();
            ad.cat_name = cat.cat_name;

            user u = db.users.Where(x => x.u_id == p.pro_fk_users).SingleOrDefault();
            ad.u_username = u.u_username;
            ad.u_image = u.u_image;
            ad.u_contact = u.u_contact;
            ad.pro_fk_users = u.u_id; 
            return View(ad);
        }

        public ActionResult DeleteAd( int ? id)
        {
            product p = db.products.Where(x => x.pro_id == id).SingleOrDefault();
            db.products.Remove(p);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // method for file or img uplaod 
        public string uploadimgfile(HttpPostedFileBase file)
        {
            Random r = new Random();
            string path = "-1";
            int random = r.Next();
            if (file != null && file.ContentLength > 0)
            {
                string extension = Path.GetExtension(file.FileName);
                if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".png"))
                {
                    try
                    {

                        path = Path.Combine(Server.MapPath("~/Content/upload"), random + Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        path = "~/Content/upload/" + random + Path.GetFileName(file.FileName);

                        //    ViewBag.Message = "File uploaded successfully";
                    }
                    catch (Exception ex)
                    {
                        path = "-1";
                    }
                }
                else
                {
                    Response.Write("<script>alert('Only jpg ,jpeg or png formats are acceptable....'); </script>");
                }
            }

            else
            {
                Response.Write("<script>alert('Please select a file'); </script>");
                path = "-1";
            }



            return path;
        }
        //end .............................
    }
}