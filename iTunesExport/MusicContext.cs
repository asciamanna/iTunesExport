using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Metadata.Edm;
using System.Linq;

namespace iTunesExport {
  public interface IMusicContext : IDisposable {
    IDbSet<Album> Albums { get; set; }
    IDbSet<AlbumTrack> AlbumTracks { get; set; }
    int SaveChanges();
    void ClearTables();
    int UpdateExisting(IEnumerable<Album> albumsToUpdate);
  }
  public class MusicContext : DbContext, IMusicContext, IDisposable {
    public IDbSet<Album> Albums { get; set; }
    public IDbSet<AlbumTrack> AlbumTracks { get; set; }

    public void ClearTables() {
      var metadata = ((IObjectContextAdapter)this).ObjectContext.MetadataWorkspace;
      var tables = metadata.GetItems<EntityType>(DataSpace.SSpace);
      foreach (var table in tables) {
        this.Database.ExecuteSqlCommand(string.Format("delete from {0}", table.Name)); //sqlce doesn't support truncate
      }
    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder) {
      //removing pluralization of table names because SSpace items are singular 
      //and I need those to match to build delete sql above
      modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
    }

    public int UpdateExisting(IEnumerable<Album> albumsToUpdate) {
      //EF doesn't support bulk updates
      var recordsUpdated = 0;
      foreach (var album in albumsToUpdate) {
        var matchingRecord = this.Albums.FirstOrDefault(a => a.Artist == album.Artist && a.Name == album.Name);
        if (matchingRecord != null) {
          matchingRecord.Update(album);
          album.AlbumID = matchingRecord.AlbumID;
          recordsUpdated++;
        }
      }
      return recordsUpdated;
    }
  }
}
