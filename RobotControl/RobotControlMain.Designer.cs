namespace RobotControl
{
    partial class RobotControlMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.txtRobotIPAddress = new System.Windows.Forms.TextBox();
            this.txtCameraRSTLURL = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lstLabelsToFind = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.pctImage = new System.Windows.Forms.PictureBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
            this.lblRobotStatus = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pctImage)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Robot IP Address:";
            // 
            // txtRobotIPAddress
            // 
            this.txtRobotIPAddress.Location = new System.Drawing.Point(12, 31);
            this.txtRobotIPAddress.Name = "txtRobotIPAddress";
            this.txtRobotIPAddress.Size = new System.Drawing.Size(246, 23);
            this.txtRobotIPAddress.TabIndex = 1;
            this.txtRobotIPAddress.TextChanged += new System.EventHandler(this.txtRobotIPAddress_TextChanged);
            // 
            // txtCameraRSTLURL
            // 
            this.txtCameraRSTLURL.Location = new System.Drawing.Point(12, 86);
            this.txtCameraRSTLURL.Name = "txtCameraRSTLURL";
            this.txtCameraRSTLURL.Size = new System.Drawing.Size(246, 23);
            this.txtCameraRSTLURL.TabIndex = 3;
            this.txtCameraRSTLURL.TextChanged += new System.EventHandler(this.txtCameraRSTLURL_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Camera RSTP URL:";
            // 
            // lstLabelsToFind
            // 
            this.lstLabelsToFind.FormattingEnabled = true;
            this.lstLabelsToFind.ItemHeight = 15;
            this.lstLabelsToFind.Location = new System.Drawing.Point(12, 143);
            this.lstLabelsToFind.Name = "lstLabelsToFind";
            this.lstLabelsToFind.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lstLabelsToFind.Size = new System.Drawing.Size(246, 169);
            this.lstLabelsToFind.TabIndex = 4;
            this.lstLabelsToFind.SelectedIndexChanged += new System.EventHandler(this.lstLabelsToFind_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "Items to Find:";
            // 
            // btnStart
            // 
            this.btnStart.Enabled = false;
            this.btnStart.Location = new System.Drawing.Point(12, 327);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(246, 22);
            this.btnStart.TabIndex = 6;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            // 
            // pctImage
            // 
            this.pctImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pctImage.Location = new System.Drawing.Point(277, 85);
            this.pctImage.Name = "pctImage";
            this.pctImage.Size = new System.Drawing.Size(378, 264);
            this.pctImage.TabIndex = 7;
            this.pctImage.TabStop = false;
            // 
            // lblRobotStatus
            // 
            this.lblRobotStatus.AutoSize = true;
            this.lblRobotStatus.Location = new System.Drawing.Point(277, 34);
            this.lblRobotStatus.Name = "lblRobotStatus";
            this.lblRobotStatus.Size = new System.Drawing.Size(84, 15);
            this.lblRobotStatus.TabIndex = 8;
            this.lblRobotStatus.Text = "lblRobotStatus";
            // 
            // RobotControlMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(677, 375);
            this.Controls.Add(this.lblRobotStatus);
            this.Controls.Add(this.pctImage);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lstLabelsToFind);
            this.Controls.Add(this.txtCameraRSTLURL);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtRobotIPAddress);
            this.Controls.Add(this.label1);
            this.Name = "RobotControlMain";
            this.Text = "RobotControlMain";
            ((System.ComponentModel.ISupportInitialize)(this.pctImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private TextBox txtRobotIPAddress;
        private TextBox txtCameraRSTLURL;
        private Label label2;
        private ListBox lstLabelsToFind;
        private Label label3;
        private Button btnStart;
        private PictureBox pctImage;
        private ToolTip toolTip1;
        private ToolTip toolTip2;
        private Label lblRobotStatus;
    }
}