using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using P23.MetaTrader4.Manager;
using P23.MetaTrader4.Manager.Contracts;
using P23.MetaTrader4.Manager.Contracts.Configuration;
using System.IO;
using System.Reflection;

namespace MTmanAPI
{

    /// <summary>
    /// Main Class
    /// </summary>
    public partial class Form1 : Form
    {
         AutoResetEvent are12 = new AutoResetEvent(false);
        //  
        public ClrWrapper clrWrapper = ConnectionMT4Server.clrWrapper;
        private void AdvancedPumpingSwitch()
        {
            ConnectionMT4Server.clrWrapper.PumpingSwitchEx(PumpingMode.Default);
            
            ConnectionMT4Server.clrWrapper.PumpingStarted += (sender, eventArgs) =>
                {
                    are12.Set();
                };
            are12.WaitOne();
        }

       
      

        /// <summary>
        /// Initially define the number
        /// of tab pages size
        /// </summary>
        public TabPage[] newPage = new TabPage[100];
        /// <summary>
        /// Define the tab pages name
        /// and in the runtime create the tab control
        /// tab named here
        /// </summary>
        public string[] tabPageStrings={ "Main", "Market Watch", "Symbols", "Users", "Online", "Orders", "Summary", "Exposure", "Margins", "Requests", "Plugins", /*"Dealer",*/ "Mailbox", "News", "Reports", "Daily Reports", "Journal", "Logs","Others" };
        /// <summary>
        /// Define the Group Box name
        /// and in the runtime show the appropriate 
        /// Group box and hide the others
        /// </summary>
        public string[] groupBoxStrings = { "Main", "MarketWatch", "Symbols", "Users", "Online", "Orders", "Summary", "Exposure", "Margins", "Requests", "Plugins", "Dealer", "Mailbox", "News", "Reports", "DailyReports", "Journal", "Logs", "Others" };
        
        
        /// <summary>
        ///  Create an object of ConnectionMT4Server.clrWrapper
        ///  Wrapper around mtmanapi.dll to 
        /// provide managed access to MT4 manager API
        /// </summary>
    

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
        public DateTime ToDateTime( uint _time)
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
        public bool MarketWatchTimeEnable=false;

        /// <summary>
        /// Constructor 
        /// initialize the component
        /// </summary>
        public Form1()
        {
           
            InitializeComponent();
            int i = 0;

            foreach(var pages in tabPageStrings)
            {

                newPage[i] = new TabPage(pages);
                tabControl1.TabPages.Add(newPage[i]);
                newPage[i].AutoScroll = true;
                i++;
            }
            ShowCurrentPanel("Main");
            tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;

           
        }




        public string server = "175.41.246.194:443";
        public string login = "3030";
        public string password = "atom123";
        public ConnectionParameters _connectionParameters;
  
        private void timer2CheckConnection_Tick(object sender, EventArgs e)
        {
           //CheckConnection();
         //  AutoLogin();
        }



        private void AutoLogin()
        {

            try
            {
               int n = ConnectionMT4Server.clrWrapper.IsConnected();
                // MessageBox.Show("check auto login. n: " + n);
                //    1-connect
                //    0-disconnect
                if (n.Equals(0))
                {
                        timer2CheckConnection.Stop();
                        ConnectToServer();
                        int isc = ConnectionMT4Server.clrWrapper.IsConnected();
                    if (isc.Equals(1))
                    {
                        LogIn();
                        //delete handler
                        DeleteHandler();
                        //connect

                        //1- connect
                        //0-disconnect
                        //call handler

                        MessageBox.Show("lastServerActiveTime is: .... Handler call in AutoLogin");
                        CallHandler();
                        timer2CheckConnection.Start();
                        // MessageBox.Show(" Relogin successfully");
                    }
                    else
                    {
                        timer2CheckConnection.Start();
                    }
                }
                //LastActiveCheck();
            }
            catch (Exception)
            {
                timer2CheckConnection.Start();
            }
        }

        /// <summary>
        /// Initialize in form load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {

          /* ConnectionMT4Server.SetPath();
           MessageBox.Show(ConnectionMT4Server.oldPath);*/

            //CheckConnection
            timer2CheckConnection.Enabled = true;
            timer2CheckConnection.Interval = 500;

            //others
            textBox1_server.Text = ConnectionMT4Server.server;
            textBox2_login.Text = ConnectionMT4Server.login;
            textBox3_password.Text = ConnectionMT4Server.password;
            FromDateTimePicker_DailyReports.Value=DateTime.Today.AddDays(-1); //Daily reports
            dateTimePickerFrom.Value= DateTime.Today.AddDays(-1000); //Orders
            dateTimePickerTo.Value = DateTime.Today.AddDays(1);
            dateTimePicker_From.Value = DateTime.Today.AddDays(-1); //Journal date 
            switchFlag = false;
            List<JournalFilter> journalFilterList = new List<JournalFilter>();
            journalFilterList.Add(new JournalFilter { FilterName = "Standard" });
            journalFilterList.Add(new JournalFilter { FilterName = "Logins" });
            journalFilterList.Add(new JournalFilter { FilterName = "Trades" });
            journalFilterList.Add(new JournalFilter { FilterName = "Errors" });
            journalFilterList.Add(new JournalFilter { FilterName = "Full" });
           
            comboBox1_Journal.DataSource = journalFilterList;
            comboBox1_Journal.DisplayMember = "FilterName";
            comboBox1_Journal.ValueMember = "FilterName";

            //Active check 
            //LastActiveCheck();

            //thread
            Thread thread1 = new Thread(new ThreadStart(CallHandler));
            //thread1.Start();
            // MessageBox.Show(" thread1.Start();");
          

        }

        private void MetaTraderWrapperOnPumpingStopped(object sender, EventArgs eventArgs)
        {
            MessageBox.Show("MetaTraderWrapperOnPumpingStopped");
            AdvancedPumpingSwitch();
//            ConnectionMT4Server.clrWrapper.PumpingSwitchEx(PumpingMode.Default);
        }

        /// <summary>
        /// stores Journal Filter name
        /// </summary>
        class JournalFilter
        {
            /// <summary>
            ///  Store for the name property
            /// </summary>
            public string FilterName { get; set; }
        }

        /// <summary>
        /// Event Handler for tab control's selected tab index changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_SelectedIndexChanged(Object sender, EventArgs e)
        {       
            MarketWatchTimeEnable = false;
           
            string tabNow = tabControl1.SelectedTab.Text;
            ShowCurrentPanel(tabNow);
        }

        /// <summary>
        /// Show the selected tab GroupBox
        /// Hide other Group Box
        /// <param name="_tabNow">Text</param>
        /// </summary>
        private void ShowCurrentPanel(string _tabNow)
        {
            _tabNow = _tabNow.Replace(" ", String.Empty);
            foreach (var pages in groupBoxStrings)
            {
                try
                {
                    (Controls[pages] as GroupBox).Visible = false;
                }
                catch (Exception)
                {
                    
                }  
            }
            
            try
            {
                (Controls[_tabNow] as GroupBox).Visible = true;
                (Controls[_tabNow] as GroupBox).Location = new System.Drawing.Point(12, 50);
            }
            catch (Exception )
            {

            }
        }



        /// <summary>
        /// Connect To the Server
        /// </summary>
        public void ConnectToServer()
        {
            string connectText = textBox1_server.Text;
            ConnectionMT4Server.clrWrapper.Connect(connectText);
        }



        /// <summary>
        /// Exit the Application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OkButton_MainClick(object sender, EventArgs e)
        {
            Application.Exit();
        }


        /// <summary>
        /// Exit the Application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       private void CancelButton_MainClick(object sender, EventArgs e)
        {
            Application.Exit();
        }


