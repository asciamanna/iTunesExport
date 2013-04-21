using iTunesExport;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTunesExportTests {
  [TestFixture]
  public class AlbumTest {
    [Test]
    public void Update_Updates_All_Fields_Except_ID_Name_And_Artist() {
      var album = new Album {
        AlbumID = 1,
        Artist = "John Coltrane",
        Name = "Ballads",
        ArtworkLocation = "Some Url",
        AlbumArtist = "John Coltrane",
        DateAdded = DateTime.Now.AddDays(-4),
        LastPlayed = DateTime.Now.AddDays(-2),
        Genre = "Jazz",
        PlayCount = 25,
        Year = 1964
      };

      var updatedAlbum = new Album {
        AlbumID = 77,
        Artist = "John Coltrane & McCoy Tyner",
        Name = "Ballads Remaster",
        ArtworkLocation = "Some New rl",
        AlbumArtist = "John Coltrane Quartet",
        DateAdded = DateTime.Now.AddDays(-3),
        LastPlayed = DateTime.Now.AddDays(-1),
        Genre = "Hard Bop",
        PlayCount = 55,
        Year = 1963
      };

      album.Update(updatedAlbum);
      Assert.AreEqual(updatedAlbum.ArtworkLocation, album.ArtworkLocation);
      Assert.AreEqual(updatedAlbum.AlbumArtist, album.AlbumArtist);
      Assert.AreEqual(updatedAlbum.DateAdded, album.DateAdded);
      Assert.AreEqual(updatedAlbum.LastPlayed, album.LastPlayed);
      Assert.AreEqual(updatedAlbum.Genre, album.Genre);
      Assert.AreEqual(updatedAlbum.PlayCount, album.PlayCount);
      Assert.AreEqual(updatedAlbum.Year, album.Year);

      Assert.AreNotEqual(updatedAlbum.AlbumID, album.AlbumID);
      Assert.AreNotEqual(updatedAlbum.Artist, album.Artist);
      Assert.AreNotEqual(updatedAlbum.Name, album.Name);
    }
  }
}
