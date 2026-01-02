using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SAR.BUS;
using SAR.DAL.Entities;

namespace SAR.GUI
{
    public partial class frmBaoCao : Form
    {
        private readonly CongTyService congTyService = new CongTyService();
        private readonly NhanVienService nhanVienService = new NhanVienService();

        public frmBaoCao()
        {
            InitializeComponent();
            LoadCongTy();
        }

        private void LoadCongTy()
        {
            try
            {
                var listCongTy = congTyService.GetAll();
                cboCongTy.DataSource = listCongTy;
                cboCongTy.DisplayMember = "TenCty";
                cboCongTy.ValueMember = "MaCty";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi");
            }
        }

        private void btnXem_Click(object sender, EventArgs e)
        {
            try
            {
                string maCty = cboCongTy.SelectedValue?.ToString();
                if (string.IsNullOrEmpty(maCty))
                    return;

                var nhanVienList = nhanVienService.GetAll()
                    .Where(nv => nv.MaCty == maCty).ToList();

                var congTy = (CONGTY)cboCongTy.SelectedItem;

                frmReportViewer reportViewer = new frmReportViewer(nhanVienList, congTy);
                reportViewer.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi");
            }
        }
    }
}