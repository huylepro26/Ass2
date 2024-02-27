using BusinessObject;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace SalesWinApp
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            MemberRepository memberRepository = new MemberRepository();
            List<MemberObject> memberList = (List<MemberObject>)memberRepository.GetMembers();

            MemberObject mo = memberList.SingleOrDefault(member => member.Email.Equals(txtEmail.Text) && member.Password.Equals(txtPassword.Text));
            
            if (mo != null)
            {
                this.Hide();
                frmMain mDIParent = new frmMain
                {
                    Member = mo
                };
                mDIParent.ShowDialog();
            }
            else
            {
                MessageBox.Show("Your Email or Password is incorrect");
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtEmail.Text = string.Empty;
            txtPassword.Text = string.Empty;
        }
    }
}