        /// <summary>
        /// Connect To the server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectButton_MainClick(object sender, EventArgs e)
        {
            try
            {
                ConnectToServer();
                int n = ConnectionMT4Server.clrWrapper.IsConnected();
                //1- connect
                //0-disconnect
                MessageBox.Show(n+": "+ConnectionMT4Server.clrWrapper.ErrorDescription(n));
            }
            catch (Exception es)
            {
                MessageBox.Show(es.StackTrace);
            }
        }

    
        /// <summary>
        /// Login to the server method
        /// </summary>
        /// <returns>return -1 if can't login to the server </returns>
        private int LogIn()
        {
            switchFlag = false;
            string login = textBox2_login.Text;
            string pass = textBox3_password.Text;
            try
            {                
                ConnectToServer();
                try
                {
                    int log = Int32.Parse(login);
                    int l = ConnectionMT4Server.clrWrapper.Login(log, pass);
                    isLoggedIn = true;
                    return l;

                }
                catch (Exception)
                {
                    return 2;
                }
            }
            catch (Exception)
            {
                //server  
                return -1;
            }
        }


        /// <summary>
        /// Login to the server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginButton_MainClick(object sender, EventArgs e)
        {

            int isSuccess=LogIn();

            if (isSuccess == 2)
            {
                string msg = "Can't parse the login/username";
                MessageBox.Show(msg);
            }

            if (isSuccess ==-1)
            {
                string msg = "Server isn't connected";
                MessageBox.Show(msg);
            }

            else
            {
                MessageBox.Show(ConnectionMT4Server.clrWrapper.ErrorDescription(isSuccess));
            }

        }



        /// <summary>
        /// Request a list of available groups of accounts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void GroupsButton_MainClick(object sender, EventArgs e)
        {


            if (ConnectionMT4Server.clrWrapper.IsConnected() != 0)
            {
                LogIn();
                IList<Group> users = ConnectionMT4Server.clrWrapper.GroupsRequest();
                IList<GroupListFxtf> list = new List<GroupListFxtf>();
                
                foreach (var p in users)
                {
                    string group = p.Name;
                    string company = p.Company;
                    int margincall = p.MarginCall;
                    int marginstopOut = p.MarginStopout;
                    list.Add(new GroupListFxtf(group, company, margincall, marginstopOut));
                }

                dataGridView_groupsMain.Visible = true;
                dataGridView_groupsMain.DataSource = list;
            }
            else
            {
                MessageBox.Show(CONNECT_FIRST);
                dataGridView_groupsMain.DataSource = null;
            }

        }

        /// <summary>
        /// Available groups of accounts
        /// Object that represents group configuration
        /// </summary>
        public class GroupListFxtf
        {
            /// <summary>
            /// Group name
            /// 
            /// </summary>
            [DisplayName("Group")]
            public  string Group { get; set; }

            /// <summary>
            /// Company name
            /// 
            /// </summary>
            [DisplayName("Company")]
            public  string Company { get; set; }
            /// <summary>
            /// Margin call level (percent's)
            /// 
            /// </summary>
            [DisplayName("Margin Call")]
            public  int Margincall { get; set; }
            /// <summary>
            /// Stop out level
            /// 
            /// </summary>
            [DisplayName("Margins TopOut")]
            public  int MarginstopOut { get; set; }

            /// <summary>
            /// Groups of accounts
            /// </summary>
            /// <param name="_group">Text</param>
            /// <param name="_company">Text</param>
            /// <param name="_margincall">Number</param>
            /// <param name="_marginstopOut">Number</param>
            public GroupListFxtf(string _group, string _company, int _margincall, int _marginstopOut)
            {
                Group = _group;
                Company = _company;
                Margincall = _margincall;
                MarginstopOut = _marginstopOut;
            }
        }

        /// <summary>
        /// Mail Send 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendMailButton_MainClick(object sender, EventArgs e)
        {
            if (ConnectionMT4Server.clrWrapper.IsConnected() != 0)
            {
                MailSend mail= new MailSend(ConnectionMT4Server.clrWrapper);
                mail.Show();
            }
            else
            {
                MessageBox.Show(CONNECT_FIRST);
            }
          
        }

      


        /// <summary>
        /// Disconnect from the server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisconnectButton_MainClick(object sender, EventArgs e)
        {
            if (ConnectionMT4Server.clrWrapper.IsConnected() != 0)
            {
                ConnectionMT4Server.clrWrapper.Disconnect();
                isLoggedIn = false;
                string msg = "Disconnect from the server.";
                MessageBox.Show(msg);
            }
            else
            {
                string msg = "Not connected to the server.";
                MessageBox.Show(msg);
            }
        }

        /// <summary>
        ///  SymbolsRefresh() Download symbols configuration from trade server
        /// 
        /// SymbolsGetAll() Get symbols configuration. 
        /// SymbolsRefresh() should be called before this method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshButton_SymbolsClick(object sender, EventArgs e)
        {
            if (ConnectionMT4Server.clrWrapper.IsConnected() != 0)
            {
                ConnectionMT4Server.clrWrapper.SymbolsRefresh();
//                IList<Symbol> symbols = ConnectionMT4Server.clrWrapper.SymbolsGetAll();
                IList<Symbol> symbols = ConnectionMT4Server.clrWrapper.CfgRequestSymbol();


                IList<SymbolListFxtf> list = new List<SymbolListFxtf>();

                foreach (var p in symbols)
                {
                    string symbol = p.Name;
                    string description = p.Description;
                    int stopsLevel = p.StopsLevel;
                    int spread = p.Spread;
                    string currency = p.Currency;
                    DateTime expiration = ToDateTime(p.Expiration);
                    DateTime starting = ToDateTime(p.Starting);

                    double bidTickValue = p.BidTickValue;
                    double askTickValue = p.AskTickValue;
                    list.Add(new SymbolListFxtf(symbol, description, stopsLevel, spread, bidTickValue, askTickValue,currency,starting,expiration));

                }
                dataGridView_Symbol.Visible = true;
                dataGridView_Symbol.DataSource = list;
                LogIn();
            }
            else
            {
                MessageBox.Show(CONNECT_FIRST);
                dataGridView_groupsMain.DataSource = null;
            }
        }

        /// <summary>
        /// Object that represents symbol configuration
        /// 
        /// </summary>
        public class SymbolListFxtf
        {
            /// <summary>
            /// Name
            /// 
            /// </summary>
            [DisplayName("Symbol")]
            public string Symbol { get; set; }

            /// <summary>
            /// Description
            /// 
            /// </summary>
            [DisplayName("Description")]
            public string Description { get; set; }
            /// <summary>
            /// Stops Level
            /// 
            /// </summary>
            [DisplayName("Stops Level")]
            public int StopsLevel { get; set; }
            /// <summary>
            /// Spread
            /// 
            /// </summary>
            [DisplayName("Spread")]
            public int Spread { get; set; }


            /// <summary>
            /// Tick value for bid
            /// 
            /// </summary>
            public double BidTickValue { get; set; }
            /// <summary>
            /// Tick value for ask
            /// 
            /// </summary>
            public double AskTickValue { get; set; }
            // <summary>
            /// Currency
            /// 
            /// </summary>
            public string Currency { get; set; }
            /// <summary>
            /// Trades starting date (UNIX time)
            /// 
            /// </summary>
            public DateTime Starting { get; set; }
            /// <summary>
            /// Trades end date      (UNIX time)
            /// 
            /// </summary>
            public DateTime Expiration { get; set; }
            /// <summary>
            /// Symbol configuration
            /// </summary>
            /// <param name="_symbol">Text</param>
            /// <param name="_description">Text</param>
            /// <param name="_stopsLevel">Number</param>
            /// <param name="_spread">Number</param>
            /// <param name="_bidTickValue">Number</param>
            /// <param name="_askTickValue">Number</param>
            public SymbolListFxtf(string _symbol, string _description, int _stopsLevel, int _spread, double _bidTickValue, double _askTickValue, string _currency, DateTime _starting, DateTime _expiration)
            {

                Symbol = _symbol;
                Description = _description;
                StopsLevel = _stopsLevel;
                Spread = _spread;
                BidTickValue = _bidTickValue;
                AskTickValue = _askTickValue;
                Currency = _currency;
                Starting = _starting;
                Expiration = _expiration;
            }
        }

