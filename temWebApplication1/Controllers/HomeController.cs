using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using P23.MetaTrader4.Manager;

using MT4ClassLibrary;
using P23.MetaTrader4.Manager.Contracts;
using temWebApplication1.Models;

namespace temWebApplication1.Controllers
{

    public static class ConnectionMT4Server
    {

        public static string server = "175.41.246.194:443";
        public static string login = "3030";
        public static string password = "atom123";
        public static ConnectionParameters _connectionParameters = new ConnectionParameters
        {
            Login = Int32.Parse(login),
            Password = password,
            Server = server
        };
        
       public static ClrWrapper clrWrapper = new ClrWrapper(_connectionParameters, @"E:\DS1\Home\learn\MT4\MT4\bin\Debug\mtmanapi.dll");
    }


    public class HomeController : Controller
    {
        private ClrWrapper clrWrapper = ConnectionMT4Server.clrWrapper;
        private static  ApplicationDbContext db = new ApplicationDbContext();
        private static MT4TradeViewModel objTr = new MT4TradeViewModel();

        private static MT4TradeViewModelUpdate objTrUpdate = new MT4TradeViewModelUpdate();


        public static void TradeDataUpdate(TradeRecord tradeRecord)
        {

            //Tr.activation = tradeRecord.Activation;
            //Tr.apidata = tradeRecord.ApiData;
            objTrUpdate.TradeClosePrice = tradeRecord.ClosePrice;
            objTrUpdate.TradeCloseTime = ToDateTime(tradeRecord.CloseTime);
            //TradeCommand cmd = tradeRecord.Cmd;
            objTrUpdate.TradeComment = tradeRecord.Comment;
            objTrUpdate.TradeCommission = tradeRecord.Commission;
            objTrUpdate.TradeCommissionAgent = tradeRecord.CommissionAgent;
            IList<double> convRates = tradeRecord.ConvRates;

            //open time
            objTrUpdate.TradeConvertionRate1 = convRates[0];
            //close time
            objTrUpdate.TradeConvertionRate2 = convRates[1];

            objTrUpdate.SymbolDigits = tradeRecord.Digits;
            objTrUpdate.TradeExpiretionTime = ToDateTime(tradeRecord.Expiration);
            objTrUpdate.TradeGWClosePrice = tradeRecord.GwClosePrice;
            objTrUpdate.TradeGWOpenPrice = tradeRecord.GwOpenPrice;
            //int gwOrder = tradeRecord.GwOrder;

            objTrUpdate.TradeGWVolume = tradeRecord.GwVolume;
            objTrUpdate.CustomerLoginID = tradeRecord.Login;
            objTrUpdate.TradeMagic = tradeRecord.Magic;
            objTrUpdate.TradeMarginRate = tradeRecord.MarginRate;
            objTrUpdate.TradeOpenPrice = tradeRecord.OpenPrice;
            objTrUpdate.TradeOpenTime = ToDateTime(tradeRecord.OpenTime);
            //int  order = tradeRecord.Order;
            objTrUpdate.TradeProfit = tradeRecord.Profit;
            //TradeReason reason = tradeRecord.Reason;
            objTrUpdate.TradeStopLoss = tradeRecord.Sl;

            //TradeState state = tradeRecord.State;
            //double storage = tradeRecord.Storage;
            objTrUpdate.TradeSymbol = tradeRecord.Symbol;
            objTrUpdate.TradeTaxes = tradeRecord.Taxes;
            objTrUpdate.TradeTimeStamp = ToDateTime(tradeRecord.Timestamp);
            objTrUpdate.TradeTakeProfit = tradeRecord.Tp;
            objTrUpdate.TradeVolume = tradeRecord.Volume;

            db.MT4TradeViewModelUpdate.Add(objTrUpdate);
            db.SaveChanges();
        }



        public static void TradeData(TradeRecord tradeRecord)
        {
        
            //Tr.activation = tradeRecord.Activation;
            //Tr.apidata = tradeRecord.ApiData;
            objTr.TradeClosePrice = tradeRecord.ClosePrice;
            objTr.TradeCloseTime = ToDateTime(tradeRecord.CloseTime);
            //TradeCommand cmd = tradeRecord.Cmd;
            objTr.TradeComment = tradeRecord.Comment;
            objTr.TradeCommission = tradeRecord.Commission;
            objTr.TradeCommissionAgent = tradeRecord.CommissionAgent;
            IList<double> convRates = tradeRecord.ConvRates;

            //open time
            objTr.TradeConvertionRate1 = convRates[0];
            //close time
            objTr.TradeConvertionRate2 = convRates[1];

            objTr.SymbolDigits = tradeRecord.Digits;
            objTr.TradeExpiretionTime = ToDateTime(tradeRecord.Expiration);
            objTr.TradeGWClosePrice = tradeRecord.GwClosePrice;
            objTr.TradeGWOpenPrice = tradeRecord.GwOpenPrice;
            //int gwOrder = tradeRecord.GwOrder;

            objTr.TradeGWVolume = tradeRecord.GwVolume;
            objTr.CustomerLoginID = tradeRecord.Login;
            objTr.TradeMagic = tradeRecord.Magic;
            objTr.TradeMarginRate = tradeRecord.MarginRate;
            objTr.TradeOpenPrice = tradeRecord.OpenPrice;
            objTr.TradeOpenTime = ToDateTime(tradeRecord.OpenTime);
            //int  order = tradeRecord.Order;
            objTr.TradeProfit = tradeRecord.Profit;
            //TradeReason reason = tradeRecord.Reason;
            objTr.TradeStopLoss = tradeRecord.Sl;

            //TradeState state = tradeRecord.State;
            //double storage = tradeRecord.Storage;
            objTr.TradeSymbol = tradeRecord.Symbol;
            objTr.TradeTaxes = tradeRecord.Taxes;
            objTr.TradeTimeStamp = ToDateTime(tradeRecord.Timestamp);
            objTr.TradeTakeProfit = tradeRecord.Tp;
            objTr.TradeVolume = tradeRecord.Volume;
            db.MT4TradeViewModel.Add(objTr);
            db.SaveChanges();
        }

