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
    public partial class MailSend : Form
    {
        public ClrWrapper clrWrapper = new ClrWrapper();

        public Form1 Frm1;

        public MailSend(ClrWrapper c)
        {
            clrWrapper = c;
            InitializeComponent();
        }

        /// <summary>
        /// Mail Send
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_SendMail_Click(object sender, EventArgs e)
        {
            MailBox mailBox = new MailBox();
            mailBox.Body = BodyTextBox_SendMail.ToString();
            mailBox.Subject = textBox2_Subject.ToString();

            mailBox.To =int.Parse(textBox1_TO.ToString());

            IList<int> logins = new List<int>();

            int status = clrWrapper.MailSend(mailBox, logins);
            MessageBox.Show(clrWrapper.ErrorDescription(status));

        }

        private void MailSend_Load(object sender, EventArgs e)
        {
            
       
        }
    }
}
