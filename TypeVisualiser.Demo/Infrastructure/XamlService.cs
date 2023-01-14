using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml;
using TypeVisualiser.RecentFiles;

namespace TypeVisualiser.Demo.Infrastructure
{
    public class XamlService : IXamlService
    {
        public object Load(string fullFileName)
        {
            return XamlServices.Load(fullFileName); 
        }

        public string Save(List<RecentFile> recentFiles)
        {
            return XamlServices.Save(recentFiles);
        }
    }
}
