﻿using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VirtualForEveryOne.Controllers
{
    public class HomeController : Controller
    {    //
        // GET: /Home/

        virtuallyeveryonedbEntities1 db = new virtuallyeveryonedbEntities1();

        static string logged_user;
        static string search_user;

        public ActionResult MainPage()
        {
            return View();
        }

        public ActionResult Feed()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("MainPage");
            }
            else
            {
                string get_user = Session["username"].ToString();
                ViewBag.username = get_user;

                TempData["friends"] = (from a in db.friends.Where(x => x.username == logged_user) select a).ToList();

                TempData["user"] = (from u in db.user select u).ToList();

                TempData["sharedPost"] = db.sharedPost.OrderByDescending(u => u.Id).ToList();

                TempData["answer"] = (from n in db.answer where n.status == null || n.status == "1" orderby n.Id descending select n).ToList();

                TempData["faq"] = (from f in db.faq select f).ToList();

                ViewBag.count = db.notification.Where(u => u.username == logged_user && u.state == false && u.notifier != logged_user).Count();

                return View();
            }
        }

        public ActionResult Settings()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("MainPage");
            }
            else
            {
                ViewBag.count = db.notification.Where(u => u.username == logged_user && u.state == false && u.notifier != logged_user).Count();
                TempData["faq"] = (from f in db.faq select f).ToList();

                return View(db.user.First(x => x.username.Equals("@" + logged_user)));

            }
        }

        public ActionResult Questions()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("MainPage");
            }
            else
            {
                string get_user = Session["username"].ToString();

                ViewBag.username = get_user;

                TempData["friends"] = (from a in db.friends.Where(x => x.username == logged_user) select a).ToList();
                TempData["user"] = (from u in db.user select u).ToList();

                TempData["post"] = (from n in db.post where n.tags == null || n.tags == logged_user orderby n.Id descending select n).ToList();
                //db.post.OrderByDescending(n => n.Id).ToList();
                TempData["faq"] = (from f in db.faq select f).ToList();

                ViewBag.count = db.notification.Where(u => u.username == logged_user && u.state == false && u.notifier != logged_user).Count();

                return View();
            }
        }

        public ActionResult UserProfile()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("MainPage");
            }
            else
            {
                var logged_user1 = "@" + (Session["username"].ToString()).Remove(0, 1);

                var getCount = (from a in db.answer where a.username == logged_user select a.like.Value).DefaultIfEmpty(0).Sum();

                ViewBag.totalLike = getCount;
                ViewBag.totalPost = db.post.Count(x => x.username == logged_user);
                ViewBag.count = db.notification.Where(u => u.username == logged_user && u.state == false && u.notifier != logged_user).Count();
                ViewBag.follower = db.friends.Count(f => f.username == logged_user);

                TempData["answer"] = (from ans in db.answer where ans.a_username == logged_user || ans.username == logged_user orderby ans.Id descending select ans).ToList();
                TempData["faq"] = (from f in db.faq select f).ToList();

                return View(db.user.First(x => x.username == logged_user1));
            }
        }

        public ActionResult Frndsprofile()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("MainPage");
            }
            else
            {

                if (search_user.Remove(0, 1) == logged_user)
                {
                    return RedirectToAction("UserProfile", "Home");
                }
                else
                {
                    friends check;
                    var search_user1 = search_user.Remove(0, 1);

                    check = (from a in db.friends where a.username == logged_user && a.userfriends == search_user.Remove(0, 1) select a).FirstOrDefault();

                    if (check == null)
                    {
                        ViewBag.check = 0;
                    }
                    else
                    {
                        ViewBag.check = 1;
                    }

                    var getCount = (from a in db.answer where a.username == search_user1 select a.like.Value).DefaultIfEmpty(0).Sum();

                    ViewBag.totalLike = getCount;
                    ViewBag.totalPost = db.post.Count(x => x.username == search_user1);
                    ViewBag.count = db.notification.Where(u => u.username == logged_user && u.state == false && u.notifier != logged_user).Count();
                    ViewBag.follower = db.friends.Count(f => f.username == logged_user);

                    TempData["answer"] = (from ans in db.answer where ans.a_username == search_user1 || ans.username == search_user1 orderby ans.Id descending select ans).ToList();
                    TempData["faq"] = (from f in db.faq select f).ToList();

                    return View(db.user.First(x => x.username.Equals(search_user)));
                }
            }
        }

        public ActionResult Friends()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("MainPage");
            }
            else
            {
                TempData["user"] = (from a in db.user select a).ToList();
                TempData["faq"] = (from f in db.faq select f).ToList();

                ViewBag.username = logged_user;
                ViewBag.count = db.notification.Where(u => u.username == logged_user && u.state == false && u.notifier != logged_user).Count();

                return View(db.friends.Where(x => x.username == logged_user).ToList());
            }
        }

        public ActionResult Notifications()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("MainPage");
            }
            else
            {
                TempData["post"] = db.post.ToList();
                TempData["answer"] = db.answer.ToList();
                TempData["faq"] = (from f in db.faq select f).ToList();

                ViewBag.count = db.notification.Where(u => u.username == logged_user && u.state == false && u.notifier != logged_user).Count();

                var query = (from n in db.notification where n.username == logged_user && n.notifier != logged_user orderby n.time descending select n).ToList();

                return View(query);
            }
        }

        public JsonResult Check_username()
        {
            string con_user = Request["username"];
            string username = "@" + con_user;

            foreach (var s in db.user)
            {
                if (username.ToLower().Equals(s.username))
                {
                    logged_user = username;
                    return this.Json(true, JsonRequestBehavior.AllowGet);
                }
            }

            return this.Json(false, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Save_signup_info()
        {
            string fullname = Request["fullname"];
            string username = Request["username"];
            string email = Request["Email"];
            string skills = Request["skills"];
            string password = Request["password"];

            user u = new user();

            u.fullname = fullname;
            u.username = "@" + username.ToLower();
            u.email = email;
            u.skills = skills;
            u.password = password;

            db.user.Add(u);
            db.SaveChanges();

            Session["username"] = username;

            return RedirectToAction("feed");

        }

        public JsonResult Authenticate_user()
        {
            string username = "@" + Request["log_username"];
            string password = Request["log_password"];

            foreach (var s in db.user)
            {
                if (username.Equals(s.username) && password.Equals(s.password))
                {
                    Session["username"] = username;
                    logged_user = (Session["username"].ToString()).Remove(0, 1);
                    return this.Json(true, JsonRequestBehavior.AllowGet);
                }
            }

            return this.Json(false, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Save_post()
        {
            string question = Request["textarea"];
            string anonymous = Request["checkbox"];
            string tags = Request["tags"];

            if (tags == "")
            {
                tags = null;
            }

            post p = new post();
            notification noti = new notification();

            p.username = logged_user;
            p.questions = question;
            p.anonymous = Convert.ToBoolean(Convert.ToInt32(anonymous));

            if (tags != null)
            {
                var query = (from i in db.post orderby i.Id descending select i).FirstOrDefault();

                int id = query.Id + 1;

                p.tags = tags;

                noti.postid = "q" + id.ToString();
                noti.username = tags;
                noti.notifier = logged_user;
                noti.type = "question";
                noti.state = false;
                noti.time = DateTime.Now;

                db.notification.Add(noti);
            }

            try
            {
                db.post.Add(p);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                return this.Json(false, JsonRequestBehavior.AllowGet);
            }

            return this.Json(true, JsonRequestBehavior.AllowGet);


        }

        [HttpPost]
        public ActionResult Save_img_post()
        {
            bool anonymous;

            string check = Request["img_checkbox"];

            if (check == "on")
            {
                anonymous = true;
            }
            else
                anonymous = Convert.ToBoolean(Request["img_checkbox"]);

            string question = Request["imgTxt"];
            string image;

            string MyUrl = Request.UrlReferrer.AbsolutePath;
            string[] abc;
            abc = MyUrl.Split('/');
            string path = abc[2];

            HttpPostedFileBase f1 = Request.Files[0];

            post p = new post();

            if (anonymous == false)
            {
                p.anonymous = false;
            }
            else
            {
                p.anonymous = true;
            }

            p.username = logged_user;
            p.questions = question;

            if (f1.FileName != "")
            {
                f1.SaveAs(Server.MapPath("~/Content/images/post_img/" + f1.FileName));

                image = "/Content/images/post_img/" + f1.FileName;
                p.image = image;
            }

            db.post.Add(p);
            db.SaveChanges();

            TempData["value"] = 1;

            return RedirectToAction(path);
        }

        public JsonResult Save_answer()
        {
            user u, s_user;
            notification noti = new notification();

            int ques_id = int.Parse(Request["ques_id"]);

            post p = db.post.First(n => n.Id.Equals(ques_id));

            u = db.user.First(n => n.username.Equals("@" + logged_user));

            s_user = db.user.First(n => n.username.Equals("@" + p.username));

            string answer = Request["ans-textarea"];

            answer a = new answer();

            if (p.anonymous == false)
            {
                a.s_profilepic = s_user.profilepic;
            }

            a.image = p.image;
            a.username = p.username;
            a.a_username = logged_user;
            a.question = p.questions;
            a.ans = answer;
            a.a_profilepic = u.profilepic;
            a.like = 0;
            a.dislike = 0;
            a.time = DateTime.Now;

            noti.postid = "a" + p.Id.ToString();
            noti.username = p.username;
            noti.notifier = logged_user;
            noti.type = "answer";
            noti.state = false;
            noti.time = DateTime.Now;

            try
            {
                db.answer.Add(a);
                db.notification.Add(noti);
                db.SaveChanges();
                return this.Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return this.Json(false, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult Set_like_dislike()
        {
            userLike u_like = new userLike();
            notification noti = new notification();
            answer a;

            int likefield_id = int.Parse(Request["likefield"]);
            int dislikefield_id = int.Parse(Request["dislikefield"]);

            if (likefield_id.Equals(0))
            {
                a = db.answer.First(n => n.Id.Equals(dislikefield_id));
            }
            else
            {
                a = db.answer.First(n => n.Id.Equals(likefield_id));
            }

            u_like.username = logged_user;
            u_like.questions = a.question;
            u_like.image = a.image;
            u_like.answer = a.ans;

            var check = (from ch in db.userLike.Where(y => y.questions == a.question && y.answer == a.ans && y.username == logged_user)
                         select ch).SingleOrDefault();

            if (check == null && likefield_id.Equals(0))
            {
                u_like.dislike = 1;
                u_like.like = 0;
                a.dislike = a.dislike + 1;

                noti.postid = "a" + a.Id.ToString();
                noti.username = a.username;
                noti.notifier = logged_user;
                noti.state = false;
                noti.time = DateTime.Now;
                noti.type = "dislike";

                db.notification.Add(noti);
                db.userLike.Add(u_like);
                db.SaveChanges();

                return this.Json(true, JsonRequestBehavior.AllowGet);
            }

            else if (check == null && dislikefield_id.Equals(0))
            {
                u_like.like = 1;
                u_like.dislike = 0;
                a.like = a.like + 1;

                noti.postid = "a" + a.Id.ToString();
                noti.username = a.username;
                noti.notifier = logged_user;
                noti.state = false;
                noti.time = DateTime.Now;
                noti.type = "like";

                db.notification.Add(noti);
                db.userLike.Add(u_like);
                db.SaveChanges();
                return this.Json(true, JsonRequestBehavior.AllowGet);
            }

            else if (check != null && likefield_id.Equals(0) && check.dislike.Equals(0))
            {
                check.dislike = 1;
                check.like = 0;

                a.like = a.like - 1;
                a.dislike = a.dislike + 1;

                noti.postid = "a" + a.Id.ToString();
                noti.username = a.username;
                noti.notifier = logged_user;
                noti.state = false;
                noti.time = DateTime.Now;
                noti.type = "dislike";

                db.notification.Add(noti);
                db.SaveChanges();
                return this.Json(true, JsonRequestBehavior.AllowGet);
            }

            else if (check != null && dislikefield_id.Equals(0) && check.like.Equals(0))
            {
                check.like = 1;
                check.dislike = 0;

                a.like = a.like + 1;
                a.dislike = a.dislike - 1;

                noti.postid = "a" + a.Id.ToString();
                noti.username = a.username;
                noti.notifier = logged_user;
                noti.state = false;
                noti.time = DateTime.Now;
                noti.type = "like";

                db.notification.Add(noti);
                db.SaveChanges();
                return this.Json(true, JsonRequestBehavior.AllowGet);
            }
            //new
            else if (check != null && likefield_id.Equals(0) && check.dislike.Equals(1))
            {
                db.userLike.Remove(check);
                a.dislike = a.dislike - 1;

                db.SaveChanges();
                return this.Json(true, JsonRequestBehavior.AllowGet);
            }

            else if (check != null && dislikefield_id.Equals(0) && check.like.Equals(1))
            {
                db.userLike.Remove(check);
                a.like = a.like - 1;

                db.SaveChanges();
                return this.Json(true, JsonRequestBehavior.AllowGet);
            }

            else
            {
                return this.Json(false, JsonRequestBehavior.AllowGet);
            }

            //try
            //{
            //    db.userlike.Add(u_like);
            //    db.SaveChanges();

            //    return this.Json(true, JsonRequestBehavior.AllowGet);
            //}
            //catch (Exception e)
            //{
            //    return this.Json(false, JsonRequestBehavior.AllowGet);
            //}
        }

        [HttpPost]
        public ActionResult Save_settings()
        {
            user u = (from a in db.user.Where(x => x.username == "@" + logged_user) select a).FirstOrDefault();

            var ans = (from a in db.answer.Where(y => y.a_username == logged_user) select a).ToList();
            var ans_obj1 = (from obj in db.answer.Where(z => z.username == logged_user) select obj).ToList();

            answer save_sett;
            answer save_sett1;

            string profile_img;
            string cover_img;
            string fullname = Request["fullname"];
            string skills = Request["skills"];
            string bio = Request["bio"];
            string birthday = Request["birthday"];
            string gender = Request["gender"];

            HttpPostedFileBase f1 = Request.Files[0];

            if (f1.FileName != "")
            {
                f1.SaveAs(Server.MapPath("~/Content/images/profile/" + f1.FileName));

                profile_img = "/Content/images/profile/" + f1.FileName;
                u.profilepic = profile_img;

                foreach (var itr in ans)
                {
                    save_sett = itr;
                    save_sett.a_profilepic = profile_img;
                }

                foreach (var itr in ans_obj1)
                {
                    save_sett1 = itr;
                    save_sett1.s_profilepic = profile_img;
                }
            }

            HttpPostedFileBase f2 = Request.Files[1];

            if (f2.FileName != "")
            {
                f2.SaveAs(Server.MapPath("~/Content/images/cover/" + f2.FileName));

                cover_img = "/Content/images/cover/" + f2.FileName;
                u.coverpic = cover_img;
            }

            u.fullname = fullname;
            u.dob = birthday;
            u.skills = skills;
            u.gender = gender;
            u.bio = bio;

            db.SaveChanges();

            string MyUrl = Request.UrlReferrer.AbsolutePath;

            string[] str;

            str = MyUrl.Split('/');

            string target_url = str[2];

            return RedirectToAction(target_url);
        }

        public JsonResult search_user_result()
        {
            string fullname = Request["search_user"];

            if (fullname == "")
            {
                return this.Json(false, JsonRequestBehavior.AllowGet);
            }
            else if (fullname.Substring(0, 1) == "@")
            {
                var get_result = (from a in db.user where a.username.StartsWith(fullname) select a).ToList();
                TempData["data"] = get_result;

                return this.Json(get_result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var get_result = (from a in db.user where a.fullname.StartsWith(fullname) select a).ToList();
                TempData["data"] = get_result;

                return this.Json(get_result, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult search_user_result_by_skill()
        {
            var skill = Server.UrlEncode(Request["search_user_skill"]);

            if (skill == "")
            {
                return this.Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var get_result = (from a in db.user where a.skills == skill select a).ToList();
                //TempData["data"] = get_result;
                return this.Json(get_result, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Fprofile()
        {
            var username = Request["a"];
            search_user = username;
            return this.Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Handlefriendreq()
        {
            var f_username = Request["f_username"];

            f_username = f_username.Remove(0, 1).Trim();

            friends f1 = new friends();
            friends f2 = new friends();

            f1.username = logged_user;
            f1.userfriends = f_username;
            db.friends.Add(f1);

            f2.username = f_username;
            f2.userfriends = logged_user;
            db.friends.Add(f2);

            db.SaveChanges();


            return this.Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Removefrnd()
        {
            var f_username = Request["f_username"];
            f_username = f_username.Remove(0, 1).Trim();

            var msc = (from a in db.friends where a.username == logged_user && a.userfriends == f_username select a).SingleOrDefault();
            var msc1 = (from b in db.friends where b.username == f_username && b.userfriends == logged_user select b).SingleOrDefault();

            db.friends.Remove(msc);
            db.friends.Remove(msc1);

            db.SaveChanges();
            return this.Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult HandleReports()
        {
            var postid = Request["postid"];
            var report_comment = Request["report_comment"];

            report rep = new report();

            rep.username = logged_user;
            rep.postid = postid;
            rep.comment = report_comment;
            rep.status = "pending";
            rep.time = DateTime.Now;

            db.report.Add(rep);
            db.SaveChanges();

            return this.Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ResharePost()
        {
            var postid = Request["postid"];

            sharedPost shp = new sharedPost();

            shp.username = logged_user;
            shp.answerid = Convert.ToInt32(postid);
            shp.time = DateTime.Now;

            db.sharedPost.Add(shp);
            db.SaveChanges();

            return this.Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult HandleNotification()
        {
            int postid = Convert.ToInt32(Request["postid"]);

            notification noti = db.notification.Where(n => n.Id == postid).FirstOrDefault();

            noti.state = true;
            db.SaveChanges();

            return this.Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckPassword()
        {
            var old_pass = Request["old_pass"];
            user get_user = db.user.Where(u => u.username == "@" + logged_user).SingleOrDefault();

            if (get_user.password == old_pass)
            {
                return this.Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ChangePassword()
        {
            var new_pass = Request["new_pass"];

            user get_user = db.user.Where(u => u.username == "@" + logged_user).SingleOrDefault();

            get_user.password = new_pass;

            db.SaveChanges();

            return this.Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Group()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session["username"] = null;
            return RedirectToAction("MainPage", "Home");
        }

        public ActionResult Deactivate()
        {
            Session["username"] = null;
            return RedirectToAction("MainPage", "Home");
        }
    }
}
