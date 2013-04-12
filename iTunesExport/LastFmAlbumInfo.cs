using ITunesLibraryParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LastfmClient;
using System.Text.RegularExpressions;

namespace iTunesExport {
  public interface ILastFmAlbumInfo {
    void UpdateAlbums(IEnumerable<Album> albums);
  }

  public class LastFmAlbumInfo : ILastFmAlbumInfo {
    readonly ILastfmService service;

    public LastFmAlbumInfo() : this(new LastfmService(Config.Instance.LastFmApiKey)) { }

    public LastFmAlbumInfo(ILastfmService service) {
      this.service = service;
    }
    public void UpdateAlbums(IEnumerable<Album> albums) {
      var lastFmAlbums = service.FindAllAlbums(Config.Instance.LastFmUser);
      foreach (var album in albums) {
        var lastFmAlbum = lastFmAlbums.FirstOrDefault(MatchingAlbum(album));
        if (lastFmAlbum != null) {
          if (string.IsNullOrEmpty(album.ArtworkLocation) || album.ArtworkLocation.Contains("noimage")) {
            album.ArtworkLocation = lastFmAlbum.ArtworkLocation;
          }
          if (lastFmAlbum.PlayCount > album.PlayCount) {
            album.PlayCount = lastFmAlbum.PlayCount;
          }
        }
      }
    }

    static Func<LastfmLibraryAlbum, bool> MatchingAlbum(Album album) {
      return t => ScrubAlbumData(t.Artist) == ScrubAlbumData(album.Artist) &&
             ScrubAlbumData(t.Name) == ScrubAlbumData(album.Name);
    }

    static string ScrubAlbumData(string track) {
      if (string.IsNullOrWhiteSpace(track)) return string.Empty;

      return track.Trim().ToUpper();
    }
  }
}
