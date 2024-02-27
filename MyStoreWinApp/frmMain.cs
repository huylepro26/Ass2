using BusinessObject;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace SalesWinApp
{
    public partial class frmMain : Form
    {
        public MemberObject Member { get; set; }
        public static string[] FormShowed = { "", "Closed", "Closed", "Closed","Closed","Closed","Closed" };
        private int firstShowed = 0;
        private int lastShowed = 0;

        public static ToolStripMenuItem mmStrip;
        public static ToolStripMenuItem omStrip;
        public static ToolStripMenuItem odStrip;
        public static ToolStripMenuItem pmStrip;
        public static ToolStripMenuItem rmStrip;
        public static ToolStripMenuItem profileStrip;

        private frmMemberManager MM;
        private frmOrderManager OM;
        private frmOrderDetailList OD;
        private frmProductManager PM;
        private frmReport RM;
        private frmProfile Profile;
        public frmMain()
        {
            InitializeComponent();
            mmStrip = memberManagerToolStripMenuItem;
            omStrip = orderManagerToolStripMenuItem;
            odStrip = orderDetailToolStripMenuItem;
            pmStrip = productManagerToolStripMenuItem;
            rmStrip = reportToolStripMenuItem;
            profileStrip = profileToolStripMenuItem;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            CheckRole();
            this.WindowState = FormWindowState.Maximized;
        }

        private bool CheckRole()
        {
            if (Member.Email.Equals("minhadmindepzai@gmail.com"))
            {
                
                return true;
            }
            else
            {
                memberManagerToolStripMenuItem.Visible = false;
                orderManagerToolStripMenuItem.Visible = false;
                orderDetailToolStripMenuItem.Visible = false;
                productManagerToolStripMenuItem.Visible = false;
                reportToolStripMenuItem.Visible = false;
                return false;
            }
        }

        private void HideOtherForm(int curForm)
        {
            if (curForm != lastShowed)
            {
                lastShowed = firstShowed;
                firstShowed = curForm;
            }
            int formClosed = 0;
            FormShowed[curForm] = "Opened";
            for (int i = 1; i < (FormShowed.Length); i++)
            {
                if (i != curForm)
                {
                    if (FormShowed[i].Equals("Opened"))
                    {
                        FormShowed[i] = "Hidden";
                    }
                }
                if (FormShowed[i].Equals("Closed"))
                {
                    formClosed += 1;
                }
            }
            if (formClosed ==6)
            {
                firstShowed = 0;
                lastShowed = 0;
            }
        }

        private void CloseForm(int curForm)
        {
            FormShowed[curForm] = "Closed";
            if (FormShowed[lastShowed].Equals("Hidden"))
            {
                FormShowed[lastShowed] = "Opened";
            }
        }

        

        private void memberManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (FormShowed[1].Equals("Closed"))
            {
                MM = new frmMemberManager
                {
                    Text= "Member Manager",
                    Member = Member
                };
                mmStrip.BackColor = SystemColors.ActiveCaption;
                HideOtherForm(1);
                MM.MdiParent = this;
                MM.Show();
            }
            else if (FormShowed[1].Equals("Opened"))
            {
                mmStrip.BackColor = SystemColors.Control;
                CloseForm(1);
                MM.Close();
                
            }
            else if (FormShowed[1].Equals("Hidden"))
            {
                mmStrip.BackColor = SystemColors.ActiveCaption;
                HideOtherForm(1);
                MM.Focus();
            }
        }


        private void orderManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FormShowed[2].Equals("Closed"))
            {
                OM = new frmOrderManager
                {
                    Text = "Order Manager",
                    Member = Member
                };
                omStrip.BackColor = SystemColors.ActiveCaption;
                HideOtherForm(2);
                OM.MdiParent = this;
                OM.Show();
            }
            else if (FormShowed[2].Equals("Opened"))
            {
                omStrip.BackColor = SystemColors.Control;
                CloseForm(2);
                OM.Close();
                
            }
            else if (FormShowed[2].Equals("Hidden"))
            {
                omStrip.BackColor = SystemColors.ActiveCaption;
                HideOtherForm(2);
                OM.Focus();
            }

        }

        private void orderDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FormShowed[3].Equals("Closed"))
            {
                OD = new frmOrderDetailList
                {
                    Text = "Order Detail Manager",
                    Member = Member
                };
                odStrip.BackColor = SystemColors.ActiveCaption;
                HideOtherForm(3);
                OD.MdiParent = this;
                OD.Show();
            }
            else if (FormShowed[3].Equals("Opened"))
            {
                odStrip.BackColor = SystemColors.Control;
                CloseForm(3);
                OD.Close();

            }
            else if (FormShowed[3].Equals("Hidden"))
            {
                odStrip.BackColor = SystemColors.ActiveCaption;
                HideOtherForm(3);
                OD.Focus();
            }
        }

        private void productManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FormShowed[4].Equals("Closed"))
            {
                PM = new frmProductManager
                {
                    Text = "Product Manager",
                    Member = Member
                };
                pmStrip.BackColor = SystemColors.ActiveCaption;
                HideOtherForm(4);
                PM.MdiParent = this;
                PM.Show();
            }
            else if (FormShowed[4].Equals("Opened"))
            {
                pmStrip.BackColor = SystemColors.Control;
                CloseForm(4);
                PM.Close();

            }
            else if (FormShowed[4].Equals("Hidden"))
            {
                pmStrip.BackColor = SystemColors.ActiveCaption;
                HideOtherForm(4);
                PM.Focus();
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e) => Application.Exit();

        private void reportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FormShowed[5].Equals("Closed"))
            {
                RM = new frmReport
                {
                    Text = "Statistic Report",
                };
                rmStrip.BackColor = SystemColors.ActiveCaption;
                HideOtherForm(5);
                RM.MdiParent = this;
                RM.Show();
            }
            else if (FormShowed[5].Equals("Opened"))
            {
                rmStrip.BackColor = SystemColors.Control;
                CloseForm(5);
                RM.Close();

            }
            else if (FormShowed[5].Equals("Hidden"))
            {
                rmStrip.BackColor = SystemColors.ActiveCaption;
                HideOtherForm(5);
                RM.Focus();
            }
        }

        private void profileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            if (FormShowed[6].Equals("Closed"))
            {
                Profile = new frmProfile
                {
                    Text = "Member Profile",
                    Member = Member
                };
                profileStrip.BackColor = SystemColors.ActiveCaption;
                HideOtherForm(6);
                Profile.MdiParent = this;
                Profile.Show();
            }
            else if (FormShowed[6].Equals("Opened"))
            {
                profileStrip.BackColor = SystemColors.Control;
                CloseForm(6);
                Profile.Close();

            }
            else if (FormShowed[6].Equals("Hidden"))
            {
                profileStrip.BackColor = SystemColors.ActiveCaption;
                HideOtherForm(6);
                Profile.Focus();
            }
        }
    }
}