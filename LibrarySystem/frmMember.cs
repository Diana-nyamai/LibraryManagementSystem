using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace LibrarySystem
{
    public partial class frmMember : Form
    {
        public frmMember()
        {
            InitializeComponent();
        }
        SqlConnection connect = null;
        SqlCommand cmd = null;
        ConnectionString con = new ConnectionString();
        SqlDataReader reader = null;

        #region program functions
        private void Autocomplete()
        {
            try
            {
                connect = new SqlConnection(con.connectString);
                connect.Open();
                SqlCommand sampleCmd = new SqlCommand("select member_name from tblmembers", connect);
                SqlDataAdapter da = new SqlDataAdapter(sampleCmd);
                DataSet ds = new DataSet();
                
                da.Fill(ds, "tblmembers");
                AutoCompleteStringCollection col = new AutoCompleteStringCollection();

                int cnt = 0;
                for (cnt = 0; cnt <= ds.Tables[0].Rows.Count - 1; cnt++)
                {
                    col.Add(ds.Tables[0].Rows[cnt]["member_name"].ToString());
                }
                txtMemName.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtMemName.AutoCompleteCustomSource = col;
                txtMemName.AutoCompleteMode = AutoCompleteMode.Suggest;

                connect.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static string GetCode(int maxSize)
        {
            char[] chars = new char[62];
            chars = "123456789".ToCharArray();
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();

            crypto.GetNonZeroBytes(data);
            data = new byte[maxSize];
            crypto.GetNonZeroBytes(data);

            StringBuilder results = new StringBuilder();

            foreach (byte b in data)
            {
                results.Append(chars[b % (chars.Length)]);
            }
            return results.ToString();
        }
        private void ClearAll()
        {
            txtMemberId.Text = "";
            txtMemName.Text = "";
            txtMemSurname.Text = "";
            txtAddress.Text = "";
            txtCell.Text = "";
            dtpDateOfBirth.Value = System.DateTime.Today;
            btnRegister.Enabled = true;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            txtMemName.Focus();
        }

        private void delete_records()
        {
            try
            {
                int rows_affected;
                connect = new SqlConnection(con.connectString);
                connect.Open();

                string delete = "delete from tblmembers where member_id=@mid";
                cmd = new SqlCommand(delete, connect);

                cmd.Parameters.Add(new SqlParameter("@mid", SqlDbType.VarChar)).Value = txtMemberId.Text;
                rows_affected = cmd.ExecuteNonQuery();

                if (rows_affected > 0)
                {
                    MessageBox.Show("Member details deleted successfully...!", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Autocomplete();
                    ClearAll();
                }
                else
                {
                    MessageBox.Show("No record found...!", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearAll();
                    Autocomplete();
                }

                if (connect.State == ConnectionState.Open)
                {
                    cmd.Dispose();
                    connect.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (txtMemName.Text == "")
            {
                MessageBox.Show("Please enter name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMemName.Focus();
                return;
            }
            if (txtMemSurname.Text == "")
            {
                MessageBox.Show("Please enter surname.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMemSurname.Focus();
                return;
            }
            if (txtAddress.Text == "")
            {
                MessageBox.Show("Please enter address.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtAddress.Focus();
                return;
            }
            if (txtCell.Text == "")
            {
                MessageBox.Show("Please enter contact number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCell.Focus();
                return;
            }

            if (dtpDateOfBirth.Value.Year > System.DateTime.Now.Year || dtpDateOfBirth.Value.Year == System.DateTime.Now.Year)
            {

                MessageBox.Show("Please enter correct birth year.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                txtMemberId.Text = "MEM" + GetCode(6);
                connect = new SqlConnection(con.connectString);
                connect.Open();
                string insert = "insert into tblMembers (member_id,member_name,member_surname,address,contact_no,dob) values (@d1,@d2,@d3,@d4,@d5,@d6)";
                cmd = new SqlCommand(insert, connect);

                cmd.Parameters.Add(new SqlParameter("@d1", SqlDbType.VarChar)).Value = txtMemberId.Text;
                cmd.Parameters.Add(new SqlParameter("@d2", SqlDbType.VarChar)).Value = txtMemName.Text;
                cmd.Parameters.Add(new SqlParameter("@d3", SqlDbType.VarChar)).Value = txtMemSurname.Text;
                cmd.Parameters.Add(new SqlParameter("@d4", SqlDbType.VarChar)).Value = txtAddress.Text;
                cmd.Parameters.Add(new SqlParameter("@d5", SqlDbType.NChar)).Value = txtCell.Text;
                cmd.Parameters.Add(new SqlParameter("@d6", SqlDbType.Date)).Value = dtpDateOfBirth.Value;

                cmd.ExecuteNonQuery();
                cmd.Dispose();
                connect.Close();
                MessageBox.Show("New member saved successfully...!\nMember ID is: " + txtMemberId.Text, "Member Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Autocomplete();
                ClearAll();
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }



        private void txtCell_Validate(object sender, CancelEventArgs e)
        {
            if (txtCell.TextLength < 10)
            {
                MessageBox.Show("Contact no. must be 10 digits.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCell.Focus();
            }
            if (txtCell.TextLength > 10)
            {
                MessageBox.Show("Only 10 digits are allowed.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCell.Focus();
            }

        }
        private void txtCell_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            ClearAll();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                connect = new SqlConnection(con.connectString);
                connect.Open();
                string update = "update tblMembers set member_name=@nam,member_surname=@sur,address=@addr,contact_no=@cno,dob=@dob where member_id=@mid";
                cmd = new SqlCommand(update, connect);

                cmd.Parameters.Add(new SqlParameter("@mid", SqlDbType.VarChar)).Value = txtMemberId.Text;
                cmd.Parameters.Add(new SqlParameter("@nam", SqlDbType.VarChar)).Value = txtMemName.Text;
                cmd.Parameters.Add(new SqlParameter("@sur", SqlDbType.VarChar)).Value = txtMemSurname.Text;
                cmd.Parameters.Add(new SqlParameter("@addr", SqlDbType.VarChar)).Value = txtAddress.Text;
                cmd.Parameters.Add(new SqlParameter("@cno", SqlDbType.NChar)).Value = txtCell.Text;
                cmd.Parameters.Add(new SqlParameter("@dob", SqlDbType.VarChar)).Value = dtpDateOfBirth.Value;

                cmd.ExecuteNonQuery();
                cmd.Dispose();
                connect.Close();
                MessageBox.Show("Member details updated successfully", "Update Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Autocomplete();
                ClearAll();
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


        }

        private void txtMemberId_TextChanged(object sender, EventArgs e)
        {
            btnRegister.Enabled = false;
            btnNew.Enabled = true;
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;
            try
            {
                connect = new SqlConnection(con.connectString);
                connect.Open();
                string select = "select member_name,member_surname,address,contact_no,dob from tblMembers where member_id=@mid";
                cmd = new SqlCommand(select, connect);

                cmd.Parameters.Add(new SqlParameter("@mid", SqlDbType.VarChar)).Value = txtMemberId.Text;

                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtMemName.Text = (reader.GetString(0).Trim());
                    txtMemSurname.Text = (reader.GetString(1).Trim());
                    txtAddress.Text = (reader.GetString(2).Trim());
                    txtCell.Text = (reader.GetString(3).Trim());
                    dtpDateOfBirth.Value = (DateTime)(reader.GetValue(4));
                }
                if ((reader != null))
                {
                    reader.Close();
                }

                cmd.Dispose();
                if (connect.State == ConnectionState.Open)
                {
                    connect.Close();
                }
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtMemberId.Text == "")
            {
                MessageBox.Show("No record selected to be deleted.\n Enter member_id", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (MessageBox.Show("Are you sure you want to delete?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                delete_records();
            }

        }


        private void frmMember_Load(object sender, EventArgs e)
        {
            Autocomplete();
            btnRegister.Enabled = true;
            btnNew.Enabled = true;
            
        }
        private void frmMember_Closing(object sender, FormClosingEventArgs e)
        {
            //frmMain main = new frmMain();
            this.Hide();
            //main.Show();
        }

        private void btnGetData_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmMemList frmML = new frmMemList();
            frmML.Show();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            
            this.Close();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        
    }
}
