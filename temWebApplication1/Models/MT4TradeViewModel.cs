using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace temWebApplication1.Models
{
    public class MT4TradeViewModel
    {
        public int ID { get; set; }
        public long TradeTicketID { get; set; }
        public int CustomerLoginID { get; set; }
        public string TradeSymbol { get; set; }
        public int SymbolDigits { get; set; }
        public long TradeType { get; set; }
        public int TradeVolume { get; set; }
        public DateTime? TradeOpenTime { get; set; }
        public double TradeOpenPrice { get; set; }
        public double TradeStopLoss { get; set; }
        public double TradeTakeProfit { get; set; }
        public DateTime? TradeCloseTime { get; set; }
        public DateTime? TradeExpiretionTime { get; set; }
        public int TradeReason { get; set; }
        public double TradeConvertionRate1 { get; set; }
        public double TradeConvertionRate2 { get; set; }
        public double TradeCommission { get; set; }
        public double TradeCommissionAgent { get; set; }
        public double TradeSwap { get; set; }
        public double TradeClosePrice { get; set; }
        public double TradeProfit { get; set; }
        public double TradeTaxes { get; set; }
        public string TradeComment { get; set; }
        public int TradeInternalID { get; set; }
        public double TradeMarginRate { get; set; }
        public DateTime? TradeTimeStamp { get; set; }
        public int TradeGWVolume { get; set; }
        public int TradeGWOpenPrice { get; set; }
        public int TradeGWClosePrice { get; set; }
        public DateTime? TradeModifyTime { get; set; }
        public int TradeMagic { get; set; }
    }
}