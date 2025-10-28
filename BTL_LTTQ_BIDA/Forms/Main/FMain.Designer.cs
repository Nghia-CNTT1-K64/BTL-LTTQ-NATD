namespace BTL_LTTQ_BIDA
{
    partial class FMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelHeader = new System.Windows.Forms.Panel();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.panelRight = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.QTVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ThongTinTkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.treeViewHoaDon = new System.Windows.Forms.TreeView();
            this.btnXuatHD = new System.Windows.Forms.Button();
            this.btnThemHD = new System.Windows.Forms.Button();
            this.panelHeader.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.panelRight.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.Controls.Add(this.menuStrip1);
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1082, 30);
            this.panelHeader.TabIndex = 0;
            // 
            // panelLeft
            // 
            this.panelLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelLeft.Controls.Add(this.btnXuatHD);
            this.panelLeft.Controls.Add(this.treeViewHoaDon);
            this.panelLeft.Location = new System.Drawing.Point(0, 31);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(362, 860);
            this.panelLeft.TabIndex = 0;
            // 
            // panelRight
            // 
            this.panelRight.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelRight.Controls.Add(this.btnThemHD);
            this.panelRight.Location = new System.Drawing.Point(368, 31);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(714, 860);
            this.panelRight.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.QTVToolStripMenuItem,
            this.ThongTinTkToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(266, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // QTVToolStripMenuItem
            // 
            this.QTVToolStripMenuItem.Name = "QTVToolStripMenuItem";
            this.QTVToolStripMenuItem.Size = new System.Drawing.Size(107, 24);
            this.QTVToolStripMenuItem.Text = "Quản trị viên";
            // 
            // ThongTinTkToolStripMenuItem
            // 
            this.ThongTinTkToolStripMenuItem.Name = "ThongTinTkToolStripMenuItem";
            this.ThongTinTkToolStripMenuItem.Size = new System.Drawing.Size(151, 24);
            this.ThongTinTkToolStripMenuItem.Text = "Thông tin tài khoản";
            // 
            // treeViewHoaDon
            // 
            this.treeViewHoaDon.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewHoaDon.Location = new System.Drawing.Point(3, 3);
            this.treeViewHoaDon.Name = "treeViewHoaDon";
            this.treeViewHoaDon.Size = new System.Drawing.Size(356, 757);
            this.treeViewHoaDon.TabIndex = 1;
            // 
            // btnXuatHD
            // 
            this.btnXuatHD.Location = new System.Drawing.Point(214, 783);
            this.btnXuatHD.Name = "btnXuatHD";
            this.btnXuatHD.Size = new System.Drawing.Size(127, 58);
            this.btnXuatHD.TabIndex = 2;
            this.btnXuatHD.Text = "Xuất hóa đơn";
            this.btnXuatHD.UseVisualStyleBackColor = true;
            // 
            // btnThemHD
            // 
            this.btnThemHD.Location = new System.Drawing.Point(3, 5);
            this.btnThemHD.Name = "btnThemHD";
            this.btnThemHD.Size = new System.Drawing.Size(144, 57);
            this.btnThemHD.TabIndex = 0;
            this.btnThemHD.Text = "Thêm hóa đơn";
            this.btnThemHD.UseVisualStyleBackColor = true;
            this.btnThemHD.Click += new System.EventHandler(this.btnThemHD_Click);
            // 
            // FMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1082, 903);
            this.Controls.Add(this.panelRight);
            this.Controls.Add(this.panelLeft);
            this.Controls.Add(this.panelHeader);
            this.Name = "FMain";
            this.Text = "Form1";
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelLeft.ResumeLayout(false);
            this.panelRight.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem QTVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ThongTinTkToolStripMenuItem;
        private System.Windows.Forms.Button btnXuatHD;
        private System.Windows.Forms.TreeView treeViewHoaDon;
        private System.Windows.Forms.Button btnThemHD;
    }
}

