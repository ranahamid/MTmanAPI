namespace MTmanAPI
{
    partial class NewsSend
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
            this.BodyTextBox_SendNews = new System.Windows.Forms.TextBox();
            this.SendButton_SendMail = new System.Windows.Forms.Button();
            this.SubjectTextBox_NewsSend = new System.Windows.Forms.TextBox();
            this.CategoryTextBox_NewsSend = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1_category = new System.Windows.Forms.Label();
            this.HighPriorityCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // BodyTextBox_SendNews
            // 
            this.BodyTextBox_SendNews.Location = new System.Drawing.Point(5, 102);
            this.BodyTextBox_SendNews.Multiline = true;
            this.BodyTextBox_SendNews.Name = "BodyTextBox_SendNews";
            this.BodyTextBox_SendNews.Size = new System.Drawing.Size(411, 169);
            this.BodyTextBox_SendNews.TabIndex = 11;
            // 
            // SendButton_SendMail
            // 
            this.SendButton_SendMail.Location = new System.Drawing.Point(341, 64);
            this.SendButton_SendMail.Name = "SendButton_SendMail";
            this.SendButton_SendMail.Size = new System.Drawing.Size(75, 23);
            this.SendButton_SendMail.TabIndex = 10;
            this.SendButton_SendMail.Text = "Send";
            this.SendButton_SendMail.UseVisualStyleBackColor = true;
            this.SendButton_SendMail.Click += new System.EventHandler(this.SendButton_SendMailClick);
            // 
            // SubjectTextBox_NewsSend
            // 
            this.SubjectTextBox_NewsSend.Location = new System.Drawing.Point(115, 67);
            this.SubjectTextBox_NewsSend.Name = "SubjectTextBox_NewsSend";
            this.SubjectTextBox_NewsSend.Size = new System.Drawing.Size(213, 20);
            this.SubjectTextBox_NewsSend.TabIndex = 9;
            // 
            // CategoryTextBox_NewsSend
            // 
            this.CategoryTextBox_NewsSend.Location = new System.Drawing.Point(115, 29);
            this.CategoryTextBox_NewsSend.Name = "CategoryTextBox_NewsSend";
            this.CategoryTextBox_NewsSend.Size = new System.Drawing.Size(213, 20);
            this.CategoryTextBox_NewsSend.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(38, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Subject";
            // 
            // label1_category
            // 
            this.label1_category.AutoSize = true;
            this.label1_category.Location = new System.Drawing.Point(35, 37);
            this.label1_category.Name = "label1_category";
            this.label1_category.Size = new System.Drawing.Size(49, 13);
            this.label1_category.TabIndex = 6;
            this.label1_category.Text = "Category";
            // 
            // HighPriorityCheckBox
            // 
            this.HighPriorityCheckBox.AutoCheck = false;
            this.HighPriorityCheckBox.AutoSize = true;
            this.HighPriorityCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.HighPriorityCheckBox.Location = new System.Drawing.Point(341, 29);
            this.HighPriorityCheckBox.Name = "HighPriorityCheckBox";
            this.HighPriorityCheckBox.Size = new System.Drawing.Size(82, 17);
            this.HighPriorityCheckBox.TabIndex = 12;
            this.HighPriorityCheckBox.Text = "High Priority";
            this.HighPriorityCheckBox.UseVisualStyleBackColor = true;
            // 
            // NewsSend
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 280);
            this.Controls.Add(this.HighPriorityCheckBox);
            this.Controls.Add(this.BodyTextBox_SendNews);
            this.Controls.Add(this.SendButton_SendMail);
            this.Controls.Add(this.SubjectTextBox_NewsSend);
            this.Controls.Add(this.CategoryTextBox_NewsSend);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1_category);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewsSend";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "Send News";
            this.Text = "Send News";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox BodyTextBox_SendNews;
        private System.Windows.Forms.Button SendButton_SendMail;
        private System.Windows.Forms.TextBox SubjectTextBox_NewsSend;
        private System.Windows.Forms.TextBox CategoryTextBox_NewsSend;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1_category;
        private System.Windows.Forms.CheckBox HighPriorityCheckBox;
    }
}