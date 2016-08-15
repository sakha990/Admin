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
        public ActionResult Create(int categoryId)
        {
            Category category = DBManager.GetCategoryObject(categoryId);
            ViewBag.CategoryId = category.CategoryId;
            ViewBag.CategoryName = category.CategoryName;
            ViewBag.ParentCategoryName = category.ParentCategoryName;
            ViewBag.CategoryTree = DBManager.GetCategoryTree();
            return View("Create");
        }

        // POST: Parameter/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            int categoryId = Convert.ToInt32(collection["inputCategoryId"]);
            string parameterName = Convert.ToString(collection["inputParamterName"]);
            string parameterValues = Convert.ToString(collection["inputParamterValues"]);
            DBManager.CreateCategoryParameter(categoryId, parameterName, parameterValues, "sakha");
            return RedirectToAction("Index", new { categoryId = categoryId });

        }

        // GET: Parameter/Edit/5
        public ActionResult Edit(int categoryId, string categoryParameterName)
        {
            Category category = DBManager.GetCategoryObject(categoryId);
            ViewBag.CategoryId = category.CategoryId;
            ViewBag.CategoryName = category.CategoryName;
            ViewBag.ParentCategoryName = category.ParentCategoryName;
            ViewBag.CategoryParameterName = categoryParameterName;
            ViewBag.CategoryParameterValues = DBManager.GetCategoryParameterValues(categoryId, categoryParameterName);
            ViewBag.CategoryTree = DBManager.GetCategoryTree();
            return View("Edit");
        }

        // POST: Parameter/Edit/5
        [HttpPost]
        public ActionResult Edit(FormCollection collection)
        {
            int categoryId = Convert.ToInt32(collection["inputCategoryId"]);
            string userName = "sakha";
            CategoryParameter categoryParameter = new CategoryParameter();
            categoryParameter.ParameterName = Convert.ToString(collection["inputCategoryParameterName"]);
            categoryParameter.ParameterValues = Convert.ToString(collection["inputCategoryParameterValues"]);
            DBManager.UpdateCategoryParameter(categoryId, userName, categoryParameter);
            return RedirectToAction("Index", new { categoryId = categoryId });
        }

        public ActionResult Delete(int categoryId,string categoryParameterName)
        {
            ViewBag.CategoryId = categoryId;
            ViewBag.CategoryParameterName = categoryParameterName;
            return View("ConfirmDelete");

        }
        // POST: Parameter/Delete/5
        [HttpPost]
        public ActionResult Delete(FormCollection collection)
        {
            int categoryId = Convert.ToInt32(collection["inputCategoryId"]);
            string categoryParameterName = Convert.ToString(collection["inputCategoryParameterName"]);
            DBManager.DeleteCategoryParameter(categoryId, categoryParameterName);
            return RedirectToAction("Index", new { categoryId = categoryId });

        }
    }
}