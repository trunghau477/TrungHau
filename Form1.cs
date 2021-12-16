using Bai6.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
// hihi
//haha
namespace Bai6
{
    public partial class Form1 : Form
    {
        QLSachDB context = new QLSachDB();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                List<LoaiSach> listLoaiSach = context.LoaiSaches.ToList();
                List<Sach> listSach = context.Saches.ToList();
                FillLoaiSachCombobox(listLoaiSach);
                BindGrid(listSach);
            }
            
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FillLoaiSachCombobox(List<LoaiSach> listLoaiSach)
        {
            cmbLoaiSach.DataSource = listLoaiSach;
            cmbLoaiSach.DisplayMember = "TenLoai";
            cmbLoaiSach.ValueMember = "MaLoai";
        }
        private void BindGrid(List<Sach> listSach)
        {
            dgvSach.Rows.Clear();
            foreach (var item in listSach)
            {
                int index = dgvSach.Rows.Add();
                dgvSach.Rows[index].Cells[0].Value = item.MaSach;
                dgvSach.Rows[index].Cells[1].Value = item.TenSach;
                dgvSach.Rows[index].Cells[2].Value = item.NamXB;
                dgvSach.Rows[index].Cells[3].Value = item.LoaiSach.TenLoai;
            }
        }
        private void reloadDGV()
        {
            List<Sach> listSach = context.Saches.ToList();
            BindGrid(listSach);
        }
        private void refresh()
        {
            txtMaSach.Text = "";
            txtTenSach.Text = "";
            txtNamXB.Text = "";
            cmbLoaiSach.SelectedIndex = 0;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult h = MessageBox.Show
                ("Bạn có chắc muốn thoát không?", "Error", MessageBoxButtons.OKCancel);
            if (h == DialogResult.OK)
                Application.Exit();
        }
        private int GetSelectedRow(string maSach)
        {
            for (int i = 0; i < dgvSach.Rows.Count; i++)
            {
                if (dgvSach.Rows[i].Cells[0].Value != null) //them dong nay de check null dgv
                {
                    if (dgvSach.Rows[i].Cells[0].Value.ToString() == maSach)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
        //Check Validate nhập liệu
        private bool CheckValidate()
        {
            if (txtMaSach.Text == "" || txtTenSach.Text == "" || txtNamXB.Text == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin Sách", "Thông báo", MessageBoxButtons.OK);
                return false;
            }
            else if (txtMaSach.TextLength != 4)
            {
                MessageBox.Show("Mã Sách phải có 4 kí tự!", "Thông báo", MessageBoxButtons.OK);
                return false;
            }
            return true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (CheckValidate())
            {
                if (GetSelectedRow(txtMaSach.Text) == -1)
                {
                    Sach s = new Sach()
                    {
                        MaSach = txtMaSach.Text,
                        TenSach = txtTenSach.Text,
                        NamXB = int.Parse(txtNamXB.Text),
                        MaLoai = int.Parse(cmbLoaiSach.SelectedValue.ToString())
                    };

                    context.Saches.AddOrUpdate(s);
                    context.SaveChanges();

                    reloadDGV();
                    refresh();
                    MessageBox.Show("Thêm Sách mới thành công!", "Thông báo", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show("Sachs đã tồn tại", "Thông báo", MessageBoxButtons.OK);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //Lấy Cuon sach đầu tiên có MaSach = ID cho trước
            Sach dbDelete = context.Saches.FirstOrDefault(p => p.MaSach == txtMaSach.Text);
            if (dbDelete != null)
            {
                DialogResult dr = MessageBox.Show("Bạn có muốn xoá?", "Yes/No", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    context.Saches.Remove(dbDelete);
                    context.SaveChanges(); //Lưu thay dổi
                    reloadDGV();
                    refresh();
                    MessageBox.Show("Xóa Sách thành công!", "Thông báo", MessageBoxButtons.OK);
                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy Mã Sách cần xóa!", "Thông báo", MessageBoxButtons.OK);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (CheckValidate())
            {
                
                Sach dbUpdate = context.Saches.FirstOrDefault(p => p.MaSach == txtMaSach.Text);

                if (dbUpdate != null)
                {
                    dbUpdate.TenSach = txtTenSach.Text;
                    dbUpdate.NamXB = int.Parse(txtNamXB.Text);
                    dbUpdate.MaLoai = int.Parse(cmbLoaiSach.SelectedValue.ToString());

                    context.Saches.AddOrUpdate(dbUpdate);
                    context.SaveChanges(); //Lưu thay đổi
                    reloadDGV();
                    refresh();
                    MessageBox.Show("Cập nhật dữ liệu thành công!”.", "Thông báo", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy Max Sách cần sửa!", "Thông báo", MessageBoxButtons.OK);
                }
            }
        }

        private void dgvSach_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvSach.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    dgvSach.CurrentRow.Selected = true;

                    txtMaSach.Text = dgvSach.Rows[e.RowIndex].Cells[0].FormattedValue.ToString();
                    txtTenSach.Text = dgvSach.Rows[e.RowIndex].Cells[1].FormattedValue.ToString();
                    txtNamXB.Text = dgvSach.Rows[e.RowIndex].Cells[2].FormattedValue.ToString();
                    cmbLoaiSach.SelectedIndex = cmbLoaiSach.FindString(dgvSach.Rows[e.RowIndex].Cells[3].FormattedValue.ToString());
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void thốngKêSáchTheoNămToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Show();
        }

        private void txtSearchSach_TextChanged(object sender, EventArgs e)
        {
            List<Sach> listSachSearch = new List<Sach>();
            List<Sach> listSach = context.Saches.ToList();
            string searchBook = txtSearchSach.Text;
            if (searchBook != "")
            {
                foreach (Sach item in listSach)
                {
                    if (item.TenSach.ToLower().Contains(searchBook.ToLower()))
                    {
                        listSachSearch.Add(item);
                    }
                }
                BindGrid(listSachSearch);
            }
            else
            {
                BindGrid(listSach);
            }
        }
    }
 }

