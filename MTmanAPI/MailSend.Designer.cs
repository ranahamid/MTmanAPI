namespace MTmanAPI
{
    partial class MailSend
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
            this.label1_to = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1_TO = new System.Windows.Forms.TextBox();
            this.textBox2_Subject = new System.Windows.Forms.TextBox();
            this.button1_SendMail = new System.Windows.Forms.Button();
            this.BodyTextBox_SendMail = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1_to
            // 
            this.label1_to.AutoSize = true;
            this.label1_to.Location = new System.Drawing.Point(36, 37);
            this.label1_to.Name = "label1_to";
            this.label1_to.Size = new System.Drawing.Size(20, 13);
            this.label1_to.TabIndex = 0;
            this.label1_to.Text = "To";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(39, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Subject";
            // 
            // textBox1_TO
            // 
            this.textBox1_TO.Location = new System.Drawing.Point(116, 29);
            this.textBox1_TO.Name = "textBox1_TO";
            this.textBox1_TO.Size = new System.Drawing.Size(213, 20);
            this.textBox1_TO.TabIndex = 2;
            // 
            // textBox2_Subject
            // 
            this.textBox2_Subject.Location = new System.Drawing.Point(116, 67);
            this.textBox2_Subject.Name = "textBox2_Subject";
            this.textBox2_Subject.Size = new System.Drawing.Size(213, 20);
            this.textBox2_Subject.TabIndex = 3;
            // 
            // button1_SendMail
            // 
            this.button1_SendMail.Location = new System.Drawing.Point(342, 64);
            this.button1_SendMail.Name = "button1_SendMail";
            this.button1_SendMail.Size = new System.Drawing.Size(75, 23);
            this.button1_SendMail.TabIndex = 4;
            this.button1_SendMail.Text = "Send";
            this.button1_SendMail.UseVisualStyleBackColor = true;
            this.button1_SendMail.Click += new System.EventHandler(this.button1_SendMail_Click);
            // 
            // BodyTextBox_SendMail
            // 
            this.BodyTextBox_SendMail.Location = new System.Drawing.Point(6, 102);
            this.BodyTextBox_SendMail.Multiline = true;
            this.BodyTextBox_SendMail.Name = "BodyTextBox_SendMail";
            this.BodyTextBox_SendMail.Size = new System.Drawing.Size(411, 169);
            this.BodyTextBox_SendMail.TabIndex = 5;
            // 
            // MailSend
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 280);
            this.Controls.Add(this.BodyTextBox_SendMail);
            this.Controls.Add(this.button1_SendMail);
            this.Controls.Add(this.textBox2_Subject);
            this.Controls.Add(this.textBox1_TO);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1_to);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MailSend";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Send Mail";
            this.Load += new System.EventHandler(this.MailSend_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1_to;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1_TO;
        private System.Windows.Forms.TextBox textBox2_Subject;
        private System.Windows.Forms.Button button1_SendMail;
        private System.Windows.Forms.TextBox BodyTextBox_SendMail;
    }
}