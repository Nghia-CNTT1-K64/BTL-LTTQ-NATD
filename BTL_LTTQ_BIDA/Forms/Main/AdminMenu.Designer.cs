using System.Windows.Forms;

namespace BTL_LTTQ_BIDA.Forms.Main
{
    partial class AdminMenu
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.pAdminControl = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnQLNhanVien = new System.Windows.Forms.Button();
            this.btnTroVe = new System.Windows.Forms.Button();
            this.btnThongKe = new System.Windows.Forms.Button();
            this.btnQLBan = new System.Windows.Forms.Button();
            this.btnQLDichVu = new System.Windows.Forms.Button();
            this.pAdminNhanVien = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtTenNV = new System.Windows.Forms.TextBox();
            this.cbQQTV = new System.Windows.Forms.CheckBox();
            this.txtCCCD = new System.Windows.Forms.TextBox();
            this.dtpNgaySinh = new System.Windows.Forms.DateTimePicker();
            this.txtID = new System.Windows.Forms.TextBox();
            this.btnChinhSua = new System.Windows.Forms.Button();
            this.btnThemNV = new System.Windows.Forms.Button();
            this.btnChoNghiViec = new System.Windows.Forms.Button();
            this.txtTenDN = new System.Windows.Forms.TextBox();
            this.btnTuyenDungLai = new System.Windows.Forms.Button();
            this.dgvNhanVien = new System.Windows.Forms.DataGridView();
            this.pAdminDichVu = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dgvDichVu = new System.Windows.Forms.DataGridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnChinhSuaDV = new System.Windows.Forms.Button();
            this.btnThemDV = new System.Windows.Forms.Button();
            this.btnHienThiLai = new System.Windows.Forms.Button();
            this.btnBoHienThi = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtMaDV = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtGiaDV = new System.Windows.Forms.TextBox();
            this.txtSoLuong = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtTenDV = new System.Windows.Forms.TextBox();
            this.pAdminBan = new System.Windows.Forms.Panel();
            this.cboTrangThai = new System.Windows.Forms.ComboBox();
            this.btnChinhSuaBan = new System.Windows.Forms.Button();
            this.txtGiaTien = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtMaBan = new System.Windows.Forms.TextBox();
            this.btnThemBan = new System.Windows.Forms.Button();
            this.dgvBan = new System.Windows.Forms.DataGridView();
            this.pAdminThongKe = new System.Windows.Forms.Panel();
            this.pMainMenu = new System.Windows.Forms.Panel();
            this.pAdminControl.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.pAdminNhanVien.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNhanVien)).BeginInit();
            this.pAdminDichVu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDichVu)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.pAdminBan.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBan)).BeginInit();
            this.pMainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // pAdminControl
            // 
            this.pAdminControl.Controls.Add(this.tableLayoutPanel1);
            this.pAdminControl.Dock = System.Windows.Forms.DockStyle.Left;
            this.pAdminControl.Location = new System.Drawing.Point(0, 0);
            this.pAdminControl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pAdminControl.Name = "pAdminControl";
            this.pAdminControl.Size = new System.Drawing.Size(215, 586);
            this.pAdminControl.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.btnQLNhanVien, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnTroVe, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.btnThongKe, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnQLBan, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnQLDichVu, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(215, 586);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnQLNhanVien
            // 
            this.btnQLNhanVien.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnQLNhanVien.Location = new System.Drawing.Point(3, 2);
            this.btnQLNhanVien.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnQLNhanVien.Name = "btnQLNhanVien";
            this.btnQLNhanVien.Size = new System.Drawing.Size(209, 113);
            this.btnQLNhanVien.TabIndex = 0;
            this.btnQLNhanVien.Text = "QUẢN LÝ NHÂN VIÊN";
            this.btnQLNhanVien.UseVisualStyleBackColor = true;
            this.btnQLNhanVien.Click += new System.EventHandler(this.btnQLNhanVien_Click);
            // 
            // btnTroVe
            // 
            this.btnTroVe.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTroVe.Location = new System.Drawing.Point(3, 470);
            this.btnTroVe.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnTroVe.Name = "btnTroVe";
            this.btnTroVe.Size = new System.Drawing.Size(209, 114);
            this.btnTroVe.TabIndex = 4;
            this.btnTroVe.Text = "TRỞ VỀ";
            this.btnTroVe.UseVisualStyleBackColor = true;
            this.btnTroVe.Click += new System.EventHandler(this.btnTroVe_Click);
            // 
            // btnThongKe
            // 
            this.btnThongKe.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnThongKe.Location = new System.Drawing.Point(3, 353);
            this.btnThongKe.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnThongKe.Name = "btnThongKe";
            this.btnThongKe.Size = new System.Drawing.Size(209, 113);
            this.btnThongKe.TabIndex = 3;
            this.btnThongKe.Text = "THỐNG KÊ";
            this.btnThongKe.UseVisualStyleBackColor = true;
            this.btnThongKe.Click += new System.EventHandler(this.btnThongKe_Click);
            // 
            // btnQLBan
            // 
            this.btnQLBan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnQLBan.Location = new System.Drawing.Point(3, 236);
            this.btnQLBan.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnQLBan.Name = "btnQLBan";
            this.btnQLBan.Size = new System.Drawing.Size(209, 113);
            this.btnQLBan.TabIndex = 2;
            this.btnQLBan.Text = "QUẢN LÝ BÀN";
            this.btnQLBan.UseVisualStyleBackColor = true;
            this.btnQLBan.Click += new System.EventHandler(this.btnQLBan_Click);
            // 
            // btnQLDichVu
            // 
            this.btnQLDichVu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnQLDichVu.Location = new System.Drawing.Point(3, 119);
            this.btnQLDichVu.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnQLDichVu.Name = "btnQLDichVu";
            this.btnQLDichVu.Size = new System.Drawing.Size(209, 113);
            this.btnQLDichVu.TabIndex = 1;
            this.btnQLDichVu.Text = "QUẢN LÝ DỊCH VỤ";
            this.btnQLDichVu.UseVisualStyleBackColor = true;
            this.btnQLDichVu.Click += new System.EventHandler(this.btnQLDichVu_Click);
            // 
            // pAdminNhanVien
            // 
            this.pAdminNhanVien.BackColor = System.Drawing.Color.White;
            this.pAdminNhanVien.Controls.Add(this.label4);
            this.pAdminNhanVien.Controls.Add(this.label3);
            this.pAdminNhanVien.Controls.Add(this.label5);
            this.pAdminNhanVien.Controls.Add(this.label2);
            this.pAdminNhanVien.Controls.Add(this.label6);
            this.pAdminNhanVien.Controls.Add(this.label1);
            this.pAdminNhanVien.Controls.Add(this.txtTenNV);
            this.pAdminNhanVien.Controls.Add(this.cbQQTV);
            this.pAdminNhanVien.Controls.Add(this.txtCCCD);
            this.pAdminNhanVien.Controls.Add(this.dtpNgaySinh);
            this.pAdminNhanVien.Controls.Add(this.txtID);
            this.pAdminNhanVien.Controls.Add(this.btnChinhSua);
            this.pAdminNhanVien.Controls.Add(this.btnThemNV);
            this.pAdminNhanVien.Controls.Add(this.btnChoNghiViec);
            this.pAdminNhanVien.Controls.Add(this.txtTenDN);
            this.pAdminNhanVien.Controls.Add(this.btnTuyenDungLai);
            this.pAdminNhanVien.Controls.Add(this.dgvNhanVien);
            this.pAdminNhanVien.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pAdminNhanVien.Location = new System.Drawing.Point(0, 0);
            this.pAdminNhanVien.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pAdminNhanVien.Name = "pAdminNhanVien";
            this.pAdminNhanVien.Size = new System.Drawing.Size(961, 586);
            this.pAdminNhanVien.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(551, 185);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 16);
            this.label4.TabIndex = 8;
            this.label4.Text = "Số CCCD";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(551, 130);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 16);
            this.label3.TabIndex = 9;
            this.label3.Text = "Ngày sinh";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(551, 240);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 16);
            this.label5.TabIndex = 7;
            this.label5.Text = "Quyền QTV";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(551, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 16);
            this.label2.TabIndex = 10;
            this.label2.Text = "Tên nhân viên";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(551, 295);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(98, 16);
            this.label6.TabIndex = 6;
            this.label6.Text = "Tên đăng nhập";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(551, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 16);
            this.label1.TabIndex = 11;
            this.label1.Text = "ID";
            // 
            // txtTenNV
            // 
            this.txtTenNV.Location = new System.Drawing.Point(681, 69);
            this.txtTenNV.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtTenNV.Name = "txtTenNV";
            this.txtTenNV.Size = new System.Drawing.Size(100, 22);
            this.txtTenNV.TabIndex = 5;
            // 
            // cbQQTV
            // 
            this.cbQQTV.AutoSize = true;
            this.cbQQTV.Location = new System.Drawing.Point(681, 236);
            this.cbQQTV.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbQQTV.Name = "cbQQTV";
            this.cbQQTV.Size = new System.Drawing.Size(18, 17);
            this.cbQQTV.TabIndex = 12;
            // 
            // txtCCCD
            // 
            this.txtCCCD.Location = new System.Drawing.Point(681, 180);
            this.txtCCCD.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtCCCD.Name = "txtCCCD";
            this.txtCCCD.Size = new System.Drawing.Size(100, 22);
            this.txtCCCD.TabIndex = 4;
            // 
            // dtpNgaySinh
            // 
            this.dtpNgaySinh.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpNgaySinh.Location = new System.Drawing.Point(681, 124);
            this.dtpNgaySinh.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dtpNgaySinh.Name = "dtpNgaySinh";
            this.dtpNgaySinh.Size = new System.Drawing.Size(200, 22);
            this.dtpNgaySinh.TabIndex = 13;
            // 
            // txtID
            // 
            this.txtID.Enabled = false;
            this.txtID.Location = new System.Drawing.Point(681, 14);
            this.txtID.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(100, 22);
            this.txtID.TabIndex = 14;
            // 
            // btnChinhSua
            // 
            this.btnChinhSua.Location = new System.Drawing.Point(681, 384);
            this.btnChinhSua.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnChinhSua.Name = "btnChinhSua";
            this.btnChinhSua.Size = new System.Drawing.Size(113, 33);
            this.btnChinhSua.TabIndex = 2;
            this.btnChinhSua.Text = "Chỉnh sửa";
            // 
            // btnThemNV
            // 
            this.btnThemNV.Location = new System.Drawing.Point(529, 384);
            this.btnThemNV.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnThemNV.Name = "btnThemNV";
            this.btnThemNV.Size = new System.Drawing.Size(136, 33);
            this.btnThemNV.TabIndex = 15;
            this.btnThemNV.Text = "Thêm nhân viên";
            // 
            // btnChoNghiViec
            // 
            this.btnChoNghiViec.Location = new System.Drawing.Point(811, 384);
            this.btnChoNghiViec.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnChoNghiViec.Name = "btnChoNghiViec";
            this.btnChoNghiViec.Size = new System.Drawing.Size(113, 33);
            this.btnChoNghiViec.TabIndex = 1;
            this.btnChoNghiViec.Text = "Cho nghỉ việc";
            // 
            // txtTenDN
            // 
            this.txtTenDN.Location = new System.Drawing.Point(681, 289);
            this.txtTenDN.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtTenDN.Name = "txtTenDN";
            this.txtTenDN.Size = new System.Drawing.Size(100, 22);
            this.txtTenDN.TabIndex = 3;
            // 
            // btnTuyenDungLai
            // 
            this.btnTuyenDungLai.Location = new System.Drawing.Point(681, 453);
            this.btnTuyenDungLai.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnTuyenDungLai.Name = "btnTuyenDungLai";
            this.btnTuyenDungLai.Size = new System.Drawing.Size(113, 33);
            this.btnTuyenDungLai.TabIndex = 0;
            this.btnTuyenDungLai.Text = "Tuyển dụng lại";
            // 
            // dgvNhanVien
            // 
            this.dgvNhanVien.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvNhanVien.Location = new System.Drawing.Point(3, 2);
            this.dgvNhanVien.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgvNhanVien.Name = "dgvNhanVien";
            this.dgvNhanVien.RowHeadersWidth = 51;
            this.dgvNhanVien.Size = new System.Drawing.Size(505, 465);
            this.dgvNhanVien.TabIndex = 17;
            // 
            // pAdminDichVu
            // 
            this.pAdminDichVu.BackColor = System.Drawing.Color.White;
            this.pAdminDichVu.Controls.Add(this.splitContainer1);
            this.pAdminDichVu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pAdminDichVu.Location = new System.Drawing.Point(0, 0);
            this.pAdminDichVu.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pAdminDichVu.Name = "pAdminDichVu";
            this.pAdminDichVu.Size = new System.Drawing.Size(961, 586);
            this.pAdminDichVu.TabIndex = 2;
            this.pAdminDichVu.Visible = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgvDichVu);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.splitContainer1.Size = new System.Drawing.Size(961, 586);
            this.splitContainer1.SplitterDistance = 450;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 13;
            // 
            // dgvDichVu
            // 
            this.dgvDichVu.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDichVu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDichVu.Location = new System.Drawing.Point(0, 0);
            this.dgvDichVu.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgvDichVu.Name = "dgvDichVu";
            this.dgvDichVu.RowHeadersWidth = 51;
            this.dgvDichVu.Size = new System.Drawing.Size(450, 586);
            this.dgvDichVu.TabIndex = 12;
            this.dgvDichVu.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDichVu_CellDoubleClick);
            this.dgvDichVu.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDichVu_RowEnter);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnChinhSuaDV);
            this.groupBox2.Controls.Add(this.btnThemDV);
            this.groupBox2.Controls.Add(this.btnHienThiLai);
            this.groupBox2.Controls.Add(this.btnBoHienThi);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 254);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Size = new System.Drawing.Size(506, 332);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Chức năng";
            // 
            // btnChinhSuaDV
            // 
            this.btnChinhSuaDV.Location = new System.Drawing.Point(208, 100);
            this.btnChinhSuaDV.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnChinhSuaDV.Name = "btnChinhSuaDV";
            this.btnChinhSuaDV.Size = new System.Drawing.Size(140, 37);
            this.btnChinhSuaDV.TabIndex = 4;
            this.btnChinhSuaDV.Text = "Chỉnh sửa";
            this.btnChinhSuaDV.Click += new System.EventHandler(this.btnChinhSuaDV_Click);
            // 
            // btnThemDV
            // 
            this.btnThemDV.Location = new System.Drawing.Point(33, 100);
            this.btnThemDV.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnThemDV.Name = "btnThemDV";
            this.btnThemDV.Size = new System.Drawing.Size(140, 37);
            this.btnThemDV.TabIndex = 11;
            this.btnThemDV.Text = "Thêm dịch vụ";
            this.btnThemDV.Click += new System.EventHandler(this.btnThemDV_Click);
            // 
            // btnHienThiLai
            // 
            this.btnHienThiLai.Location = new System.Drawing.Point(33, 180);
            this.btnHienThiLai.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnHienThiLai.Name = "btnHienThiLai";
            this.btnHienThiLai.Size = new System.Drawing.Size(140, 37);
            this.btnHienThiLai.TabIndex = 2;
            this.btnHienThiLai.Text = "Hiển thị lại";
            this.btnHienThiLai.Click += new System.EventHandler(this.btnHienThiLai_Click);
            // 
            // btnBoHienThi
            // 
            this.btnBoHienThi.Location = new System.Drawing.Point(208, 180);
            this.btnBoHienThi.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnBoHienThi.Name = "btnBoHienThi";
            this.btnBoHienThi.Size = new System.Drawing.Size(140, 37);
            this.btnBoHienThi.TabIndex = 3;
            this.btnBoHienThi.Text = "Bỏ hiển thị";
            this.btnBoHienThi.Click += new System.EventHandler(this.btnBoHienThi_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.txtMaDV);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.txtGiaDV);
            this.groupBox1.Controls.Add(this.txtSoLuong);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.txtTenDV);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(506, 254);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Thông tin dịch vụ";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(21, 38);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(90, 20);
            this.label12.TabIndex = 9;
            this.label12.Text = "Mã dịch vụ";
            // 
            // txtMaDV
            // 
            this.txtMaDV.Location = new System.Drawing.Point(177, 34);
            this.txtMaDV.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtMaDV.Name = "txtMaDV";
            this.txtMaDV.Size = new System.Drawing.Size(144, 26);
            this.txtMaDV.TabIndex = 10;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(21, 204);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(74, 20);
            this.label13.TabIndex = 1;
            this.label13.Text = "Số lượng";
            // 
            // txtGiaDV
            // 
            this.txtGiaDV.Location = new System.Drawing.Point(177, 145);
            this.txtGiaDV.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtGiaDV.Name = "txtGiaDV";
            this.txtGiaDV.Size = new System.Drawing.Size(144, 26);
            this.txtGiaDV.TabIndex = 5;
            // 
            // txtSoLuong
            // 
            this.txtSoLuong.Location = new System.Drawing.Point(177, 201);
            this.txtSoLuong.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtSoLuong.Name = "txtSoLuong";
            this.txtSoLuong.Size = new System.Drawing.Size(144, 26);
            this.txtSoLuong.TabIndex = 0;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(21, 94);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(95, 20);
            this.label11.TabIndex = 8;
            this.label11.Text = "Tên dịch vụ";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(21, 149);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(67, 20);
            this.label9.TabIndex = 7;
            this.label9.Text = "Giá tiền";
            // 
            // txtTenDV
            // 
            this.txtTenDV.Location = new System.Drawing.Point(177, 87);
            this.txtTenDV.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtTenDV.Name = "txtTenDV";
            this.txtTenDV.Size = new System.Drawing.Size(144, 26);
            this.txtTenDV.TabIndex = 6;
            // 
            // pAdminBan
            // 
            this.pAdminBan.BackColor = System.Drawing.Color.White;
            this.pAdminBan.Controls.Add(this.cboTrangThai);
            this.pAdminBan.Controls.Add(this.btnChinhSuaBan);
            this.pAdminBan.Controls.Add(this.txtGiaTien);
            this.pAdminBan.Controls.Add(this.label7);
            this.pAdminBan.Controls.Add(this.label8);
            this.pAdminBan.Controls.Add(this.label10);
            this.pAdminBan.Controls.Add(this.txtMaBan);
            this.pAdminBan.Controls.Add(this.btnThemBan);
            this.pAdminBan.Controls.Add(this.dgvBan);
            this.pAdminBan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pAdminBan.Location = new System.Drawing.Point(0, 0);
            this.pAdminBan.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pAdminBan.Name = "pAdminBan";
            this.pAdminBan.Size = new System.Drawing.Size(961, 586);
            this.pAdminBan.TabIndex = 3;
            this.pAdminBan.Visible = false;
            // 
            // cboTrangThai
            // 
            this.cboTrangThai.Location = new System.Drawing.Point(669, 182);
            this.cboTrangThai.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cboTrangThai.Name = "cboTrangThai";
            this.cboTrangThai.Size = new System.Drawing.Size(227, 24);
            this.cboTrangThai.TabIndex = 0;
            // 
            // btnChinhSuaBan
            // 
            this.btnChinhSuaBan.Location = new System.Drawing.Point(797, 389);
            this.btnChinhSuaBan.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnChinhSuaBan.Name = "btnChinhSuaBan";
            this.btnChinhSuaBan.Size = new System.Drawing.Size(113, 33);
            this.btnChinhSuaBan.TabIndex = 1;
            this.btnChinhSuaBan.Text = "Chỉnh sửa bàn";
            // 
            // txtGiaTien
            // 
            this.txtGiaTien.Location = new System.Drawing.Point(669, 101);
            this.txtGiaTien.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtGiaTien.Name = "txtGiaTien";
            this.txtGiaTien.Size = new System.Drawing.Size(100, 22);
            this.txtGiaTien.TabIndex = 2;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(539, 190);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 16);
            this.label7.TabIndex = 3;
            this.label7.Text = "Trạng thái";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(539, 107);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(52, 16);
            this.label8.TabIndex = 4;
            this.label8.Text = "Giá tiền";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(539, 25);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(52, 16);
            this.label10.TabIndex = 5;
            this.label10.Text = "Mã bàn";
            // 
            // txtMaBan
            // 
            this.txtMaBan.Location = new System.Drawing.Point(669, 18);
            this.txtMaBan.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtMaBan.Name = "txtMaBan";
            this.txtMaBan.Size = new System.Drawing.Size(100, 22);
            this.txtMaBan.TabIndex = 6;
            // 
            // btnThemBan
            // 
            this.btnThemBan.Location = new System.Drawing.Point(541, 389);
            this.btnThemBan.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnThemBan.Name = "btnThemBan";
            this.btnThemBan.Size = new System.Drawing.Size(113, 33);
            this.btnThemBan.TabIndex = 7;
            this.btnThemBan.Text = "Thêm bàn";
            // 
            // dgvBan
            // 
            this.dgvBan.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBan.Location = new System.Drawing.Point(3, 2);
            this.dgvBan.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgvBan.Name = "dgvBan";
            this.dgvBan.RowHeadersWidth = 51;
            this.dgvBan.Size = new System.Drawing.Size(509, 465);
            this.dgvBan.TabIndex = 8;
            // 
            // pAdminThongKe
            // 
            this.pAdminThongKe.BackColor = System.Drawing.Color.White;
            this.pAdminThongKe.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pAdminThongKe.Location = new System.Drawing.Point(0, 0);
            this.pAdminThongKe.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pAdminThongKe.Name = "pAdminThongKe";
            this.pAdminThongKe.Size = new System.Drawing.Size(961, 586);
            this.pAdminThongKe.TabIndex = 4;
            this.pAdminThongKe.Visible = false;
            // 
            // pMainMenu
            // 
            this.pMainMenu.Controls.Add(this.pAdminDichVu);
            this.pMainMenu.Controls.Add(this.pAdminNhanVien);
            this.pMainMenu.Controls.Add(this.pAdminBan);
            this.pMainMenu.Controls.Add(this.pAdminThongKe);
            this.pMainMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pMainMenu.Location = new System.Drawing.Point(215, 0);
            this.pMainMenu.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pMainMenu.Name = "pMainMenu";
            this.pMainMenu.Size = new System.Drawing.Size(961, 586);
            this.pMainMenu.TabIndex = 18;
            // 
            // AdminMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1176, 586);
            this.Controls.Add(this.pMainMenu);
            this.Controls.Add(this.pAdminControl);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "AdminMenu";
            this.Text = "AdminMenu";
            this.Load += new System.EventHandler(this.AdminMenu_Load);
            this.pAdminControl.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pAdminNhanVien.ResumeLayout(false);
            this.pAdminNhanVien.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNhanVien)).EndInit();
            this.pAdminDichVu.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDichVu)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.pAdminBan.ResumeLayout(false);
            this.pAdminBan.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBan)).EndInit();
            this.pMainMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pAdminControl;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnTroVe;
        private System.Windows.Forms.Button btnThongKe;
        private System.Windows.Forms.Button btnQLBan;
        private System.Windows.Forms.Button btnQLDichVu;
        private System.Windows.Forms.Button btnQLNhanVien;
        private System.Windows.Forms.Panel pAdminNhanVien;
        private System.Windows.Forms.Panel pAdminDichVu;
        private System.Windows.Forms.Button btnHienThiLai;
        private System.Windows.Forms.Button btnBoHienThi;
        private System.Windows.Forms.Button btnChinhSuaDV;
        private System.Windows.Forms.TextBox txtGiaDV;
        private System.Windows.Forms.TextBox txtTenDV;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtMaDV;
        private System.Windows.Forms.Button btnThemDV;
        private System.Windows.Forms.DataGridView dgvDichVu;
        private System.Windows.Forms.Panel pAdminBan;
        private System.Windows.Forms.ComboBox cboTrangThai;
        private System.Windows.Forms.Button btnChinhSuaBan;
        private System.Windows.Forms.TextBox txtGiaTien;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtMaBan;
        private System.Windows.Forms.Button btnThemBan;
        private System.Windows.Forms.DataGridView dgvBan;
        private System.Windows.Forms.Panel pAdminThongKe;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtSoLuong;
        private Panel pMainMenu;
        private DataGridView dgvNhanVien;
        private Label label1;
        private Button btnTuyenDungLai;
        private Button btnChoNghiViec;
        private Button btnChinhSua;
        private TextBox txtTenDN;
        private TextBox txtCCCD;
        private TextBox txtTenNV;
        private Label label6;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private CheckBox cbQQTV;
        private DateTimePicker dtpNgaySinh;
        private TextBox txtID;
        private Button btnThemNV;
        private SplitContainer splitContainer1;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
    }
}
