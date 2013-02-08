using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTunesExport {
  public class ConfigScope : IDisposable {
    IConfig oldConfig;

    public ConfigScope(IConfig config) {
      oldConfig = Config.Instance;
      Config.Instance = config;
    }
    public void Dispose() {
      Config.Instance = oldConfig;
    }
  }
}
