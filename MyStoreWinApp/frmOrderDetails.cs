using BusinessObject;
using DataAccess;
using DataAccess.Repository;
using System;
using System.Windows.Forms;

namespace SalesWinApp
{
    public partial class frmOrderDetails : Form
    {
        public frmOrderDetails()
        {
            InitializeComponent();
        }

        public IOrderRepository OrderRepository { get; set; }
        public bool InsertOrUpdate { get; set; }
        public OrderObject orderInfo { get; set; }

        public MemberObject Member { get; set; }

        private void frmOrderDetails_Load(object sender, EventArgs e)
        {
            if (InsertOrUpdate == true)
            {
                dtpOrderDate.Text = orderInfo.OrderDate.ToString();
                dtpRequiredDate.Text = orderInfo.RequiredDate.ToString();
                dtpShippedDate.Text = orderInfo.ShippedDate.ToString();
                intFreight.Text = orderInfo.Freight.ToString();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int memberID = Member.MemberID;
                OrderObject order = new OrderObject
                {
                    MemberID = memberID,
                    OrderDate = dtpOrderDate.Value,
                    RequiredDate = dtpRequiredDate.Value,
                    ShippedDate = dtpShippedDate.Value,
                    Freight = intFreight.Value
                };
                if (InsertOrUpdate == false)
                {
                    OrderRepository.InsertOrder(order);
                    this.DialogResult = DialogResult.OK;
                    MessageBox.Show("Order has been created Successfully!");
                    Close();
                }
                else
                {
                    OrderRepository.UpdateOrder(order);
                    this.DialogResult = DialogResult.OK;
                    MessageBox.Show("Order has been updated Successfully!");
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, InsertOrUpdate == false ? "Add a new order" : "Update a order");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) => Close();
    }
}