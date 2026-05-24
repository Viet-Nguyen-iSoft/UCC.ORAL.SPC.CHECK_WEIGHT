namespace CheckWeigherFood.FrmChild
{
  partial class FrmMasterData
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMasterData));
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
      this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
      this.btnExport = new System.Windows.Forms.Button();
      this.btnImportExcel = new System.Windows.Forms.Button();
      this.btnSave = new System.Windows.Forms.Button();
      this.dgvDataProducts = new System.Windows.Forms.DataGridView();
      this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
      this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
      this.tableLayoutPanel1.SuspendLayout();
      this.tableLayoutPanel2.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dgvDataProducts)).BeginInit();
      this.SuspendLayout();
      // 
      // tableLayoutPanel1
      // 
      this.tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(25)))), ((int)(((byte)(68)))));
      this.tableLayoutPanel1.ColumnCount = 1;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
      this.tableLayoutPanel1.Controls.Add(this.dgvDataProducts, 0, 2);
      this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 3;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 450);
      this.tableLayoutPanel1.TabIndex = 3;
      // 
      // tableLayoutPanel2
      // 
      this.tableLayoutPanel2.ColumnCount = 4;
      this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
      this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
      this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
      this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel2.Controls.Add(this.btnExport, 0, 0);
      this.tableLayoutPanel2.Controls.Add(this.btnImportExcel, 0, 0);
      this.tableLayoutPanel2.Controls.Add(this.btnSave, 2, 0);
      this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
      this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
      this.tableLayoutPanel2.Name = "tableLayoutPanel2";
      this.tableLayoutPanel2.RowCount = 1;
      this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel2.Size = new System.Drawing.Size(800, 55);
      this.tableLayoutPanel2.TabIndex = 2;
      // 
      // btnExport
      // 
      this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.btnExport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(151)))), ((int)(((byte)(206)))));
      this.btnExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnExport.ForeColor = System.Drawing.Color.White;
      this.btnExport.Image = ((System.Drawing.Image)(resources.GetObject("btnExport.Image")));
      this.btnExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.btnExport.Location = new System.Drawing.Point(203, 3);
      this.btnExport.Name = "btnExport";
      this.btnExport.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
      this.btnExport.Size = new System.Drawing.Size(194, 49);
      this.btnExport.TabIndex = 21;
      this.btnExport.Text = "    Export Excel";
      this.btnExport.UseVisualStyleBackColor = false;
      this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
      // 
      // btnImportExcel
      // 
      this.btnImportExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.btnImportExcel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(151)))), ((int)(((byte)(206)))));
      this.btnImportExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnImportExcel.ForeColor = System.Drawing.Color.White;
      this.btnImportExcel.Image = ((System.Drawing.Image)(resources.GetObject("btnImportExcel.Image")));
      this.btnImportExcel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.btnImportExcel.Location = new System.Drawing.Point(3, 3);
      this.btnImportExcel.Name = "btnImportExcel";
      this.btnImportExcel.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
      this.btnImportExcel.Size = new System.Drawing.Size(194, 49);
      this.btnImportExcel.TabIndex = 19;
      this.btnImportExcel.Text = "    Import Excel";
      this.btnImportExcel.UseVisualStyleBackColor = false;
      this.btnImportExcel.Click += new System.EventHandler(this.btnImportExcel_Click);
      // 
      // btnSave
      // 
      this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(151)))), ((int)(((byte)(206)))));
      this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnSave.ForeColor = System.Drawing.Color.White;
      this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
      this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.btnSave.Location = new System.Drawing.Point(403, 3);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new System.Drawing.Size(144, 49);
      this.btnSave.TabIndex = 20;
      this.btnSave.Text = "   Save";
      this.btnSave.UseVisualStyleBackColor = false;
      this.btnSave.Visible = false;
      this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
      // 
      // dgvDataProducts
      // 
      dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(37)))), ((int)(((byte)(78)))));
      this.dgvDataProducts.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
      this.dgvDataProducts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
      this.dgvDataProducts.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(19)))), ((int)(((byte)(52)))));
      dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(119)))), ((int)(((byte)(170)))));
      dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
      dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dgvDataProducts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
      this.dgvDataProducts.ColumnHeadersHeight = 85;
      dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
      dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
      dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.dgvDataProducts.DefaultCellStyle = dataGridViewCellStyle3;
      this.dgvDataProducts.Dock = System.Windows.Forms.DockStyle.Fill;
      this.dgvDataProducts.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnF2;
      this.dgvDataProducts.EnableHeadersVisualStyles = false;
      this.dgvDataProducts.GridColor = System.Drawing.Color.DimGray;
      this.dgvDataProducts.Location = new System.Drawing.Point(3, 63);
      this.dgvDataProducts.Name = "dgvDataProducts";
      dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dgvDataProducts.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
      this.dgvDataProducts.RowHeadersVisible = false;
      this.dgvDataProducts.RowHeadersWidth = 60;
      dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(63)))), ((int)(((byte)(116)))));
      this.dgvDataProducts.RowsDefaultCellStyle = dataGridViewCellStyle5;
      this.dgvDataProducts.RowTemplate.Height = 35;
      this.dgvDataProducts.Size = new System.Drawing.Size(794, 384);
      this.dgvDataProducts.TabIndex = 3;
      // 
      // backgroundWorker1
      // 
      this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
      this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
      // 
      // openFileDialog1
      // 
      this.openFileDialog1.FileName = "openFileDialog1";
      this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
      // 
      // FrmMasterData
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(800, 450);
      this.Controls.Add(this.tableLayoutPanel1);
      this.Name = "FrmMasterData";
      this.Text = "FrmMasterData";
      this.Load += new System.EventHandler(this.FrmMasterData_Load);
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.dgvDataProducts)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    private System.Windows.Forms.Button btnExport;
    private System.Windows.Forms.Button btnImportExcel;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.DataGridView dgvDataProducts;
    private System.ComponentModel.BackgroundWorker backgroundWorker1;
    private System.Windows.Forms.OpenFileDialog openFileDialog1;
  }
}