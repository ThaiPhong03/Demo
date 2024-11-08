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
    public partial class F5_QuanLyTacGia : Form
    {
        private string ketnoi = "Data Source=NờTêPê\\THAIPHONG;Initial Catalog=QLTVS;Integrated Security=True;Encrypt=False";
        private string chuyennut = ""; // Biến để xác định chức năng đang được thực hiện
        public F5_QuanLyTacGia()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            // Khởi động form với các ô nhập liệu bị vô hiệu hóa
            Vohieutextbox();
            capnhatdata();

            btnLuu.Visible = btnQuaylai.Visible = false;
            btnThem.Visible = btnSua.Visible = btnXoa.Visible = btnThoat.Visible = true;
        }
        private void Vohieutextbox()
        {
            // Vô hiệu hóa các ô nhập liệu
            txtMaTG.Enabled = txtTenTG.Enabled = txtDiachi.Enabled = false;
        }
        private void Kichhoattextbox()
        {
            // Kích hoạt các ô nhập liệu
            txtMaTG.Enabled = txtTenTG.Enabled = txtDiachi.Enabled = true;
        }
        private void Xoatextbox()
        {
            // Xóa nội dung trong các ô nhập liệu
            txtTenTG.Clear();
            txtMaTG.Clear();
            txtDiachi.Clear();

        }

        private void dtgvDanhsach_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Xử lý sự kiện click vào một ô trong DataGridView
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dtgvDanhsach.Rows[e.RowIndex];

                // Cập nhật các giá trị của các ô TextBox dựa trên dòng được chọn
                txtMaTG.Text = selectedRow.Cells["MaTG"].Value.ToString();
                txtTenTG.Text = selectedRow.Cells["TenTG"].Value.ToString();
                txtDiachi.Text = selectedRow.Cells["DiaChi"].Value.ToString();

            }
        }

        private void btnQuaylai_Click(object sender, EventArgs e)
        {
            Xoatextbox();
            Vohieutextbox();
            btnLuu.Visible = btnQuaylai.Visible = false;
            btnThem.Visible = btnSua.Visible = btnXoa.Visible = btnThoat.Visible = true;
        }
        private void capnhatdata()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ketnoi))
                {
                    connection.Open();
                    string query = "SELECT  MaTG, TenTG , DiaChi FROM QLTacGia";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dtgvDanhsach.DataSource = dataTable;

                    dtgvDanhsach.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dtgvDanhsach.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dtgvDanhsach.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                    dtgvDanhsach.Columns[0].HeaderText = "Mã Tác Giả";
                    dtgvDanhsach.Columns[1].HeaderText = "Tên Tác Giả";
                    dtgvDanhsach.Columns[2].HeaderText = "Địa Chỉ";

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }
        private void luu_them()
        {
            /*if (!functionSelected)
            {
                MessageBox.Show("Vui lòng chọn một chức năng trước khi nhập thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }*/

            string matacgia = txtMaTG.Text;
            string tentacgia = txtTenTG.Text;
            string diachi = txtDiachi.Text;

            if (string.IsNullOrWhiteSpace(matacgia) || string.IsNullOrWhiteSpace(tentacgia) || string.IsNullOrWhiteSpace(diachi))
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
                    string checkQuery = "SELECT COUNT(*) FROM QLTacGia WHERE MaTG = @MaTacGia";
                    SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                    checkCommand.Parameters.AddWithValue("@MaTacGia", matacgia);
                    int existingCount = (int)checkCommand.ExecuteScalar();

                    if (existingCount > 0)
                    {
                        // Hiển thị thông báo cho người dùng
                        DialogResult result = MessageBox.Show("Mã tác giả đã tồn tại. Bạn có muốn tiếp tục nhập thông tin mới?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

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
                    string insertQuery = "INSERT INTO QLTacGia ( MaTG, TenTG, DiaChi) VALUES (@MaTG , @TenTG, @DiaChi)";
                    SqlCommand command = new SqlCommand(insertQuery, connection);
                    command.Parameters.AddWithValue("@MaTG", matacgia);
                    command.Parameters.AddWithValue("@TenTG", tentacgia);
                    command.Parameters.AddWithValue("@DiaChi", diachi);

                    command.ExecuteNonQuery();

                    MessageBox.Show("Thêm tác giả thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            MessageBox.Show("Bạn đã chọn chức năng Thêm Tác Giả. Vui lòng nhập thông tin và nhấn 'Lưu' để hoàn thành.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnThem.Visible = btnSua.Visible = btnXoa.Visible = false;
            btnQuaylai.Visible = btnLuu.Visible = true;
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

        private void Form5_FormClosed(object sender, FormClosedEventArgs e)
        {

            DialogResult result = MessageBox.Show("Bạn có muốn thoát khỏi chương trình ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Hide();
            }
        }

        private void luu_sua()
        {
            string matacgia = txtMaTG.Text;
            string tentacgia = txtTenTG.Text;
            string diachi = txtDiachi.Text;

            try
            {
                using (SqlConnection connection = new SqlConnection(ketnoi))
                {
                    connection.Open();
                    string query = "UPDATE QLTacGia SET MaTG = @MaTG, TenTG = @TenTG, DiaChi = @DiaChi WHERE MaTG = @MaTG";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@MaTG", matacgia);
                    command.Parameters.AddWithValue("@TenTG", tentacgia);
                    command.Parameters.AddWithValue("@DiaChi", diachi);

                    command.ExecuteNonQuery();

                    MessageBox.Show("Cập nhật thông tin Tác Giả thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                txtMaTG.Text = selectedRow.Cells["MaTG"].Value.ToString();
                txtTenTG.Text = selectedRow.Cells["TenTG"].Value.ToString();
                txtDiachi.Text = selectedRow.Cells["DiaChi"].Value.ToString();

                MessageBox.Show("Bạn đã chọn chức năng Sửa Tác Giả. Vui lòng chỉnh sửa thông tin và nhấn 'Lưu' để hoàn thành.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtMaTG.Enabled = false;
                btnLuu.Visible = btnQuaylai.Visible = true;
                btnThem.Visible = btnSua.Visible = btnXoa.Visible = false;
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một Tác Giả để sửa thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void luu_xoa()
        {
            string matacgia = dtgvDanhsach.SelectedRows[0].Cells["MaTG"].Value.ToString();

            // Thực hiện truy vấn xoá thông tin sách dựa trên mã sách
            try
            {
                using (SqlConnection connection = new SqlConnection(ketnoi))
                {
                    connection.Open();

                    string deleteQuery = "DELETE FROM QLTacGia WHERE MaTG = @MaTG";
                    SqlCommand command = new SqlCommand(deleteQuery, connection);
                    command.Parameters.AddWithValue("@MaTG", matacgia);
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
                MessageBox.Show("Vui lòng chọn Tác Giả cần xoá trước khi thực hiện xoá.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            MessageBox.Show("Bạn đã chọn chức năng Xoá Sách. Vui lòng nhập thông tin và nhấn 'Lưu' để hoàn thành.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            btnLuu.Visible = btnQuaylai.Visible = true;
            btnThem.Visible = btnSua.Visible = btnXoa.Visible = false;
        }
    }
}
