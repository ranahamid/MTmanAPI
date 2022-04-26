using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using P23.MetaTrader4.Manager;
using P23.MetaTrader4.Manager.Contracts;

namespace MTmanAPI
{
    public partial class NewsSend : Form
    {
        public ClrWrapper clrWrapper = new ClrWrapper();
        public Form1 Frm1;

        public NewsSend(ClrWrapper c)
        {
            clrWrapper = c;
            InitializeComponent();
        }

        /// <summary>
        /// Publish news
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendButton_SendMailClick(object sender, EventArgs e)
        {
            string category = CategoryTextBox_NewsSend.Text;
            string subject = SubjectTextBox_NewsSend.Text;
            bool priority = HighPriorityCheckBox.Checked;
            string body = BodyTextBox_SendNews.Text;

            NewsTopic news = new NewsTopic();
            news.Category = category;
            news.Topic = subject;
            news.Body = body;

            int status=clrWrapper.NewsSend(news);
            MessageBox.Show(clrWrapper.ErrorDescription(status));

        }
    }
}