        /// <summary>
        /// Get all users in pumping mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       
        private void RequestButton_UsersClick(object sender, EventArgs e)
        {
            //user
           
            if (ConnectionMT4Server.clrWrapper.IsConnected() != 0)
            {
                LogIn();
                //User Request
                PumpingSwitch();
                IList<UserRecord> tradeRecords = ConnectionMT4Server.clrWrapper.UsersGet();
                IList<UserListFxtf> list = new List<UserListFxtf>();

                foreach (var p in tradeRecords)
                {
                    int login = p.Login;
                    string name = p.Name;
                    string group = p.Group;
                    int leverage = p.Leverage;
                    double balance = p.Balance;
                    list.Add(new UserListFxtf(login, name, group, leverage, balance));                  
                }
                dataGridView2_Users.Visible = true;
                dataGridView2_Users.DataSource = list;
            }
            else
            {
                MessageBox.Show(CONNECT_FIRST);
                dataGridView2_Users.DataSource = null;
            }
        }

        /// <summary>
        /// Object that represents user record
        /// 
        /// </summary>
        public class UserListFxtf
        {
            /// <summary>
            /// Account number
            /// 
            /// </summary>
            [DisplayName("Login")]
            public int Login { get; set; }

            /// <summary>
            /// Name
            /// 
            /// </summary>
            [DisplayName("Name")]
            public string Name { get; set; }
            /// <summary>
            /// Group user belongs to
            /// 
            /// </summary>
            [DisplayName("Group")]
            public string Group { get; set; }
            /// <summary>
            /// Leverage
            /// 
            /// </summary>
            [DisplayName("Leverage")]
            public int Leverage { get; set; }
            /// <summary>
            /// Balance
            /// 
            /// </summary>
            [DisplayName("Balance")]
            public double Balance { get; set; }


            /// <summary>
            /// Symbol configuration
            /// </summary>
            /// <param name="_login">Text</param>													
            /// <param name="_name">Text</param>	
            /// <param name="_group">Text</param>		
            /// <param name="_leverage">number</param>	   
            /// <param name="_balance">number</param>													
            public UserListFxtf(int _login, string _name, string _group, int _leverage, double _balance)
            {
                Login = _login;
                Name = _name;
                Group = _group;
                Leverage = _leverage;
                Balance = _balance;
            }
        }

        /// <summary>
        /// Switch into pumping mode
        /// delegate will be invoked on any pumping event
        /// </summary>
        private void PumpingSwitch()
        {
            if (switchFlag == false)
            {
                var are = new AutoResetEvent(false);
                ConnectionMT4Server.clrWrapper.PumpingSwitch(i =>
                    {
                        if (i == 0) // 0 - means pumping started
                        are.Set();
                    });

                are.WaitOne();
                switchFlag = true;
            }
        }



        /// <summary>
        /// Get online users in pumping mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
        private void RefreshButton_OnlineClick(object sender, EventArgs e)
        {
            if (ConnectionMT4Server.clrWrapper.IsConnected() != 0)
            {
                LogIn();
                PumpingSwitch();

                IList<OnlineRecord> onlineRecords = ConnectionMT4Server.clrWrapper.OnlineGet();
                IList<OnlineRecordsListFxtf> list = new List<OnlineRecordsListFxtf>();

             /*
              *Another way to doing it.  
              IList<OnlineRecord> list2 = new List<OnlineRecord>();
                foreach (var p in onlineRecords)
                {
                 list2.Add(new OnlineRecord
                    {
                        Login = p.Login,
                        Group = p.Group,
                        Counter = p.Counter,

                    });
                }
            */
                foreach (var p in onlineRecords)
                {
                    int login = p.Login;
                    string group = p.Group;
                    int counter = p.Counter;

                    list.Add(new OnlineRecordsListFxtf(login, group, counter));
                }
               
                dataGridViewOnlineUser.Visible = true;
                dataGridViewOnlineUser.DataSource = list;
            }
            else
            {
                dataGridViewOnlineUser.DataSource = null;
                MessageBox.Show(CONNECT_FIRST);
            }
        }


        /// <summary>
        /// Object that represents online record
        /// 
        /// </summary>
        public class OnlineRecordsListFxtf
        {    /// <summary>
             /// User login
             /// 
             /// </summary>
            [DisplayName("Login")]
            public int Login { get; set; }
            /// <summary>
            /// User group
            /// 
            /// </summary>
            [DisplayName("Group")]
            public string Group { get; set; }
            /// <summary>
            /// Connections counter
            /// 
            /// </summary>
            [DisplayName("counter")]
            public int Counter { get; set; }

            /// <summary>
            /// Symbol configuration
            /// </summary>
            /// <param name="_login">Number</param>
            /// <param name="_group">Text</param>
            /// <param name="_counter">Number</param>
            public OnlineRecordsListFxtf(int _login, string _group, int _counter)
            {
                Login = _login;
                Group = _group;
                Counter = _counter;
            }
        }

        /// <summary>
        /// Request list of all orders
        /// 
        /// </summary>
        private void RequestButton_OrdersTabClick(object sender, EventArgs e)
        {
            if (ConnectionMT4Server.clrWrapper.IsConnected() != 0)
            {
               
               IList<TradeRecord> tradeRecords = ConnectionMT4Server.clrWrapper.TradesRequest();
               IList<TradeRecordsListFxtf> list = new List<TradeRecordsListFxtf>();
                
               foreach (var p in tradeRecords)
                {
                    int login = p.Login;
                    int order = p.Order;
                    string symbol = p.Symbol;
                    double profit = p.Profit;
                    DateTime openTime = ToDateTime(p.OpenTime);
                    DateTime clsoeTime = ToDateTime(p.CloseTime);
                    DateTime timeStamp = ToDateTime(p.Timestamp);

                    list.Add(new TradeRecordsListFxtf(login, order, symbol, profit, openTime, clsoeTime, timeStamp));
                 }
               list = list.OrderBy(o => o.OpenTime).ThenBy(o => o.Login).ToList();
               dataGridView1_Orders.Visible = true;
               dataGridView1_Orders.DataSource = list;    
            }
            else
            {
                MessageBox.Show(CONNECT_FIRST);
                dataGridView1_Orders.DataSource = null;
            }
        }


        public void helloAdded(ClrWrapper sender, TradeRecord record)
        {
            MessageBox.Show("TradeAdded: " + record.Order);
        }
        public void helloupdated(ClrWrapper sender, TradeRecord record)
        {
            MessageBox.Show("TradeUpdated: " + record.Order);
        }
        public void helloDeleted(ClrWrapper sender, TradeRecord record)
        {
            MessageBox.Show("TradeDeleted: " + record.Order);
        }
        public void helloClosed(ClrWrapper sender, TradeRecord record)
        {
            MessageBox.Show("TradeClosed: " + record.Order);
        }

      //  protected EventHandler ServerTimeCheckEvent;

        public void DeleteHandler()
        {
            ConnectionMT4Server.clrWrapper.TradeAdded -= helloAdded;
            ConnectionMT4Server.clrWrapper.TradeUpdated -= helloupdated;
            ConnectionMT4Server.clrWrapper.TradeDeleted -= helloDeleted;
            ConnectionMT4Server.clrWrapper.TradeClosed -= helloClosed;
           // MessageBox.Show("Finiish In DeleteHandler.");
        }


