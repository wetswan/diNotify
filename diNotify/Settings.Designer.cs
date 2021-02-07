
namespace diNotify
{
    partial class Settings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            this.btnStartListener = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnSendTestToast = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.cbBeep = new System.Windows.Forms.CheckBox();
            this.cbLed = new System.Windows.Forms.CheckBox();
            this.cbShowAppName = new System.Windows.Forms.CheckBox();
            this.seconds = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.seconds)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStartListener
            // 
            this.btnStartListener.Location = new System.Drawing.Point(36, 27);
            this.btnStartListener.Name = "btnStartListener";
            this.btnStartListener.Size = new System.Drawing.Size(183, 34);
            this.btnStartListener.TabIndex = 0;
            this.btnStartListener.Text = "Start listener";
            this.btnStartListener.UseVisualStyleBackColor = true;
            this.btnStartListener.Click += new System.EventHandler(this.button1_ClickAsync);
            // 
            // btnReset
            // 
            this.btnReset.Enabled = false;
            this.btnReset.Location = new System.Drawing.Point(36, 86);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(183, 34);
            this.btnReset.TabIndex = 1;
            this.btnReset.Text = "Reset display";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.ResetDisplay);
            // 
            // btnSendTestToast
            // 
            this.btnSendTestToast.Enabled = false;
            this.btnSendTestToast.Location = new System.Drawing.Point(243, 27);
            this.btnSendTestToast.Name = "btnSendTestToast";
            this.btnSendTestToast.Size = new System.Drawing.Size(183, 34);
            this.btnSendTestToast.TabIndex = 2;
            this.btnSendTestToast.Text = "Send Toast";
            this.btnSendTestToast.UseVisualStyleBackColor = true;
            this.btnSendTestToast.Click += new System.EventHandler(this.SendTestToastMessage);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(36, 174);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(731, 264);
            this.richTextBox1.TabIndex = 3;
            this.richTextBox1.Text = "";
            // 
            // cbBeep
            // 
            this.cbBeep.AutoSize = true;
            this.cbBeep.Location = new System.Drawing.Point(460, 27);
            this.cbBeep.Name = "cbBeep";
            this.cbBeep.Size = new System.Drawing.Size(197, 29);
            this.cbBeep.TabIndex = 4;
            this.cbBeep.Text = "Beep on notification";
            this.cbBeep.UseVisualStyleBackColor = true;
            // 
            // cbLed
            // 
            this.cbLed.AutoSize = true;
            this.cbLed.Location = new System.Drawing.Point(460, 62);
            this.cbLed.Name = "cbLed";
            this.cbLed.Size = new System.Drawing.Size(239, 29);
            this.cbLed.TabIndex = 5;
            this.cbLed.Text = "LED blinks on notification";
            this.cbLed.UseVisualStyleBackColor = true;
            // 
            // cbShowAppName
            // 
            this.cbShowAppName.AutoSize = true;
            this.cbShowAppName.Location = new System.Drawing.Point(460, 97);
            this.cbShowAppName.Name = "cbShowAppName";
            this.cbShowAppName.Size = new System.Drawing.Size(223, 29);
            this.cbShowAppName.TabIndex = 6;
            this.cbShowAppName.Text = "Show application name";
            this.cbShowAppName.UseVisualStyleBackColor = true;
            // 
            // seconds
            // 
            this.seconds.Location = new System.Drawing.Point(460, 137);
            this.seconds.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.seconds.Name = "seconds";
            this.seconds.Size = new System.Drawing.Size(46, 31);
            this.seconds.TabIndex = 7;
            this.seconds.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(512, 139);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(273, 25);
            this.label1.TabIndex = 8;
            this.label1.Text = "seconds a notication is displayed";
            // 
            // trayIcon
            // 
            this.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("trayIcon.Icon")));
            this.trayIcon.Text = "diNotify";
            this.trayIcon.Visible = true;
            this.trayIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.trayIcon_MouseDoubleClick);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.seconds);
            this.Controls.Add(this.cbShowAppName);
            this.Controls.Add(this.cbLed);
            this.Controls.Add(this.cbBeep);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.btnSendTestToast);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnStartListener);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Settings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "diNotify";
            this.Resize += new System.EventHandler(this.ResizeHandler);
            ((System.ComponentModel.ISupportInitialize)(this.seconds)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStartListener;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnSendTestToast;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.CheckBox cbBeep;
        private System.Windows.Forms.CheckBox cbLed;
        private System.Windows.Forms.CheckBox cbShowAppName;
        private System.Windows.Forms.NumericUpDown seconds;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NotifyIcon trayIcon;
    }
}

