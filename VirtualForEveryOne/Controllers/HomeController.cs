using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VirtualForEveryOne.Models;

namespace VirtualForEveryOne.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        VirtualContext db = new VirtualContext();

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

                TempData["friends"] = (from a in db.Friendses.Where(x => x.username == logged_user) select a).ToList();

                TempData["user"] = (from u in db.Users select u).ToList();

                TempData["sharedPost"] = db.SharedPosts.OrderByDescending(u => u.id).ToList();

                TempData["answer"] =
                    (from n in db.Answers where n.status == null || n.status == "1" orderby n.Id descending select n)
                    .ToList();

                TempData["faq"] = (from f in db.Faqs select f).ToList();

                ViewBag.count = db.Notifications
                    .Where(u => u.username == logged_user && u.state == false && u.notifier != logged_user).Count();

                return View();
            }
        }
        [HttpGet]
        public ActionResult EditAnswer(int id)
        {
            Answer answers = new UserMethods().GetAnswersById(id);
            return View(answers);
        }

        [HttpPost]
        public ActionResult EditAnswer(Answer answer)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            answer.time = DateTime.Now;
            new UserMethods().UpdateAnswer(answer);
            return View();
        }

        public ActionResult Settings()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("MainPage");
            }
            else
            {
                //User us = new UserMethods().GetUser();
                ViewBag.count = db.Notifications
                    .Where(u => u.username == logged_user && u.state == false && u.notifier != logged_user).Count();
                TempData["faq"] = (from f in db.Faqs select f).ToList();
                //Session.Add(logged_user, us);
                return View(db.Users.First(x => x.username.Equals("@" + logged_user)));

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

                TempData["friends"] = (from a in db.Friendses.Where(x => x.username == logged_user) select a).ToList();
                TempData["user"] = (from u in db.Users select u).ToList();

                TempData["post"] =
                    (from n in db.Posts where n.tag == null || n.tag == logged_user orderby n.Id descending select n)
                    .ToList();
                //db.post.OrderByDescending(n => n.Id).ToList();
                TempData["faq"] = (from f in db.Faqs select f).ToList();

                ViewBag.count = db.Notifications
                    .Where(u => u.username == logged_user && u.state == false && u.notifier != logged_user).Count();

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

                var getCount = (from a in db.Answers where a.username == logged_user select a.like.Value)
                    .DefaultIfEmpty(0).Sum();

                ViewBag.totalLike = getCount;
                ViewBag.totalPost = db.Posts.Count(x => x.username == logged_user);
                ViewBag.count = db.Notifications
                    .Where(u => u.username == logged_user && u.state == false && u.notifier != logged_user).Count();
                ViewBag.follower = db.Friendses.Count(f => f.username == logged_user);

                TempData["answer"] =
                    (from ans in db.Answers
                     where ans.a_username == logged_user || ans.username == logged_user
                     orderby ans.Id descending
                     select ans).ToList();
                TempData["faq"] = (from f in db.Faqs select f).ToList();

                return View(db.Users.First(x => x.username == logged_user1));
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
                    Friends check;
                    var search_user1 = search_user.Remove(0, 1);

                    check = (from a in db.Friendses
                             where a.username == logged_user && a.userfrirends == search_user.Remove(0, 1)
                             select a).FirstOrDefault();

                    if (check == null)
                    {
                        ViewBag.check = 0;
                    }
                    else
                    {
                        ViewBag.check = 1;
                    }

                    var getCount = (from a in db.Answers where a.username == search_user1 select a.like.Value)
                        .DefaultIfEmpty(0).Sum();

                    ViewBag.totalLike = getCount;
                    ViewBag.totalPost = db.Posts.Count(x => x.username == search_user1);
                    ViewBag.count = db.Notifications.Where(u =>
                        u.username == logged_user && u.state == false && u.notifier != logged_user).Count();
                    ViewBag.follower = db.Friendses.Count(f => f.username == logged_user);

                    TempData["answer"] =
                        (from ans in db.Answers
                         where ans.a_username == search_user1 || ans.username == search_user1
                         orderby ans.Id descending
                         select ans).ToList();
                    TempData["faq"] = (from f in db.Faqs select f).ToList();

                    return View(db.Users.First(x => x.username.Equals(search_user)));
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
                TempData["user"] = (from a in db.Users select a).ToList();
                TempData["faq"] = (from f in db.Faqs select f).ToList();

                ViewBag.username = logged_user;
                ViewBag.count = db.Notifications
                    .Where(u => u.username == logged_user && u.state == false && u.notifier != logged_user).Count();

                return View(db.Friendses.Where(x => x.username == logged_user).ToList());
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
                TempData["post"] = db.Posts.ToList();
                TempData["answer"] = db.Answers.ToList();
                TempData["faq"] = (from f in db.Faqs select f).ToList();

                ViewBag.count = db.Notifications
                    .Where(u => u.username == logged_user && u.state == false && u.notifier != logged_user).Count();

                var query = (from n in db.Notifications
                             where n.username == logged_user && n.notifier != logged_user
                             orderby n.time descending
                             select n).ToList();

                return View(query);
            }
        }

        public JsonResult Check_username()
        {
            string con_user = Request["username"];
            string username = "@" + con_user;

            foreach (var s in db.Users)
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
        [ValidateAntiForgeryToken]
        public ActionResult Save_signup_info(User u)
        {

            string fullname = Request["fullname"];
            string username = Request["username"];
            string email = Request["Email"];
            string skills = Request["skills"];
            string password = Request["password"];
            u.fullname = fullname;
            u.username = "@" + username.ToLower();
            u.email = email;
            u.skills = skills;
            u.password = password;
            if (!ModelState.IsValid)
            {
                return View("mainPage");
            }
            db.Users.Add(u);
            db.SaveChanges();

            Session["username"] = username;

            return RedirectToAction("feed");

        }

        public JsonResult Authenticate_user()
        {
            string username = "@" + Request["log_username"];
            string password = Request["log_password"];

            foreach (var s in db.Users)
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

            Post p = new Post();
            Notification noti = new Notification();

            p.username = logged_user;
            p.question = question;
            p.anonymous = Convert.ToBoolean(Convert.ToInt32(anonymous));

            if (tags != null)
            {
                var query = (from i in db.Posts orderby i.Id descending select i).FirstOrDefault();

                int id = query.Id + 1;

                p.tag = tags;

                noti.postid = "q" + id.ToString();
                noti.username = tags;
                noti.notifier = logged_user;
                noti.type = "question";
                noti.state = false;
                noti.time = DateTime.Now;

                db.Notifications.Add(noti);
            }

            try
            {
                db.Posts.Add(p);
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

            Post p = new Post();

            if (anonymous == false)
            {
                p.anonymous = false;
            }
            else
            {
                p.anonymous = true;
            }

            p.username = logged_user;
            p.question = question;

            if (f1.FileName != "")
            {
                f1.SaveAs(Server.MapPath("~/Content/images/post_img/" + f1.FileName));

                image = "/Content/images/post_img/" + f1.FileName;
                p.image = image;
            }

            db.Posts.Add(p);
            db.SaveChanges();

            TempData["value"] = 1;

            return RedirectToAction(path);
        }

        public JsonResult Save_answer()
        {
            User u, s_user;
            Notification noti = new Notification();

            int ques_id = int.Parse(Request["ques_id"]);

            Post p = db.Posts.First(n => n.Id.Equals(ques_id));

            u = db.Users.First(n => n.username.Equals("@" + logged_user));

            s_user = db.Users.First(n => n.username.Equals("@" + p.username));

            string answer = Request["ans-textarea"];

            Answer a = new Answer();

            if (p.anonymous == false)
            {
                a.s_profilepic = s_user.profilepic;
            }

            a.image = p.image;
            a.username = p.username;
            a.a_username = logged_user;
            a.question = p.question;
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
                db.Answers.Add(a);
                db.Notifications.Add(noti);
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
            UserLike u_like = new UserLike();
            Notification noti = new Notification();
            Answer a;

            int likefield_id = int.Parse(Request["likefield"]);
            int dislikefield_id = int.Parse(Request["dislikefield"]);

            if (likefield_id.Equals(0))
            {
                a = db.Answers.First(n => n.Id.Equals(dislikefield_id));
            }
            else
            {
                a = db.Answers.First(n => n.Id.Equals(likefield_id));
            }

            u_like.username = logged_user;
            u_like.question = a.question;
            u_like.image = a.image;
            u_like.answer = a.ans;

            var check = (from ch in db.UserLikes.Where(y =>
                    y.question == a.question && y.answer == a.ans && y.username == logged_user)
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

                db.Notifications.Add(noti);
                db.UserLikes.Add(u_like);
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

                db.Notifications.Add(noti);
                db.UserLikes.Add(u_like);
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

                db.Notifications.Add(noti);
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

                db.Notifications.Add(noti);
                db.SaveChanges();
                return this.Json(true, JsonRequestBehavior.AllowGet);
            }
            //new
            else if (check != null && likefield_id.Equals(0) && check.dislike.Equals(1))
            {
                db.UserLikes.Remove(check);
                a.dislike = a.dislike - 1;

                db.SaveChanges();
                return this.Json(true, JsonRequestBehavior.AllowGet);
            }

            else if (check != null && dislikefield_id.Equals(0) && check.like.Equals(1))
            {
                db.UserLikes.Remove(check);
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
            User u = (from a in db.Users.Where(x => x.username == "@" + logged_user) select a).FirstOrDefault();

            var ans = (from a in db.Answers.Where(y => y.a_username == logged_user) select a).ToList();
            var ans_obj1 = (from obj in db.Answers.Where(z => z.username == logged_user) select obj).ToList();

            Answer save_sett;
            Answer save_sett1;

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
                var get_result = (from a in db.Users where a.username.StartsWith(fullname) select a).ToList();
                TempData["data"] = get_result;

                return this.Json(get_result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var get_result = (from a in db.Users where a.fullname.StartsWith(fullname) select a).ToList();
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
                var get_result = (from a in db.Users where a.skills == skill select a).ToList();
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

            Friends f1 = new Friends();
            Friends f2 = new Friends();

            f1.username = logged_user;
            f1.userfrirends = f_username;
            db.Friendses.Add(f1);

            f2.username = f_username;
            f2.userfrirends = logged_user;
            db.Friendses.Add(f2);

            db.SaveChanges();


            return this.Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Removefrnd()
        {
            var f_username = Request["f_username"];
            f_username = f_username.Remove(0, 1).Trim();

            var msc = (from a in db.Friendses where a.username == logged_user && a.userfrirends == f_username select a)
                .SingleOrDefault();
            var msc1 = (from b in db.Friendses where b.username == f_username && b.userfrirends == logged_user select b)
                .SingleOrDefault();

            db.Friendses.Remove(msc);
            db.Friendses.Remove(msc1);

            db.SaveChanges();
            return this.Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult HandleReports()
        {
            var postid = Request["postid"];
            var report_comment = Request["report_comment"];

            Report rep = new Report();

            rep.username = logged_user;
            rep.postid = postid;
            rep.comments = report_comment;
            rep.status = "pending";
            rep.time = DateTime.Now;

            db.Reports.Add(rep);
            db.SaveChanges();

            return this.Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ResharePost()
        {
            var postid = Request["postid"];

            SharedPost shp = new SharedPost();

            shp.username = logged_user;
            shp.answerid = Convert.ToInt32(postid);
            shp.time = DateTime.Now;

            db.SharedPosts.Add(shp);
            db.SaveChanges();

            return this.Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult HandleNotification()
        {
            int postid = Convert.ToInt32(Request["postid"]);

            Notification noti = db.Notifications.Where(n => n.Id == postid).FirstOrDefault();

            noti.state = true;
            db.SaveChanges();

            return this.Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ChangePassword(string username)
        {
            ViewBag.username = username;
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(FormCollection formdata, string username)
        {

            using (db)
            {
                User user = db.Users.SingleOrDefault(x => x.username == username);
                if (user != null)
                {

                    user.password = formdata["password"];
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("MainPage", "Home");
                }
            }

            return View();
        }
        //public JsonResult CheckPassword()
        //{
        //    var old_pass = Request["old_pass"];
        //    User get_user = db.Users.Where(u => u.username == "@" + logged_user).SingleOrDefault();

        //    if (get_user.password == old_pass)
        //    {
        //        return this.Json(true, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return this.Json(false, JsonRequestBehavior.AllowGet);
        //    }
        //}

        //public JsonResult ChangePassword()
        //{
        //    var new_pass = Request["new_pass"];

        //    User get_user = db.Users.Where(u => u.username == "@" + logged_user).SingleOrDefault();

        //    get_user.password = new_pass;

        //    db.SaveChanges();

        //    return this.Json(true, JsonRequestBehavior.AllowGet);
        //}
        [HttpGet]
        public ActionResult Group()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Group(Group group)
        {
            if (Session["username"] != null)
            {
                if (group != null)
                {
                    long numb = DateTime.Now.Ticks;
                    int count = 0;

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase file = Request.Files[i];
                        if (file != null && file.ContentLength > 0)
                        {
                            string name = file.FileName;
                            string url = "/Content/group/" + numb + "_" + ++count +
                                         file.FileName.Substring(file.FileName.LastIndexOf("."));
                            string path = Request.MapPath(url);
                            file.SaveAs(path);

                        }
                        else
                        {
                            string name = "No Image";
                            string url = "https://dummyimage.com/800x600/d6d6d6/000000.jpg&text=Sorry+No+Image+Avaialable";

                        }
                    }

                    db.Groups.Add(group);
                    db.SaveChanges();
                    return RedirectToAction("success", "Home");
                }
            }
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

        public ActionResult success()
        {
            return View();

        }

        public ActionResult VisitGroup(int id)
        {
            Group group = new UserMethods().GetGroupById(id);
            if (id != null)
            {
                ViewBag.groupMembers = new UserMethods().GetGroupMembers(id);
            }
            return View(group);

        }
        [HttpGet]
        public ActionResult AddMembers(int groupId)
        {
            ViewBag.groupId = groupId;
            return View();
        }
        [HttpPost]
        public ActionResult AddMembers(GroupMember groupMember, int groupId)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            groupMember.groudid = groupId;
            db.GroupMembers.Add(groupMember);
            db.SaveChanges();
            return View("success");
        }

        public List<GroupMember> GetGroupMember(int groupid)
        {
            if (groupid != null)
            {
                List<GroupMember> groupMembers = new UserMethods().GetGroupMembers(groupid);

                return groupMembers;
            }

            return null;
        }
    }


}
