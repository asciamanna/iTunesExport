using ITunesLibraryParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace iTunesExport {
  public interface IAlbumTranslator {
    IEnumerable<Album> Convert(List<Track> tracks);
  }
  public class AlbumTranslator : IAlbumTranslator {

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
        AlbumArtist = Trim(gt.First().AlbumArtist), Artist = Trim(gt.First().Artist),
        Year = gt.First().Year, Genre = Trim(gt.First().Genre), Name = RemoveDiscNumbers(Trim(gt.First().Album))
      };
    }

    static string Trim(string value) {
      return String.IsNullOrEmpty(value) ? string.Empty : value.Trim();
    }

    IEnumerable<Album> BuildCompilationAlbums(List<Track> tracks) {
      var compilationTracks = tracks.Where(t => t.PartOfCompilation).ToList();
      compilationTracks.ForEach(ct => ct.Artist = ct.Album);
      var groupedTracks = compilationTracks.GroupBy(ct => ct.Album);
      return groupedTracks.Select(AlbumGenerator());
    }

    public static string RemoveDiscNumbers(string album) {
      return Regex.Replace(album, @"\s*(\(|\[)\s*Disc\s*\d\s*(\)|\])\s*", string.Empty);
    }


  }
}