        public void CallHandler()
        {
            AdvancedPumpingSwitch();
            ConnectionMT4Server.clrWrapper.TradeAdded += helloAdded;
            ConnectionMT4Server.clrWrapper.TradeUpdated += helloupdated;
            ConnectionMT4Server.clrWrapper.TradeDeleted += helloDeleted;
            ConnectionMT4Server.clrWrapper.TradeClosed += helloClosed;
            MessageBox.Show("Finiish In CallHandler: ");
            
        }
        

        /// <summary>
        /// Object that represents trade record
        /// 
        /// </summary>
        public class TradeRecordsListFxtf
        {
           

            /// <summary>
            /// Order ticket
            /// 
            /// </summary>
            public int Order { get; set; }
            /// <summary>
            /// Owner's login
            /// 
            /// </summary>
            public int Login { get; set; }
            /// <summary>
            /// Security
            /// 
            /// </summary>
            public string Symbol { get; set; }
            /// <summary>
            /// Security precision
            /// 
            /// </summary>
            public int Digits { get; set; }
            /// <summary>
            /// Trade command
            /// 
            /// </summary>
            public TradeCommand Cmd { get; set; }
            /// <summary>
            /// Volume
            /// 
            /// </summary>
            public int Volume { get; set; }
            /// <summary>
            /// Open time
            /// 
            /// </summary>
            public DateTime OpenTime { get; set; }
            /// <summary>
            /// Reserved
            /// 
            /// </summary>
            public TradeState State { get; set; }
            /// <summary>
            /// Open price
            /// 
            /// </summary>
            public double OpenPrice { get; set; }
            /// <summary>
            /// Stop loss
            /// 
            /// </summary>
            public double Sl { get; set; }
            /// <summary>
            /// Take profit
            /// 
            /// </summary>
            public double Tp { get; set; }
            /// <summary>
            /// Close time
            /// 
            /// </summary>
            public DateTime CloseTime { get; set; }
            /// <summary>
            /// Gateway order volume
            /// 
            /// </summary>
            public int GwVolume { get; set; }
            /// <summary>
            /// Pending order's expiration time
            /// 
            /// </summary>
            public DateTime Expiration { get; set; }
            /// <summary>
            /// Trade reason
            /// 
            /// </summary>
            public TradeReason Reason { get; set; }
            /// <summary>
            /// Convertation rates from profit currency to group deposit currency
            ///             (first element-for open time, second element-for close time)
            /// 
            /// </summary>
            public IList<double> ConvRates { get; set; }
            /// <summary>
            /// Commission
            /// 
            /// </summary>
            public double Commission { get; set; }
            /// <summary>
            /// Agent commission
            /// 
            /// </summary>
            public double CommissionAgent { get; set; }
            /// <summary>
            /// Order swaps
            /// 
            /// </summary>
            public double Storage { get; set; }
            /// <summary>
            /// Close price
            /// 
            /// </summary>
            public double ClosePrice { get; set; }
            /// <summary>
            /// Profit
            /// 
            /// </summary>
            public double Profit { get; set; }
            /// <summary>
            /// Taxes
            /// 
            /// </summary>
            public double Taxes { get; set; }
            /// <summary>
            /// Special value used by client experts
            /// 
            /// </summary>
            public int Magic { get; set; }
            /// <summary>
            /// Comment
            /// 
            /// </summary>
            public string Comment { get; set; }
            /// <summary>
            /// Gateway order ticket
            /// 
            /// </summary>
            public int GwOrder { get; set; }
            /// <summary>
            /// used by MT Manager
            /// 
            /// </summary>
            public ActivationType Activation { get; set; }
            /// <summary>
            /// Gateway order price deviation (pips) from order open price
            /// 
            /// </summary>
            public short GwOpenPrice { get; set; }
            /// <summary>
            /// Gateway order price deviation (pips) from order close price
            /// 
            /// </summary>
            public short GwClosePrice { get; set; }
            /// <summary>
            /// Margin convertation rate (rate of convertation from margin currency to deposit one)
            /// 
            /// </summary>
            public double MarginRate { get; set; }
            /// <summary>
            /// Timestamp when traderecord was requested
            /// 
            /// </summary>
            public DateTime Timestamp { get; set; }
            /// <summary>
            /// This field stores user data of Manager API
            /// 
            /// </summary>
            public IList<int> ApiData { get; set; }



            /// <summary>
            /// Trade record 
            /// </summary>
            /// <param name="_login">Number</param>													
            /// <param name="_symbol">Text</param>		
            /// <param name="_profit">Number</param>	   									

            /// <param name="_openTime">DateTime</param>
            /// <param name="_clsoeTime">DateTime</param>
            /// <param name="_timeStamp">DateTime</param>


            public TradeRecordsListFxtf(int _login,int _order, string _symbol, double _profit, DateTime _openTime, DateTime _clsoeTime, DateTime _timeStamp)
            {
                Order = _order;
                Login = _login;
                Symbol = _symbol;
                Profit = _profit;
                OpenTime = _openTime;
                CloseTime = _clsoeTime;
                Timestamp = _timeStamp;
            }
            public TradeRecordsListFxtf(int _order, string _symbol, double _openPrice, double _closePrice, double _profit, DateTime _openTime, DateTime _clsoeTime, DateTime _timeStamp)
            {
                Order = _order;
                Symbol = _symbol;
                OpenPrice = _openPrice;
                ClosePrice = _closePrice;
                Profit = _profit;
                OpenTime = _openTime;
                CloseTime = _clsoeTime;
                Timestamp = _timeStamp;
            }


        }

        private void TradeGet()
        {
            IList<TradeRecord> tradeRecords = ConnectionMT4Server.clrWrapper.TradesRequest();
            IList<TradeRecordsListFxtf> list = new List<TradeRecordsListFxtf>();
            int loginvalue = int.Parse(LoginTextBox_Orders.Text);

            foreach (var p in tradeRecords)
            {
                int login = p.Login;
                int order = p.Order;
                string symbol = p.Symbol;
                double profit = p.Profit;
                DateTime openTime = ToDateTime(p.OpenTime);
                DateTime clsoeTime = ToDateTime(p.CloseTime);
                DateTime timeStamp = ToDateTime(p.Timestamp);

                if (login.Equals(loginvalue))
                {
                    list.Add(new TradeRecordsListFxtf(login, order, symbol, profit, openTime, clsoeTime, timeStamp));
                }
            }

            list = list.OrderBy(o => o.OpenTime).ToList();
            dataGridView1_Orders.Visible = true;
            dataGridView1_Orders.DataSource = list;
        }


        private void GetButton_OrdersTabClick(object sender, EventArgs e)
        {
            if (ConnectionMT4Server.clrWrapper.IsConnected() != 0)
            {
                LogIn();
                TradeGet();
                

            }
            else
            {
                MessageBox.Show(CONNECT_FIRST);
                dataGridView1_Orders.DataSource = null;
            }
        }




