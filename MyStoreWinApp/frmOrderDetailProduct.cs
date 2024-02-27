using BusinessObject;
using DataAccess;
using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesWinApp
{
    public partial class frmOrderDetailProduct : Form
    {

        IProductRepository productRepository = new ProductRepository();
        IOrderRepository orderRepository = new OrderRepository();
        IOrderDetailRepository orderDetailRepository = new OrderDetailRepository();
        BindingSource source;

        public bool InsertOrUpdate { get; set; }
        public ProductObject PO { get; set; }
        public OrderObject OO { get; set; }
        public OrderDetailObject orderDetailInfo { get; set; }
       



        public frmOrderDetailProduct()
        {
            InitializeComponent();
        }  
        

        private void frmOrderDetailProduct_Load(object sender, EventArgs e)
        {
            
            //When update disable dgv
            if (InsertOrUpdate == true)
            {
                intQuantity.Focus();
                btnAdd.Text = "Update";
                txtOrderID.Text = orderDetailInfo.OrderID.ToString();
                txtProductName.Text = orderDetailInfo.ProductID.ToString();
                dgvOrderID.Enabled = false;
                dgvOrderProductList.Enabled = false;
            }
            else
            {
                LoadDetailProductList();
                dgvOrderID.ClearSelection();
                dgvOrderProductList.ClearSelection();
            }
        }

        private ProductObject GetProductObject()
        {
            return (ProductObject)dgvOrderProductList.CurrentRow.DataBoundItem;
        }

        private OrderObject GetOrderObject()
        {
            return (OrderObject)dgvOrderID.CurrentRow.DataBoundItem;
        }

        private void dgvOrderProductList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            PO = GetProductObject();
            txtProductName.Text = PO.ProductName;
        }

        private void dgvOrderID_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            OO = GetOrderObject();
            txtOrderID.Text = OO.OrderID.ToString();
        }

        public void LoadDetailProductList()
        {
            var products = productRepository.GetProducts();
            try
            {
                source = new BindingSource();
                source.DataSource = products;
                dgvOrderProductList.DataSource = null;
                dgvOrderProductList.DataSource = source;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Load product list");
            }

            var orderList = orderRepository.GetOrders();
            try
            {
               
                source = new BindingSource();
                source.DataSource = orderList;
                dgvOrderID.DataSource = null;
                dgvOrderID.DataSource = source;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Load orderID list");
            }
        }


        private void btnClose_Click(object sender, EventArgs e) => Close();

        

        public void ReloadODList(frmOrderDetailList frmOrderDetailList)
        {
            frmOrderDetailList.LoadOrderDetailList();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (InsertOrUpdate != true)
            {
                try
                {
                    var orderDetail = new OrderDetailObject
                    {
                        OrderID = int.Parse(txtOrderID.Text),
                        ProductID = PO.ProductID,
                        UnitPrice = PO.UnitPrice,
                        Quantity = Convert.ToInt32(intQuantity.Value),
                        Discount = (double)decimalDiscount.Value
                    };
                    orderDetailRepository.InsertOrderDetail(orderDetail);
                    MessageBox.Show("Order Detail has been added Successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Add an Order Detail");
                }
            }
            else
            {
                try
                {
                    var orderDetail = new OrderDetailObject
                    {
                        OrderID = int.Parse(txtOrderID.Text),
                        ProductID = PO.ProductID,
                        UnitPrice = PO.UnitPrice,
                        Quantity = Convert.ToInt32(intQuantity.Value),
                        Discount = (double)decimalDiscount.Value
                    };
                    orderDetailRepository.UpdateOrderDetail(orderDetail);
                    MessageBox.Show("Order Detail has been updated Successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Update an Order Detail");
                }
            }
        }

        

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadDetailProductList();
        }

        
    }
}
