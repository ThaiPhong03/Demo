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
    public partial class F7_MuonTra : Form
    {
        private string ketnoi = "Data Source=NờTêPê\\THAIPHONG;Initial Catalog=QLTVS;Integrated Security=True;Encrypt=False";
        private string chuyennut = ""; // Biến để xác định chức năng đang được thực hiện
        public F7_MuonTra()
        {
            InitializeComponent();


            capnhatdataDanhSach();
            capnhatdataTenSach();
            capnhatdataMaDG();
            Vohieutextbox();


            capnhatdataTraSach();

        }
        private void Vohieutextbox()
        {
            txtMasach.Enabled = txtMaloaisach.Enabled =txtSoluong.Enabled=txtMatacgia.Enabled= cbMadocgia.Enabled= txtMaSachChoMuon.Enabled= txtSLMuon.Enabled = false;
        }
        private void Xoatextbox()
        {
            txtSLMuon.Clear();
            dtpkNgaymuon.Value = DateTime.Today;
            dtpkNgayhentra.Value = DateTime.Today;

        }
        //11111111111111111111111111111111111111111111111111//
        private void btnThoatMuonSach_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn thoát khỏi chương trình ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Hide();
            }
        }
        private void capnhatdataTenSach()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ketnoi))
                {
                    connection.Open();
                    // Truy vấn lấy danh sách tên sách từ bảng QLSach
                    string query = "SELECT TenSach FROM QLSach";
                    SqlCommand command = new SqlCommand(query, connection);

                    // Đọc dữ liệu từ truy vấn
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        cbTensach.Items.Add(reader["TenSach"].ToString());
                    }

                    // Đóng kết nối
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            /*Phương thức capnhatdataTenSach làm những công việc sau:

            Kết nối tới cơ sở dữ liệu bằng chuỗi kết nối ketnoi.
            Thực thi câu truy vấn để lấy danh sách tên sách từ bảng QLSach.
            Đọc dữ liệu và thêm tên sách vào ComboBox cbTensach.
            Đóng kết nối sau khi hoàn thành(tự động nhờ using).
            Hiển thị thông báo lỗi nếu có ngoại lệ xảy ra.*/
        }
        private void capnhatdataMaDG()
        {
            try
            {
                // Tạo kết nối đến cơ sở dữ liệu
                using (SqlConnection connection = new SqlConnection(ketnoi))
                {
                    connection.Open();
                    // Truy vấn lấy danh sách Mã độc giả từ bảng QLNguoiMuon
                    string query = "SELECT MaDG FROM QLNguoiMuon";
                    SqlCommand command = new SqlCommand(query, connection);

                    // Đọc dữ liệu từ truy vấn
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        cbMadocgia.Items.Add(reader["MaDG"].ToString());
                    }

                    // Đóng kết nối
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void cbTensach_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedTenSach = cbTensach.SelectedItem.ToString();

            try
            {
                // Tạo kết nối đến cơ sở dữ liệu
                using (SqlConnection connection = new SqlConnection(ketnoi))
                {
                    connection.Open();

                    // Truy vấn lấy thông tin sách dựa trên tên sách đã chọn
                    string query = "SELECT * FROM QLSach WHERE TenSach = @TenSach";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@TenSach", selectedTenSach);

                    // Đọc dữ liệu từ truy vấn
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        txtMasach.Text = reader["MaSach"].ToString();
                        txtMaloaisach.Text = reader["MaLoaiSach"].ToString();
                        txtSoluong.Text = reader["SoLuong"].ToString();
                        txtMatacgia.Text = reader["MaTG"].ToString();
                        txtMaSachChoMuon.Text = reader["MaSach"].ToString(); // Hiển thị MaSach vào txtMasachChomuon
                    }

                    // Đóng kết nối
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void dtgvDanhsach_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dtgvDanhsach.Rows[e.RowIndex];
                // Cập nhật các giá trị của các ô TextBox dựa trên dòng được chọn
                cbMadocgia.Text = selectedRow.Cells["MaDG"].Value.ToString();
                txtMaSachChoMuon.Text = selectedRow.Cells["MaSach"].Value.ToString();
                txtSLMuon.Text = selectedRow.Cells["SoLuong"].Value.ToString(); 
                DateTime ngayMuon = Convert.ToDateTime(selectedRow.Cells["NgayMuon"].Value);
                dtpkNgaymuon.Value = ngayMuon;
                DateTime ngayHenTra = Convert.ToDateTime(selectedRow.Cells["NgayHenTra"].Value);
                dtpkNgayhentra.Value = ngayHenTra;
            }
        }
        private void capnhatdataDanhSach()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ketnoi))
                {
                    connection.Open();

                    // Điều kiện kiểm tra nếu cột NgayTra có giá trị null hay không
                    string query = "SELECT MaDG, MaSach, SoLuong, NgayMuon, NgayHenTra FROM QLMuonTraSach WHERE NgayTra IS NULL";

                    SqlCommand command = new SqlCommand(query, connection);//biểu diễn và thực thi câu lệnh truy vấn
                    SqlDataAdapter adapter = new SqlDataAdapter(command);// điền dữ liệu từ cơ sở dl vào datatable
                    DataTable dataTable = new DataTable();// lưu trữ kq truy vấn
                    adapter.Fill(dataTable);// điền dữ liệu từ cơ sở dl vào datatable

                    // Kiểm tra nếu không có dòng nào trong kết quả
                    if (dataTable.Rows.Count == 0)
                    {
                        // Nếu không có kết quả nào có NgayTra null, hiển thị tất cả
                        query = "SELECT MaDG, MaSach, SoLuong, NgayMuon, NgayHenTra FROM QLMuonTraSach";
                        command = new SqlCommand(query, connection);
                        adapter = new SqlDataAdapter(command);
                        dataTable = new DataTable();
                        adapter.Fill(dataTable);
                    }

                    dtgvDanhsach.DataSource = dataTable;// gán datatable cho dtgv để hiển thị dl

                    dtgvDanhsach.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dtgvDanhsach.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dtgvDanhsach.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dtgvDanhsach.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dtgvDanhsach.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                    dtgvDanhsach.Columns[0].HeaderText = "Mã ĐG";
                    dtgvDanhsach.Columns[1].HeaderText = "Mã Sách";
                    dtgvDanhsach.Columns[2].HeaderText = "SL";
                    dtgvDanhsach.Columns[3].HeaderText = "Ngày Mượn";
                    dtgvDanhsach.Columns[4].HeaderText = "Ngày Hẹn Trả";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void luu_them()
        {

            // Lấy thông tin từ các điều khiển trên giao diện
            string maDG = cbMadocgia.Text;
            string maSach = txtMasach.Text;
            string soLuongMuonStr = txtSLMuon.Text;
            string ngayMuon = dtpkNgaymuon.Value.ToString("yyyy-MM-dd"); // Định dạng ngày thành chuỗi yyyy-MM-dd
            string ngayHenTra = dtpkNgayhentra.Value.ToString("yyyy-MM-dd");

            // Kiểm tra và chuyển đổi số lượng mượn từ dạng chuỗi sang số nguyên
            if (!int.TryParse(soLuongMuonStr, out int soLuongMuon) || soLuongMuon <= 0)
            {
                MessageBox.Show("Số lượng mượn không hợp lệ. Vui lòng nhập số nguyên dương.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(ketnoi))
                {
                    connection.Open();

                    // Kiểm tra số lượng sách còn trong kho
                    string checkQuery = "SELECT SoLuong FROM QLSach WHERE MaSach = @MaSach";
                    SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                    checkCommand.Parameters.AddWithValue("@MaSach", maSach);

                    int soLuongHienCo = (int)checkCommand.ExecuteScalar();

                    // Kiểm tra số lượng mượn có vượt quá số lượng sách có sẵn trong kho không
                    if (soLuongMuon > soLuongHienCo)
                    {
                        MessageBox.Show("Số lượng sách trong kho không đủ để cho mượn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    // Thực hiện cập nhật số lượng sách còn lại trong kho (QLSach)
                    string updateQuery = "UPDATE QLSach SET SoLuong = SoLuong - @SoLuongMuon WHERE MaSach = @MaSach";
                    SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                    updateCommand.Parameters.AddWithValue("@SoLuongMuon", soLuongMuon);
                    updateCommand.Parameters.AddWithValue("@MaSach", maSach);
                    updateCommand.ExecuteNonQuery();

                    // Thêm thông tin mượn sách vào bảng QLMuonTraSach
                    string insertQuery = "INSERT INTO QLMuonTraSach (MaDG, MaSach, SoLuong, NgayMuon, NgayHenTra) VALUES (@MaDG, @MaSach, @SoLuongMuon, @NgayMuon, @NgayHenTra)";
                    SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                    insertCommand.Parameters.AddWithValue("@MaDG", maDG);
                    insertCommand.Parameters.AddWithValue("@MaSach", maSach);
                    insertCommand.Parameters.AddWithValue("@SoLuongMuon", soLuongMuon);
                    insertCommand.Parameters.AddWithValue("@NgayMuon", ngayMuon);
                    insertCommand.Parameters.AddWithValue("@NgayHenTra", ngayHenTra);
                    insertCommand.ExecuteNonQuery();

                    // Lấy số lượng sách còn lại sau khi cập nhật
                    int soLuongConLai = soLuongHienCo - soLuongMuon;

                    // Hiển thị số lượng còn lại trong TextBox txtSoluong
                    txtSoluong.Text = soLuongConLai.ToString();

                    MessageBox.Show("Cho mượn sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    capnhatdataDanhSach(); // Cập nhật lại danh sách sau khi thực hiện thành công
                    Xoatextbox();
                    capnhatdataTraSach();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thực hiện cho mượn sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnChoMuon_Click(object sender, EventArgs e)
        {
            chuyennut = "Them";
            cbMadocgia.Enabled = txtSLMuon.Enabled = true;
            Xoatextbox();
            MessageBox.Show("Bạn đã chọn chức năng Cho Mượn. Vui lòng nhập thông tin và nhấn 'Lưu' để hoàn thành.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnChoMuon.Visible = btnSua.Visible = btnXoa.Visible = false;
            btnQuaylai.Visible = btnLuu.Visible = true;
        }
        private void capnhatSoLuongSach(string maSach, int soLuongMuonCu, int soLuongMuonMoi)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ketnoi))
                {
                    connection.Open();

                    // Lấy số lượng hiện tại của sách
                    string selectQuery = "SELECT SoLuong FROM QLSach WHERE MaSach = @MaSach";
                    SqlCommand selectCommand = new SqlCommand(selectQuery, connection);
                    selectCommand.Parameters.AddWithValue("@MaSach", maSach);
                    int soLuongHienTai = Convert.ToInt32(selectCommand.ExecuteScalar());

                    // Tính toán sự thay đổi số lượng mượn
                    int soLuongThayDoi = soLuongMuonMoi - soLuongMuonCu;

                    // Tính toán số lượng mới của sách
                    int soLuongMoi = soLuongHienTai - soLuongThayDoi;
                    // Đảm bảo số lượng mới không nhỏ hơn 0
                    if (soLuongMoi >= 0)
                    {
                        string updateQuery = "UPDATE QLSach SET SoLuong = @SoLuongMoi WHERE MaSach = @MaSach";
                        SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                        updateCommand.Parameters.AddWithValue("@SoLuongMoi", soLuongMoi);
                        updateCommand.Parameters.AddWithValue("@MaSach", maSach);
                        updateCommand.ExecuteNonQuery();
                    }
                    else
                    {
                        MessageBox.Show("Số lượng sách không hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật số lượng sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void luu_sua()
        {
            string maDG = cbMadocgia.Text;
            string maSach = txtMaSachChoMuon.Text;
            int soLuongMuonMoi = Convert.ToInt32(txtSLMuon.Text);
            string ngayMuon = dtpkNgaymuon.Value.ToString("yyyy-MM-dd");
            string ngayHenTra = dtpkNgayhentra.Value.ToString("yyyy-MM-dd");
            try
            {
                using (SqlConnection connection = new SqlConnection(ketnoi))
                {
                    connection.Open();

                    // Tìm số lượng đã mượn trước đó để tính toán
                   string selectSoLuongMuonQuery = "SELECT SUM(SoLuong) FROM QLMuonTraSach WHERE MaSach = @MaSach";
                    //string selectSoLuongMuonQuery = "SELECT SoLuong FROM QLMuonTraSach WHERE MaDG = @MaDG AND MaSach = @MaSach ";

                    SqlCommand selectSoLuongMuonCommand = new SqlCommand(selectSoLuongMuonQuery, connection);
                    selectSoLuongMuonCommand.Parameters.AddWithValue("@MaSach", maSach);
                    object result = selectSoLuongMuonCommand.ExecuteScalar();
                    int soLuongMuonCu = (result == DBNull.Value) ? 0 : Convert.ToInt32(result);

                    // Cập nhật thông tin mượn trả sách trong bảng QLMuonTraSach
                     string updateQuery = "UPDATE QLMuonTraSach SET MaSach = @MaSach, SoLuong = @SoLuong, NgayMuon = @NgayMuon, NgayHenTra = @NgayHenTra WHERE MaDG = @MaDG";
                    
                    SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                    updateCommand.Parameters.AddWithValue("@MaSach", maSach);
                    updateCommand.Parameters.AddWithValue("@SoLuong", soLuongMuonMoi);
                    updateCommand.Parameters.AddWithValue("@NgayMuon", ngayMuon);
                    updateCommand.Parameters.AddWithValue("@NgayHenTra", ngayHenTra);
                    updateCommand.Parameters.AddWithValue("@MaDG", maDG);
                    updateCommand.ExecuteNonQuery();

                    // Cập nhật số lượng sách trong bảng QLSach
                    capnhatSoLuongSach(maSach, soLuongMuonCu, soLuongMuonMoi);
                 
                    // Hiển thị thông báo thành công
                    MessageBox.Show("Cập nhật thông tin Cho Mượn Sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Xoatextbox();
                    btnLuu.Visible = btnQuaylai.Visible = false;
                    btnChoMuon.Visible = btnSua.Visible = btnXoa.Visible = btnThoatMuonSach.Visible = true;
                    capnhatdataDanhSach();
                    capnhatdataTraSach();

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
            txtSLMuon.Enabled = true;
            if (dtgvDanhsach.SelectedRows.Count > 0)
             {
                 MessageBox.Show("Bạn đã chọn chức năng Sửa Mượn Sách. Vui lòng chỉnh sửa thông tin và nhấn 'Lưu' để hoàn thành.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                 cbMadocgia.Enabled = txtMaSachChoMuon.Enabled = false;
                 btnLuu.Visible = btnQuaylai.Visible = true;
                 btnChoMuon.Visible = btnSua.Visible = btnXoa.Visible = false;
                
            }
             else
             {
                 MessageBox.Show("Vui lòng chọn một sách để sửa thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
             }
        }
       
        private void luu_xoa()
        {
            if (dtgvDanhsach.SelectedRows.Count > 0)
            {
                string maDG = dtgvDanhsach.SelectedRows[0].Cells["MaDG"].Value.ToString();
                string maSach = dtgvDanhsach.SelectedRows[0].Cells["MaSach"].Value.ToString();
                int soLuong = Convert.ToInt32(dtgvDanhsach.SelectedRows[0].Cells["SoLuong"].Value);
                string ngayMuon = Convert.ToDateTime(dtgvDanhsach.SelectedRows[0].Cells["NgayMuon"].Value).ToString("yyyy-MM-dd");
                string ngayHenTra = Convert.ToDateTime(dtgvDanhsach.SelectedRows[0].Cells["NgayHenTra"].Value).ToString("yyyy-MM-dd");

                try
                {
                    using (SqlConnection connection = new SqlConnection(ketnoi))
                    {
                        connection.Open();

                        // Lấy số lượng hiện tại của sách
                        string selectQuery = "SELECT SoLuong FROM QLSach WHERE MaSach = @MaSach";
                        SqlCommand selectCommand = new SqlCommand(selectQuery, connection);
                        selectCommand.Parameters.AddWithValue("@MaSach", maSach);
                        int soLuongHienTai = Convert.ToInt32(selectCommand.ExecuteScalar());

                        // Tính toán số lượng mới sau khi xóa bản ghi mượn sách
                        int soLuongMoi = soLuongHienTai + soLuong;

                        // Cập nhật lại số lượng sách trong bảng QLSach
                        string updateQuery = "UPDATE QLSach SET SoLuong = @SoLuongMoi WHERE MaSach = @MaSach";
                        SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                        updateCommand.Parameters.AddWithValue("@SoLuongMoi", soLuongMoi);
                        updateCommand.Parameters.AddWithValue("@MaSach", maSach);
                        updateCommand.ExecuteNonQuery();

                        // Xóa bản ghi mượn trả sách trong bảng QLMuonTraSach
                        string deleteQuery = "DELETE FROM QLMuonTraSach WHERE MaDG = @MaDG AND MaSach = @MaSach AND SoLuong = @SoLuong AND NgayMuon = @NgayMuon AND NgayHenTra = @NgayHenTra";
                        SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection);
                        deleteCommand.Parameters.AddWithValue("@MaDG", maDG);
                        deleteCommand.Parameters.AddWithValue("@MaSach", maSach);
                        deleteCommand.Parameters.AddWithValue("@SoLuong", soLuong);
                        deleteCommand.Parameters.AddWithValue("@NgayMuon", ngayMuon);
                        deleteCommand.Parameters.AddWithValue("@NgayHenTra", ngayHenTra);
                        deleteCommand.ExecuteNonQuery();

                        MessageBox.Show("Đã xoá thông tin Cho Mượn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        capnhatdataDanhSach();
                        capnhatdataTraSach();
                        Xoatextbox();
                        btnLuu.Visible = btnQuaylai.Visible = false;
                        btnChoMuon.Visible = btnSua.Visible = btnXoa.Visible = btnThoatMuonSach.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một bản ghi để xoá.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void btnXoa_Click(object sender, EventArgs e)
        {
            chuyennut = "Xoa";
            // Kiểm tra xem người dùng đã chọn dòng nào trên DataGridView chưa
            if (dtgvDanhsach.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn Độc Giả Mượn Sách cần xoá trước khi thực hiện xoá.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            MessageBox.Show("Bạn đã chọn chức năng Xoá Độc Giả Mượn Sách . Vui lòng nhập thông tin và nhấn 'Lưu' để hoàn thành.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            btnLuu.Visible = btnQuaylai.Visible = true;
            btnChoMuon.Visible = btnSua.Visible = btnXoa.Visible = false;
        }
        private void btnQuaylai_Click(object sender, EventArgs e)
        {
            Xoatextbox();
            Vohieutextbox();
            btnLuu.Visible = btnQuaylai.Visible = false;
            btnChoMuon.Visible = btnSua.Visible = btnXoa.Visible = btnThoatMuonSach.Visible = true;
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
        //222222222222222222222222222222222222222222222//
        private void capnhatdataTraSach()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ketnoi))
                {
                    connection.Open();

                    //string query = "SELECT  MaDG, MaSach, SoLuong, NgayMuon, NgayHenTra, NgayTra FROM QLMuonTraSach";

                    string query = @"SELECT m.MaDG, m.MaSach, m.SoLuong, m.NgayMuon, m.NgayHenTra, m.NgayTra, s.TenSach
                             FROM QLMuonTraSach m
                             INNER JOIN QLSach s ON m.MaSach = s.MaSach";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Thiết lập DataSource cho DataGridView
                    dtgvDanhsach_TS.DataSource = dataTable;

                    dtgvDanhsach_TS.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dtgvDanhsach_TS.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dtgvDanhsach_TS.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dtgvDanhsach_TS.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dtgvDanhsach_TS.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dtgvDanhsach_TS.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dtgvDanhsach_TS.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                    // Đổi tên header cho các cột
                    dtgvDanhsach_TS.Columns["MaDG"].HeaderText = "Mã ĐG";
                    dtgvDanhsach_TS.Columns["MaSach"].HeaderText = "Mã Sách";
                    dtgvDanhsach_TS.Columns["TenSach"].HeaderText = "Tên Sách";
                    dtgvDanhsach_TS.Columns["SoLuong"].HeaderText = "SL";
                    dtgvDanhsach_TS.Columns["NgayMuon"].HeaderText = "Ngày Mượn";
                    dtgvDanhsach_TS.Columns["NgayHenTra"].HeaderText = "Ngày Hẹn Trả";
                    dtgvDanhsach_TS.Columns["NgayTra"].HeaderText = "Ngày Trả";

                    // Xác định chỉ số cột của TenSach trong DataGridView
                    int tenSachColumnIndex = dtgvDanhsach_TS.Columns["TenSach"].Index;

                    // Di chuyển cột TenSach đến vị trí mong muốn (ví dụ: sau cột SoLuong)
                    dtgvDanhsach_TS.Columns["TenSach"].DisplayIndex = 2; // Ví dụ: hiển thị sau cột số lượng (chỉ số 2)

                    // Ẩn cột MaSach nếu không cần thiết
                    //dtgvDanhsach_TS.Columns["MaSach"].Visible = false;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void dtgvDanhsach_TS_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Xử lý sự kiện click vào một ô trong DataGridView
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dtgvDanhsach_TS.Rows[e.RowIndex];

                // Lấy các giá trị từ dòng được chọn và hiển thị lên các TextBox và DateTimePicker
                txtMaDG_TS.Text = selectedRow.Cells["MaDG"].Value.ToString();
                txtMaSach_TS.Text = selectedRow.Cells["MaSach"].Value.ToString();
                txtSoLuong_TS.Text = selectedRow.Cells["SoLuong"].Value.ToString();
                txtTenSach_TS.Text = selectedRow.Cells["TenSach"].Value.ToString(); // Hiển thị tên sách

                DateTime ngayMuon = Convert.ToDateTime(selectedRow.Cells["NgayMuon"].Value);
                dtpkNgayMuon_TS.Value = ngayMuon;
                DateTime ngayHenTra = Convert.ToDateTime(selectedRow.Cells["NgayHenTra"].Value);
                dtpkNgayHenTra_TS.Value = ngayHenTra;

                // Kiểm tra giá trị NgayTra để hiển thị lên DateTimePicker NgayTra
                if (selectedRow.Cells["NgayTra"].Value != DBNull.Value)
                {
                    DateTime ngayTra = Convert.ToDateTime(selectedRow.Cells["NgayTra"].Value);
                    dtpkNgayTra_TS.Value = ngayTra;
                }
                else
                {
                    // Nếu NgayTra là null, không hiển thị giá trị trên DateTimePicker NgayTra
                    dtpkNgayTra_TS.Value = DateTime.Now; // Hoặc có thể để ngày mặc định khác tại đây
                }
            }
        }
        private void capnhatSoLuongTraSach(string maSach, int soLuongTra)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ketnoi))
                {
                    connection.Open();

                    // Lấy số lượng hiện tại của sách
                    string selectQuery = "SELECT SoLuong FROM QLSach WHERE MaSach = @MaSach";
                    SqlCommand selectCommand = new SqlCommand(selectQuery, connection);
                    selectCommand.Parameters.AddWithValue("@MaSach", maSach);
                    int soLuongHienTai = Convert.ToInt32(selectCommand.ExecuteScalar());

                    // Tính toán số lượng mới của sách
                    int soLuongMoi = soLuongHienTai + soLuongTra;

                    // Cập nhật số lượng mới của sách
                    string updateQuery = "UPDATE QLSach SET SoLuong = @SoLuongMoi WHERE MaSach = @MaSach";
                    SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                    updateCommand.Parameters.AddWithValue("@SoLuongMoi", soLuongMoi);
                    updateCommand.Parameters.AddWithValue("@MaSach", maSach);
                    updateCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật số lượng sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTraSach_TS_Click(object sender, EventArgs e)
        {

            if (dtgvDanhsach_TS.CurrentRow != null)
            {
                DataGridViewRow selectedRow = dtgvDanhsach_TS.CurrentRow;

                string maDG = selectedRow.Cells["MaDG"].Value.ToString();
                string maSach = selectedRow.Cells["MaSach"].Value.ToString();
                int soLuongMuon = Convert.ToInt32(selectedRow.Cells["SoLuong"].Value);

                DateTime ngayMuon = Convert.ToDateTime(selectedRow.Cells["NgayMuon"].Value);
                DateTime ngayHenTra = Convert.ToDateTime(selectedRow.Cells["NgayHenTra"].Value);

                try
                {
                    using (SqlConnection connection = new SqlConnection(ketnoi))
                    {
                        connection.Open();

                        // Kiểm tra nếu NgayTra đã có giá trị
                        string checkNgayTraQuery = "SELECT NgayTra FROM QLMuonTraSach WHERE MaDG = @MaDG AND MaSach = @MaSach AND NgayMuon = @NgayMuon AND NgayHenTra = @NgayHenTra";
                        SqlCommand checkNgayTraCommand = new SqlCommand(checkNgayTraQuery, connection);
                        checkNgayTraCommand.Parameters.AddWithValue("@MaDG", maDG);
                        checkNgayTraCommand.Parameters.AddWithValue("@MaSach", maSach);
                        checkNgayTraCommand.Parameters.AddWithValue("@NgayMuon", ngayMuon);
                        checkNgayTraCommand.Parameters.AddWithValue("@NgayHenTra", ngayHenTra);
                        object ngayTraObj = checkNgayTraCommand.ExecuteScalar();

                        if (ngayTraObj != DBNull.Value)
                        {
                            // Nếu NgayTra đã có giá trị
                            txtThongBao_TS.Text = "Sách đã được trả.";
                            return;
                        }

                        // Lấy ngày trả mới từ DateTimePicker NgayTra
                        DateTime ngayTra = dtpkNgayTra_TS.Value;

                        // Kiểm tra xem ngày trả có được chọn chưa
                        if (ngayTra == DateTime.MinValue)
                        {
                            MessageBox.Show("Vui lòng chọn ngày trả sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Cập nhật ngày trả trong bảng QLMuonTraSach
                        string updateNgayTraQuery = "UPDATE QLMuonTraSach SET NgayTra = @NgayTra WHERE MaDG = @MaDG AND MaSach = @MaSach AND NgayMuon = @NgayMuon AND NgayHenTra = @NgayHenTra";
                        SqlCommand updateNgayTraCommand = new SqlCommand(updateNgayTraQuery, connection);
                        updateNgayTraCommand.Parameters.AddWithValue("@NgayTra", ngayTra);
                        updateNgayTraCommand.Parameters.AddWithValue("@MaDG", maDG);
                        updateNgayTraCommand.Parameters.AddWithValue("@MaSach", maSach);
                        updateNgayTraCommand.Parameters.AddWithValue("@NgayMuon", ngayMuon);
                        updateNgayTraCommand.Parameters.AddWithValue("@NgayHenTra", ngayHenTra);
                        updateNgayTraCommand.ExecuteNonQuery();

                        // Cập nhật số lượng sách trong bảng QLSach
                        capnhatSoLuongTraSach(maSach, soLuongMuon);

                        // Hiển thị thông báo thành công
                        MessageBox.Show("Trả sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        capnhatdataTraSach();

                        // Kiểm tra xem ngày trả có đúng hạn không
                        if (ngayTra <= ngayHenTra)
                        {
                            txtThongBao_TS.Text = "Đúng hạn!";
                            
                        }
                        else
                        {
                            txtThongBao_TS.Text = "Quá hạn!";
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
                MessageBox.Show("Vui lòng chọn một sách để trả.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            capnhatdataDanhSach();
        }
    }
}
