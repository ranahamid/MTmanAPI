using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using P23.MetaTrader4.Manager;
using P23.MetaTrader4.Manager.Contracts;
using P23.MetaTrader4.Manager.Contracts.Configuration;

namespace MT4ClassLibrary

{
    public class Class1
    {
        public ClrWrapper clrWrapper;
        public string textBox1_server = "175.41.246.194:443";
        public string textBox2_login = "3030";
        public string textBox3_password = "atom123";
        public Class1()
        {
            ConnectionParameters c=new ConnectionParameters();
            c.Login = Int32.Parse(textBox2_login);
            c.Password = textBox3_password;
            c.Server = textBox1_server;
            clrWrapper = new ClrWrapper(c, @"E:\DS1\Home\learn\MT4\MT4\bin\Debug\mtmanapi.dll");
            clrWrapper.PumpingSwitchEx(PumpingMode.Default);
        }
      
        public void ConnectToServer()
        {
            string connectText = textBox1_server;
            clrWrapper.Connect(connectText);
        }

        public int LogIn()
        {      
            string login = textBox2_login;
            string pass = textBox3_password;
            try
            {
                int log = Int32.Parse(login);
                ConnectToServer();
                int l = clrWrapper.Login(log, pass);
                return l;
            }
            catch (Exception)
            {
                return -1;
            }
        }



    }
}
