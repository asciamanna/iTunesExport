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
    public decimal AveragePlayCount { get; set; }
    public DateTime DateAdded { get; set; }
    public DateTime? LastPlayed { get; set; }

    public override bool Equals(object obj) {
      var that = obj as Album;
      if (that == null) return false;
      return this.AlbumID == that.AlbumID && this.Name == that.Name &&
        this.Artist == that.Artist && this.AlbumArtist == that.AlbumArtist &&
        this.Genre == that.Genre && this.Year == that.Year &&
        this.AveragePlayCount == that.AveragePlayCount &&
        this.DateAdded == that.DateAdded;
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
        result = (result * 397) ^ (AveragePlayCount.GetHashCode());
        result = (result * 397) ^ (DateAdded.GetHashCode());
        return result;
      }
    }
  }


}
