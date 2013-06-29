namespace iTunesExport.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAlbumTracks : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AlbumTrack",
                c => new
                    {
                        AlbumTrackId = c.Long(nullable: false, identity: true),
                        Name = c.String(maxLength: 4000),
                        PlayingTime = c.String(maxLength: 4000),
                        TrackNumber = c.Int(nullable: false),
                        AlbumID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.AlbumTrackId)
                .ForeignKey("dbo.Album", t => t.AlbumID, cascadeDelete: true)
                .Index(t => t.AlbumID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.AlbumTrack", new[] { "AlbumID" });
            DropForeignKey("dbo.AlbumTrack", "AlbumID", "dbo.Album");
            DropTable("dbo.AlbumTrack");
        }
    }
}
