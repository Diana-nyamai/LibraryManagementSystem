using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using LibrarySystem.LibraryDBDataSetTableAdapters;

namespace LibrarySystem
{
    public partial class frmBookRpt : Form
    {
        public frmBookRpt()
        {
            InitializeComponent();
        }
        

        private void frmBookRpt_Load(object sender, EventArgs e)
        {
            ReportDataSource reportSource;
            this.reportViewer1.LocalReport.ReportPath = "BooksReport.rdlc";

            try
            {
                tblBooksTableAdapter da = new tblBooksTableAdapter();
                LibraryDBDataSet ds = new LibraryDBDataSet();

                da.Fill(ds.tblBooks);
                reportSource = new ReportDataSource("LibraryDBDataSet", ds.Tables["tblBooks"]);

                this.reportViewer1.LocalReport.DataSources.Add(reportSource);
                this.reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
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
