using ITunesLibraryParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace iTunesExport {
  public interface IAlbumTranslator {
    IEnumerable<Album> Convert(List<Track> tracks);
  }
  public class AlbumTranslator : IAlbumTranslator {
    //SqlCE doesn't support DateTime2 so sql server mindate needs to be used 
    //for null dates.
    static DateTime SqlServerMinDate = new DateTime(1753, 1, 1);

    public IEnumerable<Album> Convert(List<Track> tracks) {
      RemoveTracksWithoutAlbumNames(tracks);
      var albums = BuildAlbums(tracks).ToList();
      albums.AddRange(BuildCompilationAlbums(tracks));
      return albums;
    }

    void RemoveTracksWithoutAlbumNames(List<Track> tracks) {
      tracks.RemoveAll(t => string.IsNullOrEmpty(t.Album));
    }

    IEnumerable<Album> BuildAlbums(List<Track> tracks) {
      var groupedTracks = tracks.Where(t => !t.PartOfCompilation).GroupBy(t => Trim(t.Artist) + "-" + Trim(t.Album));
      return groupedTracks.Where(gt => gt.Count() > 2).Select(AlbumGenerator());
    }

    static Func<IGrouping<string, Track>, Album> AlbumGenerator() {
      return gt => new Album {
        AlbumArtist = Trim(gt.First().AlbumArtist),
        Artist = Trim(gt.First().Artist),
        Year = gt.First().Year,
        Genre = Trim(gt.First().Genre),
        Name = Trim(gt.First().Album),
        DateAdded = gt.First().DateAdded.HasValue ? gt.First().DateAdded.Value : SqlServerMinDate,
        LastPlayed = gt.Max(t => t.PlayDate) != null ? gt.Max(t => t.PlayDate) : SqlServerMinDate,
        PlayCount = CalculateAlbumPlayCount(gt),
        Tracks = BuildTracks(gt)
      };
    }

    static List<AlbumTrack> BuildTracks(IGrouping<string, Track> gt) {
      var tracks = new List<AlbumTrack>();
      gt.ToList().ForEach(t => {
        var track = new AlbumTrack { Name = t.Name, PlayingTime = t.PlayingTime, TrackNumber = t.TrackNumber ?? 0, };
        tracks.Add(track);
      });
      return tracks;
    }

    static int CalculateAlbumPlayCount(IGrouping<string, Track> gt) {
      return gt.Sum(g => g.PlayCount ?? 0);
    }

    static string Trim(string value) {
      return String.IsNullOrEmpty(value) ? string.Empty : value.Trim();
    }

    IEnumerable<Album> BuildCompilationAlbums(List<Track> tracks) {
      var compilationTracks = tracks.Where(t => t.PartOfCompilation).ToList();
      compilationTracks.ForEach(ct => ct.Artist = "Various Artists");
      var groupedTracks = compilationTracks.GroupBy(ct => ct.Album);
      return groupedTracks.Select(AlbumGenerator());
    }
  }
}
