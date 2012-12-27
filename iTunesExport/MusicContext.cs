using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTunesExport {
  public interface IMusicContext : IDisposable {
    IDbSet<Album> Albums { get; set; }
    int SaveChanges();
    void ClearTables();
  }
  public class MusicContext : DbContext, IMusicContext, IDisposable {
    public IDbSet<Album> Albums { get; set; }
    public void ClearTables() {
      this.Database.ExecuteSqlCommand("delete from Albums"); //sqlce doesn't support truncate
    }
  }
}
