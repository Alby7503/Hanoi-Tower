namespace Torre_di_Hanoi
{
    partial class MainForm
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
            this.lblDebug = new System.Windows.Forms.Label();
            this.lbEvents = new System.Windows.Forms.ListBox();
            this.btnResolve = new System.Windows.Forms.Button();
            this.cntDisks = new System.Windows.Forms.NumericUpDown();
            this.lblDisksNumber = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.cntDisks)).BeginInit();
            this.SuspendLayout();
            // 
            // lblDebug
            // 
            this.lblDebug.AutoSize = true;
            this.lblDebug.Location = new System.Drawing.Point(12, 9);
            this.lblDebug.Name = "lblDebug";
            this.lblDebug.Size = new System.Drawing.Size(71, 15);
            this.lblDebug.TabIndex = 1;
            this.lblDebug.Text = "Coordinates";
            // 
            // lbEvents
            // 
            this.lbEvents.FormattingEnabled = true;
            this.lbEvents.ItemHeight = 15;
            this.lbEvents.Location = new System.Drawing.Point(12, 455);
            this.lbEvents.Name = "lbEvents";
            this.lbEvents.Size = new System.Drawing.Size(1260, 94);
            this.lbEvents.TabIndex = 2;
            // 
            // btnResolve
            // 
            this.btnResolve.Location = new System.Drawing.Point(1197, 426);
            this.btnResolve.Name = "btnResolve";
            this.btnResolve.Size = new System.Drawing.Size(75, 23);
            this.btnResolve.TabIndex = 3;
            this.btnResolve.Text = "Resolve";
            this.btnResolve.UseVisualStyleBackColor = true;
            this.btnResolve.Click += new System.EventHandler(this.Resolve);
            // 
            // cntDisks
            // 
            this.cntDisks.Location = new System.Drawing.Point(1235, 12);
            this.cntDisks.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.cntDisks.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.cntDisks.Name = "cntDisks";
            this.cntDisks.Size = new System.Drawing.Size(37, 23);
            this.cntDisks.TabIndex = 4;
            this.cntDisks.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.cntDisks.ValueChanged += new System.EventHandler(this.DisksNumber_Changed);
            // 
            // lblDisksNumber
            // 
            this.lblDisksNumber.AutoSize = true;
            this.lblDisksNumber.Location = new System.Drawing.Point(1150, 14);
            this.lblDisksNumber.Name = "lblDisksNumber";
            this.lblDisksNumber.Size = new System.Drawing.Size(79, 15);
            this.lblDisksNumber.TabIndex = 5;
            this.lblDisksNumber.Text = "Disks number";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1284, 561);
            this.Controls.Add(this.lblDisksNumber);
            this.Controls.Add(this.cntDisks);
            this.Controls.Add(this.btnResolve);
            this.Controls.Add(this.lbEvents);
            this.Controls.Add(this.lblDebug);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Torre di Hanoi";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.cntDisks)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        private Label lblDebug;
        private ListBox lbEvents;
        private Button btnResolve;
        private NumericUpDown cntDisks;
        private Label lblDisksNumber;
    }
}
