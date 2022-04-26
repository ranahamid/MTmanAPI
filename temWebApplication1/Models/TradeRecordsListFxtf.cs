using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using P23.MetaTrader4.Manager.Contracts;

namespace temWebApplication1.Models
{
   

     /// <summary>
        /// Object that represents trade record
        /// 
        /// </summary>
        public class TradeRecordsListFxtf
        {
            
        public int ID { get; set; }           

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
            public double  GwOpenPrice { get; set; }
            /// <summary>
            /// Gateway order price deviation (pips) from order close price
            /// 
            /// </summary>
            public double GwClosePrice { get; set; }
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


}