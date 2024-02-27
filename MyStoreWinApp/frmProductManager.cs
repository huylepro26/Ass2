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
    public partial class frmProductManager : Form
    {
        IProductRepository productRepository = new ProductRepository();
        IOrderDetailRepository OrderDetailRepository = new OrderDetailRepository();

        BindingSource source;


        public MemberObject Member { get; set; }

        public frmProductManager()
        {
            InitializeComponent();
        }

        private void frmProductManager_Load(object sender, EventArgs e)
        {
        }

        private void dgvProductList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            frmProductDetails frmProductDetails = new frmProductDetails
            {
                Text = "Update product",
                InsertOrUpdate = true,
                productInfo = GetProductObject(),
                ProductRepository = productRepository
            };
            if (frmProductDetails.ShowDialog().Equals(DialogResult.OK))
            {
                LoadProductList();
                source.Position = source.Count - 1;
            }
        }


        private ProductObject GetProductObject()
        {
            return (ProductObject)dgvProductList.CurrentRow.DataBoundItem;
        }

        public void LoadProductList()
        {
            var products = productRepository.GetProducts();
            try
            {
                source = new BindingSource();
                source.DataSource = products;
                dgvProductList.DataSource = null;
                dgvProductList.DataSource = source;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Load member list");
            }
        }


        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadProductList();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            frmProductDetails frmProductDetails = new frmProductDetails
            {
                Text = "Add product",
                InsertOrUpdate = false,
                ProductRepository = productRepository
            };
            if (frmProductDetails.ShowDialog().Equals(DialogResult.OK))
            {
                LoadProductList();
                source.Position = source.Count - 1;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var product = GetProductObject();
                var orderDetail = OrderDetailRepository.GetOrderDetailByProductID(product.ProductID);
                if (orderDetail != null)
                {
                    MessageBox.Show("Can not delete this product because this product is contained in one or more order detail!");
                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show("Do you want to delete Product: " + product.ProductName, "Confirmation", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        productRepository.DeleteProduct(product.ProductID);
                        if (product == null)
                        {
                            MessageBox.Show("Product has been deleted successfully!");
                        }
                        LoadProductList();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Delete a member");
            }
        }

        private void btnClose_Click(object sender, EventArgs e) => Close();


        private void btnSearch_Click(object sender, EventArgs e)
        {
            string ProductName = null;
            int ProductID = 0;
            decimal UnitPrice = 0;
            int UnitInStock = 0;

            if (txtProductID.Text == "")
            {
                txtProductID.Text = "0";
            }
            else
            {
                ProductID = (int)txtProductID.Value;
            }
            if (txtUnitPrice.Text == "")
            {
                txtUnitPrice.Text = "0";
            }
            else
            {
                UnitPrice = txtUnitPrice.Value;
            }
            if (txtUnitsInStock.Text == "")
            {
                txtUnitsInStock.Text = "0";
            }
            else
            {
                UnitInStock = (int)txtUnitsInStock.Value;
            }
            if (txtProductName.Text != "")
            {
                ProductName = txtProductName.Text.ToLower();
            }

             List<ProductObject> products = (List<ProductObject>)productRepository.GetProducts();

            try
            {
                if (txtProductID.Value != 0 && txtProductID.Text != "0")
                {
                    products = products.FindAll(p => p.ProductID == ProductID);
                    Trace.WriteLine(products[0].ToString());
                    Trace.WriteLine("Hi"+ txtProductID.Text);
                }

                if (txtProductName.Text != null && txtProductName.Text != "")
                {
                    products = products.FindAll(p => p.ProductName.ToLower().Contains(ProductName));
                }

                if (txtUnitPrice.Value != 0 && txtUnitPrice.Text != "0")
                {
                    products = products.FindAll(p => p.UnitPrice == UnitPrice);
                }   

                if (txtUnitsInStock.Value != 0 && txtUnitsInStock.Text != "0")
                {
                    products = products.FindAll(p => p.UnitsInStock == UnitInStock);
                }
            } catch (Exception)
            {
                MessageBox.Show("Input is not valid", "Product Searching", MessageBoxButtons.OK);
            }
            Trace.WriteLine(products.ToString());
            source = new BindingSource();
            source.DataSource = products;




            //source = new BindingSource();
            //Trace.WriteLine(ProductID + ProductName + UnitPrice + UnitInStock);
            //if (ProductID != 0 && ProductName != null && UnitPrice != 0 && UnitInStock != 0)//1111
            //{
            //    var Products = productRepository.GetProducts().Where(m
            //        => m.ProductID.Equals(ProductID) && m.ProductName.ToLower().Contains(ProductName)
            //        && m.UnitPrice.Equals(UnitPrice) && m.UnitsInStock.Equals(UnitInStock));
            //    source.DataSource = Products;
            //}
            //if (ProductID != 0 && ProductName != null && UnitPrice != 0 && UnitInStock == 0)//1110
            //{
            //    var Products = productRepository.GetProducts().Where(m
            //         => m.ProductID.Equals(ProductID) && m.ProductName.ToLower().Contains(ProductName)
            //         && m.UnitPrice.Equals(UnitPrice));
            //    source.DataSource = Products;
            //}
            //if (ProductID != 0 && ProductName != null && UnitPrice == 0 && UnitInStock != 0)//1101
            //{
            //    var Products = productRepository.GetProducts().Where(m
            //         => m.ProductID.Equals(ProductID) && m.ProductName.ToLower().Contains(ProductName)
            //         && m.UnitsInStock.Equals(UnitInStock));
            //    source.DataSource = Products;
            //}
            //if (ProductID == 0 && ProductName != null && UnitPrice != 0 && UnitInStock != 0)//0111
            //{
            //    var Products = productRepository.GetProducts().Where(m
            //       => m.ProductName.ToLower().Contains(ProductName) && m.UnitPrice.Equals(UnitPrice)
            //       && m.UnitsInStock.Equals(UnitInStock));
            //    source.DataSource = Products;
            //}
            //if (ProductID != 0 && ProductName == null && UnitPrice != 0 && UnitInStock != 0)//1011
            //{
            //    var Products = productRepository.GetProducts().Where(m
            //       => m.ProductID.Equals(ProductID) && m.UnitPrice.Equals(UnitPrice));
            //    source.DataSource = Products;
            //}
            //if (ProductID != 0 && ProductName != null && UnitPrice == 0 && UnitInStock == 0)//1100
            //{
            //    var Products = productRepository.GetProducts().Where(m
            //       => m.ProductID.Equals(ProductID) && m.ProductName.ToLower().Contains(ProductName));
            //    source.DataSource = Products;
            //}
            //if (ProductID == 0 && ProductName != null && UnitPrice != 0 && UnitInStock == 0)//0110
            //{
            //    var Products = productRepository.GetProducts().Where(m
            //        => m.ProductName.ToLower().Contains(ProductName) && m.UnitPrice.Equals(UnitPrice));
            //    source.DataSource = Products;
            //}
            //if (ProductID != 0 && ProductName == null && UnitPrice == 0 && UnitInStock != 0)//1001
            //{
            //    var Products = productRepository.GetProducts().Where(m
            //        => m.ProductID.Equals(ProductID) && m.UnitsInStock.Equals(UnitInStock));
            //    source.DataSource = Products;
            //}
            //if (ProductID != 0 && ProductName == null && UnitPrice != 0 && UnitInStock == 0)//1010
            //{
            //    var Products = productRepository.GetProducts().Where(m
            //        => m.ProductID.Equals(ProductID) && m.UnitPrice.Equals(UnitPrice));
            //    source.DataSource = Products;
            //}
            //if (ProductID == 0 && ProductName == null && UnitPrice != 0 && UnitInStock != 0)//0011
            //{
            //    var Products = productRepository.GetProducts().Where(m
            //        => m.UnitPrice.Equals(UnitPrice) && m.UnitsInStock.Equals(UnitInStock));
            //    source.DataSource = Products;

            //}
            //if (ProductID == 0 && ProductName != null && UnitPrice == 0 && UnitInStock != 0)//0101
            //{
            //    var Products = productRepository.GetProducts().Where(m
            //        => m.ProductName.ToLower().Contains(ProductName) && m.UnitsInStock.Equals(UnitInStock));
            //    source.DataSource = Products;

            //}
            //if (ProductID != 0 && ProductName == null && UnitPrice == 0 && UnitInStock == 0)//1000
            //{
            //    var Products = productRepository.GetProducts().Where(m => m.ProductID.Equals(ProductID));
            //    source.DataSource = Products;
            //}
            //if (ProductID == 0 && ProductName != null && UnitPrice == 0 && UnitInStock == 0)
            //{
            //    var Products = productRepository.GetProducts().Where(m => m.ProductName.ToLower().Contains(ProductName));
            //    source.DataSource = Products;
            //}
            //if (ProductID == 0 && ProductName == null && UnitPrice != 0 && UnitInStock == 0)
            //{
            //    var Products = productRepository.GetProducts().Where(m => m.UnitPrice.Equals(UnitPrice));
            //    source.DataSource = Products;
            //}
            //if (ProductID == 0 && ProductName == null && UnitPrice == 0 && UnitInStock != 0)
            //{
            //    var Products = productRepository.GetProducts().Where(m => m.UnitsInStock.Equals(UnitInStock));
            //    source.DataSource = Products;
            //}
            if (ProductID == 0 && ProductName == null && UnitPrice == 0 && UnitInStock == 0)
            {
                MessageBox.Show("No information input to search!", "Product Searching");
            }
            try
            {
                dgvProductList.DataSource = null;
                dgvProductList.DataSource = source;
                Trace.WriteLine(source);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Load product list");
            }
        }



        private void frmProductManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            frmMain.FormShowed[4] = "Closed";
            frmMain.pmStrip.BackColor = SystemColors.Control;
        }

        private void btnClearSearch_Click(object sender, EventArgs e)
        {
            txtProductID.Text = "0";
            txtProductName.Text = "";
            txtUnitPrice.Text = "0";
            txtUnitsInStock.Text = "0";
        }
    }
}
