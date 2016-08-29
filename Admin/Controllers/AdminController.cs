using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
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

            return View();

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