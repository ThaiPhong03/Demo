using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TVSACH1
{
    public partial class F8_TimKiemALL : Form
    {
        private string ketnoi = "Data Source=NờTêPê\\THAIPHONG;Initial Catalog=QLTVS;Integrated Security=True;Encrypt=False";
        private string loaiTimKiem = ""; // Biến để lưu loại tìm kiếm (ở đây là theo mã sách)
        public F8_TimKiemALL()
        {
            InitializeComponent();
            capnhatdataSach();
            capnhatdataDocGia();
            capnhatdataTacGia();

        }
        private void rdMasach_CheckedChanged(object sender, EventArgs e)
        {
            if (rdMasach.Checked)
            {
                loaiTimKiem = "MaSach"; // Cập nhật loại tìm kiếm là theo mã sách
                lbThongbaoS.Text = "";
                txtTimkiem.Clear();
            }
        }
        private void rdTensach_CheckedChanged(object sender, EventArgs e)
        {
            if (rdTensach.Checked)
            {
                loaiTimKiem = "TenSach"; // Cập nhật loại tìm kiếm là theo tên sách
                lbThongbaoS.Text = "";
                txtTimkiem.Clear();
            }
        }
        private void tabTkSach_Click(object sender, EventArgs e)
        {
            //capnhatdata(); làm thừa
        }
        private void capnhatdataSach()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ketnoi))
                {
                    connection.Open();
                    string query = "SELECT MaSach, TenSach, MaLoaiSach, SoLuong, MaTG FROM QLSach";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dtgvDanhsach.DataSource = dataTable;

                    dtgvDanhsach.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dtgvDanhsach.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dtgvDanhsach.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dtgvDanhsach.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dtgvDanhsach.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                    dtgvDanhsach.Columns[0].HeaderText = "Mã Sách";
                    dtgvDanhsach.Columns[1].HeaderText = "Tên Sách";
                    dtgvDanhsach.Columns[2].HeaderText = "Mã LS";
                    dtgvDanhsach.Columns[3].HeaderText = "Số Lượng";
                    dtgvDanhsach.Columns[4].HeaderText = "Mã Tác Giả";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn thoát khỏi chương trình ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Hide();
            }
        }
        private void btnTimkiem_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(loaiTimKiem)) // Nếu đã chọn loại tìm kiếm
            {
                string timKiem = txtTimkiem.Text.Trim();

                // Tạo chuỗi truy vấn SQL để lấy thông tin sách theo loại tìm kiếm
                string query = $"SELECT MaSach, TenSach, MaLoaiSach, SoLuong, MaTG FROM QLSach WHERE {loaiTimKiem} = @TimKiem";

                try
                {
                    using (SqlConnection connection = new SqlConnection(ketnoi))
                    {
                        connection.Open();

                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@TimKiem", timKiem);

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        dtgvDanhsach.DataSource = dataTable;

                        if (dataTable.Rows.Count > 0)
                        {
                            //MessageBox.Show($"Đã tìm thấy sách theo {loaiTimKiem}: {timKiem}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lbThongbaoS.Text = "Đã tìm thấy ";
                        }
                        else
                        {
                            lbThongbaoS.Text = "Không tìm thấy ";
                            //MessageBox.Show($"Không tìm thấy sách theo {loaiTimKiem}: {timKiem}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                //lbThongbao.Text = "Vui lòng chọn kiểu tìm kiếm (theo Mã sách hoặc Tên sách).";
                MessageBox.Show("Vui lòng chọn kiểu tìm kiếm (theo Mã sách hoặc Tên sách).", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void txtTimkiem_TextChanged(object sender, EventArgs e)
        {
            lbThongbaoS.Text = "";
            if (!string.IsNullOrEmpty(loaiTimKiem)) // Nếu đã chọn loại tìm kiếm
            {
                string timKiem = txtTimkiem.Text.Trim();
                // Tạo chuỗi truy vấn SQL để lấy thông tin sách theo loại tìm kiếm
                string query = $"SELECT MaSach, TenSach, MaLoaiSach, SoLuong, MaTG FROM QLSach WHERE {loaiTimKiem} LIKE @TimKiem";

                try
                {
                    using (SqlConnection connection = new SqlConnection(ketnoi))
                    {
                        connection.Open();

                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@TimKiem", $"%{timKiem}%");

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        dtgvDanhsach.DataSource = dataTable;

                        /*if (dataTable.Rows.Count > 0)
                        {
                            MessageBox.Show($"Đã tìm thấy sách theo {loaiTimKiem}: {timKiem}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show($"Không tìm thấy sách theo {loaiTimKiem}: {timKiem}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                       }*/
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {

                // Nếu chưa chọn loại tìm kiếm, hiển thị thông báo
                MessageBox.Show("Vui lòng chọn kiểu tìm kiếm (theo Mã sách hoặc Tên sách).", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////


        private void rdMaDG_CheckedChanged(object sender, EventArgs e)
        {
            if (rdMaDG.Checked)
            {
                loaiTimKiem = "MaDG"; // Cập nhật loại tìm kiếm là theo mã sách
                lbThongbaoDG.Text = "";
                txtTkDocgia.Clear();
            }
        }
        private void rdTenDG_CheckedChanged(object sender, EventArgs e)
        {
            if (rdTenDG.Checked)
            {
                loaiTimKiem = "TenDocGia"; // Cập nhật loại tìm kiếm là theo tên sách
                lbThongbaoDG.Text = "";
                txtTkDocgia.Clear();
            }
        }
        private void capnhatdataDocGia()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ketnoi))
                {
                    connection.Open();
                    string query = "SELECT  MaDG, TenDocGia, GioiTinh, NgayMuon, DiaChi FROM QLNguoiMuon";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Thiết lập DataSource cho DataGridView
                    dtgvDanhsachDG.DataSource = dataTable;

                    dtgvDanhsachDG.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dtgvDanhsachDG.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dtgvDanhsachDG.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dtgvDanhsachDG.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dtgvDanhsachDG.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                    // Đổi tên header cho các cột
                    dtgvDanhsachDG.Columns["MaDG"].HeaderText = "Mã DG";
                    dtgvDanhsachDG.Columns["TenDocGia"].HeaderText = "Tên Độc Gỉa";
                    dtgvDanhsachDG.Columns["GioiTinh"].HeaderText = "Giới Tính";
                    dtgvDanhsachDG.Columns["NgayMuon"].HeaderText = "Ngày Tạo";
                    dtgvDanhsachDG.Columns["DiaChi"].HeaderText = "Địa Chỉ";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnThoatDG_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn thoát khỏi chương trình ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Hide();
            }
        }
        private void btnTkDocgia_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(loaiTimKiem)) // Nếu đã chọn loại tìm kiếm
            {
                string timKiem = txtTkDocgia.Text.Trim();

                // Tạo chuỗi truy vấn SQL để lấy thông tin sách theo loại tìm kiếm
                string query = $"SELECT MaDG, TenDocGia, GioiTinh, NgayMuon, DiaChi FROM QLNguoiMuon WHERE {loaiTimKiem} = @TimKiem";

                try
                {
                    using (SqlConnection connection = new SqlConnection(ketnoi))
                    {
                        connection.Open();

                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@TimKiem", timKiem);

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        dtgvDanhsachDG.DataSource = dataTable;

                        if (dataTable.Rows.Count > 0)
                        {
                            //MessageBox.Show($"Đã tìm thấy Độc Giả theo {loaiTimKiem}: {timKiem}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lbThongbaoDG.Text = "Đã tìm thấy";
                        }
                        else
                        {
                            lbThongbaoDG.Text = "Không tìm thấy";
                            //MessageBox.Show($"Không tìm thấy Độc Giả theo {loaiTimKiem}: {timKiem}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                //lbThongbao.Text = "Vui lòng chọn kiểu tìm kiếm (theo Mã sách hoặc Tên sách).";
                MessageBox.Show("Vui lòng chọn kiểu tìm kiếm (theo Mã Độc Giả hoặc Tên Độc Giả).", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void txtTkDocgia_TextChanged(object sender, EventArgs e)
        {
            lbThongbaoDG.Text = "";
            if (!string.IsNullOrEmpty(loaiTimKiem)) // Nếu đã chọn loại tìm kiếm
            {
                string timKiem = txtTkDocgia.Text.Trim();
                // Tạo chuỗi truy vấn SQL để lấy thông tin sách theo loại tìm kiếm
                string query = $"SELECT MaDG, TenDocGia, GioiTinh, NgayMuon, DiaChi FROM QLNguoiMuon WHERE {loaiTimKiem} LIKE @TimKiem";

                try
                {
                    using (SqlConnection connection = new SqlConnection(ketnoi))
                    {
                        connection.Open();

                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@TimKiem", $"%{timKiem}%");

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        dtgvDanhsachDG.DataSource = dataTable;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn kiểu tìm kiếm (theo Mã Độc Giả hoặc Tên Độc Giả).", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void rdMaTG_CheckedChanged(object sender, EventArgs e)
        {
            if (rdMaTG.Checked)
            {
                loaiTimKiem = "MaTG"; // Cập nhật loại tìm kiếm là theo mã sách
                lbThongbaoTG.Text = "";
                txtTKTG.Clear();
            }
        }
        private void rdTenTG_CheckedChanged(object sender, EventArgs e)
        {
            if (rdTenTG.Checked)
            {
                loaiTimKiem = "TenTG"; // Cập nhật loại tìm kiếm là theo tên sách
                lbThongbaoTG.Text = "";
                txtTKTG.Clear();
            }
        }
        private void capnhatdataTacGia()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ketnoi))
                {
                    connection.Open();
                    string query = "SELECT  MaTG, TenTG,DiaChi FROM QLTacGia";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Thiết lập DataSource cho DataGridView
                    dtgvDanhsachTG.DataSource = dataTable;

                    dtgvDanhsachTG.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dtgvDanhsachTG.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dtgvDanhsachTG.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                    // Đổi tên header cho các cột
                    dtgvDanhsachTG.Columns["MaTG"].HeaderText = "Mã Tác Gỉa";
                    dtgvDanhsachTG.Columns["TenTG"].HeaderText = "Tên Độc Giả";
                    dtgvDanhsachTG.Columns["DiaChi"].HeaderText = "Địa Chỉ";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnThoatTG_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn thoát khỏi chương trình ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Hide();
            }
        }
        private void btnTKTG_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(loaiTimKiem)) // Nếu đã chọn loại tìm kiếm
            {
                string timKiem = txtTKTG.Text.Trim();

                // Tạo chuỗi truy vấn SQL để lấy thông tin sách theo loại tìm kiếm
                string query = $"SELECT MaTG, TenTG, DiaChi FROM QLTacGia WHERE {loaiTimKiem} = @TimKiem";

                try
                {
                    using (SqlConnection connection = new SqlConnection(ketnoi))
                    {
                        connection.Open();

                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@TimKiem", timKiem);

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        dtgvDanhsachDG.DataSource = dataTable;

                        if (dataTable.Rows.Count > 0)
                        {
                            //MessageBox.Show($"Đã tìm thấy Tác Giả theo {loaiTimKiem}: {timKiem}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lbThongbaoTG.Text = "Đã tìm thấy";
                        }
                        else
                        {
                            lbThongbaoTG.Text = "Không tìm thấy";
                            //MessageBox.Show($"Không tìm thấy Tác Giả theo {loaiTimKiem}: {timKiem}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                //lbThongbao.Text = "Vui lòng chọn kiểu tìm kiếm (theo Mã sách hoặc Tên sách).";
                MessageBox.Show("Vui lòng chọn kiểu tìm kiếm (theo Mã Tác Giả hoặc Tên Tác Giả).", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void txtTKTG_TextChanged(object sender, EventArgs e)
        {
            lbThongbaoTG.Text = "";
            if (!string.IsNullOrEmpty(loaiTimKiem)) // Nếu đã chọn loại tìm kiếm
            {
                string timKiem = txtTKTG.Text.Trim();
                // Tạo chuỗi truy vấn SQL để lấy thông tin sách theo loại tìm kiếm
                string query = $"SELECT MaTG, TenTG, DiaChi FROM QLTacGia WHERE {loaiTimKiem} LIKE @TimKiem";

                try
                {
                    using (SqlConnection connection = new SqlConnection(ketnoi))
                    {
                        connection.Open();

                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@TimKiem", $"%{timKiem}%");

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        dtgvDanhsachTG.DataSource = dataTable;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn kiểu tìm kiếm (theo Mã Tác Giả hoặc Tên Tác Giả).", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        private void TimKiemALL_FormClosed(object sender, FormClosedEventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn thoát khỏi chương trình ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Hide();
            }
        }
    }
}
