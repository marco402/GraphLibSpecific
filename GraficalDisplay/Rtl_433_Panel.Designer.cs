namespace GraficDisplay
{
    partial class Rtl_433_Panel
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
            this.buttonAddGraph = new System.Windows.Forms.Button();
            this.buttonAddLabel = new System.Windows.Forms.Button();
            this.buttonRefreshData = new System.Windows.Forms.Button();
            this.buttonStartTimerRefreshData = new System.Windows.Forms.Button();
            this.buttonStopTimerRefreshData = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonAddGraph
            // 
            this.buttonAddGraph.Location = new System.Drawing.Point(363, 81);
            this.buttonAddGraph.Name = "buttonAddGraph";
            this.buttonAddGraph.Size = new System.Drawing.Size(185, 57);
            this.buttonAddGraph.TabIndex = 0;
            this.buttonAddGraph.Text = "Add Graph";
            this.buttonAddGraph.UseVisualStyleBackColor = true;
            this.buttonAddGraph.Click += new System.EventHandler(this.buttonAddGraph_Click);
            // 
            // buttonAddLabel
            // 
            this.buttonAddLabel.Location = new System.Drawing.Point(124, 81);
            this.buttonAddLabel.Name = "buttonAddLabel";
            this.buttonAddLabel.Size = new System.Drawing.Size(185, 57);
            this.buttonAddLabel.TabIndex = 1;
            this.buttonAddLabel.Text = "Add Label";
            this.buttonAddLabel.UseVisualStyleBackColor = true;
            this.buttonAddLabel.Click += new System.EventHandler(this.buttonAddLabel_Click);
            // 
            // buttonRefreshData
            // 
            this.buttonRefreshData.Location = new System.Drawing.Point(363, 182);
            this.buttonRefreshData.Name = "buttonRefreshData";
            this.buttonRefreshData.Size = new System.Drawing.Size(185, 57);
            this.buttonRefreshData.TabIndex = 0;
            this.buttonRefreshData.Text = "refresh data";
            this.buttonRefreshData.UseVisualStyleBackColor = true;
            this.buttonRefreshData.Click += new System.EventHandler(this.buttonRefreshData_Click);
            // 
            // buttonStartTimerRefreshData
            // 
            this.buttonStartTimerRefreshData.Location = new System.Drawing.Point(363, 272);
            this.buttonStartTimerRefreshData.Name = "buttonStartTimerRefreshData";
            this.buttonStartTimerRefreshData.Size = new System.Drawing.Size(185, 57);
            this.buttonStartTimerRefreshData.TabIndex = 0;
            this.buttonStartTimerRefreshData.Text = "start timer refresh data";
            this.buttonStartTimerRefreshData.UseVisualStyleBackColor = true;
            this.buttonStartTimerRefreshData.Click += new System.EventHandler(this.buttonStartTimerRefreshData_Click);
            // 
            // buttonStopTimerRefreshData
            // 
            this.buttonStopTimerRefreshData.Location = new System.Drawing.Point(363, 360);
            this.buttonStopTimerRefreshData.Name = "buttonStopTimerRefreshData";
            this.buttonStopTimerRefreshData.Size = new System.Drawing.Size(185, 57);
            this.buttonStopTimerRefreshData.TabIndex = 0;
            this.buttonStopTimerRefreshData.Text = "stop timer refresh data";
            this.buttonStopTimerRefreshData.UseVisualStyleBackColor = true;
            this.buttonStopTimerRefreshData.Click += new System.EventHandler(this.buttonStopTimerRefreshData_Click);
            // 
            // Rtl_433_Panel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.buttonAddLabel);
            this.Controls.Add(this.buttonStopTimerRefreshData);
            this.Controls.Add(this.buttonStartTimerRefreshData);
            this.Controls.Add(this.buttonRefreshData);
            this.Controls.Add(this.buttonAddGraph);
            this.Location = new System.Drawing.Point(0, 320);
            this.Name = "Rtl_433_Panel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "FormMain";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonAddGraph;
        private System.Windows.Forms.Button buttonAddLabel;
        private System.Windows.Forms.Button buttonRefreshData;
        private System.Windows.Forms.Button buttonStartTimerRefreshData;
        private System.Windows.Forms.Button buttonStopTimerRefreshData;
    }
}