using System.Data;
using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;

namespace DataAccess
{
    public class OrderDetailDAO : BaseDAL
    {

        private static OrderDetailDAO instance = null;
        private static readonly object instanceLock = new object();
        private OrderDetailDAO() { }
        public static OrderDetailDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if(instance == null)
                    {
                        instance = new OrderDetailDAO();
                    }
                    return instance;
                }
            }
        }

        //private static List<OrderDetailObject> OrderDetailList = new List<OrderDetailObject>();

        public IEnumerable<OrderDetailObject> GetOrderDetailList()
        {
            IDataReader dataReader = null;
            string SQLSelect = "Select OrderID, ProductID, UnitPrice, Quantity, Discount from OrderDetail";
            var orderDetails = new List<OrderDetailObject>();
            try
            {
                dataReader = dataProvider.GetDataReader(SQLSelect, CommandType.Text, out connection);
                while (dataReader.Read())
                {
                    orderDetails.Add(new OrderDetailObject
                    {
                        OrderID = dataReader.GetInt32(0),
                        ProductID = dataReader.GetInt32(1),
                        UnitPrice = dataReader.GetDecimal(2),
                        Quantity = dataReader.GetInt32(3),
                        Discount = dataReader.GetDouble(4)
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
            return orderDetails;
        }
        public IEnumerable<OrderDetailObject> GetOrderDetailListByOrderID(int OrderID)
        {
            IDataReader dataReader = null;
            string SQLSelect = "Select OrderID, ProductID, UnitPrice, Quantity, Discount from OrderDetail " +
                "Where OrderID=@OrderID";
            var orderDetails = new List<OrderDetailObject>();
            try
            {
                var param = dataProvider.CreateParameter("@OrderID", 4, OrderID, DbType.Int32);
                dataReader = dataProvider.GetDataReader(SQLSelect, CommandType.Text, out connection, param);
                while (dataReader.Read())
                {
                    orderDetails.Add(new OrderDetailObject
                    {
                        OrderID = dataReader.GetInt32(0),
                        ProductID = dataReader.GetInt32(1),
                        UnitPrice = dataReader.GetDecimal(2),
                        Quantity = dataReader.GetInt32(3),
                        Discount = dataReader.GetDouble(4)
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
            return orderDetails;
        }

        public OrderDetailObject GetOrderDetailByID(int OrderID)
        {
            OrderDetailObject orderDetail = null;
            IDataReader dataReader = null;
            string SQLSelect = "Select OrderID, ProductID, UnitPrice, Quantity, Discount " +
                "from OrderDetail where OrderID = @OrderID";
            try
            {
                var param = dataProvider.CreateParameter("@OrderID", 4, OrderID, DbType.Int32);
                dataReader = dataProvider.GetDataReader(SQLSelect, CommandType.Text, out connection, param);
                if (dataReader.Read())
                {
                    orderDetail = new OrderDetailObject
                    {
                        OrderID = dataReader.GetInt32(0),
                        ProductID = dataReader.GetInt32(1),
                        UnitPrice = dataReader.GetDecimal(2),
                        Quantity = dataReader.GetInt32(3),
                        Discount = dataReader.GetDouble(4),
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

            return orderDetail;
        }
        public OrderDetailObject GetOrderDetailByProductID(int ProductID)
        {
            OrderDetailObject orderDetail = null;
            IDataReader dataReader = null;
            string SQLSelect = "Select OrderID, ProductID, UnitPrice, Quantity, Discount " +
                "from OrderDetail where ProductID = @ProductID";
            try
            {
                var param = dataProvider.CreateParameter("@ProductID", 4, ProductID, DbType.Int32);
                dataReader = dataProvider.GetDataReader(SQLSelect, CommandType.Text, out connection, param);
                if (dataReader.Read())
                {
                    orderDetail = new OrderDetailObject
                    {
                        OrderID = dataReader.GetInt32(0),
                        ProductID = dataReader.GetInt32(1),
                        UnitPrice = dataReader.GetDecimal(2),
                        Quantity = dataReader.GetInt32(3),
                        Discount = dataReader.GetDouble(4),
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

            return orderDetail;
        }

        public void AddNew(OrderDetailObject orderDetail)
        {
            try
            {
                OrderDetailObject mo = GetOrderDetailByID(orderDetail.OrderID);
                if (mo==null ||mo.ProductID != orderDetail.ProductID)
                {
                    string SQLInsert = "Insert OrderDetail values(@OrderID, @ProductID, @UnitPrice, @Quantity, @Discount)";
                    var parameters = new List<SqlParameter>();
                    parameters.Add(dataProvider.CreateParameter("@OrderID", 4, orderDetail.OrderID, DbType.Int32));
                    parameters.Add(dataProvider.CreateParameter("@UnitPrice", 17, orderDetail.UnitPrice, DbType.Decimal));
                    parameters.Add(dataProvider.CreateParameter("@ProductID", 4, orderDetail.ProductID, DbType.Int32));
                    parameters.Add(dataProvider.CreateParameter("@Quantity", 4, orderDetail.Quantity, DbType.Int32));
                    parameters.Add(dataProvider.CreateParameter("@Discount", 8, orderDetail.Discount, DbType.Double));
                    dataProvider.Insert(SQLInsert, CommandType.Text, parameters.ToArray());
                }
                else
                {
                    throw new Exception("Order Detail: product already exists! please update instead.");
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

        public void Update(OrderDetailObject orderDetail)
        {
            try
            {
                OrderDetailObject mo = GetOrderDetailByID(orderDetail.OrderID);

                if (mo != null)
                {
                    string SQLUpdate = "Update OrderDetail set ProductID = @ProductID,UnitPrice = @UnitPrice , Quantity = @Quantity,Discount= @Discount where OrderID = @OrderID)";
                    var parameters = new List<SqlParameter>();
                    parameters.Add(dataProvider.CreateParameter("@OrderID", 4, orderDetail.OrderID, DbType.Int32));
                    parameters.Add(dataProvider.CreateParameter("@UnitPrice", 17, orderDetail.UnitPrice, DbType.Decimal));
                    parameters.Add(dataProvider.CreateParameter("@ProductID", 4, orderDetail.ProductID, DbType.Int32));
                    parameters.Add(dataProvider.CreateParameter("@Quantity", 4, orderDetail.Quantity, DbType.Int32));
                    parameters.Add(dataProvider.CreateParameter("@Discount", 8, orderDetail.Discount, DbType.Double));
                    dataProvider.Update(SQLUpdate, CommandType.Text, parameters.ToArray());
                }
                else
                {
                    throw new Exception("OrderDetail is already exists");
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

        public void Remove(int OrderID)
        {
            try
            {
                OrderDetailObject mo = GetOrderDetailByID(OrderID);

                if (mo != null)
                {
                    string SQLDelete = "Delete OrderDetail where OrderID = @OrderID)";
                    var param = dataProvider.CreateParameter("@OrderID", 4, OrderID, DbType.Int32);
                    dataProvider.Delete(SQLDelete, CommandType.Text, param);
                }
                else
                {
                    throw new Exception("OrderDetail does not already exists");
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

