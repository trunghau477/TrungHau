using Bai6.Model;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bai6
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            QLSachDB context = new QLSachDB();
            List<Sach> listStudent = context.Saches.ToList();
            List<SachReport> listReport = new List<SachReport>();

            foreach (Sach student in listStudent)
            {

                SachReport studentReport = new SachReport();
                studentReport.MaSach = student.MaSach;
                studentReport.TenSach = student.TenSach;
                studentReport.NamXB = student.NamXB;
                studentReport.TenLoai = student.LoaiSach.TenLoai;
                listReport.Add(studentReport);

            }

            rptViewer1.LocalReport.ReportPath = "rptSach.rdlc";
            var source = new ReportDataSource("SachDataSet", listReport);
            rptViewer1.LocalReport.DataSources.Clear();
            rptViewer1.LocalReport.DataSources.Add(source);
            this.rptViewer1.RefreshReport();
        }
    }
}
