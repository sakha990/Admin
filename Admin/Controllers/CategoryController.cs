using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Admin.Models;
using Admin.Repository;

namespace Admin.Controllers
{
    public class CategoryController : Controller
    {
        public ActionResult Index(string parentCategoryName)
        {
            List<Category> categories = DBManager.GetCategories(parentCategoryName);
            ViewBag.ParentCategoryName = parentCategoryName;
            ViewBag.CategoryTree = DBManager.GetCategoryTree();
            return View("Index", categories);
        }

        public ActionResult Create(string parentCategoryName, string previousCategoryName, bool success = false)
        {

            ViewBag.PreviousCategoryName = previousCategoryName;
            ViewBag.Success = success;
            ViewBag.ParentCategoryName = parentCategoryName;

            ViewBag.categoryTree = DBManager.GetCategoryTree();
            return View("Create");
        }

        [HttpPost]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                var query = from categoryList in DBManager.GetCategories(category.ParentCategoryName).AsQueryable<Category>()
                            where categoryList.CategoryName == category.CategoryName
                            select categoryList.CategoryName;
                if (query.SingleOrDefault() == category.CategoryName)
                {
                    ViewBag.CategoryTree = DBManager.GetCategoryTree();
                    ViewBag.Success = false;
                    ModelState.AddModelError("", "Duplicate Category Name");
                    return View("Create", category);
                }
                else
                {

                    bool success = DBManager.CreateCategory(category, "sakha");
                    return RedirectToAction("Create", new { parentCategoryName = category.ParentCategoryName, previousCategoryName = category.CategoryName, success = success });
                }
            }
            else
            {

                ViewBag.CategoryTree = DBManager.GetCategoryTree();
                ViewBag.Success = false;
                return View("Create", category);
            }
        }

        [HttpPost]
        public JsonResult Delete(Category category)
        {
            bool result = DBManager.DeleteCategory(category.CategoryId);
            return Json(result);
        }

    }
}