using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SAR.DAL.Entities;

namespace SAR.GUI
{
    public partial class frmReportViewer : Form
    {
        private List<NHANVIEN> nhanVienList;
        private CONGTY congTy;

        public frmReportViewer(List<NHANVIEN> nhanVienList, CONGTY congTy)
        {
            InitializeComponent();
            this.nhanVienList = nhanVienList;
            this.congTy = congTy;
            LoadReport();
        }

        private void LoadReport()
        {
            lblTieuDe.Text = "PHIẾU KẾT QUẢ XÉT NGHIỆM";
            lblCongTy.Text = $"Công Ty: {congTy.TenCty}";

            dgvReport.Rows.Clear();
            dgvReport.Columns.Clear();
            
            dgvReport.Columns.Add("CCCD_CMND", "CCCD/CMND");
            dgvReport.Columns.Add("HoVaTen", "Họ Và Tên");
            dgvReport.Columns.Add("KetQua", "Kết Quả");

            foreach (var nv in nhanVienList)
            {
                int index = dgvReport.Rows.Add();
                dgvReport.Rows[index].Cells[0].Value = nv.ID;
                dgvReport.Rows[index].Cells[1].Value = nv.HoTen;
                dgvReport.Rows[index].Cells[2].Value = nv.AmTinh ? "Âm Tính" : "Dương Tính";
            }
        }
    }
}