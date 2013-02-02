using System;

namespace iTunesExport {
  class Program {
    static void Main(string[] args) {
      new AlbumExport().Run();
      Console.WriteLine("Press any key to exit...");
      Console.ReadKey();
    }
  }
}
