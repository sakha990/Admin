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

        public ActionResult Index(int categoryId, string sortOrder = "productname", int page = 1, int pageSize = 10)
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
         public ActionResult Create(int categoryId,string previousProductName,bool success = false)
        {
            ViewBag.PreviousProductName = previousProductName;
            ViewBag.Success = success;

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
                
                var query = from productList in DBManager.GetProducts(product.CategoryId).AsQueryable<Product>()
                            where productList.ProductName == product.ProductName
                            select productList.ProductName;
                if (query.SingleOrDefault() == product.ProductName)
                {
                    Category category = DBManager.GetCategoryObject(product.CategoryId);
                    ViewBag.CategoryId = product.CategoryId;
                    ViewBag.ParentCategoryName = category.ParentCategoryName;
                    ViewBag.CategoryName = category.CategoryName;
                    ViewBag.CategoryTree = DBManager.GetCategoryTree();
                    ViewBag.Success = false;
                    ModelState.AddModelError("", "Duplicate Product Name");
                    return View("Create", product);

                }
                else
                {     

                    bool success = DBManager.CreateProduct("sakha", product.CategoryId, product.ProductName);
                    return RedirectToAction("Create", new { categoryId = product.CategoryId, previousProductName = product.ProductName,success = success });
                }
            }
            else
            {
                
                Category category = DBManager.GetCategoryObject(product.CategoryId);
                ViewBag.CategoryId = product.CategoryId;
                ViewBag.ParentCategoryName = category.ParentCategoryName;
                ViewBag.CategoryName = category.CategoryName;
                ViewBag.CategoryTree = DBManager.GetCategoryTree();
                ViewBag.Success = false;
                return View("Create",product );
            }
        }

        [HttpPost]
        public JsonResult Delete(Product product)
        {
            bool result = DBManager.DeleteProduct(product.ProductId);
            return Json(result);
        }
    }
}
