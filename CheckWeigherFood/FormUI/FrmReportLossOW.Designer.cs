namespace CheckWeigherFood.FormUI
{
  partial class FrmReportLossOW
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
      this.tableLayoutPanel22 = new System.Windows.Forms.TableLayoutPanel();
      this.dgvData = new System.Windows.Forms.DataGridView();
      this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      this.label1 = new System.Windows.Forms.Label();
      this.btnPreview = new CheckWeigherFood.RJControl.RJButton();
      this.btnExport = new CheckWeigherFood.RJControl.RJButton();
      this.cbbYear = new System.Windows.Forms.ComboBox();
      this.label2 = new System.Windows.Forms.Label();
      this.cbbWeek = new System.Windows.Forms.ComboBox();
      this.lbRangeDate = new System.Windows.Forms.Label();
      this.tableLayoutPanel22.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
      this.tableLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // tableLayoutPanel22
      // 
      this.tableLayoutPanel22.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(17)))), ((int)(((byte)(55)))));
      this.tableLayoutPanel22.ColumnCount = 1;
      this.tableLayoutPanel22.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel22.Controls.Add(this.dgvData, 0, 1);
      this.tableLayoutPanel22.Controls.Add(this.tableLayoutPanel1, 0, 0);
      this.tableLayoutPanel22.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel22.Location = new System.Drawing.Point(0, 0);
      this.tableLayoutPanel22.Name = "tableLayoutPanel22";
      this.tableLayoutPanel22.RowCount = 2;
      this.tableLayoutPanel22.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
      this.tableLayoutPanel22.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel22.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
      this.tableLayoutPanel22.Size = new System.Drawing.Size(1284, 403);
      this.tableLayoutPanel22.TabIndex = 4;
      // 
      // dgvData
      // 
      this.dgvData.AllowUserToResizeColumns = false;
      this.dgvData.AllowUserToResizeRows = false;
      dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(37)))), ((int)(((byte)(78)))));
      this.dgvData.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
      this.dgvData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.dgvData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
      this.dgvData.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(19)))), ((int)(((byte)(52)))));
      dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(119)))), ((int)(((byte)(170)))));
      dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
      dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dgvData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
      this.dgvData.ColumnHeadersHeight = 50;
      dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
      dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
      dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.dgvData.DefaultCellStyle = dataGridViewCellStyle3;
      this.dgvData.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnF2;
      this.dgvData.EnableHeadersVisualStyles = false;
      this.dgvData.GridColor = System.Drawing.Color.DimGray;
      this.dgvData.Location = new System.Drawing.Point(3, 63);
      this.dgvData.Name = "dgvData";
      this.dgvData.ReadOnly = true;
      dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dgvData.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
      this.dgvData.RowHeadersVisible = false;
      this.dgvData.RowHeadersWidth = 60;
      dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(63)))), ((int)(((byte)(116)))));
      this.dgvData.RowsDefaultCellStyle = dataGridViewCellStyle5;
      this.dgvData.RowTemplate.Height = 35;
      this.dgvData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.dgvData.Size = new System.Drawing.Size(1278, 337);
      this.dgvData.TabIndex = 38;
      // 
      // tableLayoutPanel1
      // 
      this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tableLayoutPanel1.ColumnCount = 11;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 3F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
      this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
      this.tableLayoutPanel1.Controls.Add(this.btnPreview, 8, 0);
      this.tableLayoutPanel1.Controls.Add(this.btnExport, 9, 0);
      this.tableLayoutPanel1.Controls.Add(this.cbbYear, 1, 0);
      this.tableLayoutPanel1.Controls.Add(this.label2, 3, 0);
      this.tableLayoutPanel1.Controls.Add(this.cbbWeek, 4, 0);
      this.tableLayoutPanel1.Controls.Add(this.lbRangeDate, 6, 0);
      this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
      this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 1;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel1.Size = new System.Drawing.Size(1284, 60);
      this.tableLayoutPanel1.TabIndex = 6;
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
      this.label1.Location = new System.Drawing.Point(15, 0);
      this.label1.Margin = new System.Windows.Forms.Padding(15, 0, 0, 0);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(50, 60);
      this.label1.TabIndex = 26;
      this.label1.Text = "Năm";
      this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // btnPreview
      // 
      this.btnPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      this.btnPreview.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(68)))), ((int)(((byte)(108)))));
      this.btnPreview.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(68)))), ((int)(((byte)(108)))));
      this.btnPreview.BorderColor = System.Drawing.Color.PaleVioletRed;
      this.btnPreview.BorderRadius = 5;
      this.btnPreview.BorderSize = 0;
      this.btnPreview.FlatAppearance.BorderSize = 0;
      this.btnPreview.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnPreview.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnPreview.ForeColor = System.Drawing.Color.White;
      this.btnPreview.Location = new System.Drawing.Point(964, 7);
      this.btnPreview.Name = "btnPreview";
      this.btnPreview.Size = new System.Drawing.Size(154, 45);
      this.btnPreview.TabIndex = 34;
      this.btnPreview.Text = "Preview";
      this.btnPreview.TextColor = System.Drawing.Color.White;
      this.btnPreview.UseVisualStyleBackColor = false;
      // 
      // btnExport
      // 
      this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      this.btnExport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(68)))), ((int)(((byte)(108)))));
      this.btnExport.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(68)))), ((int)(((byte)(108)))));
      this.btnExport.BorderColor = System.Drawing.Color.PaleVioletRed;
      this.btnExport.BorderRadius = 5;
      this.btnExport.BorderSize = 0;
      this.btnExport.FlatAppearance.BorderSize = 0;
      this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnExport.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnExport.ForeColor = System.Drawing.Color.White;
      this.btnExport.Location = new System.Drawing.Point(1124, 7);
      this.btnExport.Name = "btnExport";
      this.btnExport.Size = new System.Drawing.Size(154, 45);
      this.btnExport.TabIndex = 35;
      this.btnExport.Text = "Export";
      this.btnExport.TextColor = System.Drawing.Color.White;
      this.btnExport.UseVisualStyleBackColor = false;
      // 
      // cbbYear
      // 
      this.cbbYear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      this.cbbYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cbbYear.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.cbbYear.FormattingEnabled = true;
      this.cbbYear.Location = new System.Drawing.Point(68, 13);
      this.cbbYear.Name = "cbbYear";
      this.cbbYear.Size = new System.Drawing.Size(104, 33);
      this.cbbYear.TabIndex = 39;
      // 
      // label2
      // 
      this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.label2.AutoSize = true;
      this.label2.BackColor = System.Drawing.Color.Transparent;
      this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label2.ForeColor = System.Drawing.Color.White;
      this.label2.Location = new System.Drawing.Point(220, 0);
      this.label2.Margin = new System.Windows.Forms.Padding(15, 0, 0, 0);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(54, 60);
      this.label2.TabIndex = 40;
      this.label2.Text = "Tuần";
      this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // cbbWeek
      // 
      this.cbbWeek.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      this.cbbWeek.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cbbWeek.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.cbbWeek.FormattingEnabled = true;
      this.cbbWeek.Location = new System.Drawing.Point(277, 13);
      this.cbbWeek.Name = "cbbWeek";
      this.cbbWeek.Size = new System.Drawing.Size(104, 33);
      this.cbbWeek.TabIndex = 41;
      // 
      // lbRangeDate
      // 
      this.lbRangeDate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lbRangeDate.AutoSize = true;
      this.lbRangeDate.BackColor = System.Drawing.Color.Transparent;
      this.lbRangeDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lbRangeDate.ForeColor = System.Drawing.Color.White;
      this.lbRangeDate.Location = new System.Drawing.Point(429, 0);
      this.lbRangeDate.Margin = new System.Windows.Forms.Padding(15, 0, 0, 0);
      this.lbRangeDate.Name = "lbRangeDate";
      this.lbRangeDate.Size = new System.Drawing.Size(482, 60);
      this.lbRangeDate.TabIndex = 42;
      this.lbRangeDate.Text = "...";
      this.lbRangeDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // FrmReportLossOW
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1284, 403);
      this.Controls.Add(this.tableLayoutPanel22);
      this.Name = "FrmReportLossOW";
      this.Text = "FrmReportLossOW";
      this.tableLayoutPanel22.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel22;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.Label label1;
    private RJControl.RJButton btnPreview;
    private RJControl.RJButton btnExport;
    private System.Windows.Forms.DataGridView dgvData;
    private System.Windows.Forms.ComboBox cbbYear;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.ComboBox cbbWeek;
    private System.Windows.Forms.Label lbRangeDate;
  }
}