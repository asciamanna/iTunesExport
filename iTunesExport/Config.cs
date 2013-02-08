using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTunesExport {
  public interface IConfig {
    string ITunesFileLocation { get; }
    string LastFmApiKey { get; }
  }

  public class Config : IConfig {
    static IConfig config;

    private Config() { }
    public static IConfig Instance {
      get {
        if (config == null) {
          config = new Config();
        }
        return config;
      }
      set { config = value; }
    }

    public string ITunesFileLocation {
      get {
        var iTunesLibraryLocation = ConfigurationManager.AppSettings["iTunesLibraryLocation"];
        if (string.IsNullOrEmpty(iTunesLibraryLocation)) {
          throw new ConfigurationErrorsException("iTunesLibraryLocation must be specified in App.config");
        }
        return iTunesLibraryLocation;
      }
    }

    public string LastFmApiKey {
      get {
        var lastFmApiKey = ConfigurationManager.AppSettings["lastFmApiKey"];
        if (string.IsNullOrEmpty(lastFmApiKey)) {
          throw new ConfigurationErrorsException("lastFmApiKey must be specified in App.config");
        }
        return lastFmApiKey;
      }
    }
  }
}
