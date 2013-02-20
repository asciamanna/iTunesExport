using ITunesLibraryParser;
using System;
using System.Collections.Generic;
using System.Linq;
using LastfmClient;
using System.Diagnostics;

namespace iTunesExport {
  public class AlbumExport {
    readonly IMusicContext db;
    readonly IITunesLibrary library;
    readonly IAlbumTranslator translator;

    public AlbumExport() : this(new MusicContext(), new ITunesLibrary(), new AlbumTranslator()) { }

    public AlbumExport(IMusicContext db, IITunesLibrary library, IAlbumTranslator translator) {
      this.db = db;
      this.library = library;
      this.translator = translator;
    }

    public void Run() {
      Console.WriteLine("Reading iTunes Library File...");
      var stopwatch = new Stopwatch();
      stopwatch.Start();
      var tracks = library.Parse(Config.Instance.ITunesFileLocation);
      var albums = translator.Convert(tracks.ToList());

      var updatedCount = db.UpdateExistingWithIDs(albums);
      var albumsToInsert = AlbumsToInsert(albums);
      var insertedCount = albumsToInsert.Count();
      foreach (var album in albumsToInsert) {
        db.Albums.Add(album);
      }
      Console.WriteLine("Saving iTunes Library Albums to database...");
      db.SaveChanges();
      stopwatch.Stop();
      Console.WriteLine(string.Format("FINISHED in: {0}" + Environment.NewLine + "{1} Albums updated" + Environment.NewLine + "{2} Albums inserted", stopwatch.Elapsed, updatedCount, insertedCount));
      db.Dispose();
    }
    
    static IEnumerable<Album> AlbumsToInsert(IEnumerable<Album> albums) {
      return albums.Where(a => a.AlbumID == 0);
    }

  }
}
