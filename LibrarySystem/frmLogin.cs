using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LibrarySystem
{
    public partial class frmLogin : Form
    {
        SqlConnection connect = null;
        SqlCommand cmd = null;
        SqlDataReader reader;
        ConnectionString cn = new ConnectionString();
        public frmLogin()
        {
            InitializeComponent();
        }

        public string emp;
        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text == "")
            {
                MessageBox.Show("Please enter username", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUsername.Focus();
                return;
            }
            if (txtPassword.Text == "")
            {
                MessageBox.Show("Please enter password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Focus();
                return;
            }
            try
            {
 
                connect = new SqlConnection(cn.connectString);
                connect.Open();
                string sqlSelect = "SELECT USERNAME, PASSWORD, USERROLE, EMP_ID FROM TBLUSERS WHERE USERNAME=@usern AND PASSWORD=@pword";
                cmd = new SqlCommand(sqlSelect, connect);

                cmd.Parameters.Add("@usern", System.Data.SqlDbType.VarChar).Value = txtUsername.Text;
                cmd.Parameters.Add("@pword", System.Data.SqlDbType.VarChar).Value = txtPassword.Text;

                reader = cmd.ExecuteReader();

                
                if (reader.Read() == true)
                {
                    this.Hide();
                    frmMain frmMainForm = new frmMain();
                    frmIssue issuefrm = new frmIssue();
                    frmMainForm.toolStripStatusLabel2.Text = txtUsername.Text;
                    frmMainForm.textBox1.Text = reader.GetString(2).ToString();
                    frmMainForm.textBox2.Text = reader.GetString(3).ToString();


                    if (frmMainForm.textBox1.Text == "admin" || frmMainForm.textBox1.Text == "administrator" || frmMainForm.textBox1.Text == "Admin")
                    {
                        frmMainForm.bookToolStripMenuItem.Enabled = true;
                        frmMainForm.memberInfoToolStripMenuItem1.Enabled = true;
                        frmMainForm.addCategoryToolStripMenuItem.Enabled = true;
                        frmMainForm.actionToolStripMenuItem.Enabled = true;
                        frmMainForm.employeeInfoToolStripMenuItem.Enabled = true;
                        frmMainForm.usersInfoToolStripMenuItem.Enabled = true;
                    }
                    else if (frmMainForm.textBox1.Text == "Librarian" || frmMainForm.textBox1.Text == "librarian" || frmMainForm.textBox1.Text == "LIBRARIAN")
                    {
                        frmMainForm.bookToolStripMenuItem.Enabled = true;
                        frmMainForm.memberInfoToolStripMenuItem1.Enabled = true;
                        frmMainForm.actionToolStripMenuItem.Enabled = true;
                        frmMainForm.addCategoryToolStripMenuItem.Enabled = true;
                        frmMainForm.employeeInfoToolStripMenuItem.Enabled = false;
                        frmMainForm.usersInfoToolStripMenuItem.Enabled = false;
                    }

                    else if (frmMainForm.textBox1.Text == "manager" || frmMainForm.textBox1.Text == "Manager")
                    {
                        frmMainForm.bookToolStripMenuItem.Enabled = true;
                        frmMainForm.memberInfoToolStripMenuItem1.Enabled = true;
                        frmMainForm.actionToolStripMenuItem.Enabled = false;
                        frmMainForm.addCategoryToolStripMenuItem.Enabled = false;
                        frmMainForm.employeeInfoToolStripMenuItem.Enabled = true;
                        frmMainForm.usersInfoToolStripMenuItem.Enabled = false;
                    }
                    else
                    {
                        frmMainForm.bookToolStripMenuItem.Enabled = false;
                        frmMainForm.memberInfoToolStripMenuItem1.Enabled = false;
                        frmMainForm.actionToolStripMenuItem.Enabled = false;
                        frmMainForm.employeeInfoToolStripMenuItem.Enabled = false;
                        frmMainForm.addCategoryToolStripMenuItem.Enabled = false;
                        frmMainForm.usersInfoToolStripMenuItem.Enabled = false;
                    }
                    frmMainForm.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Incorrect credentials entered.....Please try again", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtUsername.Text = "";
                    txtPassword.Text = "";
                    txtUsername.Focus();
                }
      
                if (connect.State == System.Data.ConnectionState.Open)
                {
                    connect.Close();
                }
   
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //Application.Exit();
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtUsername.Focus();
            this.WindowState = FormWindowState.Minimized;
        }

    }
}
