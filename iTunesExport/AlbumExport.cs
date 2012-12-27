using ITunesLibraryParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

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
      Console.WriteLine("Clearing old database...");
      db.ClearTables();
      foreach (var album in albums) {
        db.Albums.Add(album);
      }
      Console.WriteLine("Saving iTunes Library Albums to database...");
      db.SaveChanges();
      Console.WriteLine(string.Format("FINISHED: {0} Albums saved to the database", albums.Count()));
      db.Dispose();
    }

  }
}
