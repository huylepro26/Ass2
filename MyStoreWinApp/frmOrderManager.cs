using BusinessObject;
using DataAccess;
using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesWinApp
{
    public partial class frmOrderManager : Form
    {
        IOrderRepository OrderRepository = new OrderRepository();
        IOrderDetailRepository OrderDetailRepository = new OrderDetailRepository();

        BindingSource source;

        public MemberObject Member { get; set; }

        public OrderObject Order { get; set; }
        public frmOrderManager()
        {
            InitializeComponent();
        }
        public frmOrderManager(OrderObject order)
        {
            Order = order;
            InitializeComponent();
        }

        private OrderObject GetOrderObject()
        {
            return (OrderObject)dgvOrderList.CurrentRow.DataBoundItem;
        }

        public void LoadOrderList()
        {
            var orders = OrderRepository.GetOrders();
            try
            {
                source = new BindingSource();
                source.DataSource = orders;
                dgvOrderList.DataSource = null;
                dgvOrderList.DataSource = source;

                if (orders.Count() == 0)
                {
                    btnDelete.Enabled = false;
                }
                else
                {
                    btnDelete.Enabled = true;
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Load order list");
            }
        }


        private void frmOrderManager_Load(object sender, EventArgs e)
        {
        }

        private void dgvOrderList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            frmOrderDetails frmOrderDetails = new frmOrderDetails
            {
                Text = "Update order",
                InsertOrUpdate = true,
                orderInfo = GetOrderObject(),
                OrderRepository = OrderRepository,
                Member = Member
            };
            if (frmOrderDetails.ShowDialog().Equals(DialogResult.OK))
            {
                LoadOrderList();
                source.Position = source.Count - 1;
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadOrderList();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            frmOrderDetails frmOrderDetails = new frmOrderDetails
            {
                Text = "Add order",
                InsertOrUpdate = false,
                OrderRepository = OrderRepository,
                Member = Member
            };
            if (frmOrderDetails.ShowDialog().Equals(DialogResult.OK))
            {
                LoadOrderList();
                source.Position = source.Count - 1;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var order = GetOrderObject();
                var orderDetail = OrderDetailRepository.GetOrderDetailByOrderID(order.OrderID);
                if (orderDetail != null)
                {
                    MessageBox.Show("Can not delete this order because this order is contained in one or more order detail!");
                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show("Do you want to delete Order: " + order.OrderID, "Confirmation", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        OrderRepository.DeleteOrder(order.OrderID);
                        if (order == null)
                        {
                            MessageBox.Show("Order has been deleted successfully!");
                        }
                        LoadOrderList();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Delete a order");
            }

        }

        private void btnClose_Click(object sender, EventArgs e) => Close();

        private void btnSort_Click(object sender, EventArgs e)
        {
            var orders = OrderRepository.GetOrders().OrderByDescending(m => m.OrderDate);
            try
            {
                source = new BindingSource();

                source.DataSource = orders;

                dgvOrderList.DataSource = null;
                dgvOrderList.DataSource = source;
                if (orders.Count() == 0)
                {
                    btnDelete.Enabled = false;
                }
                else
                {
                    btnDelete.Enabled = true;
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Load order list");
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int OrderID = 0;
            int MemberID = 0;
            int temp;
            try
            {
                if (int.TryParse(txtOrderID.Text, out temp))
                {
                    OrderID = temp;
                }
                if (int.TryParse(txtMemberID.Text, out temp))
                {
                    MemberID = temp;
                }
            }
            catch (Exception)
            {

            }
            var members = OrderRepository.GetOrders().Where(OrderID != 0 && MemberID != 0 ?
               (m => m.OrderID.Equals(OrderID) && m.MemberID.Equals(MemberID))
               : m => m.OrderID.Equals(OrderID) || m.MemberID.Equals(MemberID));

            try
            {
                source = new BindingSource();
                source.DataSource = members;
                dgvOrderList.DataSource = null;
                dgvOrderList.DataSource = source;

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Load order list");
            }
        }

        private void frmOrderManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            frmMain.FormShowed[2] = "Closed";
            frmMain.omStrip.BackColor = SystemColors.Control;
        }

    }
}
