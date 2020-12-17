using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LibrarySystem
{
    public partial class frmCategory : Form
    {
        public frmCategory()
        {
            InitializeComponent();
        }
        SqlConnection connect = null;
        SqlCommand cmd = null;
        ConnectionString con = new ConnectionString();

        private void btnSave_Click(object sender, EventArgs e)
        {
           if (txtCategory.Text == "")
            {
                MessageBox.Show("Please enter category name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCategory.Focus();
                return;
            }

            try
            {
                connect = new SqlConnection(con.connectString);
                connect.Open();


                string insert = "insert into tblCategory (category) values(@category)";

                cmd = new SqlCommand(insert, connect);
                cmd.Parameters.Add(new SqlParameter("@category", SqlDbType.VarChar)).Value = txtCategory.Text;

                cmd.ExecuteNonQuery();

                MessageBox.Show("Category saved successfully.", "Category saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtCategory.Text = "";
                txtCategory.Focus();
                connect.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            txtCategory.Text = "";
            txtId.Text = "";
            btnSave.Enabled = true;
            btnDelete.Enabled = false;
            btnUpdate.Enabled = false;
            txtCategory.Focus();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtCategory.Text == "")
            {
                MessageBox.Show("Please enter category name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCategory.Focus();
                return;
            }
            try
            {
                connect = new SqlConnection(con.connectString);
                connect.Open();

                string updateStr = "update tblCategory set category=@category where id=@id";
                cmd = new SqlCommand(updateStr, connect);

                cmd.Parameters.Add(new SqlParameter("@category", SqlDbType.VarChar)).Value = txtCategory.Text;
                cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = txtId.Text;
                cmd.ExecuteNonQuery();

                MessageBox.Show("Category name updated and saved successfully.", "Category", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtCategory.Text = "";
                txtId.Text = "";
                txtCategory.Focus();
                connect.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void deleteRow()
        {
            int rows_affected = 0;
            connect = new SqlConnection(con.connectString);
            connect.Open();
            string deleteStr = "delete from tblcategory where id=@id";
            cmd = new SqlCommand(deleteStr, connect);
            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = txtId.Text;
            rows_affected = cmd.ExecuteNonQuery();

            if (rows_affected > 0)
            {
                MessageBox.Show("Category deleted", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtCategory.Text = "";
                txtId.Text = "";
                txtCategory.Focus();
            }
            else
            {
                MessageBox.Show("No record found...!", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtCategory.Text = "";
                txtId.Text = "";
                txtCategory.Focus();
            }

            connect.Close();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtCategory.Text == "")
            {
                MessageBox.Show("Please enter category name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCategory.Focus();
                return;
            }
            if (MessageBox.Show("Do you want to delete data?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                deleteRow();
            }
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            frmCategoryList frmCateList = new frmCategoryList();
            this.Hide();
            frmCateList.Show();
        }

        private void frmCategory_Load(object sender, EventArgs e)
        {
         
        }

        private void txtId_TextChanged(object sender, EventArgs e)
        {
            btnSave.Enabled = false;
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
           
            this.Close();
            
        }
    }
}
