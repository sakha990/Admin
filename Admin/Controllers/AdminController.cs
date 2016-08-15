using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Admin.Models;
using Admin.Repository;


namespace Admin.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        // I have added this line to test GitHub
        public ActionResult Index()
        {
            ViewBag.CategoryTree = DBManager.GetCategoryTree();
            return View();

        }

    }
}