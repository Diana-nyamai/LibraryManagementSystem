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
    public partial class frmCategoryList : Form
    {
        public frmCategoryList()
        {
            InitializeComponent();
        }

        ConnectionString con = new ConnectionString();
        private DataView getData()
        {
            string selectStr = "select (id)as[ID], (category)as[Category] from tblCategory";
            DataSet ds = new DataSet();
            DataView tableView = null;

            try
            {
                SqlConnection connect = new SqlConnection(con.connectString);
                connect.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                SqlCommand cmd = new SqlCommand(selectStr, connect);

                da.SelectCommand = cmd;
                da.Fill(ds);
                tableView = ds.Tables[0].DefaultView;

               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return tableView;
        }

        private void frmCategoryList_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = getData();
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {

                DataGridViewRow row = dataGridView1.SelectedRows[0];
                this.Hide();
                frmCategory frmCate = new frmCategory();
                frmCate.Show();

                frmCate.txtId.Text = row.Cells[0].Value.ToString();
                frmCate.txtCategory.Text = row.Cells[1].Value.ToString();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
