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
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1284, 561);
            this.Controls.Add(this.lbEvents);
            this.Controls.Add(this.lblDebug);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Torre di Hanoi";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        private Label lblDebug;
        private ListBox lbEvents;
    }
}
