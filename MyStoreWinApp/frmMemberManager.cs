using BusinessObject;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SalesWinApp
{
    public partial class frmMemberManager : Form
    {
        IMemberRepository memberRepository = new MemberRepository();

        BindingSource source;

        public int MemberID;

        public MemberObject Member { get; set; }

        public frmMemberManager()
        {

            InitializeComponent();
        }
        public frmMemberManager(MemberObject member)
        {
            Member = member;
            InitializeComponent();
        }


        private void frmMemberManager_Load(object sender, EventArgs e)
        {
        }

        private void dgvMemberList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            frmMemberDetails frmMemberDetails = new frmMemberDetails
            {
                Text = "Update member",
                InsertOrUpdate = true,
                memberInfo = GetMemberObject(),
                MemberRepository = memberRepository
            };
            if (frmMemberDetails.ShowDialog().Equals(DialogResult.OK))
            {
                LoadMemberList();
                source.Position = source.Count - 1;
            }
        }


        private MemberObject GetMemberObject()
        {
            try
            {
                return (MemberObject)dgvMemberList.CurrentRow.DataBoundItem;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
            return null;
        }

        public void LoadMemberList()
        {
            var members = memberRepository.GetMembers();
            try
            {
                source = new BindingSource();
                source.DataSource = members;
                dgvMemberList.DataSource = null;
                dgvMemberList.DataSource = source;

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Load member list");
            }
        }



        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadMemberList();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            frmMemberDetails frmMemberDetails = new frmMemberDetails
            {
                Text = "Add member",
                InsertOrUpdate = false,
                MemberRepository = memberRepository

            };
            if (frmMemberDetails.ShowDialog().Equals(DialogResult.OK))
            {
                LoadMemberList();
                source.Position = source.Count - 1;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var member = GetMemberObject();
                DialogResult dialogResult = MessageBox.Show("Do you want to delete member: " + member.CompanyName, "Confirmation", MessageBoxButtons.YesNo);
                if (!member.Email.Equals("admin@fstore.com"))
                {
                    if (dialogResult == DialogResult.Yes)
                    {

                        memberRepository.DeleteMember(member.MemberID);
                        if (member == null)
                        {
                            MessageBox.Show("Member has been deleted successfully!");
                        }
                        LoadMemberList();
                    }

                }
                else
                {
                    MessageBox.Show("Can not delete an Admin!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not delete this member because this member has existing orders!", "Delete a member");

            }
        }

        private void btnClose_Click(object sender, EventArgs e) => Close();
        private void btnSort_Click(object sender, EventArgs e)
        {
            var members = memberRepository.GetMembers().OrderByDescending(m => m.CompanyName);
            try
            {
                source = new BindingSource();

                source.DataSource = members;

                dgvMemberList.DataSource = null;
                dgvMemberList.DataSource = source;

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Load member list");
            }
        }

        private void btnSearch1_Click(object sender, EventArgs e)
        {
            int SearchID = 0;
            int temp;
            try
            {
                if (int.TryParse(txtSearchID.Text, out temp))
                {
                    SearchID = temp;
                }
            }
            catch (Exception)
            {

            }
            var members = memberRepository.GetMembers().Where(SearchID != 0 && txtSearchID != null ?
               (m => m.MemberID.Equals(SearchID) && m.CompanyName.Contains(txtSearchName.Text))
               : m => m.MemberID.Equals(SearchID) || m.CompanyName.Contains(txtSearchName.Text));

            try
            {
                source = new BindingSource();
                source.DataSource = members;
                dgvMemberList.DataSource = null;
                dgvMemberList.DataSource = source;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Load member list");
            }
        }

        private void frmMemberManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            frmMain.FormShowed[1] = "Closed";
            frmMain.mmStrip.BackColor = SystemColors.Control;
        }



        //private void btnSearch2_Click(object sender, EventArgs e)
        //{
        //    var members = memberRepository.GetMembers().Where(m => m.Country.Equals(txtSeachCountry.Text) && m.City.Equals(txtSearchCity.Text));
        //    try
        //    {
        //        source = new BindingSource();

        //        source.DataSource = members;


        //        dgvMemberList.DataSource = null;
        //        dgvMemberList.DataSource = source;
        //        if (members.Count() == 0)
        //        {
        //            btnDelete.Enabled = false;
        //        }
        //        else
        //        {
        //            btnDelete.Enabled = true;
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, "Load member list");
        //    }
        //}
    }
}
