using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LibrarySystem.IssuedBooksDataSetTableAdapters;
using Microsoft.Reporting.WinForms;

namespace LibrarySystem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        ConnectionString conStr = new ConnectionString();
        SqlConnection connect = null;
        SqlCommand cmd = null;
        SqlDataAdapter da = null;
        public string id; 
        private void Form1_Load(object sender, EventArgs e)
        {
            frmSearch frm = new frmSearch();
            try
            {
                ReportDataSource rpt;
                this.reportViewer1.LocalReport.ReportPath = "Report2.rdlc";
                connect = new SqlConnection(conStr.connectString);
                connect.Open();

                string sel = "select * from tblIssue_Books where member_id='" + id + "'";
                cmd = new SqlCommand(sel, connect);
                da = new SqlDataAdapter(cmd);
                IssuedBooksDataSet ds = new IssuedBooksDataSet();
                cmd.Parameters.Add(new SqlParameter("@VALUE", SqlDbType.VarChar)).Value = frm.txtMemid.Text;

                da.Fill(ds.tblIssue_Books);
                rpt = new ReportDataSource("IssuedBooksDataSet", ds.Tables["tblIssue_Books"]);
                this.reportViewer1.LocalReport.DataSources.Add(rpt);
                this.reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
                this.reportViewer1.ZoomMode = ZoomMode.Percent;
                this.reportViewer1.ZoomPercent = 50;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.reportViewer1.RefreshReport();
        }
    }
}
