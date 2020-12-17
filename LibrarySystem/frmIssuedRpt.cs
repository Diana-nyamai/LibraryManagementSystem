using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using LibrarySystem.IssuedBooksDataSetTableAdapters;

namespace LibrarySystem
{
    public partial class frmIssuedRpt : Form
    {
        public frmIssuedRpt()
        {
            InitializeComponent();
        }
     
        private void frmIssued_Load(object sender, EventArgs e)
        {

            ReportDataSource rpt;

            try
            {
                this.reportViewer1.LocalReport.ReportPath = "Report1.rdlc";
                tblIssue_BooksTableAdapter da = new tblIssue_BooksTableAdapter();
                IssuedBooksDataSet ds = new IssuedBooksDataSet();

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
