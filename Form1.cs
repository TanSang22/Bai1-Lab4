using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp5.Models;

namespace WindowsFormsApp5
{
    public partial class Form1 : Form
    {
        private Model1 context;
        private Student SelectedStudent;
        public Form1()
        {
            InitializeComponent();
        }
        private void loadData()
        {
            context = new Model1();
            List<Student> lstStudent = context.Students.ToList();
            List<Faculty> lstFaculty = context.Faculties.ToList();
            fillFacultyComboBox(lstFaculty);
            BlindGrid(lstStudent);

        }
        private void ClearForm()
        {
            txtMSSV.Clear();
            txtHoTen.Clear();
            txtDiemTB.Clear();
            cboKhoa.SelectedIndex = -1;
        }
        private void BlindGrid(List<Student> lstStudent)
        {
            dataGridView1.Rows.Clear();
            foreach (var item in lstStudent)
            {
                int index = dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Cells[0].Value = item.StudentID;
                dataGridView1.Rows[index].Cells[1].Value = item.FullName;
                dataGridView1.Rows[index].Cells[2].Value = item.Faculty.FacultyName;
                dataGridView1.Rows[index].Cells[3].Value = item.AverageScore;

            }

        }
        private void fillFacultyComboBox(List<Faculty> lstfaculty)
        {
            cboKhoa.DataSource = lstfaculty;
            cboKhoa.ValueMember = "FacultyID";
            cboKhoa.DisplayMember = "FacultyName";
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            loadData();
        }
        private void dtgSinhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                string studentId = row.Cells[0].Value.ToString();
                SelectedStudent = context.Students.FirstOrDefault(s => s.StudentID == studentId);
                if (studentId != null)
                {
                    txtMSSV.Text = SelectedStudent.StudentID.ToString();
                    txtHoTen.Text = SelectedStudent.FullName;
                    cboKhoa.SelectedValue = SelectedStudent.FacultyID;
                    txtDiemTB.Text = SelectedStudent.AverageScore.ToString();


                }

            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMSSV.Text) ||
                string.IsNullOrWhiteSpace(txtHoTen.Text) ||
                string.IsNullOrWhiteSpace(txtDiemTB.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }
            if (txtMSSV.Text.Length != 10)
            {
                MessageBox.Show("Mã số sinh viên phải có 10 kí tự!");
                return;
            }
            try
            {
                Student student = new Student()
                {
                    StudentID = txtMSSV.Text,
                    FullName = txtHoTen.Text,
                    FacultyID = Convert.ToInt32(cboKhoa.SelectedValue),
                    AverageScore = float.Parse(txtDiemTB.Text)
                };
                context.Students.Add(student);
                context.SaveChanges();
                ClearForm();
                loadData();
                MessageBox.Show("Thêm mới dữ liệu thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSua_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMSSV.Text) ||
        string.IsNullOrWhiteSpace(txtHoTen.Text) ||
        string.IsNullOrWhiteSpace(txtDiemTB.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }
            if (txtMSSV.Text.Length != 10)
            {
                MessageBox.Show("Mã số sinh viên phải có 10 kí tự!");
                return;
            }

            if (SelectedStudent != null)
            {
                try
                {
                    SelectedStudent.FullName = txtHoTen.Text;
                    SelectedStudent.AverageScore = float.Parse(txtDiemTB.Text);
                    SelectedStudent.FacultyID = Convert.ToInt32(cboKhoa.SelectedValue);

                    context.SaveChanges();
                    ClearForm();
                    loadData();
                    MessageBox.Show("Cập nhật dữ liệu thành công!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy MSSV cần sửa!");
            }
        }

        private void btnXoa_Click_1(object sender, EventArgs e)
        {
            if (SelectedStudent == null)
            {
                MessageBox.Show("Không tìm thấy MSSV cần xóa!");
                return;
            }
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa sinh viên có mã số " + SelectedStudent.StudentID + "?",
                                                  "Thông báo",
                                                  MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                try
                {
                    context.Students.Remove(SelectedStudent);
                    context.SaveChanges();

                    MessageBox.Show("Xóa sinh viên thành công!");
                    ClearForm();
                    loadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult rs = MessageBox.Show("Ban co chac chan thoat !", "Xac Nhan", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            if (rs == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                string studentId = row.Cells[0].Value.ToString();

                // Debug log
                Console.WriteLine("Selected Student ID: " + studentId);

                SelectedStudent = context.Students.FirstOrDefault(s => s.StudentID == studentId);

                if (SelectedStudent != null)
                {
                    txtMSSV.Text = SelectedStudent.StudentID;
                    txtHoTen.Text = SelectedStudent.FullName;
                    cboKhoa.SelectedValue = SelectedStudent.FacultyID;
                    txtDiemTB.Text = SelectedStudent.AverageScore.ToString();
                }
            }
        }
    }
}