        private void AdvancedPumpingSwitch()
        {


            var are = new AutoResetEvent(false);
            clrWrapper.PumpingSwitchEx(PumpingMode.Default);

            clrWrapper.PumpingStarted += (sender, eventArgs) =>
                {
                    are.Set();
                };
            are.WaitOne();
        }


        /// <summary>
        /// Switch flag to check
        /// pumping switch
        /// </summary>
        public bool switchFlag = false;

        /// <summary>
        /// log in flag
        /// </summary>
        public bool isLoggedIn = false;
        /// <summary>
        /// Display message in case of no connection or logged out.
        /// </summary>
        public const string CONNECT_FIRST = "Please, connect first.";

        /// <summary>
        /// All dates in MT4 represented as Unix timestamp 
        /// with slight modification: it defined as the number
        /// of seconds that have elapsed since 00:00:00 Trade
        /// Server Time Zone, Thursday, 1 January 1970.
        /// To convert it to .NET DateTime use following extension
        /// </summary>
        /// <param name="_time">DateTime</param>
        /// <returns>Return as DateTime of uint time
        /// </returns>
        public static DateTime  ToDateTime(uint _time)
        {
            return new DateTime(1970, 1, 1).AddSeconds(_time);
        }

        /// <summary>
        /// All dates in MT4 represented as Unix timestamp 
        /// with slight modification: it defined as the number
        /// of seconds that have elapsed since 00:00:00 Trade
        /// Server Time Zone, Thursday, 1 January 1970.
        /// To convert .NET DateTime to Unix time use following extension
        /// </summary>
        /// <param name="_time">DateTime</param>
        /// <returns>
        /// Return as uint time of DateTime
        /// </returns>

        public uint ToUnixTime(DateTime _time)
        {
            return (uint)_time.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }
        /// <summary>
        /// Timer for Market watch
        /// </summary>
        public bool MarketWatchTimeEnable = false;

        /// <summary>
        /// Connect To the Server
        /// </summary>
        public void ConnectToServer()
        {
            string connectText = ConnectionMT4Server.server;
            clrWrapper.Connect(connectText);
        }


        public ActionResult Index()
        {

            ConnectToServer();
            //login
            switchFlag = false;
            string login = ConnectionMT4Server.login;
            string pass = ConnectionMT4Server.password;

            try
            {

                ConnectToServer();
                try
                {
                    int log = Int32.Parse(login);
                    int l = clrWrapper.Login(log, pass);
                    isLoggedIn = true;
                 

                }
                catch (Exception )
                {
                
                }
            }
            catch (Exception )
            {
                //server

               
            }

         //order
            if (clrWrapper.IsConnected() != 0)
            {
             
                IList<TradeRecord> tradeRecords = clrWrapper.TradesRequest();
                foreach (var p in tradeRecords)
                {
                    TradeData(p);
                }


         
                
               // dataGridView1_Orders.Visible = true;
               // dataGridView1_Orders.DataSource = list;

                AdvancedPumpingSwitch();

                clrWrapper.TradeUpdated += (sender2, record) =>
                                {
                                    Response.Write(record);
                                    Response.Write("record");
                                    TradeDataUpdate(record);
                                    Trace.WriteLine("Updated record");
                                    // MessageBox.Show("TradeUpdated: " + record);

                                };
                clrWrapper.TradeAdded += (sender2, record) =>
                                {
                                    TradeDataUpdate(record);
                                    Response.Write("record");
                                    Response.Write(record);
                                    //MessageBox.Show("TradeAdded: " + record);

                                };
                clrWrapper.TradeDeleted += (sender2, record) =>
                                {
                                    TradeDataUpdate(record);
                                    Response.Write("record");
                                    Response.Write(record);
                                    //MessageBox.Show("TradeDeleted: " + record);

                                };
                clrWrapper.TradeClosed += (sender2, record) =>
                                {
                                    TradeDataUpdate(record);
                                    Response.Write("record");
                                    Response.Write(record);
                                    //MessageBox.Show("TradeClosed: " + record);
                                };

            }
            else
            {
             /*   MessageBox.Show(CONNECT_FIRST);
                dataGridView1_Orders.DataSource = null;*/
            }
            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }


}