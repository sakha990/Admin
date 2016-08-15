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

        public ActionResult Create(string parentCategory)
        {
            
            ViewBag.categoryTree = DBManager.GetCategoryTree();
            ViewBag.ParentCategory = parentCategory;
            return View("Create");
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            string category = Convert.ToString(collection["inputCategory"]);
            string parentCategory = Convert.ToString(collection["inputParentCategory"]);
            DBManager.CreateCategory(category, parentCategory, "sakha");
            return RedirectToAction("Index", new { parentCategory = parentCategory });
        }

        [HttpPost]
        public ActionResult Delete(FormCollection collection)
        {
            string parentCategory = Convert.ToString(collection["inputParentCategory"]);
            int categoryId = Convert.ToInt32(collection["inputCategoryId"]);
            DBManager.DeleteCategory(categoryId);
            return RedirectToAction("Index", new { parentCategory = parentCategory });

        }
    }
}
