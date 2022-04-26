using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using P23.MetaTrader4.Manager;
using P23.MetaTrader4.Manager.Contracts;

namespace MTmanAPI
{
    public static class ConnectionMT4Server 
    {
        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public static string server = "175.41.246.194:443";
        public static string login = "3030";
        public static string password = "atom123";
        public static ConnectionParameters _connectionParameters = new ConnectionParameters
        {
            Login = Int32.Parse(login),
            Password = password,
            Server = server
        };

        // public static ClrWrapper clrWrapper = new ClrWrapper(_connectionParameters, @"E:\DS1\Home\learn\MT4\MT4\bin\Debug\mtmanapi.dll");

        public static string oldPath=string.Empty;
        
        /*   
          public static void SetPath()
           {
               string searchPath = AssemblyDirectory;
                oldPath = Environment.GetEnvironmentVariable("PATH"); ;
               System.Environment.SetEnvironmentVariable("Path", searchPath + ";" + oldPath);
           }
         */

        public static ClrWrapper clrWrapper = new ClrWrapper(_connectionParameters, Path.Combine(AssemblyDirectory, "mtmanapi.dll"));
        //public static ClrWrapper clrWrapper = new ClrWrapper();
    }
}
