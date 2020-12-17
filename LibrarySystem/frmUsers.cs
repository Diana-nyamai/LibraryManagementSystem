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
    public partial class frmUsers : Form
    {
        SqlConnection connect = null;
        SqlCommand cmd = null;
        ConnectionString conStr = new ConnectionString();
        SqlDataReader reader = null;
        public frmUsers()
        {
            InitializeComponent();
        }
        private void reset()
        {
            txtUser.Clear();
            txtRole.Clear();
            txtPwd1.Clear();
            txtPwd2.Clear();
            txtEmp.Clear();
            txtUser.Focus();
            btnRegister.Enabled = true;
            btnNew.Enabled = true;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            txtRole.Enabled = true;
        }
        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (txtUser.Text == "")
            {
                MessageBox.Show("Please enter username.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUser.Focus();
                return;
            }

            if (txtRole.Text == "")
            {
                MessageBox.Show("Please enter user role.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtRole.Focus();
                return;
            }
            if (txtPwd1.Text == "")
            {
                MessageBox.Show("Please enter password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPwd1.Focus();
                return;
            }
            if (txtPwd2.Text == "")
            {
                MessageBox.Show("Please repeat password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPwd2.Focus();
                return;
            }
            if (txtPwd1.Text != txtPwd2.Text)
            {
                MessageBox.Show("Please enter matching passwords.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPwd1.Clear();
                txtPwd2.Clear();
                txtPwd1.Focus();
                return;
            }
            try
            {
                frmEmployee emp = new frmEmployee();
                connect = new SqlConnection(conStr.connectString);
                connect.Open();
                string insert = "insert into tblusers (username,password,userrole,emp_id) values (@usr,@pwd,@usrl,@emp_id)";
                cmd = new SqlCommand(insert, connect);
                cmd.Parameters.Add(new SqlParameter("@usr", SqlDbType.VarChar)).Value = txtUser.Text;
                cmd.Parameters.Add(new SqlParameter("@pwd", SqlDbType.VarChar)).Value = txtPwd1.Text;
                cmd.Parameters.Add(new SqlParameter("@usrl", SqlDbType.VarChar)).Value = txtRole.Text;
                cmd.Parameters.Add(new SqlParameter("@emp_id", SqlDbType.VarChar)).Value = txtEmp.Text;
                cmd.ExecuteNonQuery();
                MessageBox.Show("User registered successfully...!", "New User", MessageBoxButtons.OK, MessageBoxIcon.Information);
                reset();
                emp.Close();
                this.Close();                
                cmd.Dispose();
                connect.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            reset();
        }

        private void txtUser_TextChanged(object sender, EventArgs e)
        {
            //btnRegister.Enabled = false;
            btnDelete.Enabled = true;
            btnUpdate.Enabled = true;
         
            try
            {
                connect = new SqlConnection(conStr.connectString);
                connect.Open();
                string select = "select username,password,userrole from tblusers where username=@usr";
                cmd = new SqlCommand(select, connect);

                cmd.Parameters.Add(new SqlParameter("@usr", SqlDbType.VarChar)).Value = txtUser.Text;
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtPwd1.Text = (reader.GetString(1).Trim());
                    txtPwd2.Text = (reader.GetString(1).Trim());
                    txtRole.Text = (reader.GetString(2).Trim());
                }
                if (reader != null)
                {
                    reader.Close();
                }

                if (connect.State == ConnectionState.Open)
                {
                    connect.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
        } 
        
        private void frmClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            //frmMain main = new frmMain();
            //main.Show();

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                connect = new SqlConnection(conStr.connectString);
                connect.Open();
                string updateQry = "update tblusers set username=@uname,password=@pwd,userrole=@urole where username=@uname";
                cmd = new SqlCommand(updateQry, connect);
                cmd.Parameters.Add(new SqlParameter("@uname", SqlDbType.VarChar)).Value = txtUser.Text;
                cmd.Parameters.Add(new SqlParameter("@pwd", SqlDbType.VarChar)).Value = txtPwd1.Text;
                cmd.Parameters.Add(new SqlParameter("@urole", SqlDbType.VarChar)).Value = txtRole.Text;
               
                cmd.ExecuteNonQuery();
                connect.Close();
                MessageBox.Show("Login details updated successfully..!", "Update Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Autocomplete();
                reset();
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Autocomplete()
        {
            try
            {
                connect = new SqlConnection(conStr.connectString);
                connect.Open();
                string selectQry = "select username from tblusers";
                SqlCommand sampleCmd = new SqlCommand(selectQry, connect);
                SqlDataAdapter da = new SqlDataAdapter(sampleCmd);
                DataSet ds = new DataSet();
                AutoCompleteStringCollection col = new AutoCompleteStringCollection();
                da.Fill(ds, "tblusers");

                for (int cnt = 0; cnt < ds.Tables[0].Rows.Count - 1; cnt++)
                {
                    col.Add(ds.Tables[0].Rows[cnt]["tblusers"].ToString());
                }

                txtUser.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtUser.AutoCompleteCustomSource = col;
                txtUser.AutoCompleteMode = AutoCompleteMode.Suggest;

                connect.Close();
            }
            catch
            {
            }
        }
        private void frmUsers_Load(object sender, EventArgs e)
        {
            Autocomplete();
        }
        private void delete_records()
        {
            int row_affected = 0;
            connect = new SqlConnection(conStr.connectString);
            connect.Open();
            string deleteQry = "delete from tblusers where username=@uname and password=@pwd";
            cmd = new SqlCommand(deleteQry, connect);

            cmd.Parameters.Add(new SqlParameter("@uname", SqlDbType.VarChar)).Value = txtUser.Text;
            cmd.Parameters.Add(new SqlParameter("@pwd", SqlDbType.VarChar)).Value = txtPwd1.Text;
            
            row_affected = cmd.ExecuteNonQuery();

            if (row_affected > 0)
            {
                MessageBox.Show("User deleted permanently..!", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Autocomplete();
                reset();
            }
            else
            {
                MessageBox.Show("No user record found", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                reset();
                Autocomplete();
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtUser.Text == "")
            {
                MessageBox.Show("Please enter username.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUser.Focus();
                return;
            }

            if (txtRole.Text == "")
            {
                MessageBox.Show("Please enter user role.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtRole.Focus();
                return;
            }
            if (txtPwd1.Text == "")
            {
                MessageBox.Show("Please enter password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPwd1.Focus();
                return;
            }
            if (txtPwd2.Text == "")
            {
                MessageBox.Show("Please repeat password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPwd2.Focus();
                return;
            }
            if (txtPwd1.Text != txtPwd2.Text)
            {
                MessageBox.Show("Please enter matching passwords.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPwd1.Clear();
                txtPwd2.Clear();
                txtPwd1.Focus();
                return;
            }
            if (MessageBox.Show("Are you sure you want to delete?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                delete_records();
            }
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