        public void TradeHistory()
        {
            DateTime dt1 = dateTimePickerFrom.Value.Date;
            uint from = ToUnixTime(dt1);

            DateTime dt2 = dateTimePickerTo.Value.Date;
            uint to = ToUnixTime(dt2);

            if ((int)to - (int)from >= 0)
            {
                try
                {
                    int login = int.Parse(LoginTextBox_Orders.Text);
                    IList<TradeRecord> tradeRecords = ConnectionMT4Server.clrWrapper.TradesUserHistory(login, from, to);
                    IList<TradeRecordsListFxtf> list = new List<TradeRecordsListFxtf>();
                    foreach (var p in tradeRecords)
                    {
                        int order = p.Order;
                        string symbol = p.Symbol;
                        double openPrice = p.OpenPrice;
                        double closePrice = p.ClosePrice;
                        double profit = p.Profit;
                        DateTime openTime = ToDateTime(p.OpenTime);
                        DateTime clsoeTime = ToDateTime(p.CloseTime);
                        DateTime timeStamp = ToDateTime(p.Timestamp);

                        list.Add(new TradeRecordsListFxtf(order, symbol, openPrice, closePrice, profit, openTime, clsoeTime, timeStamp));
                    }
                    list = list.OrderBy(o => o.Order).ThenBy(o => o.OpenTime).ToList();
                    dataGridView1_Orders.Visible = true;
                    dataGridView1_Orders.DataSource = list;
                }
                catch (Exception)
                {
                    const string msg = "Can't parse the ID field.";
                    MessageBox.Show(msg);
                }


            }
            else
            {
                const string msg = "'To date' must be greater or equal to 'from date'.";
                MessageBox.Show(msg);
            }
        }

        /// <summary>
        /// Request trades for specific user in period
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserHistoryButton_OrdersClick(object sender, EventArgs e)
        {
            if (ConnectionMT4Server.clrWrapper.IsConnected() != 0)
            {
                LogIn();
                TradeHistory();
                

            }
            else
            {
                MessageBox.Show(CONNECT_FIRST);
                dataGridView1_Orders.DataSource = null;
            }
        }
        


        /// <summary>
        ///  Request for the server log for a certain period of time
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RequestButton_JournalClick(object sender, EventArgs e)

        {
            if (ConnectionMT4Server.clrWrapper.IsConnected() != 0)
            {
                LogIn();
                string filter = comboBox1_Journal.SelectedText;


                DateTime dt1 = dateTimePicker_From.Value.Date;
                uint from = ToUnixTime(dt1);
                
                DateTime dt2 = dateTimePicker_To.Value.Date;
                uint to = ToUnixTime(dt2);

                if ((int) to - (int) from >= 0)
                {

                    IList<ServerLog> serverLogs = ConnectionMT4Server.clrWrapper.JournalRequest(0, from, to, filter);
                    IList<JournalRequestFxtf> list = new List<JournalRequestFxtf>();

                    foreach (var p in serverLogs)
                    {
                        string time = p.Time;
                        string ip = p.Ip;
                        string message = p.Message;

                        list.Add(new JournalRequestFxtf(time, ip, message));
                    }
                    dataGridView_JournalRequest.Visible = true;
                    dataGridView_JournalRequest.DataSource = list;
                }
                else
                {
                    string msg = "'To date' must be greater or equal to 'from date'.";
                    MessageBox.Show(msg);
                }

            }
            else
            {
                MessageBox.Show(CONNECT_FIRST);
                dataGridView_JournalRequest.DataSource = null;
            }   
        }



        /// <summary>
        /// Object that represents server log
        /// 
        /// </summary>
        public class JournalRequestFxtf
        {
            /// <summary>
            /// Time
            /// 
            /// </summary>
            [DisplayName("Time")]
            public string Time { get; set; }
            /// <summary>
            /// IP
            /// 
            /// </summary>
            [DisplayName("IP")]
            public string IP { get; set; }
            /// <summary>
            /// Message
            /// 
            /// </summary>
            [DisplayName("Message")]
            public string Message { get; set; }

            /// <summary>
            /// Server log 
            /// </summary>
            /// <param name="_time">Text</param>													
            /// <param name="_ip">Text</param>		
            /// <param name="_message">Text</param>	   									

            public JournalRequestFxtf(string _time, string _ip, string _message)
            {
                Time = _time;
                IP = _ip;
                Message = _message;
            }
        }







        IList<SymbolInfoFxtf> listMarketWatch = new List<SymbolInfoFxtf>();
        /// <summary>
        /// Market Watch 
        /// </summary>
        public void MarketWatchFunc()
        {
            if (MarketWatchTimeEnable && ConnectionMT4Server.clrWrapper.IsConnected() != 0)
            {
                MarketWatchTimeEnable = false;

                IList<SymbolInfo> symbolInfos = ConnectionMT4Server.clrWrapper.SymbolInfoUpdated();


                foreach (var p in symbolInfos)
                {
                    string symbol = p.Symbol;
                    double bid = p.Bid;
                    double ask = p.Ask;
                    string Direction = p.Direction.ToString();

                    //update the price of matched symbol
                    var list = listMarketWatch.Where(d => d.Symbol == symbol).FirstOrDefault();
                    if (list != null)
                    {
                        list.Bid = bid;
                        list.Ask = ask;

                    }
                    else
                    {
                        listMarketWatch.Add(new SymbolInfoFxtf(symbol, bid, ask, Direction));
                        //listMarketWatch.Insert(0, new SymbolInfoFxtf(symbol, bid, ask));
                    }
                }
                dataGridView_MarketWatch.DataSource = listMarketWatch.ToList();
                dataGridView_MarketWatch.Visible = true;
                MarketWatchTimeEnable = true;
            }
        }

        /// <summary>
        /// Get updated prices in pumping mode
        /// 
        /// </summary>
        private void RefreshButton_MarketWatchClick(object sender, EventArgs e)

        {
            if (ConnectionMT4Server.clrWrapper.IsConnected() != 0)
            {
                PumpingSwitch();
                MarketWatchTimeEnable = true;
                MarketWatchFunc();
                timer1.Enabled = true;
                
            }
            else
            {

                MarketWatchTimeEnable = false;
                MessageBox.Show(CONNECT_FIRST);
                dataGridView_MarketWatch.DataSource = null;
            }
        }

