using System;
using System.Linq;
using System.Web.Mvc;
using VirtualForEveryOne.Models;

namespace VirtualForEveryOne.Controllers
{
    public class AdminController : Controller
    {
        VirtualContext db1 = new VirtualContext();


        static string logged_admin;

        // GET: /Admin/

        public ActionResult Index()
        {

            return View();
        }


        public ActionResult UserAdd()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(db1.Users.ToList());
            }
        }

        public ActionResult GroupAdd()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(db1.Groups.ToList());
            }
        }


        public ActionResult Report()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(db1.Reports.ToList());
            }
        }


        public ActionResult Authenticate_user()
        {
            string username = Request["username"];
            string password = Request["password"];

            foreach (var s in db1.Admins)
            {
                if (username.Equals(s.name) && password.Equals(s.password))
                {
                    Session["admin"] = username;
                    logged_admin = (Session["admin"].ToString());
                    return RedirectToAction("UserAdd");
                }
            }
            TempData["alert message"] = "Invalid Id or password";
            return RedirectToAction("Index");

        }



        public ActionResult Delete(int id, string username)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                User u = db1.Users.SingleOrDefault(m => m.Id == id);
                if (u == null)
                {
                    return HttpNotFound();
                }
                return View(u);
            }
        }

        //
        // User: Delete

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id, string username)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                User u = db1.Users.SingleOrDefault(m => m.Id == id);
                string name = u.username.Remove(0, 1);
                db1.Users.Remove(u);

                db1.Answers.Where(p => p.username == name).ToList().ForEach(p => db1.Answers.Remove(p));
                db1.Friendses.Where(p => p.username == name || p.userfrirends == name).ToList().ForEach(p => db1.Friendses.Remove(p));
                //db1.notification.Where(p => p.username == name || p.notifier == name).ToList().ForEach(p => db1.notification.Remove(p));
                db1.Posts.Where(p => p.username == name).ToList().ForEach(p => db1.Posts.Remove(p));
                db1.Reports.Where(p => p.username == name).ToList().ForEach(p => db1.Reports.Remove(p));
                db1.SharedPosts.Where(p => p.username == name).ToList().ForEach(p => db1.SharedPosts.Remove(p));
                db1.UserLikes.Where(p => p.username == name).ToList().ForEach(p => db1.UserLikes.Remove(p));

                db1.SaveChanges();
                TempData["alert message"] = "User has been deleted from All tables";
                return RedirectToAction("UserAdd");
            }
        }


        //group delete
        public ActionResult DeleteGroup(int id = 0)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                Group g = db1.Groups.Find(id);
                if (g == null)
                {
                    return HttpNotFound();
                }
                return View(g);
            }
        }


        [HttpPost, ActionName("DeleteGroup")]
        public ActionResult DeleteGroupConfirmed(int id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                Group g = db1.Groups.Find(id);
                db1.Groups.Remove(g);


                db1.SaveChanges();
                TempData["alert message"] = "Group has been deleted";
                return RedirectToAction("GroupAdd");
            }
        }

        public ActionResult Logout()
        {
            Session["admin"] = null;
            return RedirectToAction("Index", "Admin");
        }

        public ActionResult ViewReport(int id = 0)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                Report u = db1.Reports.Find(id);
                if (u == null)
                {
                    return HttpNotFound();
                }
                return View(u);
            }
        }

        public ActionResult DeletePost(int id = 0, int reportid = 0)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                Answer u = db1.Answers.Find(id);
                if (u == null)
                {
                    TempData["alert message"] = "PostId is Invalid";
                    Report r = db1.Reports.Find(reportid);
                    r.status = "Complete";
                    db1.SaveChanges();
                    return RedirectToAction("viewReport", new { id = reportid });
                    //return HttpNotFound();
                }
                return View(u);
            }
        }

        //
        // POST: Delete



        [HttpPost, ActionName("DeletePost")]
        public ActionResult DeleteConfirmed(int id, int reportid)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index");
            }
            else
            {

                Answer u = db1.Answers.Find(id);
                db1.Answers.Remove(u);

                Report r = db1.Reports.Find(reportid);
                r.status = "Deleted";

                int i = int.Parse(r.postid);
                Answer a = db1.Answers.Where(o => o.Id == i).First(); // notify person against complaint
                string user = a.username;
                Notification n = new Notification();
                n.postid = "r" + r.postid.ToString();
                n.username = user;
                n.notifier = "Admin";
                n.state = false;
                n.time = DateTime.Now;
                n.type = "deleted your post";
                db1.Notifications.Add(n);


                Notification n1 = new Notification(); // notify who complaint
                n1.postid = "r" + r.postid.ToString();
                n1.username = r.username;
                n1.notifier = "Admin";
                n1.state = false;
                n1.time = DateTime.Now;
                n1.type = "took action and deleted the post";

                db1.Notifications.Add(n1);

                db1.SaveChanges();


                db1.SaveChanges();
                TempData["alert message"] = "Notification of Post Deletion  have been sent to both";
                return RedirectToAction("viewReport", new { id = reportid });
            }

        }




        public ActionResult Warn(int reportid)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                Report r = db1.Reports.Find(reportid);
                r.status = "warned";


                int i = int.Parse(r.postid);
                Answer a = db1.Answers.Where(o => o.Id == i).First(); // notify person against complaint
                string user = a.username;
                Notification n = new Notification();
                n.postid = "r" + r.postid.ToString();
                n.username = user;
                n.notifier = "Admin";
                n.state = false;
                n.time = DateTime.Now;
                n.type = "warned you on your post";
                db1.Notifications.Add(n);


                Notification n1 = new Notification(); // notify who complaint
                n1.postid = "r" + r.postid.ToString();
                n1.username = r.username;
                n1.notifier = "Admin";
                n1.state = false;
                n1.time = DateTime.Now;
                n1.type = "gave warning againist your complaint";

                db1.Notifications.Add(n1);

                db1.SaveChanges();

                TempData["alert message"] = "Notification of warning have been sent to both";
                return RedirectToAction("viewReport", new { id = reportid });
            }
        }

    }
}
