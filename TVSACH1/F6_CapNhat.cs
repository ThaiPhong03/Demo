using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TVSACH1
{
    public partial class F6_CapNhat : Form
    {
        private string ketnoi = "Data Source=NờTêPê\\THAIPHONG;Initial Catalog=QLTVS;Integrated Security=True;Encrypt=False";
        private string chuyennut = ""; // Biến để xác định chức năng đang được thực hiện

        public F6_CapNhat()
        {
            InitializeComponent();
            
        }

        private void Form4Capnhat_Load(object sender, EventArgs e)
        {
            // Khởi động form với các ô nhập liệu bị vô hiệu hóa
            Vohieutextbox();
            
            LoadDataComboBox();
            capnhatdata();
            btnLuu.Visible = btnQuaylai.Visible = false;
            btnThem.Visible = btnSua.Visible = btnXoa.Visible = btnThoat.Visible = true;
        }
        private void Vohieutextbox()
        {
            // Vô hiệu hóa các ô nhập liệu
            txtMasach.Enabled = txtTensach.Enabled = txtMaloaisach.Enabled = txtSoluong.Enabled = cbTG.Enabled = false;
        }
        private void Kichhoattextbox()
        {
            // Kích hoạt các ô nhập liệu
            txtMasach.Enabled = txtTensach.Enabled = txtMaloaisach.Enabled = txtSoluong.Enabled = cbTG.Enabled = true;
        }
        private void Xoatextbox()
        {
            // Xóa nội dung trong các ô nhập liệu
            txtMasach.Clear();
            txtTensach.Clear();
            txtMaloaisach.Clear();
            txtSoluong.Clear();
            //cbTG.Items.Clear();
        }
        private void dtgvDanhsach_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Xử lý sự kiện click vào một ô trong DataGridView
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dtgvDanhsach.Rows[e.RowIndex];

                // Cập nhật các giá trị của các ô TextBox dựa trên dòng được chọn
                txtMasach.Text = selectedRow.Cells["MaSach"].Value.ToString();
                txtTensach.Text = selectedRow.Cells["TenSach"].Value.ToString();
                txtMaloaisach.Text = selectedRow.Cells["MaLoaiSach"].Value.ToString();
                txtSoluong.Text = selectedRow.Cells["SoLuong"].Value.ToString();
                cbTG.Text = selectedRow.Cells["MaTG"].Value.ToString();
            }
        }
        private void btnQuaylai_Click(object sender, EventArgs e)
        {
            Xoatextbox();
            Vohieutextbox();
            btnLuu.Visible = btnQuaylai.Visible = false;
            btnThem.Visible = btnSua.Visible = btnXoa.Visible = btnThoat.Visible = true;
        }
        private void LoadDataComboBox()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ketnoi))
                {
                    connection.Open();

                    string query = "SELECT MaTG FROM QLTacGia";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    // Xóa danh sách cũ trong combobox
                   //cbTG.Items.Clear();

                    // Đọc dữ liệu từ SqlDataReader và thêm vào combobox
                    while (reader.Read())
                    {
                        string maTG = reader["MaTG"].ToString();
                        cbTG.Items.Add(maTG);
                    }

                    // Đóng SqlDataReader
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
       
        private void capnhatdata()
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
                    dtgvDanhsach.Columns[3].HeaderText = "SL";
                    dtgvDanhsach.Columns[4].HeaderText = "Mã TG";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void luu_them()
        {

            string maSach = txtMasach.Text;
            string tenSach = txtTensach.Text;
            string maLoaiSach = txtMaloaisach.Text;
            string soLuong = txtSoluong.Text;
            string maTacGia = cbTG.Text;

            if (string.IsNullOrWhiteSpace(maSach) || string.IsNullOrWhiteSpace(tenSach) || string.IsNullOrWhiteSpace(maLoaiSach) || string.IsNullOrWhiteSpace(soLuong) || string.IsNullOrWhiteSpace(maTacGia))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin sách.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(ketnoi))
                {
                    connection.Open();

                    // Kiểm tra xem mã sách có trùng lặp không
                    string checkQuery = "SELECT COUNT(*) FROM QLSach WHERE MaSach = @MaSach";
                    SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                    checkCommand.Parameters.AddWithValue("@MaSach", maSach);
                    int existingCount = (int)checkCommand.ExecuteScalar();

                    if (existingCount > 0)
                    {
                        // Hiển thị thông báo cho người dùng
                        DialogResult result = MessageBox.Show("Mã sách đã tồn tại. Bạn có muốn tiếp tục nhập thông tin mới?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                        if (result == DialogResult.No)
                        {
                            Vohieutextbox();
                            btnLuu.Visible = false;
                            return;
                            // Người dùng không muốn tiếp tục nhập thông tin
                        }
                        else
                        {
                            // Người dùng muốn tiếp tục nhập thông tin
                            Xoatextbox(); // Xóa các ô nhập liệu để người dùng nhập thông tin mới
                            return;
                        }
                    }
                    // Nếu không có sự trùng lặp, tiến hành thêm sách mới
                    string insertQuery = "INSERT INTO QLSach (MaSach, TenSach, MaLoaiSach, SoLuong, MaTG) VALUES (@MaSach, @TenSach, @MaLoaiSach, @SoLuong, @MaTG)";
                    SqlCommand command = new SqlCommand(insertQuery, connection);
                    command.Parameters.AddWithValue("@MaSach", maSach);
                    command.Parameters.AddWithValue("@TenSach", tenSach);
                    command.Parameters.AddWithValue("@MaLoaiSach", maLoaiSach);
                    command.Parameters.AddWithValue("@SoLuong", soLuong);
                    command.Parameters.AddWithValue("@MaTG", maTacGia);
                    command.ExecuteNonQuery();

                    MessageBox.Show("Thêm sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    capnhatdata();
                    Xoatextbox();
                    //functionSelected = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            //functionSelected = true;
            chuyennut = "Them";
            Kichhoattextbox();
            Xoatextbox();
            //LoadDataComboBox();
            MessageBox.Show("Bạn đã chọn chức năng Thêm Sách. Vui lòng nhập thông tin và nhấn 'Lưu' để hoàn thành.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnThem.Visible = btnSua.Visible = btnXoa.Visible = false;
            btnQuaylai.Visible = btnLuu.Visible = true;

        }
        private void luu_sua()
        {
            string maSach = txtMasach.Text;
            string tenSach = txtTensach.Text;
            string maLoaiSach = txtMaloaisach.Text;
            string soLuong = txtSoluong.Text;
            string maTacGia = cbTG.Text;

            try
            {
                using (SqlConnection connection = new SqlConnection(ketnoi))
                {
                    connection.Open();
                   
                    string query = "UPDATE QLSach SET TenSach = @TenSach, MaLoaiSach = @MaLoaiSach, SoLuong = @SoLuong, MaTG = @MaTG WHERE MaSach = @MaSach";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@MaSach", maSach);
                    command.Parameters.AddWithValue("@TenSach", tenSach);
                    command.Parameters.AddWithValue("@MaLoaiSach", maLoaiSach);
                    command.Parameters.AddWithValue("@SoLuong", soLuong);
                    command.Parameters.AddWithValue("@MaTG", maTacGia);
                    command.ExecuteNonQuery();

                    MessageBox.Show("Cập nhật thông tin sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    capnhatdata();
                    Xoatextbox();
                    btnLuu.Visible = btnQuaylai.Visible = false;
                    btnThem.Visible = btnSua.Visible = btnXoa.Visible = btnThoat.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnSua_Click(object sender, EventArgs e)
        {
            chuyennut = "Sua";
            if (dtgvDanhsach.SelectedRows.Count > 0)
            {
                Kichhoattextbox();
                // Lấy dòng đang được chọn
                DataGridViewRow selectedRow = dtgvDanhsach.SelectedRows[0];
                // Lấy giá trị từ các ô của dòng đó và hiển thị lên các TextBox tương ứng
                txtMasach.Text = selectedRow.Cells["MaSach"].Value.ToString();
                txtTensach.Text = selectedRow.Cells["TenSach"].Value.ToString();
                txtMaloaisach.Text = selectedRow.Cells["MaLoaiSach"].Value.ToString();
                txtSoluong.Text = selectedRow.Cells["SoLuong"].Value.ToString();
                cbTG.Text = selectedRow.Cells["MaTG"].Value.ToString();
                MessageBox.Show("Bạn đã chọn chức năng Sửa Sách. Vui lòng chỉnh sửa thông tin và nhấn 'Lưu' để hoàn thành.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtMasach.Enabled = false;
                btnLuu.Visible = btnQuaylai.Visible = true;
                btnThem.Visible = btnSua.Visible = btnXoa.Visible = false;
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sách để sửa thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void luu_xoa()
        {
            string maSach = dtgvDanhsach.SelectedRows[0].Cells["MaSach"].Value.ToString();

            // Thực hiện truy vấn xoá thông tin sách dựa trên mã sách
            try
            {
                using (SqlConnection connection = new SqlConnection(ketnoi))
                {
                    connection.Open();

                    string deleteQuery = "DELETE FROM QLSach WHERE MaSach = @MaSach";
                    SqlCommand command = new SqlCommand(deleteQuery, connection);
                    command.Parameters.AddWithValue("@MaSach", maSach);
                    command.ExecuteNonQuery();

                    MessageBox.Show("Đã xoá thông tin sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    capnhatdata();

                    Xoatextbox();
                    btnLuu.Visible = btnQuaylai.Visible = false;
                    btnThem.Visible = btnSua.Visible = btnXoa.Visible = btnThoat.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnXoa_Click(object sender, EventArgs e)
        {
            chuyennut = "Xoa";
            // Kiểm tra xem người dùng đã chọn dòng nào trên DataGridView chưa
            if (dtgvDanhsach.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn Sách cần xoá trước khi thực hiện xoá.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            MessageBox.Show("Bạn đã chọn chức năng Xoá Sách. Vui lòng nhập thông tin và nhấn 'Lưu' để hoàn thành.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            btnLuu.Visible = btnQuaylai.Visible = true;
            btnThem.Visible = btnSua.Visible = btnXoa.Visible = false;

        }
        private void btnLuu_Click(object sender, EventArgs e)
        {

            if (chuyennut == "Them")
            {
                luu_them();
            }
            else if (chuyennut == "Sua")
            {
                luu_sua();
            }
            else if (chuyennut == "Xoa")
            {
                luu_xoa();
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

        private void Form4Capnhat_FormClosed(object sender, FormClosedEventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn thoát khỏi chương trình ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Hide();
            }
        }
    }
}
//DisableInputFields    vo o hiệu txtx
//EnableInputFields     kich hoạt txt
//ClearInputFields xoá txt