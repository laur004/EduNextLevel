using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProiectDAW.Data;
using ProiectDAW.Models;

namespace ProiectDAW.Controllers
{
    public class SubjectsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SubjectsController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager
        )
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var subjects = (from subj in db.Subjects
                            orderby subj.Name
                            select new Subject
                            {
                                Id = subj.Id,
                                Name = subj.Name
                            }).ToList();

            ViewBag.Subjects = subjects;
            //Console.WriteLine($"Subjects Count: {subjects.Count}"); // Log pentru numărul de elemente

            SetAccessRights();

            return View();
        }

        //public IActionResult Show(int id) 
        //{
        //    Subject subject= db.Subjects.Find(id);
        //    return View(subject);
        //}

        [Authorize(Roles ="Admin")]
        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult New(Subject subject)
        {
            if (ModelState.IsValid)
            {
                db.Subjects.Add(subject);
                db.SaveChanges();
                TempData["message"] = "Materia a fost adaugata";
                return RedirectToAction("Index");
            }
            else
            {
                return View(subject);
            }
            
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {   
            Subject subject = db.Subjects.Find(id);
            return View(subject);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id, Subject req_subject)
        {
            Subject subject = db.Subjects.Find(id);

            if (ModelState.IsValid)
            {
                subject.Name = req_subject.Name;
                db.SaveChanges();
                TempData["message"] = "Materia a fost modificata!";
                return RedirectToAction("Index");

            }
            return View(req_subject);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {


            var subject = db.Subjects
                    .Include(s => s.Chapters)
                        .ThenInclude(c => c.Articles)
                            .ThenInclude(a => a.Comments)
                    .FirstOrDefault(s => s.Id == id);

            //Subject subject = db.Subjects.Find(id);
            db.Subjects.Remove(subject);
            db.SaveChanges();
            TempData["message"] = "Materia a fost stearsa!";
            return RedirectToAction("Index");
        }



        // Conditiile de afisare pentru butoanele de editare si stergere
        // butoanele aflate in view-uri
        private void SetAccessRights()
        {
            ViewBag.AfisareButoane = false;

            if (User.IsInRole("User"))
            {
                ViewBag.AfisareButoane = true;
            }

            ViewBag.UserCurent = _userManager.GetUserId(User);

            ViewBag.EsteAdmin = User.IsInRole("Admin");
        }

    }
}
