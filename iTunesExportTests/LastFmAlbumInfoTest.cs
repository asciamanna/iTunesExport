using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LastfmClient;
using iTunesExport;
using ITunesLibraryParser;

namespace iTunesExportTests {
  [TestFixture]
  public class LastFmAlbumInfoTest {
    [Test]
    public void UpdateAlbums_UpdatesPlayCounts_If_LastFm_Count_Is_Greater() {
      var lastFmClient = MockRepository.GenerateMock<ILastfmService>();
      var config = MockRepository.GenerateMock<IConfig>();
      var user = "george";
      var lastFmLibraryAlbums = new List<LastfmLibraryAlbum> { new LastfmLibraryAlbum { Artist = "John Coltrane", Name = "Giant Steps", PlayCount = 6 }};
      var albums = new List<Album> { new Album { Artist = "John Coltrane", Name = "Giant Steps", PlayCount = 2 }};
      var lastFmTrackInfo = new LastFmAlbumInfo(lastFmClient);
      config.Expect(c => c.LastFmUser).Return(user);
      lastFmClient.Expect(l => l.FindAllAlbums(user)).Return(lastFmLibraryAlbums);
      
      using (new ConfigScope(config)) {
        lastFmTrackInfo.UpdateAlbums(albums);
      }
      Assert.AreEqual(6, albums.First().PlayCount);
    }

    [Test]
    public void UpdateAlbums_Updates_PlayCounts_Keeps_Album_Play_Count_If_LastFm_Count_Is_Less() {
      var lastFmClient = MockRepository.GenerateMock<ILastfmService>();
      var config = MockRepository.GenerateMock<IConfig>();
      var user = "george";
      var lastFmLibraryAlbums = new List<LastfmLibraryAlbum> { new LastfmLibraryAlbum { Artist = "John Coltrane", Name = "Giant Steps", PlayCount = 6 } };
      var albums = new List<Album> { new Album { Artist = "John Coltrane", Name = "Giant Steps", PlayCount = 12 } };
      var lastFmTrackInfo = new LastFmAlbumInfo(lastFmClient);
      config.Expect(c => c.LastFmUser).Return(user);
      lastFmClient.Expect(l => l.FindAllAlbums(user)).Return(lastFmLibraryAlbums);

      using (new ConfigScope(config)) {
        lastFmTrackInfo.UpdateAlbums(albums);
      }
      Assert.AreEqual(12, albums.First().PlayCount);
    }

    [Test]
    public void UpdateAlbums_Gets_ArtworkLocation_From_LastFM() {
      var lastFmClient = MockRepository.GenerateMock<ILastfmService>();
      var config = MockRepository.GenerateMock<IConfig>();
      var user = "george";
      var lastFmLibraryAlbums = new List<LastfmLibraryAlbum> { new LastfmLibraryAlbum { Artist = "John Coltrane", Name = "Giant Steps", ArtworkLocation = @"http://uri.here.com/asdf" } };
      var albums = new List<Album> { new Album { Artist = "John Coltrane", Name = "Giant Steps", PlayCount = 12 } };
      var lastFmAlbumInfo = new LastFmAlbumInfo(lastFmClient);
      config.Expect(c => c.LastFmUser).Return(user);
      lastFmClient.Expect(l => l.FindAllAlbums(user)).Return(lastFmLibraryAlbums);

      using (new ConfigScope(config)) {
        lastFmAlbumInfo.UpdateAlbums(albums);
      }
      Assert.AreEqual(lastFmLibraryAlbums.First().ArtworkLocation, albums.First().ArtworkLocation);
    }
  }
}
