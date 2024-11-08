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
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace TVSACH1
{
    public partial class F9_QuanLyDangKyTK : Form
    {
        private string ketnoi = "Data Source=NờTêPê\\THAIPHONG;Initial Catalog=QLTVS;Integrated Security=True;Encrypt=False";
        private string chuyennut = ""; // Biến để xác định chức năng đang được thực hiện
        public F9_QuanLyDangKyTK()
        {
            InitializeComponent();

        }
        private void QuanLyDangKyTK_Load(object sender, EventArgs e)
        {
            // Khởi động form với các ô nhập liệu bị vô hiệu hóa
            Vohieutextbox();
            capnhatdata();
            LoadChucVu();

            btnLuu.Visible = btnQuaylai.Visible = false;
            btnThem.Visible = btnSua.Visible = btnXoa.Visible = btnThoat.Visible = true;
        }
        private void Vohieutextbox()
        {
            // Vô hiệu hóa các ô nhập liệu
            txtTenTK.Enabled = txtMK.Enabled = false;
        }
        private void Kichhoattextbox()
        {
            // Kích hoạt các ô nhập liệu
            txtTenTK.Enabled = txtMK.Enabled = true;
        }
        private void Xoatextbox()
        {
            // Xóa nội dung trong các ô nhập liệu
            txtTenTK.Clear();
            txtMK.Clear();
        }
        private void capnhatdata()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ketnoi))
                {
                    connection.Open();
                    string query = "SELECT  Taikhoan, Matkhau, Chucvu FROM QLTaiKhoan";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Thiết lập DataSource cho DataGridView
                    dtgvDanhsach.DataSource = dataTable;

                    dtgvDanhsach.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dtgvDanhsach.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dtgvDanhsach.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                    // Đổi tên header cho các cột
                    dtgvDanhsach.Columns[0].HeaderText = "Tài Khoản";
                    dtgvDanhsach.Columns[1].HeaderText = "Mật Khẩu";
                    dtgvDanhsach.Columns[2].HeaderText = "Chức Vụ";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadChucVu()
        {
            List<string> chucVuList = new List<string>();

            // Câu truy vấn SQL để lấy danh sách chức vụ từ bảng QLTaiKhoan
            string query = "SELECT DISTINCT Chucvu FROM QLTaiKhoan";

            // Kết nối đến cơ sở dữ liệu và thực hiện truy vấn
            using (SqlConnection connection = new SqlConnection(ketnoi))
            {
                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    // Đọc các dòng dữ liệu trả về từ truy vấn
                    while (reader.Read())
                    {
                        string chucVu = reader["Chucvu"].ToString();
                        chucVuList.Add(chucVu);
                    }

                    // Đóng đối tượng đọc dữ liệu
                    reader.Close();

                    // Gán danh sách chức vụ vào ComboBox
                    cbChucVu.DataSource = chucVuList;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }
        private void dtgvDanhsach_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Xử lý sự kiện click vào một ô trong DataGridView
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dtgvDanhsach.Rows[e.RowIndex];

                // Cập nhật các giá trị của các ô TextBox dựa trên dòng được chọn
                txtTenTK.Text = selectedRow.Cells["Taikhoan"].Value.ToString();
                txtMK.Text = selectedRow.Cells["Matkhau"].Value.ToString();
                //txtNhapLaiMk.Text = selectedRow.Cells["Matkhau"].Value.ToString();
                cbChucVu.Text = selectedRow.Cells["Chucvu"].Value.ToString(); // Sử dụng ComboBox


            }
        }
        private void btnQuaylai_Click(object sender, EventArgs e)
        {
            Xoatextbox();
            Vohieutextbox();
            btnLuu.Visible = btnQuaylai.Visible = false;
            btnThem.Visible = btnSua.Visible = btnXoa.Visible = btnThoat.Visible = true;
        }
        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn thoát khỏi chương trình ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {

                this.Hide();

            }
        }
        private void QuanLyDangKyTK_FormClosed(object sender, FormClosedEventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn thoát khỏi chương trình ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Hide();
            }
        }
        private void luu_them()
        {
            string TK = txtTenTK.Text;
            string MK = txtMK.Text;
            string CV = cbChucVu.Text;

            if (string.IsNullOrWhiteSpace(TK) || string.IsNullOrWhiteSpace(MK) || string.IsNullOrWhiteSpace(CV))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(ketnoi))
                {
                    connection.Open();

                    // Kiểm tra xem tên tài khoản đã tồn tại chưa
                    string checkQuery = "SELECT COUNT(*) FROM QLTaiKhoan WHERE Taikhoan = @TK";
                    SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                    checkCommand.Parameters.AddWithValue("@TK", TK);
                    int existingCount = (int)checkCommand.ExecuteScalar();

                    if (existingCount > 0)
                    {
                        // Hiển thị thông báo cho người dùng
                        DialogResult result = MessageBox.Show("Tên Tài Khoản đã tồn tại. Bạn có muốn tiếp tục nhập thông tin mới?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                        if (result == DialogResult.No)
                        {
                            // Người dùng không muốn tiếp tục, reset các ô nhập liệu
                            Vohieutextbox();
                            btnLuu.Visible = false;
                            return;
                        }
                        else
                        {
                            // Người dùng muốn tiếp tục nhập thông tin mới, reset các ô nhập liệu
                            Xoatextbox();
                            return;
                        }
                    }

                    // Nếu không có sự trùng lặp, tiến hành thêm tài khoản mới
                    string insertQuery = "INSERT INTO QLTaiKhoan (Taikhoan, Matkhau, Chucvu) VALUES (@TK, @MK, @CV)";
                    SqlCommand command = new SqlCommand(insertQuery, connection);
                    command.Parameters.AddWithValue("@TK", TK);
                    command.Parameters.AddWithValue("@MK", MK);
                    command.Parameters.AddWithValue("@CV", CV);
                    command.ExecuteNonQuery();

                    MessageBox.Show("Thêm Tài Khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Cập nhật lại danh sách tài khoản sau khi thêm thành công
                    capnhatdata();

                    // Reset các ô nhập liệu sau khi thêm thành công
                    Xoatextbox();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            chuyennut = "Them";
            Kichhoattextbox();
            Xoatextbox();
            MessageBox.Show("Bạn đã chọn chức năng Thêm Tài Khoản. Vui lòng nhập thông tin và nhấn 'Lưu' để hoàn thành.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnThem.Visible = btnSua.Visible = btnXoa.Visible = false;
            btnQuaylai.Visible = btnLuu.Visible = true;
        }
        private void luu_sua()
        {
            string TK = txtTenTK.Text;
            string MK = txtMK.Text;
            string CV = cbChucVu.Text;
            try
            {
                using (SqlConnection connection = new SqlConnection(ketnoi))
                {
                    connection.Open();
                    string query = "UPDATE QLTaiKhoan SET Taikhoan = @TK, Matkhau = @MK, Chucvu = @CV WHERE Taikhoan = @TK";


                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@TK", TK);
                    command.Parameters.AddWithValue("@MK", MK);
                    command.Parameters.AddWithValue("@CV", CV);
                    command.ExecuteNonQuery();

                    MessageBox.Show("Cập nhật thông tin Tài Khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    capnhatdata();
                    txtTenTK.Enabled = false;
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

                txtTenTK.Text = selectedRow.Cells["Taikhoan"].Value.ToString();
                txtMK.Text = selectedRow.Cells["Matkhau"].Value.ToString();
                cbChucVu.Text = selectedRow.Cells["Chucvu"].Value.ToString();

                MessageBox.Show("Bạn đã chọn chức năng Sửa Tài Khoản. Vui lòng chỉnh sửa thông tin và nhấn 'Lưu' để hoàn thành.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtTenTK.Enabled = false;
                btnLuu.Visible = btnQuaylai.Visible = true;
                btnThem.Visible = btnSua.Visible = btnXoa.Visible = false;
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một Tài Khoản để sửa thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void luu_xoa()
        {
            // Lấy thông tin tài khoản đang đăng nhập từ Form1 (ví dụ: lưu trong biến globalTenTaiKhoan)
            string tenTaiKhoanDangNhap = F2_DangNhap.TenTaiKhoanDangNhap;
            string taikhoan = dtgvDanhsach.SelectedRows[0].Cells["Taikhoan"].Value.ToString();

            if (taikhoan == tenTaiKhoanDangNhap)
            {
                MessageBox.Show("Bạn không thể xoá tài khoản đang đăng nhập.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Ngăn không cho phép xoá tài khoản đang đăng nhập
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(ketnoi))
                {
                    connection.Open();

                    string deleteQuery = "DELETE FROM QLTaiKhoan WHERE Taikhoan = @TK";
                    SqlCommand command = new SqlCommand(deleteQuery, connection);
                    command.Parameters.AddWithValue("@TK", taikhoan);
                    command.ExecuteNonQuery();

                    MessageBox.Show("Đã xoá thông tin Tài Khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Vui lòng chọn Tài Khoản cần xoá trước khi thực hiện xoá.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            MessageBox.Show("Bạn đã chọn chức năng Xoá Tài Khoản. Vui lòng nhập thông tin và nhấn 'Lưu' để hoàn thành.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
    }
}
