using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LibrarySystem
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void bookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBookInfo frmBook = new frmBookInfo();
            
            frmBook.Show();
        }


        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
            Application.Restart();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
             
                if (MessageBox.Show("Are you sure you want to exit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    //foreach (Form frm in this.MdiChildren)
                    //{
                    //    frm.Close();
                    //}
                   
                    Application.Exit();
                    //this.Close();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void usersInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUsers frmUsr = new frmUsers();
            
            frmUsr.Show();
        }

        private void employeeInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmEmployee frmEmp = new frmEmployee();
            
            frmEmp.Show();
        }

        private void memberInfoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
           
            frmMember frmMem = new frmMember();
                     
            frmMem.Show();
        }

        
        private void actionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            
            frmIssue frmIss = new frmIssue();
            
            frmIss.emp = this.textBox2.Text;
            
            frmIss.Show();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

           
        }

        private void addCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCategory frmCat = new frmCategory();
            
            frmCat.Show();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel4.Text = System.DateTime.Now.ToString("hh:mm:ss" + "    " + "dd/MMM/yyyy");
        }

        private void availableBooksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBookRpt rpt = new frmBookRpt();
            
            rpt.Show();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAbout abt = new frmAbout();
            abt.Show();
        }

        private void issuedBorrowedBooksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSearch search = new frmSearch();
            search.Show();
            //frmIssuedRpt rpt = new frmIssuedRpt();
            //rpt.Show();
        }
    }
}
