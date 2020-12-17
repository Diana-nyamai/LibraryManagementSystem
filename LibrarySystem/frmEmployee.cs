using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace LibrarySystem
{
    public partial class frmEmployee : Form
    {
        public frmEmployee()
        {
            InitializeComponent();
        }
        ConnectionString conStr = new ConnectionString();
        SqlConnection connectionStr = null;
        SqlCommand cmd = null;
        SqlDataReader rdr = null;

        private string GetUniqueCode(int maxSize)
        {
            char[] chars = new char[70];
            chars = "123456789".ToCharArray();
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            data = new byte[maxSize];
            crypto.GetNonZeroBytes(data);

            StringBuilder result = new StringBuilder();

            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }
        private void Autocomplete()
        {
            try
            {
                connectionStr = new SqlConnection(conStr.connectString);
                connectionStr.Open();
                SqlCommand sampleCmd = new SqlCommand("select emp_name from tblemployee", connectionStr);
                SqlDataAdapter da = new SqlDataAdapter(sampleCmd);
                DataSet ds = new DataSet();

                da.Fill(ds, "tblemployee");
                AutoCompleteStringCollection col = new AutoCompleteStringCollection();
                int cnt = 0;
                for (cnt = 0; cnt < ds.Tables[0].Rows.Count - 1; cnt++)
                {
                    col.Add(ds.Tables[0].Rows[cnt]["emp_name"].ToString());
                }
                txtName.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtName.AutoCompleteCustomSource = col;
                txtName.AutoCompleteMode = AutoCompleteMode.Suggest;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void reset()
        {
            txtEmpId.Text = "";
            txtName.Text = "";
            txtSurname.Text = "";
            dateOfbirth.Value = System.DateTime.Now;
            txtEmpId.Text = "";
            txtAddress.Text = "";
            txtContactno.Text = "";
            txtPosition.Text = "";
            txtEmailAdd.Text = "";
            txtName.Text = "";
            btnSave.Enabled = true;
            btnDelete.Enabled = false;
            btnUpdate.Enabled = false;
            txtEmpId.Focus();

        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "")
            {
                MessageBox.Show("Please enter employee name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtName.Focus();
                return;
            }
            if (txtSurname.Text == "")
            {
                MessageBox.Show("Please enter employee surname", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSurname.Focus();
                return;
            }
            if (txtPosition.Text == "")
            {
                MessageBox.Show("Please enter employee\'s position.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPosition.Focus();
                return;
            }
            if (dateOfbirth.Value.Year > System.DateTime.Now.Year || dateOfbirth.Value.Year == System.DateTime.Now.Year)
            {
                MessageBox.Show("Please enter correct birth year", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

        
            if (txtAddress.Text == "")
            {
                MessageBox.Show("Please enter address.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtAddress.Focus();
                return;
            }
            if (txtContactno.Text == "")
            {
                MessageBox.Show("Please enter Contact no.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtContactno.Focus();
                return;
            }
          
        
            if (txtEmailAdd.Text == "")
            {
                MessageBox.Show("Please enter email.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtEmailAdd.Focus();
                return;
            }
            try
            {
                frmUsers userFrm = new frmUsers();
                txtEmpId.Text = "EMP-" + GetUniqueCode(5);

                //first Check if EMP_ID is existing
                connectionStr = new SqlConnection(conStr.connectString);
                connectionStr.Open();
                string selectQry = "select emp_id from tblemployee where emp_id=@find";
                cmd = new SqlCommand(selectQry, connectionStr);
                cmd.Parameters.Add(new SqlParameter("@find", SqlDbType.VarChar)).Value = txtEmpId.Text;
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    MessageBox.Show("Employee ID already exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtEmpId.Text = "";
                    txtName.Focus();

                    if ((rdr != null))
                    {
                        rdr.Close();
                    }
                    return;
                }
                connectionStr = new SqlConnection(conStr.connectString);
                connectionStr.Open();
                string insertQry = "insert into tblemployee values(@emp_id,@emp_name,@emp_sur,@emp_dob,@cont_no,@addr,@posi,@email)";
                cmd = new SqlCommand(insertQry, connectionStr);

                cmd.Parameters.Add(new SqlParameter("@emp_id", SqlDbType.VarChar)).Value = txtEmpId.Text;
                cmd.Parameters.Add(new SqlParameter("@emp_name", SqlDbType.VarChar)).Value = txtName.Text;
                cmd.Parameters.Add(new SqlParameter("@emp_sur", SqlDbType.VarChar)).Value = txtSurname.Text;
                cmd.Parameters.Add(new SqlParameter("@emp_dob", SqlDbType.Date)).Value = dateOfbirth.Value;
                cmd.Parameters.Add(new SqlParameter("@cont_no", SqlDbType.NChar)).Value = txtContactno.Text;
                cmd.Parameters.Add(new SqlParameter("@addr", SqlDbType.VarChar)).Value = txtAddress.Text;
                cmd.Parameters.Add(new SqlParameter("@posi", SqlDbType.VarChar)).Value = txtPosition.Text;
                cmd.Parameters.Add(new SqlParameter("@email", SqlDbType.VarChar)).Value = txtEmailAdd.Text;
                cmd.ExecuteNonQuery();
                MessageBox.Show("Employee info saved successful...!\n" + txtEmpId.Text, "New Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Autocomplete();
                userFrm.txtRole.Text = this.txtPosition.Text;
                userFrm.txtEmp.Text = this.txtEmpId.Text;
                this.Close();
                userFrm.Show();
                userFrm.txtRole.Enabled = false;
                reset();
                connectionStr.Close();
                cmd.Dispose();
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "")
            {
                MessageBox.Show("Please enter employee name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtName.Focus();
                return;
            }
            if (txtSurname.Text == "")
            {
                MessageBox.Show("Please enter employee surname", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSurname.Focus();
                return;
            }
            if (dateOfbirth.Value.Year > System.DateTime.Now.Year || dateOfbirth.Value.Year == System.DateTime.Now.Year)
            {
                MessageBox.Show("Please enter correct birth year", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (txtContactno.Text == "")
            {
                MessageBox.Show("Please enter Contact no.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtContactno.Focus();
                return;
            }
            if (txtAddress.Text == "")
            {
                MessageBox.Show("Please enter address.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtAddress.Focus();
                return;
            }
            if (txtEmailAdd.Text == "")
            {
                MessageBox.Show("Please enter email.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtEmailAdd.Focus();
                return;
            }

            try
            {
                connectionStr = new SqlConnection(conStr.connectString);
                connectionStr.Open();
                string updateQry = "update tblemployee set emp_name=@N, emp_surname=@S, emp_dobirth=@D, contact_no=@C, address=@A, position=@P, email=@E where emp_id=@id";
                cmd = new SqlCommand(updateQry, connectionStr);

                cmd.Parameters.Add(new SqlParameter("@N", SqlDbType.VarChar)).Value = txtName.Text;
                cmd.Parameters.Add(new SqlParameter("@S", SqlDbType.VarChar)).Value = txtSurname.Text;
                cmd.Parameters.Add(new SqlParameter("@D", SqlDbType.Date)).Value = dateOfbirth.Value;
                cmd.Parameters.Add(new SqlParameter("@C", SqlDbType.NChar)).Value = txtContactno.Text;
                cmd.Parameters.Add(new SqlParameter("@A", SqlDbType.VarChar)).Value = txtAddress.Text;
                cmd.Parameters.Add(new SqlParameter("@P", SqlDbType.VarChar)).Value = txtPosition.Text;
                cmd.Parameters.Add(new SqlParameter("@E", SqlDbType.VarChar)).Value = txtEmailAdd.Text;
                cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.VarChar)).Value = txtEmpId.Text;
                cmd.ExecuteNonQuery();

                MessageBox.Show("Employee details updated successfully.", "Update Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Autocomplete();
                reset();
                cmd.Dispose();
                connectionStr.Close();
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void txtContactno_KeyPress(object sender, KeyPressEventArgs e)
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
        private void txtContactno_Validate(object sender, CancelEventArgs e)
        {
            if (txtContactno.TextLength < 10)
            {
                MessageBox.Show("Contact no. digits cannot be less than 10", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtContactno.Focus();
            }
        }
        private void delete_record()
        {
            if (txtEmpId.Text == "")
            {
                MessageBox.Show("Please enter employee id.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtName.Focus();
                return;
            }
            try
            {
                int rows_affected = 0;
                connectionStr = new SqlConnection(conStr.connectString);
                connectionStr.Open();
                string deleteQry = "delete from tblEmployee where emp_id=@id";                
                cmd = new SqlCommand(deleteQry, connectionStr);
                 
                //Delete tblEmployee data
                cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.VarChar)).Value = txtEmpId.Text;
                rows_affected = cmd.ExecuteNonQuery();

                if (rows_affected > 0)
                {
                    MessageBox.Show("Employee record deleted successfully.", "Delete Employee", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Autocomplete();
                    //reset();
                }
                else
                {
                    MessageBox.Show("No record found.", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    reset();
                    Autocomplete();
                }

            }
            catch
            {
            }
            connectionStr.Close();
        }
        private void delete_user()
        {
            int row_affected = 0;
            connectionStr = new SqlConnection(conStr.connectString);
            connectionStr.Open();
            string deleteQry = "delete from tblusers where emp_id=@para";
            cmd = new SqlCommand(deleteQry, connectionStr);

            cmd.Parameters.Add(new SqlParameter("@para", SqlDbType.VarChar)).Value = txtEmpId.Text;
            
            row_affected = cmd.ExecuteNonQuery();

            if (row_affected > 0)
            {
                MessageBox.Show("User deleted permanently..!", "Delete User", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Autocomplete();
                reset();
            }
            else
            {
                MessageBox.Show("No user record found", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                reset();
                Autocomplete();
            }
            connectionStr.Close();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                delete_record();
                delete_user();
            }


        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            reset();
        }

        private void frmEmployee_Load(object sender, EventArgs e)
        {
            Autocomplete();
        }
        //private void frmClosing(object sender, FormClosingEventArgs e)
        //{
        //    //this.Hide();
            
        //}

        private void btnGetAll_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmEmpRecords frm = new frmEmpRecords();
            frm.Show();
        }

        private void txtEmpId_TextChanged(object sender, EventArgs e)
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