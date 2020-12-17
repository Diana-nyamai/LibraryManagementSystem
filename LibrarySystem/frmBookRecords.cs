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
    public partial class frmBookRecords : Form
    {
        public frmBookRecords()
        {
            InitializeComponent();
        }
        ConnectionString con = new ConnectionString();
        private DataView GetAll()
        {
            string selectQry = "select (book_id)as[Book Id], (book_title)as[Title], (Author)as[Author], (Publisher)as[Publisher], (Category)as[Category], (Quantity)as[Quantity] from tblbooks";
            DataSet ds = new DataSet();
            DataView tableView = null;
            try
            {
                SqlConnection connect = new SqlConnection(con.connectString);
                connect.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                SqlCommand sampleCmd = new SqlCommand(selectQry,connect);

                da.SelectCommand = sampleCmd;
                da.Fill(ds);

                tableView = ds.Tables[0].DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return tableView;
        }
        private void frmBookRecords_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = GetAll();
        }
        private void frmBookRec_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            frmBookInfo frmBookIn = new frmBookInfo();
            frmBookIn.Show();
            
        }

        private void dataGridView1_RowMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                DataGridViewRow row = dataGridView1.SelectedRows[0];
                this.Hide();
                frmBookInfo frmBook = new frmBookInfo();
                
                frmBook.Show();


                frmBook.txtBookId.Text = row.Cells[0].Value.ToString();
                frmBook.txtTitle.Text = row.Cells[1].Value.ToString();
                frmBook.txtAuthor.Text = row.Cells[2].Value.ToString();
                frmBook.txtPublisher.Text = row.Cells[3].Value.ToString();
                frmBook.cmbCategory.Text = row.Cells[4].Value.ToString();
                frmBook.nudQuantity.Value = Convert.ToDecimal(row.Cells[5].Value.ToString());
                
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
