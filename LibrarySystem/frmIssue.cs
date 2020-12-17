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
    public partial class frmIssue : Form
    {
        ConnectionString conStr = new ConnectionString();
        SqlConnection connect = null;
        SqlCommand cmd = null;
        SqlDataAdapter da = null;
        SqlDataReader reader = null;
        public frmIssue()
        {
            InitializeComponent();
        }
        private string GetUniqueKey(int maxSize)
        {
            char[] chars = new char[60];
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
        private void GetData()
        {
            try
            {
                connect = new SqlConnection(conStr.connectString);
                connect.Open();
                string selectQry = "select issue_id, book_id, member_id, status from tblIssue_Books where member_id='" + txtMem_id.Text + "'";
                DataSet ds = new DataSet();
                cmd = new SqlCommand(selectQry, connect);
                da = new SqlDataAdapter(cmd);

                da.Fill(ds, "tblIssue_Books");
                dataGridView1.DataSource = ds.Tables["tblIssue_Books"].DefaultView;

                connect.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void reset()
        {
            cmbBTitle.SelectedIndex = -1;
            txtMem_id.Text = "";
            listView1.Items.Clear();
            dataGridView1.DataSource = null;
            
            cmbBTitle.Focus();
        }
        public string emp;
        private void frmIssue_Load(object sender, EventArgs e)
        {
            frmMain main = new frmMain();
            
            SystemSettings sysSettings = new SystemSettings(emp);
            this.txtEmp_id.Text = sysSettings.display();
            try
            {

                connect = new SqlConnection(conStr.connectString);
                connect.Open();
                string selectQry = "select distinct RTRIM(book_title) from tblbooks";
                DataSet ds = null;
                cmd = new SqlCommand(selectQry, connect);
                da = new SqlDataAdapter(cmd);
                DataTable dt = null;

                ds = new DataSet("ds");
                da.Fill(ds);
                dt = ds.Tables[0];
                cmbBTitle.Items.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    cmbBTitle.Items.Add(row[0]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            if (cmbBTitle.SelectedIndex == -1)
            {
                MessageBox.Show("Select a book to issue out.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmbBTitle.Focus();
                return;
            }
            if (int.Parse(txtQuantity.Text) <= 0)
            {
                MessageBox.Show("Book is not available.", "Library System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmbBTitle.Focus();
                return;
            }
            if (txtMem_id.Text == "")
            {
                MessageBox.Show("Enter membership id.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMem_id.Focus();
                return;
            }
            if (txtEmp_id.Text == "")
            {
                MessageBox.Show("Enter Employee id.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtEmp_id.Focus();
                return;
            }
            try
            {


                connect = new SqlConnection(conStr.connectString);

                //Check if borrower has not returned the book
                connect.Open();
                string find = "select * from tblIssue_Books where book_id=@value1 and member_id=@value2";
                cmd = new SqlCommand(find, connect);

                cmd.Parameters.Add(new SqlParameter("@value1", SqlDbType.VarChar)).Value = book_id;
                cmd.Parameters.Add(new SqlParameter("@value2", SqlDbType.VarChar)).Value = txtMem_id.Text;
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    if (reader.GetString(7) == "issued_out")
                    {
                        MessageBox.Show("Member Id: '" + txtMem_id.Text + "' has not returned the book", "Library System", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        reader.Close();
                        
                        return;
                    }
                }

                connect.Close();
                //insert into tblIssue_Books table              
                //connect = new SqlConnection(conStr.connectString);                    
                connect.Open();

                DateTime date = new DateTime();
                date = dateTimePicker1.Value.AddDays(14);
                string insertQry = "insert into tblissue_books values(@data1,@data2,@data3,@data4,@data5,@data6,@data7,@data8)";

                cmd = new SqlCommand(insertQry, connect);

                //Check if item on the listView is already listed
                for (int i = 0; i <= listView1.Items.Count - 1; i++)
                {
                    if (listView1.Items[i].SubItems[1].Text == cmbBTitle.Text)
                    {
                        MessageBox.Show("Book already issued out to Member: " + txtMem_id.Text, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        cmbBTitle.SelectedIndex = -1;
                        return;
                    }
                }

                cmd.Parameters.Add(new SqlParameter("@data1", SqlDbType.VarChar)).Value = "ISS" + GetUniqueKey(4);
                cmd.Parameters.Add(new SqlParameter("@data2", SqlDbType.Date)).Value = System.DateTime.Now;
                cmd.Parameters.Add(new SqlParameter("@data3", SqlDbType.Date)).Value = date;
                cmd.Parameters.Add(new SqlParameter("@data4", SqlDbType.VarChar)).Value = book_id;
                cmd.Parameters.Add(new SqlParameter("@data5", SqlDbType.VarChar)).Value = cmbBTitle.SelectedItem;
                cmd.Parameters.Add(new SqlParameter("@data6", SqlDbType.VarChar)).Value = txtMem_id.Text;
                cmd.Parameters.Add(new SqlParameter("@data7", SqlDbType.VarChar)).Value = txtEmp_id.Text;
                cmd.Parameters.Add(new SqlParameter("@data8", SqlDbType.VarChar)).Value = "issued_out";
                cmd.ExecuteNonQuery();
                   
                MessageBox.Show("Book Issued", "Book Issue", MessageBoxButtons.OK, MessageBoxIcon.Information);
                GetData();
                cmd.Dispose();
                connect.Close();

                //update tblBooks quantity
                connect = new SqlConnection(conStr.connectString);
                connect.Open();
                string updateString = "update tblbooks set quantity=quantity - 1 where book_id=@book_id";
                cmd = new SqlCommand(updateString, connect);

                cmd.Parameters.Add(new SqlParameter("@book_id", SqlDbType.VarChar)).Value = book_id;
                cmd.ExecuteNonQuery();

                cmd.Dispose();
                connect.Close();

                //Add infomation on the listView1
                if (listView1.Items.Count == 0)
                {
                    ListViewItem item = new ListViewItem();
                    item.SubItems[0].Text = book_id.ToString();
                    item.SubItems.Add(cmbBTitle.Text);
                    item.SubItems.Add(book_author);
                    item.SubItems.Add(date.ToString("dd-MMM-yyyy"));
                    listView1.Items.Add(item);
                    cmbBTitle.SelectedIndex = -1;
                    return;
                }

   

                ListViewItem item2 = new ListViewItem();
                item2.SubItems[0].Text = book_id;
                item2.SubItems.Add(cmbBTitle.Text);
                item2.SubItems.Add(book_author);
                item2.SubItems.Add(date.ToString("dd-MMM-yyyy"));
                listView1.Items.Add(item2);
                cmbBTitle.SelectedIndex = -1;
                return;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            reset();
        }
        static string book_id, book_author;
        private void cmbBTitle_SelectedIndexChanged(object sender, EventArgs e)
        {
            connect = new SqlConnection(conStr.connectString);

            //Get book_id and author
            connect.Open();
            string selectQry = "select book_id,book_title,author,quantity from tblbooks where book_title='" + cmbBTitle.SelectedItem + "'";
            cmd = new SqlCommand(selectQry, connect);

            //cmd.Parameters.Add(new SqlParameter("@title", SqlDbType.VarChar)).Value = cmbBTitle.SelectedItem;

            //string book_id = "", book_author = "";
            int quantity = 0;
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                book_id = reader.GetString(0).ToString();
                book_author = reader.GetString(2).ToString();
                quantity = reader.GetInt32(3);
            }
            txtQuantity.Text = quantity.ToString();
            //cmd.Dispose();
            reader = null;
            connect.Close();

        }
        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (cmbBTitle.SelectedIndex == -1)
            {
                MessageBox.Show("Select a book to return back.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (txtMem_id.Text == "")
            {
                MessageBox.Show("Enter membership id", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMem_id.Focus();
                return;
            }
            try
            {
                connect = new SqlConnection(conStr.connectString);
                connect.Open();

                string updateStr = "update tblIssue_Books set status = 'returned back' where book_title=@title and member_id=@mem_id";
                cmd = new SqlCommand(updateStr, connect);
                cmd.Parameters.Add(new SqlParameter("@title", SqlDbType.VarChar)).Value = cmbBTitle.SelectedItem;
                cmd.Parameters.Add(new SqlParameter("@mem_id", SqlDbType.VarChar)).Value = txtMem_id.Text;
                cmd.ExecuteNonQuery();
                MessageBox.Show("Book returned back", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                connect.Close();

                connect = new SqlConnection(conStr.connectString);
                
                connect.Open();

                string updateStr2 = "update tblBooks set quantity=quantity + 1 where book_title=@book";
                cmd = new SqlCommand(updateStr2, connect);
                cmd.Parameters.Add(new SqlParameter("@book", SqlDbType.VarChar)).Value = cmbBTitle.SelectedItem;
                cmd.ExecuteNonQuery();
                reset();
                GetData();
                connect.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void txtMem_id_TextChanged(object sender, EventArgs e)
        {
           GetData();
        }

    }
}
