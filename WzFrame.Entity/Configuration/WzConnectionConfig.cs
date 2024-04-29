using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WzFrame.Entity.Configuration
{
    public class DatabaseConfig : IJsonConfiguration
    {
        public bool InitMenu { get; set; }
        public List<WzConnectionConfig> ConnectionConfigs { get; set; }
    }


    public class WzConnectionConfig : ConnectionConfig
    {
        public WzConnectionConfig()
        {
            ConfigureExternalServices = new ConfigureExternalServices()
            {
                EntityService = (c, p) =>
                {
                    if (p.IsPrimarykey == false && new NullabilityInfoContext()
                     .Create(c).WriteState is NullabilityState.Nullable)
                    {
                        p.IsNullable = true;
                    }
                }
            };
        }


        public bool Enable { get; set; }
        public bool InitDb { get; set; }
        public bool InitTable { get; set; }



    }
}
