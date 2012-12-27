using iTunesExport;
using ITunesLibraryParser;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTunesImportTests {
  [TestFixture]
  public class AlbumTranslatorTest {
    Track track;
    [SetUp]
    public void Setup() {
      track = new Track {
        Album = "My Favorite Things",
        Artist = "John Coltrane",
        AlbumArtist = "John Coltrane",
        Genre = "Jazz",
        Name = "Summertime",
        Year = 1961
      };
    }

    [Test]
    public void Convert_Trims_Whitespace_on_all_string_attributes() {
      var track = new Track {
        Album = "  John Coltrane ", AlbumArtist = "John Coltrane   ",
        Genre = "  Jazz ", Artist = " John Coltrane", Year = 1961
      };
      var translator = new AlbumTranslator();

      var albums = translator.Convert(new List<Track> { track, track, track });
      var album = albums.First();
      Assert.AreEqual(track.Album.Trim(), album.Name);
      Assert.AreEqual(track.AlbumArtist.Trim(), album.AlbumArtist);
      Assert.AreEqual(track.Genre.Trim(), album.Genre);
    }

    [Test]
    public void Convert_Converts_Multiple_Tracks_From_Same_Album_To_A_Single_Album() {
      var track2 = track.Copy();
      track2.Name = "But Not For Me";

      var track3 = track.Copy();
      track3.Name = "My Favorite Things";

      var translator = new AlbumTranslator();
      var albums = translator.Convert(new List<Track> { track, track2, track3 });
      Assert.AreEqual(1, albums.Count());
      var album = albums.First();
      Assert.AreEqual(track2.Artist, album.Artist);
      Assert.AreEqual(track2.AlbumArtist, album.AlbumArtist);
      Assert.AreEqual(track2.Genre, album.Genre);
      Assert.AreEqual(track2.Year, album.Year);
      Assert.AreEqual(track2.Album, album.Name);
    }

    [Test]
    public void Convert_Removes_Any_Tracks_Without_An_Album_Name() {
      track.Album = string.Empty;

      var translator = new AlbumTranslator();
      var albums = translator.Convert(new List<Track> { track });
      Assert.AreEqual(0, albums.Count());
    }

    [TestCase(1, 0)]
    [TestCase(2, 0)]
    [TestCase(3, 1)]
    public void Convert_Doesnt_Create_Albums_If_Less_Than_Three_Tracks_For_Album(int tracksForAlbum, int expectedNumberOfAlbums) {
      var tracks = new List<Track>();

      for (int i = 0; i < tracksForAlbum; i++) {
        var testTrack = track.Copy();
        testTrack.Name = "track" + i;
        tracks.Add(testTrack);
      }

      var translator = new AlbumTranslator();
      var albums = translator.Convert(tracks);
      Assert.AreEqual(expectedNumberOfAlbums, albums.Count());
    }

    [Test]
    public void Convert_Creates_CompilationAlbums_With_Artist_Same_As_Album_Name() {
      var compTrack1 = track.Copy();
      compTrack1.PartOfCompilation = true;
      compTrack1.Artist = "First Artist";

      var compTrack2 = track.Copy();
      compTrack2.PartOfCompilation = true;
      compTrack2.Artist = "Second Artist";

      var translator = new AlbumTranslator();
      var albums = translator.Convert(new List<Track> { compTrack1, compTrack2 });
      Assert.AreEqual(1, albums.Count());
      Assert.AreEqual(compTrack1.Album, albums.First().Artist);
    }
  }
}
