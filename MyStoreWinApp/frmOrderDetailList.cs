using BusinessObject;
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
    public partial class frmOrderDetailList : Form
    {
        IOrderDetailRepository OrderDetailRepository = new OrderDetailRepository();

        BindingSource source;

        public int OrderID;



        public MemberObject Member { get; set; }

        public frmOrderDetailList()
        {
            InitializeComponent();
        }

        private void dgvOrderDetailList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            frmOrderDetailProduct frmProductDetails = new frmOrderDetailProduct
            {
                Text = "Update Order Detail",
                InsertOrUpdate = true,
                orderDetailInfo = GetOrderDetailObject()
            };
            if (frmProductDetails.ShowDialog().Equals(DialogResult.OK))
            {
                LoadOrderDetailList();
                source.Position = source.Count - 1;
            }
        }

        private OrderDetailObject GetOrderDetailObject()
        {
            return (OrderDetailObject)dgvOrderDetailList.CurrentRow.DataBoundItem;
        }

        public void LoadOrderDetailList()
        {
            var orderDetails = OrderDetailRepository.GetOrderDetails();
            try
            {
                source = new BindingSource();
                source.DataSource = orderDetails;
                dgvOrderDetailList.DataSource = null;
                dgvOrderDetailList.DataSource = source;

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Load member list");
            }
        }



        private void btnAdd_Click(object sender, EventArgs e)
        {

            frmOrderDetailProduct frmOrderDetailProduct = new frmOrderDetailProduct
            {
                Text = "Add Order Detail",
                InsertOrUpdate = false
            };
            if (frmOrderDetailProduct.ShowDialog().Equals(DialogResult.OK))
            {
                LoadOrderDetailList();
                source.Position = source.Count - 1;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var orderDetail = GetOrderDetailObject();
                DialogResult dialogResult = MessageBox.Show("Do you want to delete this Order Detail: ", "Confirmation", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    OrderDetailRepository.DeleteOrderDetail(orderDetail.OrderID);
                    if (orderDetail == null)
                    {
                        MessageBox.Show("Order Detail has been deleted successfully!");
                    }
                    LoadOrderDetailList();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Delete a order");
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadOrderDetailList();
        }

        private void btnClose_Click(object sender, EventArgs e) => this.Close();

        private void frmOrderDetailList_FormClosing(object sender, FormClosingEventArgs e)
        {
            frmMain.FormShowed[3] = "Closed";
            frmMain.odStrip.BackColor = SystemColors.Control;
        }
    }
}