        /// <summary>
        /// Timer that refresh ask and bid value in every 3000 milliseconds
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Interval = 1000; //milliseconds
            MarketWatchFunc();
        }





        /// <summary>
        /// Object that represents symbol info
        /// 
        /// </summary>
        public class SymbolInfoFxtf
        {    /// <summary>
             /// Symbol name
             /// 
             /// </summary>
            [DisplayName("Symbol")]
            public string Symbol { get; set; }
            /// <summary>
            /// Bid
            /// 
            /// </summary>
            [DisplayName("Bid")]
            public double Bid { get; set; }
            /// <summary>
            /// Ask
            /// 
            /// </summary>
            [DisplayName("Ask")]
            public double Ask { get; set; }


            public string Direction { get; set; }

            /// <summary>
            /// Symbol info
            /// </summary>
            /// <param name="_symbol">Text</param>													
            /// <param name="_bid">Number</param>		
            /// <param name="_ask">Number</param>	   									

            public SymbolInfoFxtf(string _symbol, double _bid, double _ask, string _direction)
            {
                Symbol = _symbol;
                Bid = _bid;
                Ask = _ask;
                Direction = _direction;
            }
        }


        /// <summary>
        ///  Get trade summary for all symbols in pumping mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RequestButton_SummaryClick(object sender, EventArgs e)
        {
            if (ConnectionMT4Server.clrWrapper.IsConnected() != 0)
            {
                PumpingSwitch();
                IList<SymbolSummary> symbolSummaries = ConnectionMT4Server.clrWrapper.SummaryGetAll();
                IList<SummaryFxtf> list = new List<SummaryFxtf>();

                foreach (var p in symbolSummaries)
                {
                    string symbol = p.Symbol;
                    long buyLots = p.BuyLots;
                    long sellLots = p.SellLots;

                    double buyPrice = p.BuyPrice;
                    double sellPrice = p.SellPrice;
                    double covProfit = p.CovProfit;
                    list.Add(new SummaryFxtf(symbol, buyLots, sellLots, buyPrice, sellPrice, covProfit));
                }
             
                dataGridViewRequest_Summary.DataSource = list;
                dataGridViewRequest_Summary.Visible = true;
            }
            else
            {
                MessageBox.Show(CONNECT_FIRST);
                dataGridViewRequest_Summary.DataSource = null;
            }
        }

        /// <summary>
        /// Object that represents symbol summary
        /// </summary>
        public class SummaryFxtf
        {
            /// <summary>
            /// Symbol
            /// 
            /// </summary>
            public string Symbol { get; set; }

            /// <summary>
            /// Buy volume
            /// 
            /// </summary>
            public long BuyLots { get; set; }
            /// <summary>
            /// Sell volume
            /// 
            /// </summary>
            public long SellLots { get; set; }

            /// <summary>
            /// Average buy price
            /// 
            /// </summary>
            public double BuyPrice { get; set; }
            /// <summary>
            /// Average sell price
            /// 
            /// </summary>
            public double SellPrice { get; set; }

            /// <summary>
            /// Coverage profit
            /// 
            /// </summary>
            public double CovProfit { get; set; }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="_symbol">Text</param>
            /// <param name="_buyLots">Number</param>
            /// <param name="_sellLots">Number</param>
            /// <param name="_buyPrice">Number</param>
            /// <param name="_sellPrice">Number</param>
            /// <param name="_covProfit">Number</param>
            public SummaryFxtf(string _symbol ,long _buyLots, long _sellLots, double _buyPrice, double _sellPrice, double _covProfit )
            {
                Symbol = _symbol;
                BuyLots = _buyLots;
                SellLots = _sellLots;
                BuyPrice = _buyPrice;
                SellPrice = _sellPrice;
                CovProfit = _covProfit;
            }
        }



        /// <summary>
        /// Get company's exposure for currencies in pumping mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RequestButton_ExposureClick(object sender, EventArgs e)
        {
            if (ConnectionMT4Server.clrWrapper.IsConnected() != 0)
            {
                PumpingSwitch();
                IList<ExposureValue> exposureValues= ConnectionMT4Server.clrWrapper.ExposureGet();
                IList<ExposureValueFxtf> list = new List<ExposureValueFxtf>();

                foreach (var p in exposureValues)
                {
                    string currency = p.Currency;
                    double clients = p.Clients;
                    double coverage = p.Coverage;

                    list.Add(new ExposureValueFxtf(currency,clients,coverage));
                }

                dataGridViewExposure.DataSource = list;
                dataGridViewExposure.Visible = true;
            }
            else
            {
                MessageBox.Show(CONNECT_FIRST);
                dataGridViewExposure.DataSource = null;
            }
        }


        public class ExposureValueFxtf
        {
            /// <summary>
            /// Currency
            /// 
            /// </summary>
            public string Currency { get; set; }
            /// <summary>
            /// Clients volume
            /// 
            /// </summary>
            public double Clients { get; set; }
            /// <summary>
            /// Coverage volume
            /// 
            /// </summary>
            public double Coverage { get; set; }



            /// <summary>
            /// 
            /// </summary>
            /// <param name="_currency">Text</param>
            /// <param name="_clients">Number</param>
            /// <param name="_coverage">Number</param>
            public ExposureValueFxtf(string _currency,double _clients,double _coverage)
            {
                Currency = _currency;
                Clients = _clients;
                Coverage = _coverage;
            }
        }

        /// <summary>
        /// Get list of margin requirements of accounts in pumping mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RequestButton_MarginsClick(object sender, EventArgs e)
        {

            if (ConnectionMT4Server.clrWrapper.IsConnected() != 0)
            {
                PumpingSwitch();
                IList<MarginLevel> marginLevels = ConnectionMT4Server.clrWrapper.MarginsGet();
                IList<MarginLevelFxtf> list = new List<MarginLevelFxtf>();

                foreach (var p in marginLevels)
                {
                    int login = p.Login;
                    double margin = p.Margin;
                    double marginFree = p.MarginFree;
                    double level = p.Level;
                    string levelType = p.LevelType.ToString();
                    list.Add(new MarginLevelFxtf(login, margin, marginFree, level, levelType));
                }

                dataGridViewMargins.DataSource = list;
                dataGridViewMargins.Visible = true;
            }
            else
            {
                MessageBox.Show(CONNECT_FIRST);
                dataGridViewMargins.DataSource = null;
            }
        }

        /// <summary>
        /// Object that represents margin level
        /// </summary>
        public class MarginLevelFxtf
        {
            /// <summary>
            /// User login
            /// 
            /// </summary>
            public int Login { get; set; }
            /// <summary>
            /// Margin requirements
            /// 
            /// </summary>
            public double Margin { get; set; }
            /// <summary>
            /// Free margin
            /// 
            /// </summary>
            public double MarginFree { get; set; }
            /// <summary>
            /// Margin level
            /// 
            /// </summary>
            public double Level { get; set; }
            /// <summary>
            /// Level type(ok/margincall/stopout)
            /// 
            /// </summary>
            public string LevelType { get; set; }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="_login">Number</param>
            /// <param name="_margin">Number</param>
            /// <param name="_marginFree">Number</param>
            /// <param name="_level">Number</param>
            /// <param name="_levelType">Text</param>
            public MarginLevelFxtf(int _login,double _margin, double _marginFree, double _level,string _levelType)
            {
                Login = _login;
                Margin = _margin;
                MarginFree = _marginFree;
                Level = _level;
                LevelType = _levelType;
            }
        }


        private void RequestButton_LogsClick(object sender, EventArgs e)
        {

            
        }

        /// <summary>
        ///  Get heading of income news in pumping mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RequestButton_NewsClick(object sender, EventArgs e)
        {
          
            if (ConnectionMT4Server.clrWrapper.IsConnected() != 0)
            {
                PumpingSwitch();
                IList<NewsTopic> newsTopics = ConnectionMT4Server.clrWrapper.NewsGet();
                IList<NewsTopicFxtf> list = new List<NewsTopicFxtf>();

                foreach (var p in newsTopics)
                {
                    uint time = p.Time;
                    DateTime date = ToDateTime(time);
                    string topic = p.Topic;
                    string body = p.Body;
                    string category = p.Category;
                    list.Add(new NewsTopicFxtf(date, body, category, topic));
                }

                dataGridViewNews.DataSource = list;
                dataGridViewNews.Visible = true;
            }
            else
            {
                MessageBox.Show(CONNECT_FIRST);
                dataGridViewNews.DataSource = null;
            }

        }


        /// <summary>
        /// Object that represents news topic
        /// </summary>
        public class NewsTopicFxtf
        {
            /// <summary>
            /// News time
            /// 
            /// </summary>
            public DateTime Time { get; set; }
            /// <summary>
            /// News topic
            /// 
            /// </summary>
            public string Topic { get; set; }
            /// <summary>
            /// Body (if present)
            /// 
            /// </summary>
            public string Body { get; set; }
            /// <summary>
            /// News Category
            /// 
            /// </summary>
            public string Category { get; set; }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="_date">DateTime</param>
            /// <param name="_body">Text</param>
            /// <param name="_category">Text</param>
            /// <param name="_topic">Text</param>
            public NewsTopicFxtf(DateTime _date, string _body, string _category, string _topic)
            {
                Time = _date;
                Body = _body;
                Topic = _topic;
                Category = _category;
            }
        }

        /// <summary>
        /// Get last mails of internal mail system
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RequestButton_MailBoxClick(object sender, EventArgs e)
        {

            if (ConnectionMT4Server.clrWrapper.IsConnected() != 0)
            {
                IList<MailBox> mailBoxs = ConnectionMT4Server.clrWrapper.MailsRequest();
                IList<MailBoxFxtf> list = new List<MailBoxFxtf>();

                foreach (var p in mailBoxs)
                {
                    uint time = p.Time;
                    DateTime date = ToDateTime(time);
                    string from = p.From;
                    string subject = p.Subject;
                    string body = p.Body;
                    int  send= p.Sender;
                    list.Add(new MailBoxFxtf(date, from, subject, body, send));
                }

                dataGridViewMailBox.DataSource = list;
                dataGridViewMailBox.Visible = true;
            }
            else
            {
                MessageBox.Show(CONNECT_FIRST);
                dataGridViewMailBox.DataSource = null;
            }
            LogIn();
        }


        /// <summary>
        /// Object that represents mailbox
        /// </summary>
        class MailBoxFxtf
        {
            /// <summary>
            /// Receive time
            /// 
            /// </summary>
            public DateTime Time { get; set; }

            /// <summary>
            /// Mail sender (login)
            /// 
            /// </summary>
            public int Sender { get; set; }
            /// <summary>
            /// Mail sender (name)
            /// 
            /// </summary>
            public string From { get; set; }
            /// <summary>
            /// Mail subject
            /// 
            /// </summary>
            public string Subject { get; set; }
            /// <summary>
            /// Pointer to mail Body
            /// 
            /// </summary>
            public string Body { get; set; }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="_date">DateTime</param>
            /// <param name="_from">Text</param>
            /// <param name="_subject">Text</param>
            /// <param name="_body">Text</param>
            /// <param name="_sender">Text</param>
            public MailBoxFxtf(DateTime _date,string _from,string _subject,string _body,int _sender)
            {
                Time = _date;
                From = _from;
                Subject = _subject;
                Body = _body;
                Sender = _sender;
            }
        }



        /// <summary>
        /// Get list of installed plugins in pumping mode
        /// 
        /// </summary>
        private void RequestButton_PluginsClick(object sender, EventArgs e)
        {
            if (ConnectionMT4Server.clrWrapper.IsConnected() != 0)
            {
                PumpingSwitch();
                IList<Plugin> plugins = ConnectionMT4Server.clrWrapper.PluginsGet();
                IList<PluginFxtf> list = new List<PluginFxtf>();

                foreach (var p in plugins)
                {
                    string name = p.Info.Name;
                    uint version = p.Info.Version;
                    string file = p.File;
                    list.Add(new PluginFxtf(name, version, file));
                }

                dataGridViewPlugins.DataSource = list;
                dataGridViewPlugins.Visible = true;
            }
            else
            {
                MessageBox.Show(CONNECT_FIRST);
                dataGridViewPlugins.DataSource = null;
            }
        }

        /// <summary>
        /// Object that represents plugin information configuration
        /// </summary>
        class PluginFxtf
        {
            /// <summary>
            /// Plugin file name
            /// 
            /// </summary>
            public string File { get; set; }
            /// <summary>
            /// Plugin name
            /// 
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// Plugin version
            /// 
            /// </summary>
            public uint Version { get; set; }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="_name">Text</param>
            /// <param name="_version">Number</param>
            /// <param name="_file">Text</param>
            public PluginFxtf(string _name,uint _version,string _file)
            {
                Name = _name;
                Version = _version;
                File = _file;
            }
        }


