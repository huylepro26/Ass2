using BusinessObject;
using DataAccess;
using DataAccess.Repository;
using System;
using System.Windows.Forms;

namespace SalesWinApp
{
    public partial class frmProductDetails : Form
    {
        public frmProductDetails()
        {
            InitializeComponent();
        }

        public IProductRepository ProductRepository { get; set; }
        public bool InsertOrUpdate { get; set; }
        public ProductObject productInfo { get; set; }


        private void frmProductDetails_Load(object sender, EventArgs e)
        {
            if (InsertOrUpdate == true)
            {
                txtCategoryID.Text = productInfo.CategoryID.ToString();
                txtProductName.Text = productInfo.ProductName.ToString();
                txtWeight.Text = productInfo.Weight.ToString();
                txtUnitPrice.Text = productInfo.UnitPrice.ToString();
                txtUnitsInStock.Text = productInfo.UnitsInStock.ToString();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var product = new ProductObject
                {
                    CategoryID = int.Parse(txtCategoryID.Text),
                    ProductName = txtProductName.Text,
                    Weight = txtWeight.Text,
                    UnitPrice = decimal.Parse(txtUnitPrice.Text),
                    UnitsInStock = int.Parse(txtUnitsInStock.Text)
                };

                if (InsertOrUpdate == false)
                {
                    ProductRepository.InsertProduct(product);
                    this.DialogResult = DialogResult.OK;
                    MessageBox.Show("Product has been created Successfully!");
                    Close();

                }
                else
                {
                    ProductRepository.UpdateProduct(product);
                    this.DialogResult = DialogResult.OK;
                    MessageBox.Show("Product has been updated Successfully!");
                    Close();
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, InsertOrUpdate == false ? "Add a new product" : "Update a product");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) => Close();

    }
}
