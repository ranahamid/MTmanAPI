namespace temWebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MT4TradeViewModel1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MT4TradeViewModelUpdate",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TradeTicketID = c.Long(nullable: false),
                        CustomerLoginID = c.Int(nullable: false),
                        TradeSymbol = c.String(),
                        SymbolDigits = c.Int(nullable: false),
                        TradeType = c.Long(nullable: false),
                        TradeVolume = c.Int(nullable: false),
                        TradeOpenTime = c.DateTime(),
                        TradeOpenPrice = c.Double(nullable: false),
                        TradeStopLoss = c.Double(nullable: false),
                        TradeTakeProfit = c.Double(nullable: false),
                        TradeCloseTime = c.DateTime(),
                        TradeExpiretionTime = c.DateTime(),
                        TradeReason = c.Int(nullable: false),
                        TradeConvertionRate1 = c.Double(nullable: false),
                        TradeConvertionRate2 = c.Double(nullable: false),
                        TradeCommission = c.Double(nullable: false),
                        TradeCommissionAgent = c.Double(nullable: false),
                        TradeSwap = c.Double(nullable: false),
                        TradeClosePrice = c.Double(nullable: false),
                        TradeProfit = c.Double(nullable: false),
                        TradeTaxes = c.Double(nullable: false),
                        TradeComment = c.String(),
                        TradeInternalID = c.Int(nullable: false),
                        TradeMarginRate = c.Double(nullable: false),
                        TradeTimeStamp = c.DateTime(),
                        TradeGWVolume = c.Int(nullable: false),
                        TradeGWOpenPrice = c.Int(nullable: false),
                        TradeGWClosePrice = c.Int(nullable: false),
                        TradeModifyTime = c.DateTime(),
                        TradeMagic = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MT4TradeViewModelUpdate");
        }
    }
}
