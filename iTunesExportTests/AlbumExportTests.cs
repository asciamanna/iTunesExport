using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using iTunesExport;
using ITunesLibraryParser;
using Rhino.Mocks;
using System.Data.Entity;

namespace iTunesExportTests {
  [TestFixture]
  public class AlbumExportTests {
    [Test]
    public void Run() {
      var db = MockRepository.GenerateMock<IMusicContext>();
      var library = MockRepository.GenerateMock<IITunesLibrary>();
      var translator = MockRepository.GenerateMock<IAlbumTranslator>();
      var config = MockRepository.GenerateMock<IConfig>();

      var album = new Album { Artist = "Chroma Key", Genre = "Rock", Name = "You Go Now", Year = 1999 };
      var tracks = new List<Track>();
      var dbSetAlbums = new FakeDbSet<Album>();
      var fileLocation = "loc";

      db.Expect(d => d.ClearTables());
      db.Stub(d => d.Albums).Return(dbSetAlbums);
      config.Stub(c => c.ITunesFileLocation).Return(fileLocation);
      library.Stub(l => l.Parse(fileLocation)).Return(tracks);
      translator.Stub(t => t.Convert(tracks)).Return(new List<Album> { album });
      db.Expect(d => d.SaveChanges()).Return(1);

      new AlbumExport(db, library, translator, config).Run();

      Assert.AreEqual(1, dbSetAlbums.Count());
      Assert.AreEqual(album, dbSetAlbums.First());
      db.VerifyAllExpectations();
    }
  }
}
