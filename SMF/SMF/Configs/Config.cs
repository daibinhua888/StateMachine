using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMFramework.Configs
{
    internal static class Config
    {
        internal static string RepositoryConnectionString { get; set; }

        static Config()
        {
            RepositoryConnectionString = ConfigurationManager.ConnectionStrings["SMF.Repository"].ConnectionString;
        }
    }
}
