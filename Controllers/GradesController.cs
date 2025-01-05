using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProiectDAW.Data;
using ProiectDAW.Models;

namespace ProiectDAW.Controllers
{
	
	public class GradesController : Controller
	{

        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public GradesController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager
        )
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
		{
			var Grades = from grad in db.Grades
						   orderby grad.GradeName
						   select grad;

			ViewBag.Grades = Grades;

			return View();
		}

        [Authorize(Roles = "Admin")]
        public IActionResult Show(int id)
		{
			Grade grade = db.Grades.Find(id);
			return View(grade);
		}

        [Authorize(Roles = "Admin")]
        public IActionResult New()
		{
			return View();
		}

		[HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult New(Grade grade)
		{
			if (ModelState.IsValid)
			{
				db.Grades.Add(grade);
				db.SaveChanges();
				TempData["message"] = "Clasa a fost adaugata";
				return RedirectToAction("Index");
			}
			else
			{
				return View(grade);
			}

		}

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
		{
			Grade grade = db.Grades.Find(id);
			return View(grade);
		}

		[HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id, Grade req_grade)
		{
			Grade grade = db.Grades.Find(id);

			if (ModelState.IsValid)
			{
				grade.GradeName = req_grade.GradeName;
				db.SaveChanges();
				TempData["message"] = "clasa a fost modificata!";
				return RedirectToAction("Index");

			}
			return View(req_grade);
		}

		[HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
		{
			Grade grade = db.Grades.Include("Chapters")
						.Include("Chapters.Articles")
						.Include("Chapters.Articles.Comments")
						.Where(s => s.Id==id).First();

			db.Grades.Remove(grade);
			db.SaveChanges();
			TempData["message"] = "clasa a fost stearsa!";
			return RedirectToAction("Index");
		}



		public IActionResult GradesForSubject(int id)
		{
            var grades = (from c in db.Chapters
                          join g in db.Grades on c.GradeId equals g.Id
                          where c.SubjectId == id
                          select g).Distinct().ToList();

            ViewBag.Grades = grades;


            //ViewBag.SubjectId = id;

            ViewBag.Chapters = (from c in db.Chapters
                             join g in db.Grades
                             on c.GradeId equals g.Id
                             where c.SubjectId == id
                             orderby c.Id
                             select c).ToList();

			ViewBag.Subject = (from s in db.Subjects
								   where s.Id == id
								   select s).FirstOrDefault();

			//ViewBag.SubjectName = (from s in db.Subjects
			//                                where s.Id == id
			//                                select s.Name).FirstOrDefault();

			SetAccessRights();

            return View();

           

        }



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
