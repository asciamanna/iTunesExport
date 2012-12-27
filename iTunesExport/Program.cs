using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace iTunesExport {
  class Program {
    static void Main(string[] args) {
      new AlbumExport().Run();
      Console.WriteLine("Press any key to exit...");
      Console.ReadKey();
    }
  }
}
