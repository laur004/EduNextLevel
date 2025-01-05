using ProiectDAW.Data;
using ProiectDAW.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace ArticlesApp.Controllers
{
    public class ArticlesController : Controller
    {   

        //PAS 10

        private readonly IWebHostEnvironment _env;
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public ArticlesController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IWebHostEnvironment env
        )
        {
            _env = env;
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        //private readonly ApplicationDbContext db;
        //private readonly IWebHostEnvironment _env;
        //public ArticlesController(ApplicationDbContext context, IWebHostEnvironment env)
        //{
        //    db = context;
        //    _env = env;
        //}

        // Se afiseaza lista tuturor articolelor impreuna cu categoria 
        // din care fac parte
        // HttpGet implicit

        //[Authorize(Roles = "Admin,User")]
        public IActionResult Index()
        {
            var articles = db.Articles.Include("Chapter")
                                        .Include("User");

            // ViewBag.OriceDenumireSugestiva
            ViewBag.Articles = articles;

            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
                ViewBag.Alert = TempData["messageType"];
            }

            return View();
        }

        // Se afiseaza un singur articol in functie de id-ul sau 
        // impreuna cu categoria din care face parte
        // In plus sunt preluate si toate comentariile asociate unui articol
        // HttpGet implicit
        public IActionResult Show(int id)
        {
            Article article = db.Articles.Include("Chapter")
                                         .Include("Comments")
                                         .Include("User")
                                         .Include("Comments.User")
                              .Where(art => art.Id == id)
                              .First();

            SetAccessRights();

            return View(article);
        }




        // Adaugarea unui comentariu asociat unui articol in baza de date
        // Toate rolurile pot adauga comentarii in baza de date
        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public IActionResult Show([FromForm] Comment comment)
        {
            comment.Date = DateTime.Now;

            // preluam Id-ul utilizatorului care posteaza comentariul
            comment.UserId = _userManager.GetUserId(User);

            try
            {
                db.Comments.Add(comment);
                db.SaveChanges();
                return Redirect("/Articles/Show/" + comment.ArticleId);
            }
            catch (Exception)
            {
                Article art = db.Articles.Include("Chapter")
                                         .Include("User")
                                         .Include("Comments")
                                         .Include("Comments.User")
                               .Where(art => art.Id == comment.ArticleId)
                               .First();

                //return Redirect("/Articles/Show/" + comm.ArticleId);

                SetAccessRights();

                return View(art);
            }
        }



        // Se afiseaza formularul in care se vor completa datele unui articol
        // impreuna cu selectarea categoriei din care face parte
        // HttpGet implicit

        [Authorize(Roles ="User,Admin")]
        public IActionResult New()
        {
          
            Article article = new Article();

            article.Chap = GetAllChapters();

            return View(article);
        }

        // POST: Procesează datele trimise de utilizator

        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> New(Article article, IFormFile Image)
        {
            article.Date = DateTime.Now;

            //preluam id-ul user-ului
            article.UserId = _userManager.GetUserId(User);

 
            if (Image != null && Image.Length > 0)
            {
                // Verificăm extensia
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".mp4", ".mov" };
                var fileExtension = Path.GetExtension(Image.FileName).ToLower();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("ArticleImage", "Fișierul trebuie să fie o imagine (jpg, jpeg, png, gif) sau un video (mp4,  mov).");

                    article.Chap = GetAllChapters();

                    return View(article);
                }

                // Cale stocare
                var storagePath = Path.Combine(_env.WebRootPath, "images", Image.FileName);
                var databaseFileName = "/images/" + Image.FileName;

                // Salvare fișier
                using (var fileStream = new FileStream(storagePath, FileMode.Create))
                {
                    await Image.CopyToAsync(fileStream);
                }

                ModelState.Remove(nameof(article.Image));


                article.Image = databaseFileName;
                
            }

            if(TryValidateModel(article))
            {
                // Adăugare articol
                db.Articles.Add(article);
                await db.SaveChangesAsync();

                TempData["message"] = "Articolul a fost adaugat";
                TempData["messageType"] = "alert-success";

                // Redirecționare după succes
                return RedirectToAction("Index", "Articles");
            }
            
            article.Chap = GetAllChapters();
            return View(article);
        }


        // Se editeaza un articol existent in baza de date impreuna cu categoria din care face parte
        // Categoria se selecteaza dintr-un dropdown
        // HttpGet implicit
        // Se afiseaza formularul impreuna cu datele aferente articolului din baza de date

        [Authorize(Roles ="Admin,User")]
        public IActionResult Edit(int id)
        {

            Article article = db.Articles.Include("Chapter")
                                         .Where(art => art.Id == id)
                                         .First();

            article.Chap = GetAllChapters();

            if( (article.UserId== _userManager.GetUserId(User)) || User.IsInRole("Admin"))
            { 
                return View(article);
            }
            else
            {
                
                TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unui articol care nu va apartine";
                TempData["messageType"] = "alert-danger";

                return RedirectToAction("Index");
                //return RedirectToAction("Show", new { id });
            }
        }

        // Se adauga articolul modificat in baza de date
        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public IActionResult Edit(int id, Article requestArticle)
        {
            Article article = db.Articles.Find(id);

            try
            {
                if ((article.UserId == _userManager.GetUserId(User)) || User.IsInRole("Admin"))
                {

                    article.Title = requestArticle.Title;
                    article.Content = requestArticle.Content;
                    article.Date = DateTime.Now;
                    article.ChapterId = requestArticle.ChapterId;
                    db.SaveChanges();
                    TempData["message"] = "Articolul a fost modificat";
                    TempData["messageType"] = "alert-success";

                    return RedirectToAction("Index");
                    //return RedirectToAction("Show", new { id });

                }
                else
                {
                    TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unui articol care nu va apartine";
                    TempData["messageType"] = "alert-danger";

                    return RedirectToAction("Index");
                    //return RedirectToAction("Show", new { id });
                }

            }
            catch (Exception e)
            {
                requestArticle.Chap = GetAllChapters();
                return View(requestArticle);
            }
        }


        // Se sterge un articol din baza de date 
        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public ActionResult Delete(int id)
        {
            Article article = db.Articles.Include("Comments")
                                         .Where(art => art.Id == id)
                                         .First();

            if ((article.UserId == _userManager.GetUserId(User)) || User.IsInRole("Admin"))
            {

                // Construiește calea completă către imagine
                var filePath = Path.Combine(_env.WebRootPath, article.Image.TrimStart('/'));

                // Șterge imaginea dacă există
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                db.Articles.Remove(article);
                db.SaveChanges();
                TempData["message"] = "Articolul a fost sters";
                TempData["messageType"] = "alert-success";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unui articol care nu va apartine";
                TempData["messageType"] = "alert-danger";

                return RedirectToAction("Index");
                //return RedirectToAction("Show", new { id });
            }
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




        [NonAction]
        public IEnumerable<SelectListItem> GetAllChapters()
        {
            // generam o lista de tipul SelectListItem fara elemente
            var selectList = new List<SelectListItem>();

            // extragem toate categoriile din baza de date
            var categories = from cat in db.Chapters
                             select cat;

            // iteram prin categorii
            foreach (var Chapter in categories)
            {
                // adaugam in lista elementele necesare pentru dropdown
                // id-ul categoriei si denumirea acesteia
                selectList.Add(new SelectListItem
                {
                    Value = Chapter.Id.ToString(),
                    Text = Chapter.ChapterTitle
                });
            }
            /* Sau se poate implementa astfel: 
             * 
            foreach (var Chapter in categories)
            {
                var listItem = new SelectListItem();
                listItem.Value = Chapter.Id.ToString();
                listItem.Text = Chapter.ChapterName;

                selectList.Add(listItem);
             }*/


            // returnam lista de categorii
            return selectList;
        }

        public IActionResult ArticlesForSubjectAndGradeAndChapter(int subjectid, int gradeid, int chapterid)
        {
            var articles = (from a in db.Articles
                           where a.ChapterId == chapterid
                           select a).ToList();

            ViewBag.Articles = articles;

            ViewBag.Grade = (from g in db.Grades
                             where g.Id == gradeid
                             select g).FirstOrDefault();

            ViewBag.Subject = (from s in db.Subjects
                               where s.Id == subjectid
                               select s).FirstOrDefault();

            ViewBag.Chapter = (from c in db.Chapters
                                where c.Id == chapterid
                                select c).FirstOrDefault();

            return View();
        }

    }
}