/*
        public void UserList()
        {
            int login;

            //User Request
            PumpingSwitch();
            IList<UserRecord> tradeRecords = ConnectionMT4Server.clrWrapper.UsersGet();
            ListLogin = new List<int>();
            foreach (var p in tradeRecords)
            {
                login = p.Login;
                ListLogin.Add(login);
            }
        }*/

        /// <summary>
        /// Get daily reports
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RequestButton_DailyReportsClick(object sender, EventArgs e)
        {
            if (ConnectionMT4Server.clrWrapper.IsConnected() != 0)
            {
                LogIn();
                var watch = System.Diagnostics.Stopwatch.StartNew();

                DateTime dt1 = FromDateTimePicker_DailyReports.Value.Date;
                uint from = ToUnixTime(dt1);

                DateTime dt2 = ToDateTimePicker_DailyReports.Value.Date;
                uint to = ToUnixTime(dt2);

                
                if ((int)to - (int)from >= 86400) //checking one day interval in seconds
                {
                    try
                    {
                      //  int login = int.Parse(LogintextBox_DailyReports.Text);
                        IList<UserRecord> Users = ConnectionMT4Server.clrWrapper.UsersRequest();
                        IList<int> listLogin = new List<int>();
                  
                        foreach (var p in Users)
                        {
                            listLogin.Add(p.Login);
                        }
                      //  listLogin.Add(2015052217);


                        DailyGroupRequest request = new DailyGroupRequest();
                        request.To = to;
                        request.From = from;
                        request.Name = "Daily"; //NEED TO EDIT

                        //UserList();


                        IList<DailyReport> dailyReportses = ConnectionMT4Server.clrWrapper.DailyReportsRequest(request, listLogin);
                        IList<DailyReportFxtf> list = new List<DailyReportFxtf>();
                   
                        foreach (var p in dailyReportses)
                        {
                            int log = p.Login;
                            uint time = p.Ctm;
                            DateTime dateTime = ToDateTime(time);
                            double credit = p.Credit;
                            double equity = p.Equity;
                            double balance = p.Balance;
                            double profit = p.Profit;
                            double profitClosed = p.ProfitClosed;
                            double balancePrev = p.BalancePrev;
                            double deposit=p.Deposit;
                            list.Add(new DailyReportFxtf(log, dateTime, credit, equity, balance, profit, profitClosed, balancePrev,deposit));
                        }
                        dataGridView_DailyReports.DataSource = list;
                        dataGridView_DailyReports.Visible = true;

                        watch.Stop();
                        var elapsedMs = watch.ElapsedMilliseconds;
                        Message_label.Text = "count: "+list.Count.ToString() + ", seconds=" + (elapsedMs / 1000).ToString()+"from: "+ from.ToString()+", to: "+ to.ToString();
                        LogIn();
                    }
                    catch (Exception )
                    {
                        const string msg = "Can't parse the ID field.";
                        MessageBox.Show(msg);
                    }
                   
                }
                else
                {
                    const string msg = "From date and To date must be between 1 day.";
                    MessageBox.Show(msg);
                }
            }
            else
            {
                MessageBox.Show(CONNECT_FIRST);
                dataGridView_DailyReports.DataSource = null;
            }
            LogIn();
        }


        /// <summary>
        /// Object that represents daily report
        /// </summary>
        class DailyReportFxtf
        {
            /// <summary>
            /// Login
            /// 
            /// </summary>
            public int Login { get; set; }

            /// <summary>
            /// Time
            /// 
            /// </summary>
            public DateTime Date { get; set; }

            /// <summary>
            /// Credit
            /// 
            /// </summary>
            public double Credit { get; set; }
            /// <summary>
            /// Equity
            /// 
            /// </summary>
            public double Equity { get; set; }
            /// <summary>
            /// Balance
            /// 
            /// </summary>
            public double Balance { get; set; }


            /// <summary>
            /// Closed profit/loss
            /// </summary>
            public double ProfitClosed { get; set; }

            /// <summary>
            /// Floating profit/loss
            /// </summary>
            public double Profit { get; set; }
            public double BalancePrev { get; set; }
            /// <summary>
            /// Deposit
            /// </summary>
            public double Deposit { get; set; }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="_Login"></param>
            /// <param name="_dateTime"></param>
            /// <param name="_credit"></param>
            /// <param name="_equity"></param>
            /// <param name="_balance"></param>
            public DailyReportFxtf(int _Login,DateTime _dateTime, double _credit, double _equity, double _balance, double profit, double profitClosed, double balancePrev,double deposit)
            {
                   Login = _Login;
                   Date = _dateTime;
                   Credit = _credit;
                   Equity = _equity;
                   Balance = _balance;
                   ProfitClosed = profitClosed;
                    Profit = profit;
                BalancePrev = balancePrev;
                Deposit = deposit;
            }
        }


        /// <summary>
        /// Send News
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendNewsButton_MainClick(object sender, EventArgs e)
        {
            if (ConnectionMT4Server.clrWrapper.IsConnected() != 0)
            {
                NewsSend newsForm = new NewsSend(ConnectionMT4Server.clrWrapper);
                newsForm.Show();
            }
            else
            {
                MessageBox.Show(CONNECT_FIRST);
            }
        }

        /// <summary>
        /// Enable pumping switch mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PumpingButton_MainClick(object sender, EventArgs e)
        {
            if (ConnectionMT4Server.clrWrapper.IsConnected() != 0)
            {
                PumpingSwitch();
                string msg = "Pumping Switch started.";
                MessageBox.Show(msg);
            }
            else
            {
                MessageBox.Show(CONNECT_FIRST);
            }
        }


        private void ConnectDealerButton_DealerClick(object sender, EventArgs e)
        {
       
        }


        private void TimeButton_OthersClick(object sender, EventArgs e)
        {
            if (isLoggedIn && ConnectionMT4Server.clrWrapper.IsConnected() != 0)
            {
                uint time = (uint)ConnectionMT4Server.clrWrapper.ServerTime();
                DateTime dateTime = ToDateTime(time);
                MessageBox.Show(dateTime.ToString());
            }
            else
            {
                const string msg = "You aren't logged in.";
                MessageBox.Show(msg);
            }
           
        }
        IList<TradeRecordsListFxtf> listAdmin = new List<TradeRecordsListFxtf>();
        private IList<TradeRecord> tradeRecordsSync;
        private void PingButton_OthersClick(object sender, EventArgs e)
        {
            // MessageBox.Show(ConnectionMT4Server.clrWrapper.ErrorDescription(ConnectionMT4Server.clrWrapper.Ping()));
            if (ConnectionMT4Server.clrWrapper.IsConnected() != 0)
            {
                //PumpingSwitch();
                var watch = System.Diagnostics.Stopwatch.StartNew();

                //IList<TradeRecord> tradeRecords = ConnectionMT4Server.clrWrapper.AdmTradesRequest("*",0);
                tradeRecordsSync = ConnectionMT4Server.clrWrapper.TradesSyncRead();


                int n = 0;
                foreach (var p in tradeRecordsSync)
                {
                    int login = p.Login;
                    int order = p.Order;
                    string symbol = p.Symbol;
                    double profit = p.Profit;
                    DateTime openTime = ToDateTime(p.OpenTime);
                    DateTime clsoeTime = ToDateTime(p.CloseTime);
                    DateTime timeStamp = ToDateTime(p.Timestamp);
                    n++;
                    listAdmin.Add(new TradeRecordsListFxtf(login, order, symbol, profit, openTime, clsoeTime, timeStamp));
                }


                listAdmin = listAdmin.OrderBy(o => o.OpenTime).ThenBy(o => o.Login).ToList();
                dataGridViewOthers.Visible = true;
                dataGridViewOthers.DataSource = listAdmin;
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Message_label.Text = listAdmin.Count.ToString() + "=" + (elapsedMs/1000).ToString()+"=count="+n.ToString();
                LogIn();
            }
            else
            {
                MessageBox.Show(CONNECT_FIRST);
                dataGridViewOthers.DataSource = null;
            }
        }
   
        private void ServerRestartButton_OthersClick(object sender, EventArgs e)
        {
            /*     MessageBox.Show(ConnectionMT4Server.clrWrapper.ErrorDescription(ConnectionMT4Server.clrWrapper.SrvRestart()));
                 isLoggedIn = false;*/

            if (ConnectionMT4Server.clrWrapper.IsConnected() != 0)
            {
                //PumpingSwitch();
                var watch = System.Diagnostics.Stopwatch.StartNew();

                tradeRecordsSync = ConnectionMT4Server.clrWrapper.AdmTradesRequest("*",0);
                //IList<TradeRecord> tradeRecords = ConnectionMT4Server.clrWrapper.TradesSyncRead();

             

                foreach (var p in tradeRecordsSync)
                {
                    int login = p.Login;
                    int order = p.Order;
                    string symbol = p.Symbol;
                    double profit = p.Profit;
                    DateTime openTime = ToDateTime(p.OpenTime);
                    DateTime clsoeTime = ToDateTime(p.CloseTime);
                    DateTime timeStamp = ToDateTime(p.Timestamp);

                    listAdmin.Add(new TradeRecordsListFxtf(login, order, symbol, profit, openTime, clsoeTime, timeStamp));
                }


                listAdmin = listAdmin.OrderBy(o => o.OpenTime).ThenBy(o => o.Login).ToList();
                dataGridViewOthers.Visible = true;
                dataGridViewOthers.DataSource = listAdmin;
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Message_label.Text = listAdmin.Count.ToString() + "=" + (elapsedMs / 1000).ToString();
                LogIn();
            }
            else
            {
                MessageBox.Show(CONNECT_FIRST);
                dataGridViewOthers.DataSource = null;
            }
        }

        /// <summary>
        /// Request all available on MT server data feeds
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ServerFeedButton_OthersClick(object sender, EventArgs e)
        {
            if (ConnectionMT4Server.clrWrapper.IsConnected() != 0)
            {

                IList<ServerFeed> serverFeeds = ConnectionMT4Server.clrWrapper.SrvFeeders();
                IList<ServerFeedFxtf> list = new List<ServerFeedFxtf>();

                foreach (var p in serverFeeds)
                {

                    string file = p.File;
                    string description = p.Feed.Description;
                    int version = p.Feed.Version;
                    string name = p.Feed.Name;
                    string copyright = p.Feed.Copyright;
                    string web = p.Feed.Web;
                    string email = p.Feed.Email;
                    string server = p.Feed.Server;
                    string userName = p.Feed.UserName;

                    list.Add(new ServerFeedFxtf(file,description,version,name,copyright,web,email,server,userName));
                }

                dataGridViewOthers.DataSource = list;
                dataGridViewOthers.Visible = true;
            }
            else
            {
                MessageBox.Show(CONNECT_FIRST);
                dataGridViewOthers.DataSource = null;
            }
            LogIn();
        }

        /// <summary>
        /// Object that represents server feed
        /// </summary>
        class ServerFeedFxtf
        {
            /// <summary>
            /// Feeder file name
            /// 
            /// </summary>
            public string File { get; set; }
            /// <summary>
            /// Feeder description
            /// 
            /// </summary>
            public string Description  { get; set; }

            /// <summary>
            /// Data source version
            /// 
            /// </summary>
            public int Version { get; set; }
            /// <summary>
            /// Data source name
            /// 
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Copyright string
            /// 
            /// </summary>
            public string Copyright { get; set; }

            /// <summary>
            /// Data source web
            /// 
            /// </summary>
            public string Web { get; set; }
            /// <summary>
            /// Data source email
            /// 
            /// </summary>
            public string Email { get; set; }
            /// <summary>
            /// Feeder server
            /// 
            /// </summary>
            public string Server { get; set; }
            /// <summary>
            /// Default feeder name
            /// 
            /// </summary>
            public string UserName { get; set; }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="_file">Text</param>
            /// <param name="_description">Text</param>
            /// <param name="_version">Number</param>
            /// <param name="_name">Text</param>
            /// <param name="_copyright">Text</param>
            /// <param name="_web">Text</param>
            /// <param name="_email">Text</param>
            /// <param name="_server">Text</param>
            /// <param name="_userName">Text</param>
            public ServerFeedFxtf(string _file, string _description, int _version, string _name, string _copyright, string _web, string _email, string _server, string _userName)
            {
                File = _file;
                Description = _description;
                Version = _version;
                Name = _name;
                Copyright = _copyright;
                Web =_web;
                Email =_email;
                Server =_server;
                UserName=_userName;
            }
        }

        
    }
}
