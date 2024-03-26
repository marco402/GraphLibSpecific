namespace GraphLib
{
    partial class PlotterDisplayEx
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
       //private System.ComponentModel.IContainer components = null;
        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelTmax = new System.Windows.Forms.Label();
            this.labelT0 = new System.Windows.Forms.Label();
            this.labelUseMouseWheel = new System.Windows.Forms.Label();
            this.hScrollBarStartX = new System.Windows.Forms.HScrollBar();
            this.gPane = new GraphLib.PlotterGraphPaneEx();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            this.splitContainer1.Panel1.ForeColor = System.Drawing.SystemColors.ControlText;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.Panel2.Controls.Add(this.gPane);
            this.splitContainer1.Size = new System.Drawing.Size(598, 339);
            this.splitContainer1.SplitterDistance = 34;
            this.splitContainer1.TabIndex = 2;
            this.splitContainer1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelTmax);
            this.panel1.Controls.Add(this.labelT0);
            this.panel1.Controls.Add(this.labelUseMouseWheel);
            this.panel1.Controls.Add(this.hScrollBarStartX);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(417, 28);
            this.panel1.TabIndex = 7;
            // 
            // labelTmax
            // 
            this.labelTmax.AutoSize = true;
            this.labelTmax.Location = new System.Drawing.Point(240, 8);
            this.labelTmax.Name = "labelTmax";
            this.labelTmax.Size = new System.Drawing.Size(33, 13);
            this.labelTmax.TabIndex = 5;
            this.labelTmax.Text = "Tmax";
            // 
            // labelT0
            // 
            this.labelT0.AutoSize = true;
            this.labelT0.Location = new System.Drawing.Point(10, 8);
            this.labelT0.Name = "labelT0";
            this.labelT0.Size = new System.Drawing.Size(20, 13);
            this.labelT0.TabIndex = 3;
            this.labelT0.Text = "T0";
            // 
            // labelUseMouseWheel
            // 
            this.labelUseMouseWheel.AutoSize = true;
            this.labelUseMouseWheel.Location = new System.Drawing.Point(279, 8);
            this.labelUseMouseWheel.Name = "labelUseMouseWheel";
            this.labelUseMouseWheel.Size = new System.Drawing.Size(134, 13);
            this.labelUseMouseWheel.TabIndex = 2;
            this.labelUseMouseWheel.Text = "Use mouse wheel for zoom";
            // 
            // hScrollBarStartX
            // 
            this.hScrollBarStartX.LargeChange = 1;
            this.hScrollBarStartX.Location = new System.Drawing.Point(33, 11);
            this.hScrollBarStartX.Maximum = 10000;
            this.hScrollBarStartX.Name = "hScrollBarStartX";
            this.hScrollBarStartX.Size = new System.Drawing.Size(204, 10);
            this.hScrollBarStartX.TabIndex = 4;
            this.hScrollBarStartX.ValueChanged += new System.EventHandler(this.hScrollBar1_ValueChanged);
            // 
            // gPane
            // 
            this.gPane.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gPane.Location = new System.Drawing.Point(0, 0);
            this.gPane.Name = "gPane";
            this.gPane.Size = new System.Drawing.Size(598, 301);
            this.gPane.TabIndex = 1;
            // 
            // PlotterDisplayEx
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.splitContainer1);
            this.Name = "PlotterDisplayEx";
            this.Size = new System.Drawing.Size(598, 339);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.HScrollBar hScrollBarStartX;
        private PlotterGraphPaneEx gPane;
        private System.Windows.Forms.Label labelT0;
        private System.Windows.Forms.Label labelUseMouseWheel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelTmax;
    }
}
