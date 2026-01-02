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
    public partial class Form1 : Form
    {
        // Khai báo các Service (Giống mẫu frmStudent)
        private readonly NhanVienService nhanVienService = new NhanVienService();
        private readonly CongTyService congTyService = new CongTyService();

        public Form1()
        {
            InitializeComponent();
            LoadData();
            SetupEvents();
        }

        private void LoadData()
        {
            try
            {
                var listCongTy = congTyService.GetAll();
                var listNhanVien = nhanVienService.GetAll();

                FillCongTyCombobox(listCongTy);
                
                dgvNhanVien.Columns.Clear();
                dgvNhanVien.Columns.Add("CMND_CCCD", "CMND/CCCD");
                dgvNhanVien.Columns.Add("HoVaTen", "Họ và Tên");
                dgvNhanVien.Columns.Add("SoLanXN", "Số lần XN");
                dgvNhanVien.Columns.Add("KetQua", "Kết Quả");
                
                BindGrid(listNhanVien);
                
                groupBox2.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi");
            }
        }

        private void FillCongTyCombobox(List<CONGTY> listCongTy)
        {
            cboCongTy.DataSource = listCongTy;
            cboCongTy.DisplayMember = "TenCty"; 
            cboCongTy.ValueMember = "MaCty";    
        }

        private void BindGrid(List<NHANVIEN> listNhanVien)
        {
            dgvNhanVien.Rows.Clear();
            foreach (var item in listNhanVien)
            {
                int index = dgvNhanVien.Rows.Add();
                dgvNhanVien.Rows[index].Cells[0].Value = item.ID;
                dgvNhanVien.Rows[index].Cells[1].Value = item.HoTen;
                dgvNhanVien.Rows[index].Cells[2].Value = item.SoLanXN;
                
                dgvNhanVien.Rows[index].Cells[3].Value = item.AmTinh ? "Âm Tính" : "+";
            }
        }

        private void SetupEvents()
        {
            btnTim.Click += BtnTim_Click;
            btnCapNhat.Click += BtnCapNhat_Click;
            
            danhSáchNVDươngTínhToolStripMenuItem.Click += DanhSáchNVDươngTínhToolStripMenuItem_Click;
            danhSáchCtyĐãTestTheoYCToolStripMenuItem.Click += DanhSáchCtyĐãTestTheoYCToolStripMenuItem_Click;
            xuấtBáoCáoToolStripMenuItem.Click += XuấtBáoCáoToolStripMenuItem_Click;
        }

        private void BtnTim_Click(object sender, EventArgs e)
        {
            string id = txtID.Text.Trim();
            
            if (string.IsNullOrEmpty(id) || (id.Length != 12 && id.Length != 9))
            {
                MessageBox.Show("Vui lòng nhập CCCD hoặc CMND", "Thông báo");
                return;
            }
            
            if (!id.All(char.IsDigit))
            {
                MessageBox.Show("ID chỉ là các kí tự số", "Thông báo");
                return;
            }

            try
            {
                var nhanVien = nhanVienService.FindById(id);
                
                groupBox2.Enabled = true;
                
                if (nhanVien != null)
                {
                    txtHoTen.Text = nhanVien.HoTen;
                    txtSLNV.Text = (nhanVien.SoLanXN + 1).ToString();
                    
                    if (nhanVien.AmTinh)
                        radAmTinh.Checked = true;
                    else
                        radDuongTinh.Checked = true;
                    
                    cboCongTy.SelectedValue = nhanVien.MaCty;
                }
                else
                {
                    txtHoTen.Text = "";
                    txtSLNV.Text = "1"; 
                    radAmTinh.Checked = true; 
                    cboCongTy.SelectedIndex = 0; 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi");
            }
        }

        private void BtnCapNhat_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtID.Text) || string.IsNullOrEmpty(txtHoTen.Text))
                return;

            try
            {
                var existingNV = nhanVienService.FindById(txtID.Text);
                
                var nhanVien = new NHANVIEN
                {
                    ID = txtID.Text,
                    HoTen = txtHoTen.Text,
                    SoLanXN = int.Parse(txtSLNV.Text),
                    AmTinh = radAmTinh.Checked,
                    MaCty = cboCongTy.SelectedValue?.ToString()
                };

                nhanVienService.InsertUpdate(nhanVien);
                
                if (existingNV == null)
                    MessageBox.Show("Thêm mới thành công!", "Thông báo");
                else
                    MessageBox.Show("Cập nhật thành công!", "Thông báo");
                
                var listNhanVien = nhanVienService.GetAll();
                BindGrid(listNhanVien);
                
                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi");
            }
        }
        
        private void ResetForm()
        {
            groupBox2.Enabled = false;
            txtID.Text = "";
            txtHoTen.Text = "";
            txtSLNV.Text = "";
            radAmTinh.Checked = false;
            radDuongTinh.Checked = false;
            if (cboCongTy.Items.Count > 0)
                cboCongTy.SelectedIndex = 0;
        }

        private void DanhSáchNVDươngTínhToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy danh sách nhân viên dương tính
                var listDuongTinh = nhanVienService.GetDSDuongTinh();
                BindGrid(listDuongTinh);
                
                MessageBox.Show($"Hiển thị {listDuongTinh.Count} nhân viên dương tính", "Thông báo");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi");
            }
        }

        private void DanhSáchCtyĐãTestTheoYCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var dsCongTyDuYC = nhanVienService.GetCongTyDaTestDuYC();
                
                if (dsCongTyDuYC.Count > 0)
                {
                    string message = "Danh sách công ty đã tham gia test đủ theo Y/C:\n\n";
                    for (int i = 0; i < dsCongTyDuYC.Count; i++)
                    {
                        message += $"{i + 1}. {dsCongTyDuYC[i]}\n";
                    }
                    MessageBox.Show(message, "Danh sách công ty (F2)");
                }
                else
                {
                    MessageBox.Show("Không có công ty nào đã test đủ theo yêu cầu!", "Thông báo");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi");
            }
        }

        private void XuấtBáoCáoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBaoCao baoCaoForm = new frmBaoCao();
            baoCaoForm.ShowDialog();
        }
    }
}
