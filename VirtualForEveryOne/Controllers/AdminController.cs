using System;
using System.Linq;
using System.Web.Mvc;

namespace VirtualForEveryOne.Controllers
{
    public class AdminController : Controller
    {
        virtuallyeveryonedbEntities1 db1 = new virtuallyeveryonedbEntities1();


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
                return View(db1.user.ToList());
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
                return View(db1.group.ToList());
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
                return View(db1.report.ToList());
            }
        }


        public ActionResult Authenticate_user()
        {
            string username = Request["username"];
            string password = Request["password"];

            foreach (var s in db1.admin)
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



        public ActionResult Delete(int id = 0, string username = null)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                user u = db1.user.Find(id, username);
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
                user u = db1.user.Find(id, username);
                string name = u.username.Remove(0, 1);
                db1.user.Remove(u);

                db1.answer.Where(p => p.username == name).ToList().ForEach(p => db1.answer.Remove(p));
                db1.friends.Where(p => p.username == name || p.userfriends == name).ToList().ForEach(p => db1.friends.Remove(p));
                //db1.notification.Where(p => p.username == name || p.notifier == name).ToList().ForEach(p => db1.notification.Remove(p));
                db1.post.Where(p => p.username == name).ToList().ForEach(p => db1.post.Remove(p));
                db1.report.Where(p => p.username == name).ToList().ForEach(p => db1.report.Remove(p));
                db1.sharedPost.Where(p => p.username == name).ToList().ForEach(p => db1.sharedPost.Remove(p));
                db1.userLike.Where(p => p.username == name).ToList().ForEach(p => db1.userLike.Remove(p));

                db1.SaveChanges();
                TempData["alert message"] = "User has been deleted from All tables";
                return RedirectToAction("UserAdd");
            }
        }


        //group delete
        public ActionResult DeleteGroup(int id = 0, int groupid = 0)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                group g = db1.group.Find(id, groupid);
                if (g == null)
                {
                    return HttpNotFound();
                }
                return View(g);
            }
        }


        [HttpPost, ActionName("DeleteGroup")]
        public ActionResult DeleteGroupConfirmed(int id, int groupid)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                group g = db1.group.Find(id, groupid);
                db1.group.Remove(g);


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
                report u = db1.report.Find(id);
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
                answer u = db1.answer.Find(id);
                if (u == null)
                {
                    TempData["alert message"] = "PostId is Invalid";
                    report r = db1.report.Find(reportid);
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

                answer u = db1.answer.Find(id);
                db1.answer.Remove(u);

                report r = db1.report.Find(reportid);
                r.status = "Deleted";

                int i = int.Parse(r.postid);
                answer a = db1.answer.Where(o => o.Id == i).First(); // notify person against complaint
                string user = a.username;
                notification n = new notification();
                n.postid = "r" + r.postid.ToString();
                n.username = user;
                n.notifier = "Admin";
                n.state = false;
                n.time = DateTime.Now;
                n.type = "deleted your post";
                db1.notification.Add(n);


                notification n1 = new notification(); // notify who complaint
                n1.postid = "r" + r.postid.ToString();
                n1.username = r.username;
                n1.notifier = "Admin";
                n1.state = false;
                n1.time = DateTime.Now;
                n1.type = "took action and deleted the post";

                db1.notification.Add(n1);

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
                report r = db1.report.Find(reportid);
                r.status = "warned";


                int i = int.Parse(r.postid);
                answer a = db1.answer.Where(o => o.Id == i).First(); // notify person against complaint
                string user = a.username;
                notification n = new notification();
                n.postid = "r" + r.postid.ToString();
                n.username = user;
                n.notifier = "Admin";
                n.state = false;
                n.time = DateTime.Now;
                n.type = "warned you on your post";
                db1.notification.Add(n);


                notification n1 = new notification(); // notify who complaint
                n1.postid = "r" + r.postid.ToString();
                n1.username = r.username;
                n1.notifier = "Admin";
                n1.state = false;
                n1.time = DateTime.Now;
                n1.type = "gave warning againist your complaint";

                db1.notification.Add(n1);

                db1.SaveChanges();

                TempData["alert message"] = "Notification of warning have been sent to both";
                return RedirectToAction("viewReport", new { id = reportid });
            }
        }

    }
}
