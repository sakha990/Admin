using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Admin.Models;
using Admin.Repository;

namespace Admin.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index(string previousParentCategoryName, bool success = false)
        {
            ViewBag.PreviousParentCategoryName = previousParentCategoryName;
            ViewBag.Success = success;
            ViewBag.CategoryTree = DBManager.GetCategoryTree();

            var claimsIdentity = User.Identity as ClaimsIdentity;
            ViewBag.Name = claimsIdentity.FindFirst(ClaimTypes.Name).Value;

            List<Note> notes = DBManager.GetNotes();
            return View(notes);

        }

        public ActionResult CreateNote(string previousNote, bool success = false)
        {
            ViewBag.PreviousNote = previousNote;
            ViewBag.Success = success;
            ViewBag.categoryTree = DBManager.GetCategoryTree();
            return View("CreateNote");
        }

        [HttpPost]
        public ActionResult CreateNote(Note note)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            note.CreatedBy = claimsIdentity.FindFirst(ClaimTypes.Name).Value;

            if (ModelState.IsValid)
            {

                bool success = DBManager.CreateNote(note);
                return RedirectToAction("CreateNote", new { previousNote = "Note", success = success });
            }
            else
            {

                ViewBag.CategoryTree = DBManager.GetCategoryTree();
                ViewBag.Success = false;
                return View("CreateNote", note);
            }
        }

        
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            string parentCategoryName = collection["inputParentCategory"].ToString();
            ViewBag.CategoryTree = DBManager.GetCategoryTree();
            if (parentCategoryName == string.Empty)
            {
                return RedirectToAction("Index", new { previousParentCategoryName = " ", success = false });

            }
            else
            {
                bool success = DBManager.CreateParentCategory(parentCategoryName);
                return RedirectToAction("Index", new { previousParentCategoryName = parentCategoryName, success = success });
            }
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View("Login");
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(string userName, string password)
        {
            if (DBManager.IsValid(userName, password))
            {
                var identity = new ClaimsIdentity(
                  new[] { 
              // adding following 2 claim just for supporting default antiforgery provider
              //new Claim(ClaimTypes.NameIdentifier, userName),
              //new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"),
              new Claim(ClaimTypes.Name,DBManager.GetRealName(userName)),
                  }, "ApplicationCookie");

                var ctx = Request.GetOwinContext();
                var authManager = ctx.Authentication;
                authManager.SignIn(identity);

                return RedirectToAction("Index");
            }
            ViewBag.ErrorMessage = "invalid username or password";
            return View();
        }

        public ActionResult Logout()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut("ApplicationCookie");
            return RedirectToAction("Login", "Admin");
        }
    }
}