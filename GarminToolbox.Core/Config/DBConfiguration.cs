using System.Data;
using Dapper.FluentMap;using Dapper.FluentMap.Dommel;
using GarminToolbox.Core.Persistence.Mapping;
using MySql.Data.MySqlClient;
using Unity;

namespace GarminToolbox.Core.Config
{
    public class DBConfiguration
    {
        public DBConfiguration(IUnityContainer unity)
        {
            FluentMapper.Initialize(config =>
            {
                config.AddMap(new ActivityMetadataMap());
                config.ForDommel();
            });

            IDbConnection connection = new MySqlConnection("Server=192.168.2.101;Database=garmin_toolbox_net;Uid=garmintoolboxnet;Pwd=ydumuquOjsa10KUNOw2R;");
            unity.RegisterInstance(connection);
        }
    }
}