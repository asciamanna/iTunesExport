using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTunesExport {
  public class Album {
    public long AlbumID { get; set; }
    public string Name { get; set; }
    public string Artist { get; set; }
    public string AlbumArtist { get; set; }
    public string Genre { get; set; }
    public int? Year { get; set; }
    public int PlayCount { get; set; }
    public DateTime DateAdded { get; set; }
    public DateTime? LastPlayed { get; set; }
    public string ArtworkLocation { get; set; }

    public override bool Equals(object obj) {
      var that = obj as Album;
      if (that == null) return false;
      return this.AlbumID == that.AlbumID && this.Name == that.Name &&
        this.Artist == that.Artist && this.AlbumArtist == that.AlbumArtist &&
        this.Genre == that.Genre && this.Year == that.Year &&
        this.PlayCount == that.PlayCount &&
        this.DateAdded == that.DateAdded &&
        this.ArtworkLocation == that.ArtworkLocation;
    }

    public override int GetHashCode() {
      unchecked {
        var result = 0;
        result = (result * 397) ^ AlbumID.GetHashCode();
        result = (result * 397) ^ (String.IsNullOrEmpty(Name) ? 0 : Name.GetHashCode());
        result = (result * 397) ^ (String.IsNullOrEmpty(Artist) ? 0 : Artist.GetHashCode());
        result = (result * 397) ^ (String.IsNullOrEmpty(AlbumArtist) ? 0 : AlbumArtist.GetHashCode());
        result = (result * 397) ^ (String.IsNullOrEmpty(Genre) ? 0 : Genre.GetHashCode());
        result = (result * 397) ^ (Year.HasValue ? Year.GetHashCode() : 0);
        result = (result * 397) ^ (PlayCount.GetHashCode());
        result = (result * 397) ^ (DateAdded.GetHashCode());
        result = (result * 397) ^ (ArtworkLocation.GetHashCode());
        return result;
      }
    }

    public void Update(Album album) {
      this.AlbumArtist = album.AlbumArtist;
      this.DateAdded = album.DateAdded;
      this.Year = album.Year;
      this.PlayCount = album.PlayCount;
      this.LastPlayed = album.LastPlayed;
      this.ArtworkLocation = !string.IsNullOrEmpty(this.ArtworkLocation) ? this.ArtworkLocation : album.ArtworkLocation;
      this.Genre = album.Genre;
    }
  }


}
