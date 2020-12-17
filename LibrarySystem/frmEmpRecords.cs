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
    public partial class frmEmpRecords : Form
    {
        ConnectionString connStr = new ConnectionString();
        public frmEmpRecords()
        {
            InitializeComponent();
        }

        private DataView GetAll()
        {
            string selectQry = "select (emp_id)as[Employee ID], (emp_name)as[Name], (emp_surname)as[Surname], (emp_dobirth)as[Date of Birth], (contact_no)as[Contact no.], (address)as[Address], (position)as[Position], (email)as[E-mail] from tblEmployee";
            DataSet ds = new DataSet();
            DataView tableView = null;
            try
            {
                SqlConnection connect = new SqlConnection(connStr.connectString);
                connect.Open();
                SqlCommand cmd = new SqlCommand(selectQry, connect);
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(ds, "tblEmployee");
                tableView = ds.Tables[0].DefaultView;

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return tableView;
        }
        private void frmEmpRecords_FromClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            frmEmployee frm = new frmEmployee();
            frm.Show();
        }
        private void frmEmpRecords_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = GetAll();
        }
        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewRow row = dataGridView1.SelectedRows[0];
            this.Hide();
            frmEmployee frmEmp = new frmEmployee();

            frmEmp.Show();
            frmEmp.txtEmpId.Text = row.Cells[0].Value.ToString();
            frmEmp.txtName.Text = row.Cells[1].Value.ToString();
            frmEmp.txtSurname.Text = row.Cells[2].Value.ToString();
            frmEmp.dateOfbirth.Value = Convert.ToDateTime(row.Cells[3].Value.ToString());
            frmEmp.txtContactno.Text = row.Cells[4].Value.ToString();
            frmEmp.txtAddress.Text = row.Cells[5].Value.ToString();
            frmEmp.txtPosition.Text = row.Cells[6].Value.ToString();
            frmEmp.txtEmailAdd.Text = row.Cells[7].Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Refresh();
        }

    }
}
