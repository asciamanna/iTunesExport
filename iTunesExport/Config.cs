using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTunesExport {
  public interface IConfig {
    string ITunesFileLocation { get; }
  }
  public class Config : IConfig {
    public string ITunesFileLocation {
      get {
        var iTunesLibraryLocation = ConfigurationManager.AppSettings["iTunesLibraryLocation"];
        if (string.IsNullOrEmpty(iTunesLibraryLocation)) {
          throw new ConfigurationErrorsException("iTunesLibraryLocation must be specified in App.config");
        }
        return iTunesLibraryLocation;
      }
    }
  }
}
