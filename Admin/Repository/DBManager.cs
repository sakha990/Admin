using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Admin.Models;

namespace Admin.Repository
{
    public static class DBManager
    {
        // Category Section
        public static List<string> GetParerntCategoryNames()
        {
            List<string> parentCategoryNamesList = new List<string>();
            string queryString = @"SELECT parentcategoryName FROM compareadmin.dbo.ParentCategories";
            using (SqlConnection connection = new SqlConnection(
                           ConfigurationManager.ConnectionStrings["MatchConnectionString"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    parentCategoryNamesList.Add(Convert.ToString(reader[0]));
                }

                return parentCategoryNamesList;
            }
        }

        public static List<Category> GetCategories(string parentCategoryName)
        {
            List<Category> categoryList = new List<Category>();
            string queryString = @"SELECT categoryID,categoryName,parentCategoryName,createdBy,createdDate,lastUpdatedBy,lastUpdatedDate FROM compareadmin.dbo.Categories WHERE parentCategoryName = @parentCategoryName ";
            using (SqlConnection connection = new SqlConnection(
                           ConfigurationManager.ConnectionStrings["MatchConnectionString"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add("@parentCategoryName", SqlDbType.NVarChar).Value = parentCategoryName;
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Category category = new Category();
                    category.CategoryId = Convert.ToInt32(reader[0]);
                    category.CategoryName = Convert.ToString(reader[1]);
                    category.ParentCategoryName = Convert.ToString(reader[2]);
                    category.CreatedBy =  Convert.ToString(reader[3]);
                    category.CreatedDate = Convert.ToDateTime(reader[4]);
                    category.LastUpdatedBy = Convert.ToString(reader[5]);
                    if (!((reader[6]) == DBNull.Value))
                    category.LastUpdatedDate = Convert.ToDateTime(reader[6]);
                    categoryList.Add(category);
                }

                return categoryList;
            }
        }

        public static List<CategoryTree> GetCategoryTree()
        {
            List<CategoryTree> categoryTreeList = new List<CategoryTree>();
            foreach (var parentCategoryName in DBManager.GetParerntCategoryNames())
            {
                CategoryTree categoryTree = new CategoryTree();
                categoryTree.ParentCategoryName = parentCategoryName;
                categoryTree.Categories = DBManager.GetCategories(parentCategoryName);
                categoryTreeList.Add(categoryTree);
            }

            return categoryTreeList;

        }

        public static Category GetCategoryObject(int categoryId)
        {
            Category category = new Category();
            string queryString = @"SELECT categoryID,categoryName,parentCategoryName,createdBy,createdDate,lastUpdatedBy,lastUpdatedDate FROM compareadmin.dbo.Categories WHERE categoryID = @categoryID";
            using (SqlConnection connection = new SqlConnection(
                           ConfigurationManager.ConnectionStrings["MatchConnectionString"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add("@categoryID", SqlDbType.Int).Value = categoryId;
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                   {
                       category.CategoryId = Convert.ToInt32(reader[0]);
                       category.CategoryName = Convert.ToString(reader[1]);
                       category.ParentCategoryName = Convert.ToString(reader[2]);
                       category.CreatedBy = Convert.ToString(reader[3]);
                       category.CreatedDate = Convert.ToDateTime(reader[4]);
                       category.LastUpdatedBy = Convert.ToString(reader[5]);
                       if (!((reader[6]) == DBNull.Value))
                       category.LastUpdatedDate = Convert.ToDateTime(reader[6]);

                }
            }
            return category;
        }

        public static List<CategoryParameter> GetCategoryParameters(int categoryId)
        {
            List<CategoryParameter> categoryParameters = new List<CategoryParameter>();
            string queryString = @"select parameterName,parameterValues from CompareAdmin.dbo.CategoryParameters WHERE categoryID = @categoryId";
            using (SqlConnection connection = new SqlConnection(
                           ConfigurationManager.ConnectionStrings["MatchConnectionString"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add("@categoryID", SqlDbType.Int).Value = categoryId;
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    CategoryParameter categoryParamter = new CategoryParameter();
                    categoryParamter.ParameterName = Convert.ToString(reader[0]);
                    categoryParamter.ParameterValues= Convert.ToString(reader[1]);
                    categoryParameters.Add(categoryParamter);
                }
            }
            return categoryParameters;
        }

        public static bool CreateCategory(string category,string parentCategory,string userName)
        {
            string queryString = @"INSERT INTO CompareAdmin.dbo.Categories(categoryName, parentCategoryName, createdBy, createdDate) VALUES(@category, @parentCategory, @createdBy, SYSDATETIME());";
            using (SqlConnection connection = new SqlConnection(
                           ConfigurationManager.ConnectionStrings["MatchConnectionString"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add("@category", SqlDbType.NVarChar).Value = category;
                command.Parameters.Add("@parentCategory", SqlDbType.NVarChar).Value= parentCategory;
                command.Parameters.Add("@createdBy", SqlDbType.VarChar).Value= userName;

                if (command.ExecuteNonQuery() == 1)
                    return true;
                else
                    return false;
            }
        }

        public static bool DeleteCategory(int categoryId)
        {
            string queryString = @"DELETE FROM CompareAdmin.dbo.Categories WHERE categoryID = @categoryID";
            using (SqlConnection connection = new SqlConnection(
                           ConfigurationManager.ConnectionStrings["MatchConnectionString"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add("@categoryID", SqlDbType.Int).Value = categoryId;

                if (command.ExecuteNonQuery() == 1)
                    return true;
                else
                    return false;
            }
        }

        public static bool UpdateCategoryParameter(int categoryId,string userName,CategoryParameter categoryParameter)
        {
            string queryString = @" UPDATE CompareAdmin.dbo.CategoryParameters
                                    SET parameterValues = @parameterValues
                                       ,updatedBy = @updatedBy
                                       ,updatedDate = SYSDATETIME()
                                    WHERE categoryID = @categoryID 
                                    AND parameterName = @parameterName";

            using (SqlConnection connection = new SqlConnection(
                           ConfigurationManager.ConnectionStrings["MatchConnectionString"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add("@parameterValues", SqlDbType.NVarChar).Value = categoryParameter.ParameterValues;
                command.Parameters.Add("@updatedBy", SqlDbType.VarChar).Value = userName;
                command.Parameters.Add("@categoryID", SqlDbType.Int).Value = categoryId;
                command.Parameters.Add("@parameterName", SqlDbType.NVarChar).Value = categoryParameter.ParameterName;

                if (command.ExecuteNonQuery() == 1)
                    return true;
                else
                    return false;
            }
        }

        public static bool DeleteCategoryParameter(int categoryId, string categoryParameterName)
        {
            string queryString = @" DELETE FROM CompareAdmin.dbo.CategoryParameters
                                    WHERE categoryID = @categoryID 
                                    AND   parameterName = @parameterName";

            using (SqlConnection connection = new SqlConnection(
                           ConfigurationManager.ConnectionStrings["MatchConnectionString"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add("@categoryID", SqlDbType.Int).Value = categoryId;
                command.Parameters.Add("@parameterName", SqlDbType.NVarChar).Value = categoryParameterName;

                if (command.ExecuteNonQuery() == 1)
                    return true;
                else
                    return false;
            }
        }
        

        public static bool CreateCategoryParameter(int categoryId, string parameterName, string parameterValues, string userName)
        {
            string queryString = @"INSERT INTO CompareAdmin.dbo.CategoryParameters(categoryID, parameterName,parameterValues,createdBy,createdDate) VALUES(@categoryID, @parameterName,@parameterValues, @createdBy, SYSDATETIME());";
            using (SqlConnection connection = new SqlConnection(
                           ConfigurationManager.ConnectionStrings["MatchConnectionString"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add("@categoryID", SqlDbType.Int).Value = categoryId;
                command.Parameters.Add("@parameterName", SqlDbType.NVarChar).Value = parameterName;
                command.Parameters.Add("@parameterValues", SqlDbType.NVarChar).Value = parameterValues;
                command.Parameters.Add("@createdBy", SqlDbType.VarChar).Value = userName;

                if (command.ExecuteNonQuery() == 1)
                    return true;
                else
                    return false;
            }
        }

        public static string GetCategoryParameterValues(int categoryId, string parameterName)
        {
            string parameterValue = string.Empty;
            string queryString = @"SELECT parameterValues FROM CompareAdmin.dbo.CategoryParameters WHERE categoryID = @categoryID AND parameterName = @parameterName";
            using (SqlConnection connection = new SqlConnection(
                           ConfigurationManager.ConnectionStrings["MatchConnectionString"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add("@categoryID", SqlDbType.Int).Value = categoryId;
                command.Parameters.Add("@parameterName", SqlDbType.NVarChar).Value = parameterName;
                parameterValue = Convert.ToString(command.ExecuteScalar());
            }
            return parameterValue;
        }

        // Product Section 
        public static double GetProductsCount(int categoryId)
        {
            double _productsCount = 0;
            string queryString = @"SELECT COUNT(*) 
                                   FROM ( SELECT productID,productName FROM CompareAdmin.dbo.Products WHERE categoryID = "+ categoryId + ") p1";

            using (SqlConnection connection = new SqlConnection(
                           ConfigurationManager.ConnectionStrings["MatchConnectionString"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                _productsCount = Convert.ToDouble(command.ExecuteScalar());
            }
            return _productsCount;
        }

        public static bool CreateProduct(string userName,int categoryId,string productName)
        {
            string queryString = @"INSERT INTO dbo.Products(productName,categoryID,createdBy,createdDate)
                                                    VALUES(@productName,@categoryID,@createdBy,SYSDATETIME())";
            using (SqlConnection connection = new SqlConnection(
                           ConfigurationManager.ConnectionStrings["MatchConnectionString"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add("@productName", SqlDbType.NVarChar).Value = productName;
                command.Parameters.Add("@categoryID", SqlDbType.Int).Value = categoryId;
                command.Parameters.Add("@createdBy", SqlDbType.VarChar).Value = userName;
                if (command.ExecuteNonQuery() == 1)
                    return true;
                else
                    return false;
            }
        }
        public static List<Product> GetProducts(int categoryId)
        {
            List<Product> productList = new List<Product>();
            string queryString = @"SELECT productID,productName,categoryID,createdBy,createdDate,lastUpdatedBy,lastUpdatedDate  FROM dbo.Products where categoryId = @categoryId";
            using (SqlConnection connection = new SqlConnection(
                           ConfigurationManager.ConnectionStrings["MatchConnectionString"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add(new SqlParameter("@categoryId", categoryId));
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Product product = new Product();
                    product.ProductId = Convert.ToInt32(reader[0]);
                    product.ProductName = Convert.ToString(reader[1]);
                    product.CategoryId  = Convert.ToInt32(reader[2]);
                    product.CreatedBy = Convert.ToString(reader[3]);
                    product.CreatedDate = Convert.ToDateTime(reader[4]);
                    product.LastUpdatedBy = Convert.ToString(reader[5]);
                    if (!((reader[6]) == DBNull.Value))
                        product.LastUpdatedDate = Convert.ToDateTime(reader[6]);

                    productList.Add(product);
                }
                return productList;
            }
        }

        public static bool DeleteProduct(int productId)
        {
            string queryString = @"DELETE FROM CompareAdmin.dbo.Products WHERE productID = @productID";
            using (SqlConnection connection = new SqlConnection(
                           ConfigurationManager.ConnectionStrings["MatchConnectionString"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add("@productID", SqlDbType.Int).Value = productId;

                if (command.ExecuteNonQuery() == 1)
                    return true;
                else
                    return false;
            }
        }


        /*public List<Product> GetAllProducts()
        {
            List<Product> productList = new List<Product>();
            string queryString = @"SELECT TOP 100 vendorName,t1.productName,price  
				                  from compare.dbo.products t1,compare.dbo.productdetails t2,Compare.dbo.Vendors t3 
				                  where t1.productID = t2.productID
				                  AND t2.vendorID = t3.vendorID";

            using (SqlConnection connection = new SqlConnection(
                           ConfigurationManager.ConnectionStrings["MatchConnectionString"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Product product = new Product();
                    product.vendorName = Convert.ToString(reader[0]);
                    product.productName = Convert.ToString(reader[1]);
                    product.price = Convert.ToDecimal(reader[2]);
                    productList.Add(product);
                }

                return productList;

            }
        }*/

        public static int CountVendorsByProduct(string productID)
        {
            int rowCount = 0;
            using (SqlConnection connection = new SqlConnection(
                           ConfigurationManager.ConnectionStrings["MatchConnectionString"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("[Compare].[dbo].[CountVendorsByProduct]", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@productID", productID));

                rowCount = Convert.ToInt32(command.ExecuteScalar());
                return rowCount;

            }
        }
        public static List<Product> GetVendorsByProduct(string productID, string pageSize, string pageNumber)
        {
            List<Product> productList = new List<Product>();
            using (SqlConnection connection = new SqlConnection(
                           ConfigurationManager.ConnectionStrings["MatchConnectionString"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("[Compare].[dbo].[GetVendorsByProduct]", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@productID", productID));
                command.Parameters.Add(new SqlParameter("@pageSize", pageSize));
                command.Parameters.Add(new SqlParameter("@pageNumber", pageNumber));

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Product product = new Product();
                    product.ProductId = Convert.ToInt32(reader[0]);
                    product.ProductName = Convert.ToString(reader[1]);
                    //product.VendorName = Convert.ToString(reader[2]);
                    //product.Price = Convert.ToDecimal(reader[3]);
                    //product.ProductAttributes = Convert.ToString(reader[4]);

                    productList.Add(product);
                }

                return productList;


            }
        }

        public static int GetMatchedProductsCount(string strSearch)
        {
            int noOfProducts = 0;
            string queryString = @"select COUNT(*) FROM (
                                  select vendorName,t1.productName,min(price) price 
				                  from compare.dbo.products t1,compare.dbo.productdetails t2,Compare.dbo.Vendors t3 
				                  where t1.productID = t2.productID
				                  AND t2.vendorID = t3.vendorID
				                  AND t1.productName like '%" + strSearch + @"%'
				                  group by vendorName,t1.productName) t1 ";

            using (SqlConnection connection = new SqlConnection(
                           ConfigurationManager.ConnectionStrings["MatchConnectionString"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                noOfProducts = Convert.ToInt32(command.ExecuteScalar());
            }
            return noOfProducts;
        }

        public static List<Product> GetProductsByPage(string strSearch, int pageNumber)
        {
            List<Product> productList = new List<Product>();
            using (SqlConnection connection = new SqlConnection(
                           ConfigurationManager.ConnectionStrings["MatchConnectionString"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("[Compare].[dbo].[SearchProduct]", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@searchTerm", strSearch));
                command.Parameters.Add(new SqlParameter("@pageNumber", pageNumber));

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Product product = new Product();
                    //product.VendorName = Convert.ToString(reader[0]);
                    //product.ProductName = Convert.ToString(reader[1]);
                    //product.Price = Convert.ToDecimal(reader[2]);
                    productList.Add(product);
                }

                return productList;


            }
        }


        // Search Section         

        public static List<Category> SearchInCategory(string strSearch)
        {
            List<Category> categoryList = new List<Category>();

            string queryString = @"SELECT DISTINCT categoryID,categoryName 
                                   FROM Compare.dbo.Categories 
                                   WHERE categoryName like '%" + strSearch + @"%'";

            using (SqlConnection connection = new SqlConnection(
                           ConfigurationManager.ConnectionStrings["MatchConnectionString"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Category category = new Category();
                    category.CategoryId = Convert.ToInt32(reader[0]);
                    category.CategoryName = Convert.ToString(reader[1]);
                    categoryList.Add(category);
                }

                return categoryList;

            }


        }
    }
}