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
    public class AdminController : Controller
    {
        marketingDBEntities db = new marketingDBEntities();
        // GET: Admin
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(admin avm)
        {
            admin ad = db.admins.Where(x => x.ad_username == avm.ad_username && x.ad_password == avm.ad_password).SingleOrDefault();
            if (ad != null)
            {
                // adfter login admin will go to category page
                Session["ad_id"] = ad.ad_id.ToString();
                return RedirectToAction("Category");
            }
            else
            {
                ViewBag.error = "Invalid Username or Password";
            }
            return View();
        }


        // this is categor page 
        [HttpGet]
        public ActionResult Category()
        {
            if (Session["ad_id"]==null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        // this is for adding the category data into db.............................
        [HttpPost]
        public ActionResult Category(category cvm , HttpPostedFileBase imgfile)
        {
            string path = uploadimgfile(imgfile);
            if (path.Equals("-1"))
            {
                ViewBag.error = "Image could not be uploaded....";
            }
            else
            {
                category cat = new category();
                cat.cat_name = cvm.cat_name;
                cat.cat_image = path;
                cat.cat_status = 1;
                cat.cat_fk_ad = Convert.ToInt32(Session["ad_id"].ToString());
                db.categories.Add(cat);
                db.SaveChanges();
                return RedirectToAction("viewCategory");
            }

            return View();
        } // endd.....................


        // this is for the view of data that we save in our db .....................
        public ActionResult viewCategory( int ? page)
        {
            int pagesize = 6, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.categories.Where(x => x.cat_status == 1).OrderByDescending(x=>x.cat_id).ToList();
            IPagedList<category> status = list.ToPagedList(pageindex , pagesize);
            

            return View(status);
        }
        // end...............



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