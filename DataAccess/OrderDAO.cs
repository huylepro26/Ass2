using System.Data;
using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Diagnostics;

namespace DataAccess
{
    public class OrderDAO : BaseDAL
    {

        private static OrderDAO instance = null;
        private static readonly object instanceLock = new object();
        private OrderDAO() { }
        public static OrderDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new OrderDAO();
                    }
                    return instance;
                }
            }
        }

        //private static List<OrderObject> OrderList = new List<OrderObject>();

        public IEnumerable<OrderObject> GetOrderList()
        {
            IDataReader dataReader = null;
            string SQLSelect = "SELECT OrderID, MemberID, OrderDate, RequiredDate, ShippedDate, Freight FROM [Orders]";
            var orders = new List<OrderObject>();
            try
            {
                dataReader = dataProvider.GetDataReader(SQLSelect, CommandType.Text, out connection);
                while (dataReader.Read())
                {

                    orders.Add(new OrderObject
                    {
                        OrderID = dataReader.GetInt32(0),
                        MemberID = dataReader.GetInt32(1),
                        OrderDate = dataReader.GetDateTime(2),
                        RequiredDate = dataReader.GetDateTime(3),
                        ShippedDate = dataReader.GetDateTime(4),
                        Freight = dataReader.GetDecimal(5)
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dataReader.Close();
                CloseConnection();
            }
            return orders;
        }
        public List<OrderObject> GetOrderByMemberID(int MemberID)
        {
            IDataReader dataReader = null;
            List<OrderObject> OrderList = new List<OrderObject>();
            string SQLSelect = "SELECT OrderID, MemberID, OrderDate, RequiredDate, ShippedDate, Freight " +
                "FROM [Orders] WHERE MemberID=@MemberID";
            
            try
            {
                var param = dataProvider.CreateParameter("@MemberID", 4, MemberID, DbType.Int32);
                dataReader = dataProvider.GetDataReader(SQLSelect, CommandType.Text, out connection, param);
               while (dataReader.Read())
                {
                    OrderList.Add(new OrderObject
                    {
                        OrderID = dataReader.GetInt32(0),
                        MemberID = dataReader.GetInt32(1),
                        OrderDate = dataReader.GetDateTime(2),
                        RequiredDate = dataReader.GetDateTime(3),
                        ShippedDate = dataReader.GetDateTime(4),
                        Freight = dataReader.GetDecimal(5)
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dataReader.Close();
                CloseConnection();
            }
            return OrderList;
        }

        public OrderObject GetOrderByID(int orderID)
        {
            OrderObject order = null;
            IDataReader dataReader = null;
            string SQLSelect = "Select OrderID, MemberID, OrderDate, RequiredDate, ShippedDate, Freight " +
                "from [Order] where OrderID = @OrderID";
            try
            {
                var param = dataProvider.CreateParameter("@OrderID", 4, orderID, DbType.Int32);
                dataReader = dataProvider.GetDataReader(SQLSelect, CommandType.Text, out connection, param);
                if (dataReader.Read())
                {
                    order = new OrderObject
                    {
                        OrderID = dataReader.GetInt32(0),
                        MemberID = dataReader.GetInt32(1),
                        OrderDate = dataReader.GetDateTime(2),
                        RequiredDate = dataReader.GetDateTime(3),
                        ShippedDate = dataReader.GetDateTime(4),
                        Freight = dataReader.GetDecimal(5)
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dataReader.Close();
                CloseConnection();
            }

            return order;
        }
        public List<Object> GetOrderListTotalByID(DateTime StartDate, DateTime EndDate)
        {
            List<Object> OrderList = new List<Object>();
            IDataReader dataReader = null;
            string SQLSelect = "SELECT o.OrderID,o.MemberID,o.OrderDate,o.RequiredDate,o.ShippedDate,o.Freight, SUM(od.Quantity*od.UnitPrice - od.UnitPrice*od.Discount) AS Total FROM [Orders] o " +
                                "JOIN OrderDetail od ON od.OrderId = o.OrderId and OrderDate >= @StartDate AND OrderDate <= @EndDate " +
                                "GROUP BY o.OrderId,o.MemberID,o.OrderDate,o.RequiredDate,o.ShippedDate,o.Freight ORDER BY TOTAL DESC";
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(dataProvider.CreateParameter("@StartDate", 8, StartDate, DbType.DateTime));
                parameters.Add(dataProvider.CreateParameter("@EndDate", 8, EndDate, DbType.DateTime));
                dataReader = dataProvider.GetDataReader(SQLSelect, CommandType.Text, out connection, parameters.ToArray());
                while (dataReader.Read())
                {
                    OrderList.Add(new
                    {
                        OrderID = dataReader.GetInt32(0),
                        MemberID = dataReader.GetInt32(1),
                        OrderDate = dataReader.GetDateTime(2),
                        RequiredDate = dataReader.GetDateTime(3),
                        ShippedDate = dataReader.GetDateTime(4),
                        Freight = dataReader.GetDecimal(5),
                        Total = dataReader.GetDouble(6)
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dataReader.Close();
                CloseConnection();
            }

            return OrderList;
        }

        public void AddNew(OrderObject order)
        {
            try
            {
                OrderObject mo = GetOrderByID(order.OrderID);

                if (mo == null)
                {
                    string SQLInsert = "Insert [Order] values(@MemberID,@OrderDate,@RequiredDate,@ShippedDate,@Freight)";
                    var parameters = new List<SqlParameter>();

                    parameters.Add(dataProvider.CreateParameter("@MemberID", 4, order.MemberID, DbType.Int32));
                    parameters.Add(dataProvider.CreateParameter("@OrderDate", 8, order.OrderDate, DbType.DateTime));
                    parameters.Add(dataProvider.CreateParameter("@RequiredDate", 8, order.RequiredDate, DbType.DateTime));
                    parameters.Add(dataProvider.CreateParameter("@ShippedDate", 8, order.ShippedDate, DbType.DateTime));
                    parameters.Add(dataProvider.CreateParameter("@Freight", 17, order.Freight, DbType.Decimal));
                    dataProvider.Insert(SQLInsert, CommandType.Text, parameters.ToArray());
                }
                else
                {
                    throw new Exception("Order is already exists");
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

        public void Update(OrderObject order)
        {
            try
            {
                OrderObject mo = GetOrderByID(order.OrderID);

                if (mo != null)
                {
                    string SQLUpdate = "Update [Order] set OrderDate = @OrderDate , RequiredDate = @RequiredDate, ShippedDate = @ShippedDate ,Freight= @Freight where OrderID = @OrderID";
                    var parameters = new List<SqlParameter>();
                    parameters.Add(dataProvider.CreateParameter("@OrderID", 4, order.OrderID, DbType.Int32));
                    parameters.Add(dataProvider.CreateParameter("@OrderDate", 8, order.OrderDate, DbType.DateTime));
                    //parameters.Add(dataProvider.CreateParameter("@MemberID", 4, order.MemberID, DbType.Int32));
                    parameters.Add(dataProvider.CreateParameter("@RequiredDate", 8, order.RequiredDate, DbType.DateTime));
                    parameters.Add(dataProvider.CreateParameter("@ShippedDate", 8, order.ShippedDate, DbType.DateTime));
                    parameters.Add(dataProvider.CreateParameter("@Freight", 17, order.Freight, DbType.Decimal));
                    dataProvider.Update(SQLUpdate, CommandType.Text, parameters.ToArray());
                }
                else
                {
                    throw new Exception("Order is already exists");
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
                OrderObject mo = GetOrderByID(OrderID);

                if (mo != null)
                {
                    string SQLDelete = "Delete from [Order] where OrderID = @OrderID";
                    var param = dataProvider.CreateParameter("@OrderID", 4, OrderID, DbType.Int32);
                    dataProvider.Delete(SQLDelete, CommandType.Text, param);
                }
                else
                {
                    throw new Exception("Order does not exists");
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

