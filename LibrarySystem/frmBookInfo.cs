using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace LibrarySystem
{
    public partial class frmBookInfo : Form
    {
        public frmBookInfo()
        {
            InitializeComponent();
        }

        SqlConnection connect = null;
        SqlCommand cmd = null;
        ConnectionString conn = new ConnectionString();

        public static string GetCode(int maxSize)
        {
            char[] chars = new char[70];
            chars = "123456789".ToCharArray();

            byte[] bytes = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();

            crypto.GetNonZeroBytes(bytes);
            bytes = new byte[maxSize];
            crypto.GetNonZeroBytes(bytes);

            StringBuilder results = new StringBuilder();

            foreach (byte b in bytes)
            {
                results.Append(chars[b % (chars.Length)]);
            }
            return results.ToString();

        }
        public void ClearAll()
        {
            txtBookId.Text = "";
            txtTitle.Text = "";
            txtAuthor.Text = "";
            txtPublisher.Text = "";
            cmbCategory.SelectedIndex = -1;
            nudQuantity.Value = 1;
            btnSave.Enabled = true;
            btnDelete.Enabled = false;
            btnUpdate.Enabled = false;
            txtTitle.Focus();

        }
        private void Autocomplete()
        {
            try
            {
                connect = new SqlConnection(conn.connectString);
                connect.Open();
                SqlCommand sampleCmd = new SqlCommand("select * from tblBooks", connect);
                SqlDataAdapter da = new SqlDataAdapter(sampleCmd);
                DataSet ds = new DataSet();
                AutoCompleteStringCollection col = new AutoCompleteStringCollection();

                da.Fill(ds, "tblbooks");
                int cnt = 0;
                for (cnt = 0; cnt < ds.Tables[0].Rows.Count - 1; cnt++)
                {
                    col.Add(ds.Tables[0].Rows[cnt]["book_title"].ToString());
                }
                txtTitle.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtTitle.AutoCompleteCustomSource = col;
                txtTitle.AutoCompleteMode = AutoCompleteMode.Suggest;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtTitle.Text == "")
            {
                MessageBox.Show("Please enter book title.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtTitle.Focus();
                return;
            }
            if (txtAuthor.Text == "")
            {
                MessageBox.Show("Please enter author's name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtAuthor.Focus();
                return;
            }
            if (cmbCategory.SelectedIndex == -1)
            {
                MessageBox.Show("Please select category.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmbCategory.Focus();
                return;
            }
            if (txtPublisher.Text == "")
            {
                MessageBox.Show("Please enter publisher's name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPublisher.Focus();
                return;
            }
            if (nudQuantity.Value <= 0)
            {
                MessageBox.Show("Please enter quantity of books.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                nudQuantity.Focus();
                return;

            }
            try
            {
                txtBookId.Text = "B" + GetCode(5);
                connect = new SqlConnection(conn.connectString);
                connect.Open();
                string insert = "insert into tblBooks(book_id,book_title,author,publisher,category,quantity) values(@b_id,@b_t,@au,@pu,@cat,@qua)";
                cmd = new SqlCommand(insert, connect);

                cmd.Parameters.Add(new SqlParameter("@b_id", SqlDbType.VarChar)).Value = txtBookId.Text;
                cmd.Parameters.Add(new SqlParameter("@b_t", SqlDbType.VarChar)).Value = txtTitle.Text;
                cmd.Parameters.Add(new SqlParameter("@au", SqlDbType.VarChar)).Value = txtAuthor.Text;
                cmd.Parameters.Add(new SqlParameter("@pu", SqlDbType.VarChar)).Value = txtPublisher.Text;
                cmd.Parameters.Add(new SqlParameter("@cat", SqlDbType.VarChar)).Value = cmbCategory.SelectedItem;
                cmd.Parameters.Add(new SqlParameter("@qua", SqlDbType.Int)).Value = nudQuantity.Value;

                cmd.ExecuteNonQuery();
                
                MessageBox.Show("Book info successfully saved....!", "Book Record", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearAll();
                //cmd.Dispose();
                connect.Close();
                
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void frmBookInfo_Load(object sender, EventArgs e)
        {
            btnSave.Enabled = true;
            btnNew.Enabled = true;
            Autocomplete();
            
            //cmbCategory.SelectedIndex = -1;
            try
            {
                connect = new SqlConnection(conn.connectString);
                connect.Open();
                string selectQry = "select distinct RTRIM(category) from tblcategory";
                cmd = new SqlCommand(selectQry, connect);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = null;
                DataTable dTable = null;

                ds = new DataSet("ds");
                da.Fill(ds);
                dTable = ds.Tables[0];
                cmbCategory.Items.Clear();
                foreach (DataRow row in dTable.Rows)
                {
                    cmbCategory.Items.Add(row[0].ToString());
                }
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            

        }
        private void frmBookInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            frmMain frmMain = new frmMain();
            this.Hide();
            //frmMain.Show();
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            ClearAll();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtTitle.Text == "")
            {
                MessageBox.Show("Please enter book title.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtTitle.Focus();
                return;
            }
            if (txtAuthor.Text == "")
            {
                MessageBox.Show("Please enter author's name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtAuthor.Focus();
                return;
            }
            if (cmbCategory.SelectedIndex == -1)
            {
                MessageBox.Show("Please select category.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmbCategory.Focus();
                return;
            }
            if (txtPublisher.Text == "")
            {
                MessageBox.Show("Please enter publisher's name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPublisher.Focus();
                return;
            }
            if (nudQuantity.Value <= 0)
            {
                MessageBox.Show("Please enter quantity of books.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                nudQuantity.Focus();
                return;
            }

            try
            {
                connect = new SqlConnection(conn.connectString);
                connect.Open();

                string updateQry = "update tblBooks set book_id=@bid,book_title=@btitle,author=@auth,publisher=@publ,category=@cate,quantity=@qua where book_id=@book_id";
                cmd = new SqlCommand(updateQry, connect);
                cmd.Parameters.Add(new SqlParameter("@bid", SqlDbType.VarChar)).Value = txtBookId.Text;
                cmd.Parameters.Add(new SqlParameter("@btitle", SqlDbType.VarChar)).Value = txtTitle.Text;
                cmd.Parameters.Add(new SqlParameter("@auth", SqlDbType.VarChar)).Value = txtAuthor.Text;
                cmd.Parameters.Add(new SqlParameter("@publ", SqlDbType.VarChar)).Value = txtPublisher.Text;
                cmd.Parameters.Add(new SqlParameter("@cate", SqlDbType.VarChar)).Value = cmbCategory.SelectedItem;
                cmd.Parameters.Add(new SqlParameter("@qua", SqlDbType.Int)).Value = nudQuantity.Value;
                cmd.Parameters.Add(new SqlParameter("@book_id", SqlDbType.VarChar)).Value = txtBookId.Text;

                cmd.ExecuteNonQuery();
                MessageBox.Show("Book info updated successfully.", "Update Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearAll();
                connect.Close();
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtBookId.Text == "")
            {
                MessageBox.Show("No record selected to delete.\nEnter Book ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (MessageBox.Show("Are you sure you want to delete record?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                delete_records();
            }
            
        }
        private void delete_records()
        {
            try
            {
                int row_affected = 0;
                connect = new SqlConnection(conn.connectString);
                connect.Open();
                string deleteQry = "delete from tblbooks where book_id=@bid";
                cmd = new SqlCommand(deleteQry, connect);

                cmd.Parameters.Add(new SqlParameter("@bid", SqlDbType.VarChar)).Value = txtBookId.Text;

                row_affected = cmd.ExecuteNonQuery();
                if (row_affected > 0)
                {
                    MessageBox.Show("Book info deleted permanently...!", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Autocomplete();
                    ClearAll();
                }
                else
                {
                    MessageBox.Show("No record found...!", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearAll();
                    Autocomplete();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGetAll_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmBookRecords frmBR = new frmBookRecords();
            frmBR.Show();
        }

        private void txtBookId_TextChanged(object sender, EventArgs e)
        {
            btnUpdate.Enabled = true;
            btnSave.Enabled = false;
            btnDelete.Enabled = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            this.Update();
        }
   

    }
}
