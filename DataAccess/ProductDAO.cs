using System.Data;
using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;

namespace DataAccess
{
    public class ProductDAO : BaseDAL
    {

        private static ProductDAO instance = null;
        private static readonly object instanceLock = new object();
        private ProductDAO() { }
        public static ProductDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if(instance == null)
                    {
                        instance = new ProductDAO();
                    }
                    return instance;
                }
            }
        }

        //private static List<ProductObject> ProductList = new List<ProductObject>();

        public IEnumerable<ProductObject> GetProductList()
        {
            IDataReader dataReader = null;
            string SQLSelect = "Select ProductID, CategoryID, ProductName, Weight, UnitPrice, UnitsInStock from Product";
            var products = new List<ProductObject>();
            try
            {
                dataReader = dataProvider.GetDataReader(SQLSelect, CommandType.Text, out connection);
                while (dataReader.Read())
                {
                    products.Add(new ProductObject
                    {
                        ProductID = dataReader.GetInt32(0),
                        CategoryID = dataReader.GetInt32(1),
                        ProductName = dataReader.GetString(2),
                        Weight = dataReader.GetString(3),
                        UnitPrice = dataReader.GetDecimal(4),
                        UnitsInStock = dataReader.GetInt32(5),
                    }); 
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dataReader.Close();
                CloseConnection();
            }
            return products;
        }

        public ProductObject GetProductByID(int productID)
        {
            ProductObject product = null;
            IDataReader dataReader = null;
            string SQLSelect = "Select ProductID, CategoryID, ProductName, Weight, UnitPrice, UnitsInStock " +
                "from Product where ProductID = @ProductID";
            try
            {
                var param = dataProvider.CreateParameter("@ProductID", 4, productID, DbType.Int32);
                dataReader = dataProvider.GetDataReader(SQLSelect, CommandType.Text, out connection, param);
                if (dataReader.Read())
                {
                    product = new ProductObject
                    {
                        ProductID = dataReader.GetInt32(0),
                        CategoryID = dataReader.GetInt32(1),
                        ProductName = dataReader.GetString(2),
                        Weight = dataReader.GetString(3),
                        UnitPrice = dataReader.GetDecimal(4),
                        UnitsInStock = dataReader.GetInt32(5),
                    };
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dataReader.Close();
                CloseConnection();
            }

            return product;
        }

        public void AddNew(ProductObject product)
        {
            try
            {
                ProductObject mo = GetProductByID(product.ProductID);

                if (mo == null)
                {
                    string SQLInsert = "Insert Product values(@CategoryID,@ProductName, @Weight, @UnitPrice, @UnitsInStock)";
                    var parameters = new List<SqlParameter>();
                    parameters.Add(dataProvider.CreateParameter("@ProductName", 100, product.ProductName, DbType.String));
                    parameters.Add(dataProvider.CreateParameter("@CategoryID", 4, product.CategoryID, DbType.Int32));
                    parameters.Add(dataProvider.CreateParameter("@Weight", 100, product.Weight, DbType.String));
                    parameters.Add(dataProvider.CreateParameter("@UnitPrice", 17, product.UnitPrice, DbType.Decimal));
                    parameters.Add(dataProvider.CreateParameter("@UnitsInStock", 4, product.UnitsInStock, DbType.Int32));
                    dataProvider.Insert(SQLInsert, CommandType.Text, parameters.ToArray());
                }
                else
                {
                    throw new Exception("Product is already exists");
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        public void Update(ProductObject product)
        {
            try
            {
                ProductObject mo = GetProductByID(product.ProductID);

                if (mo != null)
                {
                    string SQLUpdate = "Update Product set CategoryID = @CategoryID,ProductName = @ProductName , Weight = @Weight,UnitPrice= @UnitPrice, UnitsInStock= @UnitsInStock where ProductID = @ProductID";
                    var parameters = new List<SqlParameter>();
                    parameters.Add(dataProvider.CreateParameter("@ProductID", 4, product.ProductID, DbType.Int32));
                    parameters.Add(dataProvider.CreateParameter("@ProductName", 100, product.ProductName, DbType.String));
                    parameters.Add(dataProvider.CreateParameter("@CategoryID", 4, product.CategoryID, DbType.Int32));
                    parameters.Add(dataProvider.CreateParameter("@Weight", 100, product.Weight, DbType.String));
                    parameters.Add(dataProvider.CreateParameter("@UnitPrice", 17, product.UnitPrice, DbType.Decimal));
                    parameters.Add(dataProvider.CreateParameter("@UnitsInStock", 4, product.UnitsInStock, DbType.Int32));
                    dataProvider.Update(SQLUpdate, CommandType.Text, parameters.ToArray());
                }
                else
                {
                    throw new Exception("Product is already exists");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        public void Remove(int ProductID)
        {
            try
            {
                ProductObject mo = GetProductByID(ProductID);

                if (mo != null)
                {
                    string SQLDelete = "Delete Product where ProductID = @ProductID)";
                    var param = dataProvider.CreateParameter("@ProductID", 4, ProductID, DbType.Int32);
                    dataProvider.Delete(SQLDelete, CommandType.Text, param);
                }
                else
                {
                    throw new Exception("Product does not already exists");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }//end Remove

    }//end class
}//end namespace

