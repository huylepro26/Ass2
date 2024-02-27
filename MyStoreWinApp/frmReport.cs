using BusinessObject;
using DataAccess;
using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SalesWinApp
{
    public partial class frmReport : Form
    {

        IOrderRepository OrderRepository = new OrderRepository();

        IOrderDetailRepository OrderDetailRepository = new OrderDetailRepository();

        BindingSource source;


        public frmReport()
        {
            InitializeComponent();
        }

        public void LoadOrderDetailList()
        {
           
            try
            {
                var ordersTotal = OrderRepository.GetOrderListTotalByID(dtpStart.Value, dtpEnd.Value);
                Trace.WriteLine(ordersTotal.ToString());
                source = new BindingSource();
                source.DataSource = ordersTotal;
                dgvOrderDetailList.DataSource = null;
                dgvOrderDetailList.DataSource = source;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Load member list");
            }
        }


        private void btnReport_Click(object sender, EventArgs e)
        {
            LoadOrderDetailList();

        }


        private void btnClose_Click(object sender, EventArgs e) => this.Close();

        private void frmReport_FormClosing(object sender, FormClosingEventArgs e)
        {
            frmMain.FormShowed[5] = "Closed";
            frmMain.mmStrip.BackColor = SystemColors.Control;
        }
    }
}
