using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Admin.Models;
using Admin.Repository;


namespace Admin.Controllers
{
    public class CategoryParameterController : Controller
    {
        // GET: Parameter
        public ActionResult Index(int categoryId)
        {
            List<CategoryParameter> categoryParameters = DBManager.GetCategoryParameters(categoryId);
            Category category = DBManager.GetCategoryObject(categoryId);
            ViewBag.CategoryId = category.CategoryId;
            ViewBag.CategoryName = category.CategoryName;
            ViewBag.ParentCategoryName = category.ParentCategoryName;

            ViewBag.CategoryTree = DBManager.GetCategoryTree();
            return View("Index", categoryParameters);
        }

        // GET: Parameter/Create
        public ActionResult Create(int categoryId, string previousItem, bool success = false)
        {
            ViewBag.PreviousItem = previousItem;
            ViewBag.Success = success;

            Category category = DBManager.GetCategoryObject(categoryId);
            ViewBag.CategoryId = category.CategoryId;
            ViewBag.CategoryName = category.CategoryName;
            ViewBag.ParentCategoryName = category.ParentCategoryName;
            ViewBag.CategoryTree = DBManager.GetCategoryTree();
            return View("Create");
        }

        // POST: Parameter/Create
        [HttpPost]
        public ActionResult Create(CategoryParameter categoryParameter)
        {
            Category category = DBManager.GetCategoryObject(categoryParameter.CategoryId);
            ViewBag.CategoryName = category.CategoryName;
            ViewBag.ParentCategoryName = category.ParentCategoryName;
            ViewBag.CategoryTree = DBManager.GetCategoryTree();

            if (ModelState.IsValid)
            {
                bool success = DBManager.CreateCategoryParameter(categoryParameter, "sakha");
                return RedirectToAction("Create", new { categoryId = categoryParameter.CategoryId, previousItem = categoryParameter.Name, success = success });

            }
            else
            {
                ViewBag.Success = false;
                return View("Create", categoryParameter);
            }

        }

        // GET: Parameter/Edit/5
        public ActionResult Edit(int categoryParameterId,string previousItem)
        {
            CategoryParameter categoryParameter = DBManager.GetCategoryParameter(categoryParameterId);
            Category category = DBManager.GetCategoryObject(categoryParameter.CategoryId);
            ViewBag.CategoryName = category.CategoryName;
            ViewBag.ParentCategoryName = category.ParentCategoryName;
            ViewBag.CategoryTree = DBManager.GetCategoryTree();
            ViewBag.Success = false;
            ViewBag.PreviousItem = previousItem;
            return View("Edit",categoryParameter);
        }

        // POST: Parameter/Edit/5
        [HttpPost]
        public ActionResult Edit(CategoryParameter categoryParameter)
        {
            Category category = DBManager.GetCategoryObject(categoryParameter.CategoryId);
            ViewBag.CategoryName = category.CategoryName;
            ViewBag.ParentCategoryName = category.ParentCategoryName;
            ViewBag.CategoryTree = DBManager.GetCategoryTree();

            if (ModelState.IsValid)
            {
                DBManager.UpdateCategoryParameter("sakha", categoryParameter);
                ViewBag.Success= true;
                ViewBag.PreviousItem = categoryParameter.Name;
                return View("Edit", categoryParameter);
            }
            else
            {
                ViewBag.Success = false;
                return View("Edit", categoryParameter);
            }


        }



        public ActionResult Delete(int categoryId,string categoryParameterName)
        {
            ViewBag.CategoryId = categoryId;
            ViewBag.CategoryParameterName = categoryParameterName;
            return View("ConfirmDelete");

        }
        // POST: Parameter/Delete/5
        [HttpPost]
        public JsonResult Delete(CategoryParameter categoryParameter)
        {
            bool result = DBManager.DeleteCategoryParameter(categoryParameter.CategoryId, categoryParameter.Name);
            return Json(result);
        }
    }
}