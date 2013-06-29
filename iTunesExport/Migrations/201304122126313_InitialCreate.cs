namespace iTunesExport.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Album",
                c => new
                    {
                        AlbumID = c.Long(nullable: false, identity: true),
                        Name = c.String(maxLength: 4000),
                        Artist = c.String(maxLength: 4000),
                        AlbumArtist = c.String(maxLength: 4000),
                        Genre = c.String(maxLength: 4000),
                        Year = c.Int(),
                        PlayCount = c.Int(nullable: false),
                        DateAdded = c.DateTime(nullable: false),
                        LastPlayed = c.DateTime(),
                        ArtworkLocation = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.AlbumID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Album");
        }
    }
}
