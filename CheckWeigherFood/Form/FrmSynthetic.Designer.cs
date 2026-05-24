namespace CheckWeigherFood.FrmChild
{
  partial class FrmSynthetic
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
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSynthetic));
      this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      this.dgvDataProducts = new System.Windows.Forms.DataGridView();
      this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
      this.cbShift = new System.Windows.Forms.ComboBox();
      this.label4 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.btnPreview = new System.Windows.Forms.Button();
      this.dtpFrom = new System.Windows.Forms.DateTimePicker();
      this.label1 = new System.Windows.Forms.Label();
      this.txtFGs = new System.Windows.Forms.TextBox();
      this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
      this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
      this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
      this.tableLayoutPanel1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dgvDataProducts)).BeginInit();
      this.tableLayoutPanel3.SuspendLayout();
      this.SuspendLayout();
      // 
      // tableLayoutPanel1
      // 
      this.tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(25)))), ((int)(((byte)(68)))));
      this.tableLayoutPanel1.ColumnCount = 1;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel1.Controls.Add(this.dgvDataProducts, 0, 3);
      this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 0);
      this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1);
      this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel, 0, 2);
      this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 4;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel1.Size = new System.Drawing.Size(1483, 652);
      this.tableLayoutPanel1.TabIndex = 1;
      // 
      // dgvDataProducts
      // 
      dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(37)))), ((int)(((byte)(78)))));
      this.dgvDataProducts.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
      this.dgvDataProducts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.dgvDataProducts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
      this.dgvDataProducts.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(19)))), ((int)(((byte)(52)))));
      dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(119)))), ((int)(((byte)(170)))));
      dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
      dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dgvDataProducts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
      this.dgvDataProducts.ColumnHeadersHeight = 35;
      dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
      dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
      dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.dgvDataProducts.DefaultCellStyle = dataGridViewCellStyle3;
      this.dgvDataProducts.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnF2;
      this.dgvDataProducts.EnableHeadersVisualStyles = false;
      this.dgvDataProducts.GridColor = System.Drawing.Color.DimGray;
      this.dgvDataProducts.Location = new System.Drawing.Point(0, 119);
      this.dgvDataProducts.Margin = new System.Windows.Forms.Padding(0);
      this.dgvDataProducts.Name = "dgvDataProducts";
      dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
      this.dgvDataProducts.Size = new System.Drawing.Size(1483, 533);
      this.dgvDataProducts.TabIndex = 6;
      // 
      // tableLayoutPanel3
      // 
      this.tableLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tableLayoutPanel3.ColumnCount = 11;
      this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
      this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 320F));
      this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
      this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
      this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
      this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel3.Controls.Add(this.cbShift, 7, 0);
      this.tableLayoutPanel3.Controls.Add(this.label4, 0, 0);
      this.tableLayoutPanel3.Controls.Add(this.label3, 6, 0);
      this.tableLayoutPanel3.Controls.Add(this.btnPreview, 9, 0);
      this.tableLayoutPanel3.Controls.Add(this.dtpFrom, 3, 0);
      this.tableLayoutPanel3.Controls.Add(this.label1, 2, 0);
      this.tableLayoutPanel3.Controls.Add(this.txtFGs, 1, 0);
      this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
      this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
      this.tableLayoutPanel3.Name = "tableLayoutPanel3";
      this.tableLayoutPanel3.RowCount = 1;
      this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel3.Size = new System.Drawing.Size(1483, 50);
      this.tableLayoutPanel3.TabIndex = 1;
      // 
      // cbShift
      // 
      this.cbShift.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.cbShift.FormattingEnabled = true;
      this.cbShift.Items.AddRange(new object[] {
            "Shift 1",
            "Shift 2",
            "Shift 3",
            "All"});
      this.cbShift.Location = new System.Drawing.Point(656, 8);
      this.cbShift.Margin = new System.Windows.Forms.Padding(3, 8, 3, 3);
      this.cbShift.Name = "cbShift";
      this.cbShift.Size = new System.Drawing.Size(94, 32);
      this.cbShift.TabIndex = 30;
      // 
      // label4
      // 
      this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.label4.AutoSize = true;
      this.label4.BackColor = System.Drawing.Color.Transparent;
      this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label4.ForeColor = System.Drawing.Color.White;
      this.label4.Location = new System.Drawing.Point(10, 0);
      this.label4.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(45, 50);
      this.label4.TabIndex = 23;
      this.label4.Text = "FGs";
      this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // label3
      // 
      this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.label3.AutoSize = true;
      this.label3.BackColor = System.Drawing.Color.Transparent;
      this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label3.ForeColor = System.Drawing.Color.White;
      this.label3.Location = new System.Drawing.Point(608, 0);
      this.label3.Margin = new System.Windows.Forms.Padding(15, 0, 0, 0);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(45, 50);
      this.label3.TabIndex = 29;
      this.label3.Text = "Shift";
      this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // btnPreview
      // 
      this.btnPreview.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(151)))), ((int)(((byte)(206)))));
      this.btnPreview.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnPreview.ForeColor = System.Drawing.Color.White;
      this.btnPreview.Image = ((System.Drawing.Image)(resources.GetObject("btnPreview.Image")));
      this.btnPreview.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.btnPreview.Location = new System.Drawing.Point(766, 3);
      this.btnPreview.Name = "btnPreview";
      this.btnPreview.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
      this.btnPreview.Size = new System.Drawing.Size(144, 44);
      this.btnPreview.TabIndex = 31;
      this.btnPreview.Text = "    Preview";
      this.btnPreview.UseVisualStyleBackColor = false;
      this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
      // 
      // dtpFrom
      // 
      this.dtpFrom.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.dtpFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.dtpFrom.Location = new System.Drawing.Point(276, 10);
      this.dtpFrom.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
      this.dtpFrom.Name = "dtpFrom";
      this.dtpFrom.Size = new System.Drawing.Size(314, 29);
      this.dtpFrom.TabIndex = 26;
      this.dtpFrom.ValueChanged += new System.EventHandler(this.dtpFrom_ValueChanged);
      // 
      // label1
      // 
      this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.label1.AutoSize = true;
      this.label1.BackColor = System.Drawing.Color.Transparent;
      this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label1.ForeColor = System.Drawing.Color.White;
      this.label1.Location = new System.Drawing.Point(220, 0);
      this.label1.Margin = new System.Windows.Forms.Padding(15, 0, 0, 0);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(53, 50);
      this.label1.TabIndex = 25;
      this.label1.Text = "Date:";
      this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // txtFGs
      // 
      this.txtFGs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtFGs.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtFGs.Location = new System.Drawing.Point(58, 10);
      this.txtFGs.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
      this.txtFGs.Name = "txtFGs";
      this.txtFGs.Size = new System.Drawing.Size(144, 29);
      this.txtFGs.TabIndex = 24;
      // 
      // flowLayoutPanel1
      // 
      this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 53);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new System.Drawing.Size(200, 1);
      this.flowLayoutPanel1.TabIndex = 7;
      // 
      // flowLayoutPanel
      // 
      this.flowLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.flowLayoutPanel.Location = new System.Drawing.Point(0, 55);
      this.flowLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
      this.flowLayoutPanel.Name = "flowLayoutPanel";
      this.flowLayoutPanel.Size = new System.Drawing.Size(1483, 64);
      this.flowLayoutPanel.TabIndex = 8;
      // 
      // backgroundWorker1
      // 
      this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
      this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
      // 
      // FrmSynthetic
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1483, 652);
      this.Controls.Add(this.tableLayoutPanel1);
      this.Name = "FrmSynthetic";
      this.Text = "FrmSynthetic";
      this.Load += new System.EventHandler(this.FrmSynthetic_Load);
      this.tableLayoutPanel1.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.dgvDataProducts)).EndInit();
      this.tableLayoutPanel3.ResumeLayout(false);
      this.tableLayoutPanel3.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.DataGridView dgvDataProducts;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
    private System.Windows.Forms.ComboBox cbShift;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Button btnPreview;
    private System.Windows.Forms.DateTimePicker dtpFrom;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox txtFGs;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
    private System.ComponentModel.BackgroundWorker backgroundWorker1;
  }
}