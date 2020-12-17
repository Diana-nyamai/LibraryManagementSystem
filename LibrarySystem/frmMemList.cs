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
    public partial class frmMemList : Form
    {
        public frmMemList()
        {
            InitializeComponent();
        }
        ConnectionString conStr = new ConnectionString();
        private DataView GetData()
        {
            string selectQry = "select (member_id) as [Member ID], (member_name) as [Name], (member_surname) as [Surname], (address) as [Address], (contact_no) as [Contact no.], (dob) as [Date of Birth] from tblmembers";
            DataSet ds = new DataSet();
            DataView tableView = null;
            try
            {
                SqlConnection connect = new SqlConnection(conStr.connectString);
                connect.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                SqlCommand tempCmd = new SqlCommand(selectQry, connect);

                da.SelectCommand = tempCmd;
                da.Fill(ds);
                tableView = ds.Tables[0].DefaultView;

                
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return tableView;

        }
        private void frmMemList_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = GetData();
        }

        private void frmMemList_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            frmMember frm = new frmMember();
            frm.Show();
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                DataGridViewRow row = dataGridView1.SelectedRows[0];
                this.Hide();
                frmMember frmMLst = new frmMember();                
                frmMLst.Show();

                frmMLst.txtMemberId.Text = row.Cells[0].Value.ToString();
                frmMLst.txtMemName.Text = row.Cells[1].Value.ToString();
                frmMLst.txtMemSurname.Text = row.Cells[2].Value.ToString();
                frmMLst.txtAddress.Text = row.Cells[3].Value.ToString();
                frmMLst.txtCell.Text = row.Cells[4].Value.ToString();
                frmMLst.dtpDateOfBirth.Text = row.Cells[5].Value.ToString();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
