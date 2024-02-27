
using BusinessObject;
using System.Collections.Generic;

namespace DataAccess.Repository
{
    public interface IOrderDetailRepository
    {
        IEnumerable<OrderDetailObject> GetOrderDetails();
        IEnumerable<OrderDetailObject> GetOrderDetailsByOrderID(int orderID);
        OrderDetailObject GetOrderDetailByOrderID(int orderDetail);
        OrderDetailObject GetOrderDetailByProductID(int productID);
        void InsertOrderDetail(OrderDetailObject orderDetail);
        void DeleteOrderDetail(int orderDetailID);
        void UpdateOrderDetail(OrderDetailObject orderDetail);
    }
}
