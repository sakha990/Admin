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
        public ActionResult Index(string previousParentCategoryName, bool success = false)
        {
            ViewBag.PreviousParentCategoryName = previousParentCategoryName;
            ViewBag.Success = success;
            ViewBag.CategoryTree = DBManager.GetCategoryTree();
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

    }
}