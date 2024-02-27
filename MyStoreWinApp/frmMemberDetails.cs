using BusinessObject;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SalesWinApp
{
    public partial class frmMemberDetails : Form
    {
        public frmMemberDetails()
        {
            InitializeComponent();
        }

        public IMemberRepository MemberRepository { get; set; }
        public bool InsertOrUpdate { get; set; }
        public MemberObject memberInfo { get; set; }


        private void frmMemberDetails_Load(object sender, EventArgs e)
        {
            cboCountry.SelectedIndex = 0;
            cboCity.SelectedIndex = 0;
            if (InsertOrUpdate == true)
            {
                txtCompanyName.Text = memberInfo.CompanyName.ToString();
                txtEmail.Text = memberInfo.Email.ToString();
                txtPassword.Text = memberInfo.Password.ToString();
                cboCountry.Text = memberInfo.Country.ToString();
                cboCity.Text = memberInfo.City.ToString();
            }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Regex RegexEmail = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,}$");

                var member = new MemberObject
                {
                    CompanyName = txtCompanyName.Text,
                    Email = txtEmail.Text,
                    Password = txtPassword.Text,
                    Country = cboCountry.Text,
                    City = cboCity.Text
                };
                if (InsertOrUpdate == false)
                {
                    
                    List<MemberObject> memberList = (List<MemberObject>)MemberRepository.GetMembers();
                    foreach (MemberObject mem in memberList)
                    {
                        if (mem.Email.Equals(member.Email))
                        {
                            MessageBox.Show("Your Email already exist!", "Email Validation");
                            break;
                        }
                        else if (member.Email.Equals("admin@fstore.com") || member.Email.Contains("admin"))
                        {
                            MessageBox.Show("Your Email can't contain 'admin'!", "Email Validation");
                            break;
                        }
                        else if (RegexEmail.IsMatch(txtEmail.Text))
                        {
                            MemberRepository.InsertMember(member);
                            this.DialogResult = DialogResult.OK;
                            MessageBox.Show("Member has been created Successfully!");
                            this.Close();
                            break;
                        }
                        else
                        {
                            MessageBox.Show("Your Email is invalid!", "Email Validation");
                            break;
                        }
                    }

                }
                else
                {
                    
                    MemberRepository.UpdateMember(member);
                    this.DialogResult = DialogResult.OK;
                    MessageBox.Show("Member has been updated Successfully!");
                    Close();
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, InsertOrUpdate == false ? "Add a new member" : "Update a member");
            }
        }// end btnSave_Click

        private void btnCancel_Click(object sender, EventArgs e) => Close();


    }//end Form
}
