using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BeltExam.Models;
using BeltExam.Models.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BeltExam.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext { get; set; } 

        public HomeController(MyContext context)
        {
            dbContext = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        
        [HttpPost("register")]
        public IActionResult Register(UserForm user)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.Users.Any(u => u.Email == user.Register.Email))
                {
                    ModelState.AddModelError("Register.Email", "Email already in use!");
                    return View("Index");           
                }
                else
                {
                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    user.Register.Password = Hasher.HashPassword(user.Register, user.Register.Password);
                    dbContext.Users.Add(user.Register);
                    dbContext.SaveChanges();
                    HttpContext.Session.SetInt32("UserId", user.Register.UserId);
                    return RedirectToAction("Dashboard");
                }
            }
            else
            {
                return View("Index");
            }
        }

        [HttpPost("login")]
        public IActionResult Login(UserForm log)
        {
            if(ModelState.IsValid)
            {
                User userInDb = dbContext.Users.FirstOrDefault(u => u.Email == log.Login.LoginEmail);
                if(userInDb != null)
                {
                    var hash = new PasswordHasher<Login>();
                    var result = hash.VerifyHashedPassword(log.Login, userInDb.Password, log.Login.LoginPassword);
                    if(result == 0)
                    {
                        ModelState.AddModelError("Login.LoginEmail","Invalid Email / Password");
                        ModelState.AddModelError("Login.LoginPassword", "Invalid Email/Password");
                        return View("Index");
                    }
                    else
                    {
                        HttpContext.Session.SetInt32("UserId", userInDb.UserId);
                        return RedirectToAction("Dashboard");
                    }
                }
                else
                {
                    ModelState.AddModelError("Login.LoginEmail","Invalid Email / Password");
                    ModelState.AddModelError("Login.LoginPassword", "Invalid Email/Password");
                    return View("Index");
                }
            }
            else
            {
                return View("Index");
            }
        }

        private User GetUserInDb()
        {
            return dbContext.Users.FirstOrDefault( u => u.UserId == HttpContext.Session.GetInt32("UserId"));
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        // [HttpGet("date")]
        // private IActionResult Date()
        // {

        //         return (dbContext.Activities.Include(m => m.Coordinator).Include(m => m.Attendees).ThenInclude(f => f.Guest).OrderBy(m => m.Date).ToList(););
        // }
        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            User userIndb = GetUserInDb();
            if(userIndb != null)
            {
                ViewBag.User = userIndb;
                DateTime check = DateTime.Now.Date;

                List<DojoActivity> DeleteActivites = dbContext.Activities.Where(m => m.Date < check).ToList();
                Console.WriteLine(DeleteActivites);
                foreach(var d in DeleteActivites)
                {
                    Console.WriteLine("Delete loop ran");
                    Console.WriteLine(d);
                    dbContext.Activities.Remove(d);
                    dbContext.SaveChanges();
                }
                List<DojoActivity> AllActivities = dbContext.Activities
                                                        .Include(m => m.Coordinator)
                                                        .Include(m => m.Attendees)
                                                        .ThenInclude(f => f.Guest)
                                                        .OrderBy(m => m.Date)
                                                        .ToList();
                return View(AllActivities);
            }
            else
            {
                return RedirectToAction("Logout");
            }
        }

        [HttpGet("join/{DojoActivityId}")]
        public IActionResult join(int DojoActivityId)
        {
            User userInDb = GetUserInDb();
            if(userInDb != null)
            {
                Participant joining = new Participant();
                joining.UserId = userInDb.UserId;
                joining.DojoActivityId = DojoActivityId;
                dbContext.Participants.Add(joining);  
                dbContext.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            else
            {
                return RedirectToAction("Logout");
            }
        }

        [HttpGet("leave/{DojoActivityId}")]
        public IActionResult Leave(int dojoActivityId)
        {
            User userInDb = GetUserInDb();
            if(userInDb != null)
            {
                Participant leaving = dbContext.Participants.FirstOrDefault( f => f.UserId == userInDb.UserId && f.DojoActivityId == dojoActivityId);
                dbContext.Participants.Remove(leaving);
                dbContext.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            else
            {
                return RedirectToAction("Logout");
            }
        }

        [HttpGet("new")]
        public IActionResult New()
        {
            User userIndb = GetUserInDb();
            if(userIndb != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Logout");
            }
        }

        [HttpPost("create/activity")]
        public IActionResult CreateActivity(DojoActivity newDojoActivity)
        {
            User userIndb = GetUserInDb();
            if(userIndb != null)
            {
                if(ModelState.IsValid)
                {
                    newDojoActivity.UserId = userIndb.UserId;
                    dbContext.Activities.Add(newDojoActivity);
                    dbContext.SaveChanges();
                    return Redirect("/dashboard");
                }
                else
                {
                    return View("New");
                }
            }
            else
            {
                return RedirectToAction("Logout");
            }
        }

        [HttpGet("cancel/{DojoActivityId}")]
        public IActionResult Cancel (int DojoActivityId)
        {
            User userInDb = GetUserInDb();
            if(userInDb != null)
            {
                DojoActivity cancelling = dbContext.Activities.FirstOrDefault( m => m.DojoActivityId == DojoActivityId);
                dbContext.Activities.Remove(cancelling);
                dbContext.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            else
            {
                return RedirectToAction("Logout");
            }
        }

        [HttpGet("show/{DojoActivityId}")]
        public IActionResult Show (int DojoActivityId)
        {
            User userInDb = GetUserInDb();
            if(userInDb != null)
            {
                ViewBag.User = userInDb;
                DojoActivity show = dbContext.Activities
                                            .Include( m => m.Coordinator)
                                            .Include( m => m.Attendees)
                                            .ThenInclude( f => f.Guest)
                                            .FirstOrDefault( m => m.DojoActivityId == DojoActivityId);
                return View(show);
            }
            else
            {
                return RedirectToAction("Logout");
            }
        }
    }
}
