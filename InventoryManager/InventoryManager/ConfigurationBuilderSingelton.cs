using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManager
{
    public class ConfigurationBuilderSingleton
    {
        private static ConfigurationBuilderSingleton instance = null;
        private static readonly object inctanceLock = new object();
        private static IConfigurationRoot configuration;

        private ConfigurationBuilderSingleton()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            configuration = builder.Build();
        }

        public static ConfigurationBuilderSingleton Instance
        {
            get
            {
                lock (inctanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ConfigurationBuilderSingleton();
                    }

                    return instance;
                }
            }
        }

        public static IConfigurationRoot ConfigurationRoot
        {
            get
            {
                if (configuration == null)
                {
                    var x = Instance;
                }

                return configuration;
            }
        }

    }
}
