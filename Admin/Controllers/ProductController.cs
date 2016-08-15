using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Admin.Models;
using Admin.Repository;
using PagedList;
using PagedList.Mvc;


namespace Admin.Controllers
{
    public class ProductController : Controller
    {

        public ActionResult Index(int categoryId, string sortOrder = "productname", int page = 1, int pageSize = 2)
        {
            Category category = DBManager.GetCategoryObject(categoryId);
            ViewBag.CategoryId = categoryId;
            ViewBag.ParentCategoryName = category.ParentCategoryName;
            ViewBag.CategoryName = category.CategoryName;
            ViewBag.CategoryTree = DBManager.GetCategoryTree();

            ViewBag.ProductNameSortParam = sortOrder == "productname" ? "productname_desc" : "productname";
            ViewBag.CreatedBySortParam = sortOrder == "createdby" ? "createdby_desc" : "createdby";
            ViewBag.CreatedDateSortParam = sortOrder == "createddate" ? "createddate_desc" : "createddate";
            ViewBag.LastUpdatedBySortParam = sortOrder == "lastupdatedby" ? "lastupdatedby_desc" : "lastupdatedby";
            ViewBag.LastUpdatedDateSortParam = sortOrder == "lastupdateddate" ? "lastupdateddate" : "lastupdateddate";
            ViewBag.CurrentSort = sortOrder;

            var productList = DBManager.GetProducts(categoryId).AsQueryable<Product>();
            switch (sortOrder)
            {
                case "productname":
                    productList = productList.OrderBy(x => x.ProductName);
                    break;
                case "createdby":
                    productList = productList.OrderBy(x => x.CreatedBy);
                    break;
                case "createddate":
                    productList = productList.OrderByDescending(x => x.CreatedDate);
                    break;
                case "lastupdatedby":
                    productList = productList.OrderByDescending(x => x.LastUpdatedBy);
                    break;
                case "lastupdateddate":
                    productList = productList.OrderByDescending(x => x.LastUpdatedDate);
                    break;
                default:
                    break;
            }

            return View("Index", productList.ToPagedList(page, pageSize));
        }
        //public ActionResult Create(int categoryId,string productName = "",bool previousInsert = false)
         public ActionResult Create(int categoryId)
        {
            Category category = DBManager.GetCategoryObject(categoryId);
            ViewBag.CategoryId = categoryId;
            ViewBag.ParentCategoryName = category.ParentCategoryName;
            ViewBag.CategoryName = category.CategoryName;
            ViewBag.CategoryTree = DBManager.GetCategoryTree();
            return View("Create");
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                DBManager.CreateProduct("sakha", product.CategoryId, product.ProductName);
                return RedirectToAction("Create", new { categoryId = product.CategoryId });
            }
            else
            {
                ModelState.AddModelError(String.Empty, "name is required");
                return RedirectToAction("Create", new { categoryId = product.CategoryId });

            }
        }

        [HttpPost]
        public ActionResult Delete(FormCollection collection)
        {
            int categoryId = Convert.ToInt32(collection["inputCategoryId"]);
            int productId = Convert.ToInt32(collection["inputProductId"]);

            DBManager.DeleteProduct(productId);
            return RedirectToAction("Index", new { categoryId = categoryId });
        }
    }
}
