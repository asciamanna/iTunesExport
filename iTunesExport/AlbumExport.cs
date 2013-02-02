using ITunesLibraryParser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace iTunesExport {
  public class AlbumExport {
    readonly IMusicContext db;
    readonly IITunesLibrary library;
    readonly IAlbumTranslator translator;
    readonly IConfig config;

    public AlbumExport() : this(new MusicContext(), new ITunesLibrary(), new AlbumTranslator(), new Config()) { }

    public AlbumExport(IMusicContext db, IITunesLibrary library, IAlbumTranslator translator, IConfig config) {
      this.db = db;
      this.library = library;
      this.translator = translator;
      this.config = config;
    }

    public void Run() {
      Console.WriteLine("Reading iTunes Library File...");
      var tracks = library.Parse(config.ITunesFileLocation);
      var albums = translator.Convert(tracks.ToList());

      var updatedCount = db.UpdateExistingWithIDs(albums);
      var albumsToInsert = AlbumsToInsert(albums);
      var insertedCount = albumsToInsert.Count();
      foreach (var album in albumsToInsert) {
        db.Albums.Add(album);
      }
      Console.WriteLine("Saving iTunes Library Albums to database...");
      db.SaveChanges();
      Console.WriteLine(string.Format("FINISHED: " + Environment.NewLine + "{0} Albums updated" + Environment.NewLine + "{1} Albums inserted", updatedCount, insertedCount));
      db.Dispose();
    }
    
    static IEnumerable<Album> AlbumsToInsert(IEnumerable<Album> albums) {
      return albums.Where(a => a.AlbumID == 0);
    }

  }
}
