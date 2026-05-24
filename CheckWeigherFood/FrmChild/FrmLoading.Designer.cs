namespace CheckWeigherFood.FrmChild
{
  partial class FrmLoading
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
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmLoading));
      this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
      this.pictureBox1 = new System.Windows.Forms.PictureBox();
      this.progressBar1 = new System.Windows.Forms.ProgressBar();
      this.label1 = new System.Windows.Forms.Label();
      this.timerTimeOut = new System.Windows.Forms.Timer(this.components);
      this.tableLayoutPanel1.SuspendLayout();
      this.tableLayoutPanel2.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
      this.SuspendLayout();
      // 
      // tableLayoutPanel1
      // 
      this.tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
      this.tableLayoutPanel1.ColumnCount = 3;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 5F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 5F));
      this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 1);
      this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 3;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
      this.tableLayoutPanel1.Size = new System.Drawing.Size(430, 420);
      this.tableLayoutPanel1.TabIndex = 0;
      // 
      // tableLayoutPanel2
      // 
      this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tableLayoutPanel2.BackColor = System.Drawing.Color.White;
      this.tableLayoutPanel2.ColumnCount = 1;
      this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel2.Controls.Add(this.pictureBox1, 0, 0);
      this.tableLayoutPanel2.Controls.Add(this.progressBar1, 0, 2);
      this.tableLayoutPanel2.Controls.Add(this.label1, 0, 1);
      this.tableLayoutPanel2.Location = new System.Drawing.Point(5, 5);
      this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
      this.tableLayoutPanel2.Name = "tableLayoutPanel2";
      this.tableLayoutPanel2.RowCount = 3;
      this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
      this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
      this.tableLayoutPanel2.Size = new System.Drawing.Size(420, 410);
      this.tableLayoutPanel2.TabIndex = 0;
      // 
      // pictureBox1
      // 
      this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
      this.pictureBox1.Location = new System.Drawing.Point(0, 0);
      this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new System.Drawing.Size(420, 340);
      this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBox1.TabIndex = 0;
      this.pictureBox1.TabStop = false;
      // 
      // progressBar1
      // 
      this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.progressBar1.Location = new System.Drawing.Point(3, 378);
      this.progressBar1.Name = "progressBar1";
      this.progressBar1.Size = new System.Drawing.Size(414, 29);
      this.progressBar1.Step = 5;
      this.progressBar1.TabIndex = 2;
      // 
      // label1
      // 
      this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.label1.AutoSize = true;
      this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label1.Location = new System.Drawing.Point(3, 340);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(414, 35);
      this.label1.TabIndex = 1;
      this.label1.Text = "Loading ...";
      this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // timerTimeOut
      // 
      this.timerTimeOut.Interval = 1000;
      this.timerTimeOut.Tick += new System.EventHandler(this.timerTimeOut_Tick);
      // 
      // FrmLoading
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(430, 420);
      this.Controls.Add(this.tableLayoutPanel1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.Name = "FrmLoading";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "FrmLoading";
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel2.ResumeLayout(false);
      this.tableLayoutPanel2.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    private System.Windows.Forms.PictureBox pictureBox1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Timer timerTimeOut;
    private System.Windows.Forms.ProgressBar progressBar1;
  }
}