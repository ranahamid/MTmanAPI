namespace temWebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MT4TradeViewModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MT4TradeViewModel", "TradeOpenTime", c => c.DateTime());
            AlterColumn("dbo.MT4TradeViewModel", "TradeCloseTime", c => c.DateTime());
            AlterColumn("dbo.MT4TradeViewModel", "TradeExpiretionTime", c => c.DateTime());
            AlterColumn("dbo.MT4TradeViewModel", "TradeTimeStamp", c => c.DateTime());
            AlterColumn("dbo.MT4TradeViewModel", "TradeModifyTime", c => c.DateTime());
            DropColumn("dbo.TradeRecordsListFxtfs", "Cmd");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TradeRecordsListFxtfs", "Cmd", c => c.Int(nullable: false));
            AlterColumn("dbo.MT4TradeViewModel", "TradeModifyTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.MT4TradeViewModel", "TradeTimeStamp", c => c.DateTime(nullable: false));
            AlterColumn("dbo.MT4TradeViewModel", "TradeExpiretionTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.MT4TradeViewModel", "TradeCloseTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.MT4TradeViewModel", "TradeOpenTime", c => c.DateTime(nullable: false));
        }
    }
}
