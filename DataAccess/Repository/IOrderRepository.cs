
using BusinessObject;
using System;
using System.Collections.Generic;

namespace DataAccess.Repository
{
    public interface IOrderRepository
    {
        IEnumerable<OrderObject> GetOrders();
        List<Object> GetOrderListTotalByID(DateTime StartDate, DateTime EndDate);
        List<OrderObject> GetOrderByMemberID(int memberID);
        OrderObject GetOrderByID(int orderID);
        void InsertOrder(OrderObject order);
        void DeleteOrder(int orderID);
        void UpdateOrder(OrderObject order);
    }
}
